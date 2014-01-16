using System.Collections.Generic;
using System.Management.Automation;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrPeopleInfo")]
    public sealed class GetFlickrPeopleInfo : FlickrPSCmdlet
    {
        protected override IEnumerable<object> Process(FlickrNet.Flickr client)
        {
            yield return client.PeopleGetInfo(client.TestLogin().UserId);
        }
    }
}