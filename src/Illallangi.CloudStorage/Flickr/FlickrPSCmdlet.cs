using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using FlickrNet;
using Illallangi.CloudStorage.Config;

namespace Illallangi.CloudStorage.Flickr
{
    [Cmdlet("Get", "FlickrAbstractClass")]
    public abstract class FlickrPsCmdlet : NinjectCmdlet<FlickrModule>
    {
        #region Fields

        private string currentToken;

        private IFlickrConfig currentConfig;

        private FlickrNet.Flickr currentClient;

        private IEnumerable<FlickrToken> currentTokens;

        #endregion

        #region Properties

        [Parameter(Mandatory = false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string AccessToken { get; set; }

        [Parameter(Mandatory = false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string AuthUser { get; set; }

        protected IFlickrConfig Config
        {
            get
            {
                return this.currentConfig ?? (this.currentConfig = this.GetConfig());
            }
        }

        protected IEnumerable<FlickrToken> Tokens
        {
            get
            {
                return this.currentTokens ?? (this.currentTokens = this.GetTokens());
            }
        }

        protected FlickrNet.Flickr Client
        {
            get
            {
                return this.currentClient ?? (this.currentClient = this.GetClient());
            }
        }

        private string Token
        {
            get
            {
                return this.currentToken ?? (this.currentToken = this.GetToken());
            }
        }

        #endregion

        #region Methods

        protected override void ProcessRecord()
        {
            try
            {
                this.WriteObject(this.Process(), true);
            }
            catch (FlickrException failure)
            {
                this.WriteError(new ErrorRecord(failure, failure.Message, ErrorCategory.InvalidResult, this.Config));
            }
        }

        protected abstract IEnumerable<object> Process();

        private string GetToken()
        {
            if (!string.IsNullOrWhiteSpace(this.AuthUser))
            {
                return this.Tokens.Single(t => t.UserName.Equals(this.AuthUser)).AccessToken;
            }

            if (!string.IsNullOrWhiteSpace(this.AccessToken))
            {
                return this.AccessToken;
            }

            return this.Tokens.First().AccessToken;
        }

        private IEnumerable<FlickrToken> GetTokens()
        {
            return this.Get<ICollection<FlickrToken>>();
        }

        private IFlickrConfig GetConfig()
        {
            return this.Get<IFlickrConfig>();
        }

        private FlickrNet.Flickr GetClient()
        {
            return new FlickrNet.Flickr(
                    this.Config.ApiKey,
                    this.Config.SharedSecret,
                    this.Token);
        }

        #endregion
    }
}