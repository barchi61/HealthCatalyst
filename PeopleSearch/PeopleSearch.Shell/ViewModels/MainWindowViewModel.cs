using PeopleSearch.Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;

namespace PeopleSearch.Shell.ViewModels
{
    [Export]
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand ModuleCommand { get; private set; }

        private Visibility _moduleSelectorVisibilty = Visibility.Hidden;
        public Visibility ModuleSelectorVisibilty
        {
            get { return _moduleSelectorVisibilty; }
            set { SetProperty(ref this._moduleSelectorVisibilty, value); }
        }

        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        private bool _canAppExit = true;
        public bool CanAppExit
        {
            get { return _canAppExit; }
            set { SetProperty(ref _canAppExit, value); }
        }

        private bool _canShowModules = true;
        public bool CanShowModules
        {
            get { return _canShowModules; }
            set { SetProperty(ref _canShowModules, value); }
        }

        public MainWindowViewModel(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;

            ExitCommand = new DelegateCommand(AppExit).ObservesCanExecute(() => CanAppExit);
            ModuleCommand = new DelegateCommand(ShowModules).ObservesCanExecute(() => CanShowModules);

            _applicationCommands.GetSettingCommand.RegisterCommand(ExitCommand);
            _applicationCommands.GetSettingCommand.RegisterCommand(ModuleCommand);
        }

        private void ShowModules()
        {
            ModuleSelectorVisibilty = Visibility.Visible;
        }

        private void AppExit()
        {
            App.Current.Shutdown();
        }

    }
}
