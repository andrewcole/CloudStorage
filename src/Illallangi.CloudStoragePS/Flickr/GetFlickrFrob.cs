using System;
using System.Management.Automation;
using FlickrNet;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrFrob")]
    public sealed class GetFlickrFrob : NinjectCmdlet<FlickrModule>
    {
        protected override void ProcessRecord()
        {
            try
            {
                var client = new FlickrNet.Flickr(
                    this.Get<IFlickrConfig>().ApiKey,
                    this.Get<IFlickrConfig>().SharedSecret);

                var frob = client.AuthGetFrob();

                this.WriteObject(new
                    {
                        Frob = frob,
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