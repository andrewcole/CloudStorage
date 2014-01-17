using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Illallangi.CloudStoragePS.Config
{
    public sealed class FlickrTokenCache : List<FlickrToken>
    {
        public static FlickrTokenCache FromFile()
        {
            var fileName = Environment.ExpandEnvironmentVariables(FlickrConfig.Config.TokenCache);

            return File.Exists(fileName) ?
                JsonConvert.DeserializeObject<FlickrTokenCache>(File.ReadAllText(fileName)) :
                new FlickrTokenCache().ToFile();
        }

        public IEnumerable<FlickrToken> AddToken(string userName, string accessToken)
        {
            if (1 == this.Count(token => token.UserName.Equals(userName)))
            {
                this.Single(token => token.UserName.Equals(userName)).AccessToken = accessToken;
            }
            else
            {
                this.Add(new FlickrToken { UserName = userName, AccessToken = accessToken });
            }
            
            yield return this.ToFile().Single(token => token.UserName.Equals(userName));
        }

        private FlickrTokenCache ToFile()
        {
            var fileName = Environment.ExpandEnvironmentVariables(FlickrConfig.Config.TokenCache);
            var directoryName = Path.GetDirectoryName(fileName);

            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
            
            return this;
        }
    }
}
