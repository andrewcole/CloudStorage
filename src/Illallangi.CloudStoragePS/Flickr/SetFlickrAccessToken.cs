using System.Management.Automation;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Set, "FlickrAccessToken")]
    public sealed class SetFlickrAccessToken : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string AccessToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string UserName { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(
                    FlickrTokenCache
                        .FromFile()
                        .AddToken(this.UserName, this.AccessToken));
        }
    }
}