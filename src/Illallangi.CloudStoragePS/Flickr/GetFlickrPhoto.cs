using System.Collections.Generic;
using System.Management.Automation;
using FlickrNet;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrPhoto")]
    public sealed class GetFlickrPhoto : FlickrPsCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string PhotosetId { get; set; }

        protected override IEnumerable<object> Process()
        {
            PhotosetPhotoCollection collection = null;
            do
            {
                collection = this.Client.PhotosetsGetPhotos(this.PhotosetId, PhotoSearchExtras.All, null == collection ? 0 : collection.Page + 1, 10);
                foreach (var photo in collection)
                {
                    yield return photo;
                }
            }
            while (collection.Page < collection.Pages);
        }
    }
}