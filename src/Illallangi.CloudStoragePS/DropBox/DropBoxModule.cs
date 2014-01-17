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

namespace Illallangi.CloudStoragePS.DropBox
{
    public class DropBoxModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IDropBoxConfig>()
                .ToMethod(
                    cx =>
                    (DropBoxConfig)
                    ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location)
                        .GetSection("DropBoxConfig"))
                .InSingletonScope();

            this.Bind<ICollection<DropBoxToken>>()
                .ToMethod(
                    cx =>
                        {
                            var fileName = Environment.ExpandEnvironmentVariables(cx.Kernel.Get<IDropBoxConfig>().TokenCache);
                            var cache = File.Exists(fileName) ?
                                JsonConvert.DeserializeObject<ObservableCollection<DropBoxToken>>(File.ReadAllText(fileName)) :
                                new ObservableCollection<DropBoxToken>();
                            cache.CollectionChanged += (sender, args) => File.WriteAllText(fileName, JsonConvert.SerializeObject(sender, Formatting.Indented));
                            return cache;
                        })
                .InSingletonScope();
        }
    }
}