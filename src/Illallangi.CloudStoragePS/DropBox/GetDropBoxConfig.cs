using System.Management.Automation;
using DropNet;
using DropNet.Models;
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
                    ApiKey = DropBoxConfig.Config.ApiKey,
                    AppSecret = DropBoxConfig.Config.AppSecret,
                });
        }
    }
}