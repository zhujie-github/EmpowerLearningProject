using Company.Core.Language;
using System.Windows;
using System.Windows.Threading;

namespace Company.Core.Ioc
{
    [ExposedService]
    public sealed class PrismProvider
    {
        public static ILanguageManager LanguageManager { get; private set; } = null!;

        public static IContainerExtension Container { get; private set; } = null!;

        public static IRegionManager RegionManager { get; private set; } = null!;

        public static IDialogService DialogService { get; private set; } = null!;

        public static IEventAggregator EventAggregator { get; private set; } = null!;

        public static IModuleManager ModuleManager { get; private set; } = null!;

        public static Dispatcher Dispatcher { get; private set; } = null!;

        public PrismProvider(ILanguageManager languageManager, IContainerExtension container,
            IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator,
            IModuleManager moduleManager)
        {
            LanguageManager = languageManager;
            Container = container;
            RegionManager = regionManager;
            DialogService = dialogService;
            EventAggregator = eventAggregator;
            ModuleManager = moduleManager;
            Dispatcher = Application.Current.Dispatcher;
        }
    }
}
