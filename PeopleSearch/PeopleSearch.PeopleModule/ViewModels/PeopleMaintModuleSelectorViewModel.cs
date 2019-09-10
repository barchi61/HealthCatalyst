using PeopleSearch.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace PeopleSearch.PeopleModule.ViewModels
{
    [Export]
    public class PeopleMaintModuleSelectorViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
		private IEventAggregator _eventAggregator;
		
		public ICommand ModuleExecuteCommand { get; private set; }

		[ImportingConstructor]
        public PeopleMaintModuleSelectorViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
		{
			_regionManager = regionManager;
			_eventAggregator = eventAggregator;

			ModuleExecuteCommand = new DelegateCommand(ExecuteModule, CanExecuteModule);
        }

        private void ExecuteModule()
        {
            _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("PeopleMaintView", UriKind.Relative));
        }

        private bool CanExecuteModule()
        {
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
