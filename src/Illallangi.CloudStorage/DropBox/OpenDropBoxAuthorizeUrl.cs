using System;
using System.Diagnostics;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStorage.Config;

namespace Illallangi.CloudStorage.DropBox
{
    [Cmdlet(VerbsCommon.Open, "DropBoxAuthorizeUrl")]
    public sealed class OpenDropBoxAuthorizeUrl : NinjectCmdlet<DropBoxModule>
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string UserToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string UserSecret { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var client = new DropNetClient(
                    this.Get<IDropBoxConfig>().ApiKey,
                    this.Get<IDropBoxConfig>().AppSecret,
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