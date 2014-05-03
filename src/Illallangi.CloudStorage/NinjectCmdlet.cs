using System.Management.Automation;
using Ninject;
using Ninject.Extensions.Logging.Log4net;
using Ninject.Modules;

namespace Illallangi.CloudStorage
{
    [Cmdlet(VerbsCommon.Get, NinjectCmdlet<TModule>.Noun)]
    public abstract class NinjectCmdlet<TModule> : PSCmdlet where TModule : INinjectModule, new()
    {
        #region Fields

        private const string Noun = @"Null";

        private INinjectModule currentNinjectModule;

        private INinjectModule currentLog4NetModule;

        private StandardKernel currentKernel;

        #endregion

        #region Properties

        private INinjectModule NinjectModule
        {
            get
            {
                return this.currentNinjectModule ?? (this.currentNinjectModule = NinjectCmdlet<TModule>.GetNinjectModule());
            }
        }

        private INinjectModule Log4NetModule
        {
            get
            {
                return this.currentLog4NetModule ?? (this.currentLog4NetModule = NinjectCmdlet<TModule>.GetLog4NetModule());
            }
        }

        private StandardKernel Kernel
        {
            get
            {
                return this.currentKernel ?? (this.currentKernel = this.GetKernel());
            }
        }

        #endregion

        #region Methods

        #region Protected Methods

        protected T Get<T>()
        {
            return this.Kernel.Get<T>();
        }

        #endregion

        #region Private Methods

        private static INinjectModule GetLog4NetModule()
        {
            return new Log4NetModule();
        }

        private static INinjectModule GetNinjectModule()
        {
            return new TModule();
        }

        private StandardKernel GetKernel()
        {
            return new StandardKernel(this.NinjectModule, this.Log4NetModule);
        }

        #endregion

        #endregion
    }
}