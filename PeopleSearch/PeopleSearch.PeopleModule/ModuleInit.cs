using PeopleSearch.Infrastructure;
using PeopleSearch.PeopleModule.Views;
using PeopleSearch.PeopleModule.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.IO;
using System.Reflection;

namespace PeopleSearch.PeopleModule
{
    public class ModuleInit : IModule
    {
        private static string _imagePath;
        public static string ImagePath
        {
            get
            {
                var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var rootPath = assemblyPath.Replace(@"\PeopleSearch\PeopleSearch.Shell\bin\Debug", "");
                _imagePath = Path.Combine(rootPath, @"Images");
                return _imagePath;
            }
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(PeopleModule.Views.PeopleSearchView));
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(PeopleMaintView));
            regionManager.RegisterViewWithRegion(RegionNames.ModuleSelectorRegion, typeof(PeopleSearchModuleSelectorView));
            regionManager.RegisterViewWithRegion(RegionNames.ModuleMaintenanceRegion, typeof(PeopleMaintModuleSelectorView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PeopleSearchView, PeopleSearchViewModel>("PeopleSearchView");
            containerRegistry.RegisterForNavigation<PeopleMaintView, PeopleMaintViewModel>("PeopleMaintView");
        }
    }
}