using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Set, "FlickrAccessToken")]
    public sealed class SetFlickrAccessToken : NinjectCmdlet<FlickrModule>
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string AccessToken { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string UserName { get; set; }

        protected override void ProcessRecord()
        {
            var flickrTokenCache = this.Get<ICollection<FlickrToken>>();

            if (1 == flickrTokenCache.Count(token => token.UserName.Equals(this.UserName)))
            {
                flickrTokenCache.Single(token => token.UserName.Equals(this.UserName)).AccessToken = this.AccessToken;
            }
            else
            {
                flickrTokenCache.Add(new FlickrToken { UserName = this.UserName, AccessToken = this.AccessToken });
            }

            this.WriteObject(flickrTokenCache.Single(token => token.UserName.Equals(this.UserName)));
        }
    }
}