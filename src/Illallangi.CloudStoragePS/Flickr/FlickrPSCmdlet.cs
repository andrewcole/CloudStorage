using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

using FlickrNet;

using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet("Get", "FlickrAbstractClass")]
    public abstract class FlickrPSCmdlet : PSCmdlet
    {
        #region Fields

        private string currentToken;

        private FlickrNet.Flickr currentClient;

        #endregion

        #region Properties

        private string Token
        {
            get
            {
                return this.currentToken ?? (this.currentToken = this.GetToken());
            }
        }

        protected FlickrNet.Flickr Client
        {
            get
            {
                return this.currentClient ?? (this.currentClient = this.GetClient());
            }
        }

        [Parameter(Mandatory = false)]
        public string AccessToken { get; set; }

        [Parameter(Mandatory = false)]
        public string AuthUser { get; set; }

        #endregion

        #region Methods

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

        #endregion
    }
}