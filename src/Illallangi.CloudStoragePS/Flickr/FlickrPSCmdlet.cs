using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using FlickrNet;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet("Get", "FlickrAbstractClass")]
    public abstract class FlickrPsCmdlet : PSCmdlet
    {
        #region Fields

        private string currentToken;

        private FlickrNet.Flickr currentClient;

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
                this.WriteError(new ErrorRecord(failure, failure.Message, ErrorCategory.InvalidResult, FlickrConfig.Config));
            }
        }

        protected abstract IEnumerable<object> Process();

        private string GetToken()
        {
            if (!string.IsNullOrWhiteSpace(this.AuthUser))
            {
                return FlickrTokenCache.FromFile().Single(t => t.UserName.Equals(this.AuthUser)).AccessToken;
            }

            if (!string.IsNullOrWhiteSpace(this.AccessToken))
            {
                return this.AccessToken;
            }

            return FlickrTokenCache.FromFile().First().AccessToken;
        }

        private FlickrNet.Flickr GetClient()
        {
            return new FlickrNet.Flickr(
                    FlickrConfig.Config.ApiKey,
                    FlickrConfig.Config.SharedSecret,
                    this.Token);
        }

        #endregion
    }
}