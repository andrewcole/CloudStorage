namespace Illallangi.CloudStoragePS.Flickr
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Net;

    [Cmdlet(VerbsData.Backup, "Photo")]
    public sealed class BackupFlickrPhoto : FlickrPSCmdlet
    {
        #region Fields

        private PhotoType currentType = PhotoType.Original;

        private const string PhotoObject = @"Photo";

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

        #endregion

        #region Methods

        protected override IEnumerable<object> Process()
        {
            switch (this.ParameterSetName)
            {
                case BackupFlickrPhoto.PhotoObject:
                    new WebClient().DownloadFile(this.Url, this.Path);
                    yield break;
                    break;
                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private string Url
        {
            get
            {
                switch (this.Type)
                {
                    case PhotoType.Original:
                        return this.Photo.OriginalUrl;
                        break;
                    case PhotoType.Thumbnail:
                        return this.Photo.ThumbnailUrl;
                        break;
                    case PhotoType.SquareThumbnail:
                        return this.Photo.SquareThumbnailUrl;
                        break;
                    case PhotoType.LargeSquareThumbnail:
                        return this.Photo.LargeSquareThumbnailUrl;
                        break;
                    case PhotoType.Large:
                        return this.Photo.LargeUrl;
                        break;
                    case PhotoType.Large1600:
                        return this.Photo.Large1600Url;
                        break;
                    case PhotoType.Large2048:
                        return this.Photo.Large2048Url;
                        break;
                    case PhotoType.Medium:
                        return this.Photo.MediumUrl;
                        break;
                    case PhotoType.Medium640:
                        return this.Photo.Medium640Url;
                        break;
                    case PhotoType.Medium800:
                        return this.Photo.Medium800Url;
                        break;
                    case PhotoType.Small:
                        return this.Photo.SmallUrl;
                        break;
                    case PhotoType.Small320:
                        return this.Photo.Small320Url;
                    default:
                        throw new NotImplementedException(this.Type.ToString());
                }
            }
        }

        #endregion
    }
}