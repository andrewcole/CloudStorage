using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Illallangi.CloudStorage.Flickr
{
    [Cmdlet(VerbsCommon.Get, "FlickrPeopleInfo", DefaultParameterSetName = GetFlickrPeopleInfo.FindByUserName)]
    public sealed class GetFlickrPeopleInfo : FlickrPsCmdlet
    {
        #region Fields

        private const string FindByUserName = "FindByUserName";

        private const string FindByEmail = "FindByEmail";

        private const string FindByUrl = "FindByUrl";

        private const string FindById = "FindById";

        private string currentUserId;

        #endregion

        #region Properties

        [Parameter(Mandatory = false, ParameterSetName = GetFlickrPeopleInfo.FindByUserName, Position = 1)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string UserName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = GetFlickrPeopleInfo.FindByEmail)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string Email { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = GetFlickrPeopleInfo.FindByUrl)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string Url { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = GetFlickrPeopleInfo.FindById)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string UserId
        {
            get
            {
                switch (this.ParameterSetName)
                {
                    case GetFlickrPeopleInfo.FindByUserName:
                        return string.IsNullOrWhiteSpace(this.UserName) ? 
                            this.Client.TestLogin().UserId : 
                            this.Client.PeopleFindByUserName(this.UserName).UserId;
                    case GetFlickrPeopleInfo.FindByEmail:
                        return this.Client.PeopleFindByEmail(this.Email).UserId;
                    case GetFlickrPeopleInfo.FindByUrl:
                        return this.Client.UrlsLookupUser(this.Url).UserId;
                    case GetFlickrPeopleInfo.FindById:
                        return this.currentUserId;
                    default:
                        throw new NotImplementedException(this.ParameterSetName);
                }
            }

            set
            {
                this.currentUserId = value;
            }
        }

        #endregion

        #region Methods

        protected override IEnumerable<object> Process()
        {
            yield return this.Client.PeopleGetInfo(this.UserId);
        }

        #endregion
    }
}