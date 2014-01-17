namespace Illallangi.CloudStoragePS.Flickr
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Net;

    using FlickrNet;

    [Cmdlet(VerbsCommon.Get, "Photo")]
    public sealed class GetFlickrPhoto : FlickrPSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
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