﻿using Company.Application.Initialize.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Initialize
{
    [Module(ModuleName = ModuleNames.ApplicationInitializeModule, OnDemand = true)]
    public class ApplicationInitializeModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterForNavigation<InitializeView>();
        }
    }
}
