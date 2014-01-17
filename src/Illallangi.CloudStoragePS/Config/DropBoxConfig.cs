using System.Configuration;

namespace Illallangi.CloudStoragePS.Config
{
    public interface IDropBoxConfig
    {
        string ApiKey { get; }

        string AppSecret { get; }

        string TokenCache { get; }
    }

    public sealed class DropBoxConfig : ConfigurationSection, IDropBoxConfig
    {
        [ConfigurationProperty("ApiKey", IsRequired = true)]
        public string ApiKey
        {
            get { return (string)this["ApiKey"]; }
            set { this["ApiKey"] = value; }
        }

        [ConfigurationProperty("AppSecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["AppSecret"]; }
            set { this["AppSecret"] = value; }
        }

        [ConfigurationProperty("TokenCache", DefaultValue = "%localappdata%\\Illallangi Enterprises\\CloudStoragePS\\DropBoxTokens.json")]
        public string TokenCache
        {
            get { return (string)this["TokenCache"]; }
            set { this["TokenCache"] = value; }
        }
    }
}