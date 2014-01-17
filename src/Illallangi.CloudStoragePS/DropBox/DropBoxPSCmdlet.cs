using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet("Get", "DropBoxAbstractClass", DefaultParameterSetName = "EMail")]
    public abstract class DropBoxPsCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "EMail")]
        public string EMail { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Value")]
        public string AccessToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Value")]
        public string AccessSecret { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName.Equals("EMail"))
            {
                try
                {
                    var token = DropBoxTokenCache.FromFile().Single(t => t.EMail.Equals(this.EMail));
                    this.AccessToken = token.AccessToken;
                    this.AccessSecret = token.AccessSecret;
                }
                catch (Exception failure)
                {
                    this.WriteError(new ErrorRecord(
                        failure,
                        failure.Message,
                        ErrorCategory.InvalidResult,
                        DropBoxConfig.Config));
                    return;
                }
            }

            try
            {
                var client = new DropNetClient(
                    DropBoxConfig.Config.ApiKey,
                    DropBoxConfig.Config.AppSecret,
                    this.AccessToken,
                    this.AccessSecret);

                this.WriteObject(this.Process(client), true);
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

        protected abstract IEnumerable<object> Process(DropNetClient client);
    }
}