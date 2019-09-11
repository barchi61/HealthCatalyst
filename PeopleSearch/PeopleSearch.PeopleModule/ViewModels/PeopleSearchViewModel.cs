using PeopleSearch.Data.EDM;
using PeopleSearch.Infrastructure;
using PeopleSearch.Infrastructure.Interfaces;
using PeopleSearch.Infrastructure.Services;
using PeopleSearch.PeopleModule.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PeopleSearch.PeopleModule.ViewModels
{
    [Export]
    public class PeopleSearchViewModel : BindableBase, IRegionMemberLifetime
    {
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private IApplicationCommands _applicationCommands;

        private string _searchContext;
        public string SearchContext
        {
            get { return _searchContext; }
            set { SetProperty(ref this._searchContext, value); }
        }

        private ObservableCollection<People> _peopleCollection;
        public ObservableCollection<People> PeopleCollection
        {
            get { return _peopleCollection; }
            set { SetProperty(ref this._peopleCollection, value); }
        }

        private People _currentItem;
        public People CurrentItem
        {
            get { return _currentItem; }
            set
            {
                SetProperty(ref this._currentItem, value);
                SetImage();
            }
        }

        private object _peopleImage;
        public object PeopleImage
        {
            get { return _peopleImage; }
            set { SetProperty(ref this._peopleImage, value); }
        }

        private bool _canEditPeople = true;
        public bool CanEditPeople
        {
            get { return _canEditPeople; }
            set { SetProperty(ref _canEditPeople, value); }
        }

        private bool _simulateSlowResponse;
        public bool SimulateSlowResponse
        {
            get { return _simulateSlowResponse; }
            set { SetProperty(ref this._simulateSlowResponse, value); }
        }

        private Visibility _loandingAnimationVisibility = Visibility.Hidden;
        public Visibility LoadingAnimationVisibility
        {
            get { return _loandingAnimationVisibility; }
            set
            {
                SetProperty(ref this._loandingAnimationVisibility, value);
                LoadingAnimationVisibilityChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler LoadingAnimationVisibilityChanged;

        public DelegateCommand<object> GetDataCommand { get; private set; }
        public DelegateCommand<object> EditPeopleCommand { get; private set; }

        public bool KeepAlive
        {
            get { return false; }
        }

        [ImportingConstructor]
        public PeopleSearchViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;

            GetDataCommand = new DelegateCommand<object>(GetData);
            EditPeopleCommand = new DelegateCommand<object>(EditPeople).ObservesCanExecute(() => CanEditPeople);

            _applicationCommands.EditPeopleCommand.RegisterCommand(EditPeopleCommand);
        }

        private async void GetData(object commandArg)
        {
            if (SimulateSlowResponse)
            {
                LoadingAnimationVisibility = Visibility.Visible;
                await Task.Delay(5000);
                LoadingAnimationVisibility = Visibility.Hidden;
            }

            if (commandArg != null)
            {
                
                PeopleCollection = DataService.GetData(commandArg.ToString());
            }
            else
            {
                PeopleCollection = DataService.GetData(_searchContext);
            }
        }

        private void EditPeople(object commandArg)
        {
            var parameters = new NavigationParameters();
            parameters.Add("PeopleId", CurrentItem.PeopleId);

            CurrentItem = null;
            _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("PeopleMaintView", UriKind.Relative), parameters);
        }

        private void SetImage()
        {
            if (CurrentItem != null && CurrentItem.ImagePath != null && CurrentItem.ImagePath != string.Empty)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(Path.Combine(ApplicationSettings.ImagePath, CurrentItem.ImagePath));
                image.EndInit();
                PeopleImage = image;
                image = null;
            }
            else
            {
                PeopleImage = null;
            }
        }
    }
}
