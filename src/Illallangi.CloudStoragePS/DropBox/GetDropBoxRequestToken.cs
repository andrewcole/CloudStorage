using System;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Get, "DropBoxRequestToken")]
    public sealed class GetDropBoxRequestToken : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            try
            {
                var client = new DropNetClient(
                    DropBoxConfig.Config.ApiKey,
                    DropBoxConfig.Config.AppSecret);

                var userLogin = client.GetToken();

                this.WriteObject(new
                    {
                        UserToken = userLogin.Token,
                        UserSecret = userLogin.Secret,
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