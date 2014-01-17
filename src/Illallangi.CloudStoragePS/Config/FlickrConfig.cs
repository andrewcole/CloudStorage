using System.Configuration;

namespace Illallangi.CloudStoragePS.Config
{
    public sealed class FlickrConfig : ConfigurationSection
    {
        private static string staticPath;
        private static Configuration staticExeConfig;
        private static FlickrConfig staticConfig;

        public static FlickrConfig Config
        {
            get
            {
                return FlickrConfig.staticConfig ??
                    (FlickrConfig.staticConfig = (FlickrConfig)FlickrConfig.ExeConfig.GetSection("FlickrConfig"));
            }
        }

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

        [ConfigurationProperty("TokenCache", DefaultValue = "%localappdata%\\Illallangi Enterprises\\CloudStoragePS\\FlickrTokens.json")]
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

        private static Configuration ExeConfig
        {
            get
            {
                return FlickrConfig.staticExeConfig ??
                    (FlickrConfig.staticExeConfig = ConfigurationManager.OpenExeConfiguration(FlickrConfig.Path));
            }
        }

        private static string Path
        {
            get
            {
                return FlickrConfig.staticPath ??
                    (FlickrConfig.staticPath = System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
    }
}