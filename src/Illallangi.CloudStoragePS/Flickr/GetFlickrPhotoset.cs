using System.Collections.Generic;
using System.Management.Automation;

using FlickrNet;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrPhotoset")]
    public sealed class GetFlickrPhotoset : FlickrPSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string UserId { get; set; }

        protected override IEnumerable<object> Process()
        {
            PhotosetCollection collection = null;
            do
            {
                collection = this.Client.PhotosetsGetList(this.UserId, null == collection ? 0 : collection.Page + 1, 10);
                foreach (var set in collection)
                {
                    yield return set;
                }
            }
            while (collection.Page < collection.Pages);
        }
    }
}