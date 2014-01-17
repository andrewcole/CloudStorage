using System.Management.Automation;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Get, "DropBoxConfig")]
    public sealed class GetDropBoxConfig : NinjectCmdlet<DropBoxModule>
    {
        protected override void BeginProcessing()
        {
            this.WriteObject(new
                {
                    this.Get<IDropBoxConfig>().ApiKey,
                    this.Get<IDropBoxConfig>().AppSecret,
                });
        }
    }
}