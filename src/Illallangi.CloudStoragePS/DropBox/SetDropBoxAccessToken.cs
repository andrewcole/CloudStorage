using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Set, "DropBoxAccessToken")]
    public sealed class SetDropBoxAccessToken : NinjectCmdlet<DropBoxModule>
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string AccessToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string AccessSecret { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string EMail { get; set; }

        protected override void ProcessRecord()
        {
            var dropBoxTokenCache = this.Get<ICollection<DropBoxToken>>();
            
            if (1 == dropBoxTokenCache.Count(token => token.EMail.Equals(this.EMail)))
            {
                dropBoxTokenCache.Single(token => token.EMail.Equals(this.EMail)).AccessToken = this.AccessToken;
                dropBoxTokenCache.Single(token => token.EMail.Equals(this.EMail)).AccessSecret = this.AccessSecret;
            }
            else
            {
                dropBoxTokenCache.Add(new DropBoxToken { EMail = this.EMail, AccessToken = this.AccessToken, AccessSecret = this.AccessSecret });
            }

            this.WriteObject(dropBoxTokenCache.Single(token => token.EMail.Equals(this.EMail)));
        }
    }
}