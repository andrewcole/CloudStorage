using System.Management.Automation;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Get, "DropBoxConfig")]
    public sealed class GetDropBoxConfig : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            this.WriteObject(new
                {
                    DropBoxConfig.Config.ApiKey,
                    DropBoxConfig.Config.AppSecret,
                });
        }
    }
}