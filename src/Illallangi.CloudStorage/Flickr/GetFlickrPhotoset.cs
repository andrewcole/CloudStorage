using System;
using System.Collections.Generic;
using System.Management.Automation;
using FlickrNet;

namespace Illallangi.CloudStorage.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrPhotoset")]
    public sealed class GetFlickrPhotoset : FlickrPsCmdlet
    {
        #region Fields

        private const string PhotosetsGetList = "PhotosetsGetList";

        private const string PhotosGetAllContexts = "PhotosGetAllContexts";

        #endregion

        #region Properties

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetFlickrPhotoset.PhotosetsGetList)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string UserId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetFlickrPhotoset.PhotosGetAllContexts)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string PhotoId { get; set; }

        #endregion

        #region Methods

        protected override IEnumerable<object> Process()
        {
            switch (this.ParameterSetName)
            {
                case GetFlickrPhotoset.PhotosGetAllContexts:
                    foreach (var set in this.Client.PhotosGetAllContexts(this.PhotoId).Sets)
                    {
                        yield return this.Client.PhotosetsGetInfo(set.PhotosetId);
                    }

                    break;
                case GetFlickrPhotoset.PhotosetsGetList:
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

                    break;
                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        #endregion
    }
}