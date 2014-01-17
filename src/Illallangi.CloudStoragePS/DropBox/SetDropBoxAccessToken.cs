using System.Management.Automation;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
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
                    DropBoxTokenCache
                        .FromFile()
                        .AddToken(this.EMail, this.AccessToken, this.AccessSecret));
        }
    }
}