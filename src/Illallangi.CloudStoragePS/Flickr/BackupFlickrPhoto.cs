using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;

namespace Illallangi.CloudStoragePS.Flickr
{
    [Cmdlet(VerbsData.Backup, "Photo")]
    public sealed class BackupFlickrPhoto : FlickrPsCmdlet
    {
        #region Fields

        private const string PhotoObject = @"Photo";
        
        private PhotoType currentType = PhotoType.Original;

        #endregion

        #region Properties

        [Parameter(Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Mandatory = false)]
        public PhotoType Type
        {
            get
            {
                return this.currentType;
            }

            set
            {
                this.currentType = value;
            }
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = BackupFlickrPhoto.PhotoObject)]
        public FlickrNet.Photo Photo { get; set; }

        private string Url
        {
            get
            {
                switch (this.Type)
                {
                    case PhotoType.Original:
                        return this.Photo.OriginalUrl;
                    case PhotoType.Thumbnail:
                        return this.Photo.ThumbnailUrl;
                    case PhotoType.SquareThumbnail:
                        return this.Photo.SquareThumbnailUrl;
                    case PhotoType.LargeSquareThumbnail:
                        return this.Photo.LargeSquareThumbnailUrl;
                    case PhotoType.Large:
                        return this.Photo.LargeUrl;
                    case PhotoType.Large1600:
                        return this.Photo.Large1600Url;
                    case PhotoType.Large2048:
                        return this.Photo.Large2048Url;
                    case PhotoType.Medium:
                        return this.Photo.MediumUrl;
                    case PhotoType.Medium640:
                        return this.Photo.Medium640Url;
                    case PhotoType.Medium800:
                        return this.Photo.Medium800Url;
                    case PhotoType.Small:
                        return this.Photo.SmallUrl;
                    case PhotoType.Small320:
                        return this.Photo.Small320Url;
                    default:
                        throw new NotImplementedException(this.Type.ToString());
                }
            }
        }

        #endregion

        #region Methods

        protected override IEnumerable<object> Process()
        {
            switch (this.ParameterSetName)
            {
                case BackupFlickrPhoto.PhotoObject:
                    new WebClient().DownloadFile(this.Url, this.Path);
                    yield break;
                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        #endregion
    }
}