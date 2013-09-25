using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.PowerShell
{
    [Cmdlet(VerbsCommon.Set, "DropBoxAccessToken")]
    public sealed class SetDropBoxAccessToken : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string AccessToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string AccessSecret { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string EMail { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(
                    TokenCache
                        .FromFile()
                        .AddToken(this.EMail, this.AccessToken, this.AccessSecret));
        }
    }
}