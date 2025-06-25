using Company.Core.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Ioc
{
    [ExposedService(Lifetime.Singleton, true)]
    public sealed class PrismProvider
    {
        public static ILanguageManager? LanguageManager { get; private set; }

        public static IContainerExtension? Container { get; private set; }

        public static IRegionManager? RegionManager { get; private set; }

        public static IDialogService? DialogService { get; private set; }

        public static IEventAggregator? EventAggregator { get; private set; }

        public static IModuleManager? ModuleManager { get; private set; }

        public PrismProvider(ILanguageManager languageManager, IContainerExtension container, IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator, IModuleManager moduleManager)
        {
            LanguageManager = languageManager;
            Container = container;
            RegionManager = regionManager;
            DialogService = dialogService;
            EventAggregator = eventAggregator;
            ModuleManager = moduleManager;
        }
    }
}
