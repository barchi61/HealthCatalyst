using PeopleSearch.Infrastructure.Interfaces;
using PeopleSearch.Infrastructure.Services;
using PeopleSearch.Shell.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.IO;
using System.Reflection;
using System.Windows;

namespace PeopleSearch.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var rootPath = assemblyPath.Replace(@"\PeopleSearch\PeopleSearch.Shell\bin\Debug", "");
            var dataPath = Path.Combine(rootPath, @"Data\People.db");
            var imagePath = Path.Combine(rootPath, @"Images\");

            ApplicationSettings.DataPath = dataPath;
            ApplicationSettings.ImagePath = imagePath;

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<PeopleModule.ModuleInit>();
        }
    }
}
