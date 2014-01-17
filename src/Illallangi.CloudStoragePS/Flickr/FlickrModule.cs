using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using Illallangi.CloudStoragePS.Config;
using Newtonsoft.Json;
using Ninject;
using Ninject.Modules;

namespace Illallangi.CloudStoragePS.Flickr
{
    public class FlickrModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFlickrConfig>()
                .ToMethod(
                    cx =>
                    (FlickrConfig)
                    ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location)
                        .GetSection("FlickrConfig"))
                .InSingletonScope();

            this.Bind<ICollection<FlickrToken>>()
                .ToMethod(
                    cx =>
                        {
                            var fileName = Environment.ExpandEnvironmentVariables(cx.Kernel.Get<IFlickrConfig>().TokenCache);
                            var cache = File.Exists(fileName) ?
                                JsonConvert.DeserializeObject<ObservableCollection<FlickrToken>>(File.ReadAllText(fileName)) :
                                new ObservableCollection<FlickrToken>();
                            cache.CollectionChanged += (sender, args) => File.WriteAllText(fileName, JsonConvert.SerializeObject(sender, Formatting.Indented));
                            return cache;
                        })
                .InSingletonScope();
        }
    }
}