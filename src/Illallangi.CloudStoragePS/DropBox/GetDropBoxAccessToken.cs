using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Get, "DropBoxAccessToken", DefaultParameterSetName = GetDropBoxAccessToken.Cache)]
    public sealed class GetDropBoxAccessToken : NinjectCmdlet<DropBoxModule>
    {
        private const string Cache = "Cache";

        private const string Api = "Api";

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetDropBoxAccessToken.Api)]
        public string UserToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetDropBoxAccessToken.Api)]
        public string UserSecret { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetDropBoxAccessToken.Api)]
        public string AuthorizeUrl { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetDropBoxAccessToken.Cache)]
        public string Account { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case GetDropBoxAccessToken.Api:
                        var client = new DropNetClient(
                            this.Get<IDropBoxConfig>().ApiKey,
                            this.Get<IDropBoxConfig>().AppSecret,
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

                        break;
                    case GetDropBoxAccessToken.Cache:
                        this.WriteObject(
                                this.Get<ICollection<DropBoxToken>>()
                                    .Where(token => string.IsNullOrWhiteSpace(this.Account) || token.EMail.Equals(this.Account)));
                        break;
                    default:
                        throw new NotImplementedException();
                }
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