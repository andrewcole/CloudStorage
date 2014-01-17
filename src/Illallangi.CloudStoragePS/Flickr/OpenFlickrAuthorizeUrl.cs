using System;
using System.Diagnostics;
using System.Management.Automation;
using FlickrNet;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Open, "FlickrAuthorizeUrl")]
    public sealed class OpenFlickrAuthorizeUrl : NinjectCmdlet<FlickrModule>
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string Frob { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var client = new FlickrNet.Flickr(
                    this.Get<IFlickrConfig>().ApiKey,
                    this.Get<IFlickrConfig>().SharedSecret);

                var authorizeUrl = client.AuthCalcUrl(this.Frob, FlickrNet.AuthLevel.Write);
                Process.Start(authorizeUrl);

                this.WriteObject(new
                    {
                        this.Frob,
                        AuthorizeUrl = authorizeUrl,
                    });
            }
            catch (FlickrException failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    this.Get<IFlickrConfig>()));
            }
            catch (Exception failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    this.Get<IFlickrConfig>()));
            }
        }
    }
}