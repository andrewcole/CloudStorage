using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStorage.Config;

namespace Illallangi.CloudStorage.DropBox
{
    [Cmdlet("Get", "DropBoxAbstractClass", DefaultParameterSetName = "EMail")]
    public abstract class DropBoxPsCmdlet : NinjectCmdlet<DropBoxModule>
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "EMail")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string EMail { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Value")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string AccessToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Value")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string AccessSecret { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName.Equals("EMail"))
            {
                try
                {
                    var token = this.Get<ICollection<DropBoxToken>>().Single(t => t.EMail.Equals(this.EMail));
                    this.AccessToken = token.AccessToken;
                    this.AccessSecret = token.AccessSecret;
                }
                catch (Exception failure)
                {
                    this.WriteError(new ErrorRecord(
                        failure,
                        failure.Message,
                        ErrorCategory.InvalidResult,
                        this.Get<IDropBoxConfig>()));
                    return;
                }
            }

            try
            {
                var client = new DropNetClient(
                    this.Get<IDropBoxConfig>().ApiKey,
                    this.Get<IDropBoxConfig>().AppSecret,
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

        protected abstract IEnumerable<object> Process(DropNetClient client);
    }
}