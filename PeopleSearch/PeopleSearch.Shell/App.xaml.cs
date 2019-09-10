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
