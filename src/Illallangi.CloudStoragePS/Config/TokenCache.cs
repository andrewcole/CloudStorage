using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Illallangi.CloudStoragePS.Config
{
    public sealed class TokenCache : List<Token>
    {
        public IEnumerable<Token> AddToken(string eMail, string accessToken, string accessSecret)
        {
            if (1 == this.Count(token => token.EMail.Equals(eMail)))
            {
                this.Single(token => token.EMail.Equals(eMail)).AccessToken = accessToken;
                this.Single(token => token.EMail.Equals(eMail)).AccessSecret = accessSecret;
            }
            else
            {
                this.Add(new Token { EMail = eMail, AccessToken = accessToken, AccessSecret = accessSecret });
            }
            
            yield return this.ToFile().Single(token => token.EMail.Equals(eMail));
        }

        public TokenCache ToFile()
        {
            var fileName = Environment.ExpandEnvironmentVariables(DropBoxConfig.Config.TokenCache);
            
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
            
            return this;
        }

        public static TokenCache FromFile()
        {
            var fileName = Environment.ExpandEnvironmentVariables(DropBoxConfig.Config.TokenCache);

            return File.Exists(fileName) ?
                JsonConvert.DeserializeObject<TokenCache>(File.ReadAllText(fileName)) :
                new TokenCache().ToFile();
        }
    }
}
