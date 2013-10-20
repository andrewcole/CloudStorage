using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using FlickrNet;
using FlickrNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.PowerShell
{
    [Cmdlet(VerbsCommon.Get, "FlickrAccessToken", DefaultParameterSetName = "Cache")]
    public sealed class GetFlickrAccessToken : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "API")]
        public string Frob { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "API")]
        public string AuthorizeUrl { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Cache")]
        public string UserName { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "API":
                    try
                    {
                        var client = new FlickrNet.Flickr(
                            FlickrConfig.Config.ApiKey,
                            FlickrConfig.Config.SharedSecret);

                        var authToken = client.AuthGetToken(this.Frob);
                        
                        this.WriteObject(new
                        {
                            AccessToken = authToken.Token,
                            UserName = authToken.User.UserName,
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
                    break;
                case "Cache":
                    this.WriteObject(
                            FlickrTokenCache
                                .FromFile()
                                .Where(token => string.IsNullOrWhiteSpace(this.UserName) || token.UserName.Equals(this.UserName)));
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }
    }
}