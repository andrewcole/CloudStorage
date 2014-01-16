﻿using System;
using System.Linq;
using System.Management.Automation;

using FlickrNet;

using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.PowerShell
{
    [Cmdlet(VerbsCommon.Get, "FlickrAccessToken", DefaultParameterSetName = GetFlickrAccessToken.Cache)]
    public sealed class GetFlickrAccessToken : PSCmdlet
    {
        private const string Cache = "Cache";

        private const string API = "API";

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetFlickrAccessToken.API)]
        public string Frob { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetFlickrAccessToken.API)]
        public string AuthorizeUrl { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetFlickrAccessToken.Cache)]
        public string UserName { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case GetFlickrAccessToken.API:
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
                case GetFlickrAccessToken.Cache:
                    this.WriteObject(
                            FlickrTokenCache
                                .FromFile()
                                .Where(token => string.IsNullOrWhiteSpace(this.UserName) || token.UserName.Equals(this.UserName)));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}