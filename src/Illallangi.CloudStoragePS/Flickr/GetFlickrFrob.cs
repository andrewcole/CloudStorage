using System;
using System.Management.Automation;
using FlickrNet;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrFrob")]
    public sealed class GetFlickrFrob : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            try
            {
                var client = new FlickrNet.Flickr(
                    FlickrConfig.Config.ApiKey,
                    FlickrConfig.Config.SharedSecret);

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