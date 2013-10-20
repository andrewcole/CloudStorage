using System;
using System.Diagnostics;
using System.Management.Automation;
using FlickrNet;
using FlickrNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Open, "FlickrAuthorizeUrl")]
    public sealed class OpenFlickrAuthorizeUrl : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Frob { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var client = new FlickrNet.Flickr(
                    FlickrConfig.Config.ApiKey,
                    FlickrConfig.Config.SharedSecret);

                var authorizeUrl = client.AuthCalcUrl(this.Frob, FlickrNet.AuthLevel.Write);
                Process.Start(authorizeUrl);

                this.WriteObject(new
                    {
                        Frob = this.Frob,
                        AuthorizeUrl = authorizeUrl,
                    });
            }
            catch (FlickrException failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    FlickrConfig.Config));
            }
            catch (Exception failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    FlickrConfig.Config));
            }
        }
    }
}