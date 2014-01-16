using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

using FlickrNet;

using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet("Get", "FlickrAbstractClass", DefaultParameterSetName = FlickrPSCmdlet.EMail)]
    public abstract class FlickrPSCmdlet : PSCmdlet
    {
        private const string EMail = "EMail";

        private const string Value = "Value";

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = FlickrPSCmdlet.EMail)]
        public string UserName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = FlickrPSCmdlet.Value)]
        public string AccessToken { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName.Equals(FlickrPSCmdlet.EMail))
            {
                try
                {
                    var token = FlickrTokenCache.FromFile().Single(t => t.UserName.Equals(this.UserName));
                    this.AccessToken = token.AccessToken;
                }
                catch (Exception failure)
                {
                    this.WriteError(new ErrorRecord(
                        failure,
                        failure.Message,
                        ErrorCategory.InvalidResult,
                        FlickrConfig.Config));
                    return;
                }
            }
            try
            {
                var client = new FlickrNet.Flickr(FlickrConfig.Config.ApiKey, FlickrConfig.Config.SharedSecret, this.AccessToken);
                this.WriteObject(this.Process(client), true);
            }
            catch (FlickrException failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    FlickrConfig.Config));
                return;
            }
            catch (Exception failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    FlickrConfig.Config));
                return;
            }
        }

        protected abstract IEnumerable<object> Process(FlickrNet.Flickr client);
    }
}