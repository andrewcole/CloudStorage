using System.Configuration;

namespace Illallangi.CloudStorage.Config
{
    public interface IFlickrConfig
    {
        string ApiKey { get; }

        string SharedSecret { get; }

        string TokenCache { get; }

        string WaitPrompt { get; }
    }

    public sealed class FlickrConfig : ConfigurationSection, IFlickrConfig
    {
        [ConfigurationProperty("ApiKey", IsRequired = true)]
        public string ApiKey
        {
            get { return (string)this["ApiKey"]; }
            set { this["ApiKey"] = value; }
        }

        [ConfigurationProperty("SharedSecret", IsRequired = true)]
        public string SharedSecret
        {
            get { return (string)this["SharedSecret"]; }
            set { this["SharedSecret"] = value; }
        }

        [ConfigurationProperty("TokenCache", DefaultValue = "%localappdata%\\Illallangi Enterprises\\CloudStorage\\FlickrTokens.json")]
        public string TokenCache
        {
            get { return (string)this["TokenCache"]; }
            set { this["TokenCache"] = value; }
        }

        [ConfigurationProperty("WaitPrompt", DefaultValue = "Press any key to continue...")]
        public string WaitPrompt
        {
            get { return (string)this["WaitPrompt"]; }
            set { this["WaitPrompt"] = value; }
        }
    }
}