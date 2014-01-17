using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Illallangi.CloudStoragePS.Config
{
    public sealed class DropBoxTokenCache : List<DropBoxToken>
    {
        public static DropBoxTokenCache FromFile()
        {
            var fileName = Environment.ExpandEnvironmentVariables(DropBoxConfig.Config.TokenCache);

            return File.Exists(fileName) ?
                JsonConvert.DeserializeObject<DropBoxTokenCache>(File.ReadAllText(fileName)) :
                new DropBoxTokenCache().ToFile();
        }

        public IEnumerable<DropBoxToken> AddToken(string email, string accessToken, string accessSecret)
        {
            if (1 == this.Count(token => token.EMail.Equals(email)))
            {
                this.Single(token => token.EMail.Equals(email)).AccessToken = accessToken;
                this.Single(token => token.EMail.Equals(email)).AccessSecret = accessSecret;
            }
            else
            {
                this.Add(new DropBoxToken { EMail = email, AccessToken = accessToken, AccessSecret = accessSecret });
            }
            
            yield return this.ToFile().Single(token => token.EMail.Equals(email));
        }

        private DropBoxTokenCache ToFile()
        {
            var fileName = Environment.ExpandEnvironmentVariables(DropBoxConfig.Config.TokenCache);
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
