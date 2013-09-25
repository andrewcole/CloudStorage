using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.PowerShell
{
    [Cmdlet(VerbsCommon.Get, "DropBoxAccessToken", DefaultParameterSetName = "Cache")]
    public sealed class GetDropBoxAccessToken : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName="API")]
        public string UserToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "API")]
        public string UserSecret { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "API")]
        public string AuthorizeUrl { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Cache")]
        public string Account { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "API":
                    try
                    {
                        var client = new DropNetClient(
                            DropBoxConfig.Config.ApiKey,
                            DropBoxConfig.Config.AppSecret,
                            this.UserToken,
                            this.UserSecret);

                        var accessToken = client.GetAccessToken();
                        var accountInfo = client.AccountInfo();

                        this.WriteObject(new
                        {
                            AccessToken = accessToken.Token,
                            AccessSecret = accessToken.Secret,
                            EMail = accountInfo.email,
                        });
                    }
                    catch (DropboxException failure)
                    {
                        this.WriteError(new ErrorRecord(
                            failure,
                            failure.Response.Content,
                            ErrorCategory.InvalidResult,
                            DropBoxConfig.Config));
                    }
                    catch (Exception failure)
                    {
                        this.WriteError(new ErrorRecord(
                            failure,
                            failure.Message,
                            ErrorCategory.InvalidResult,
                            DropBoxConfig.Config));
                    }
                    break;
                case "Cache":
                    this.WriteObject(
                            TokenCache
                                .FromFile()
                                .Where(token => string.IsNullOrWhiteSpace(this.Account) || token.EMail.Equals(this.Account)));
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            
        }
    }
}