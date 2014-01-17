using System;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Get, "DropBoxRequestToken")]
    public sealed class GetDropBoxRequestToken : NinjectCmdlet<DropBoxModule>
    {
        protected override void ProcessRecord()
        {
            try
            {
                var client = new DropNetClient(
                    this.Get<IDropBoxConfig>().ApiKey,
                    this.Get<IDropBoxConfig>().AppSecret);

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
                    this.Get<IDropBoxConfig>()));
            }
            catch (Exception failure)
            {
                this.WriteError(new ErrorRecord(
                    failure,
                    failure.Message,
                    ErrorCategory.InvalidResult,
                    this.Get<IDropBoxConfig>()));
            }
        }
    }
}