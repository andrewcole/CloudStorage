//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Management.Automation;
//using FlickrNet;
//using FlickrNet.Exceptions;
//using Illallangi.CloudStoragePS.Config;

//namespace Illallangi.CloudStoragePS.Flickr
//{
//    [Cmdlet("Get", "FlickrAbstractClass", DefaultParameterSetName="EMail")]
//    public abstract class FlickrPSCmdlet : PSCmdlet
//    {
//        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName="EMail")]
//        public string EMail { get; set; }

//        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Value")]
//        public string AccessToken { get; set; }

//        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Value")]
//        public string AccessSecret { get; set; }

//        protected override void ProcessRecord()
//        {
//            if (this.ParameterSetName.Equals("EMail"))
//            {
//                try
//                {
//                    var token = TokenCache.FromFile().Single(t => t.EMail.Equals(this.EMail));
//                    this.AccessToken = token.AccessToken;
//                    this.AccessSecret = token.AccessSecret;
//                }
//                catch (Exception failure)
//                {
//                    this.WriteError(new ErrorRecord(
//                        failure,
//                        failure.Message,
//                        ErrorCategory.InvalidResult,
//                        FlickrConfig.Config));
//                    return;
//                }
//            }
//            try
//            {
//                var client = new DropNetClient(
//                    FlickrConfig.Config.ApiKey,
//                    FlickrConfig.Config.AppSecret,
//                    this.AccessToken,
//                    this.AccessSecret);

//                this.WriteObject(this.Process(client), true);
//            }
//            catch (FlickrException failure)
//            {
//                this.WriteError(new ErrorRecord(
//                    failure,
//                    failure.Response.Content,
//                    ErrorCategory.InvalidResult,
//                    FlickrConfig.Config));
//                return;
//            }
//            catch (Exception failure)
//            {
//                this.WriteError(new ErrorRecord(
//                    failure,
//                    failure.Message,
//                    ErrorCategory.InvalidResult,
//                    FlickrConfig.Config));
//                return;
//            }
//        }

//        protected abstract IEnumerable<Object> Process(DropNetClient client);
//    }
//}