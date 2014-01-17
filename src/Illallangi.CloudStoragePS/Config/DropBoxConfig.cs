using System.Configuration;

namespace Illallangi.CloudStoragePS.Config
{
    public sealed class DropBoxConfig : ConfigurationSection
    {
        private static string staticPath;
        private static Configuration staticExeConfig;
        private static DropBoxConfig staticConfig;

        public static DropBoxConfig Config
        {
            get
            {
                return DropBoxConfig.staticConfig ??
                    (DropBoxConfig.staticConfig = (DropBoxConfig)DropBoxConfig.ExeConfig.GetSection("DropBoxConfig"));
            }
        }

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

        [ConfigurationProperty("WaitPrompt", DefaultValue = "Press any key to continue...")]
        public string WaitPrompt
        {
            get { return (string)this["WaitPrompt"]; }
            set { this["WaitPrompt"] = value; }
        }
        
        private static string Path
        {
            get
            {
                return DropBoxConfig.staticPath ??
                    (DropBoxConfig.staticPath = System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }

        private static Configuration ExeConfig
        {
            get
            {
                return DropBoxConfig.staticExeConfig ??
                    (DropBoxConfig.staticExeConfig = ConfigurationManager.OpenExeConfiguration(DropBoxConfig.Path));
            }
        }
    }
}