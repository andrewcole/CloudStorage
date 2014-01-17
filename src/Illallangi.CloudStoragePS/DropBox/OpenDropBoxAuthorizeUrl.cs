using System;
using System.Diagnostics;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Open, "DropBoxAuthorizeUrl")]
    public sealed class OpenDropBoxAuthorizeUrl : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string UserToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string UserSecret { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var client = new DropNetClient(
                    DropBoxConfig.Config.ApiKey,
                    DropBoxConfig.Config.AppSecret,
                    this.UserToken,
                    this.UserSecret);

                var authorizeUrl = client.BuildAuthorizeUrl();
                Process.Start(authorizeUrl);

                this.WriteObject(new
                    {
                        this.UserToken,
                        this.UserSecret,
                        AuthorizeUrl = authorizeUrl,
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
        }
    }
}