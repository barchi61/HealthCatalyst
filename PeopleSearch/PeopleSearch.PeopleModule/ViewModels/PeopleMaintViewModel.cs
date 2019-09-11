using PeopleSearch.Data.EDM;
using PeopleSearch.Infrastructure.Interfaces;
using PeopleSearch.Infrastructure.Services;
using PeopleSearch.PeopleModule.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PeopleSearch.PeopleModule.ViewModels
{
    [Export]
    public class PeopleMaintViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private IApplicationCommands _applicationCommands;

        private People _currentItem;
        public People CurrentItem
        {
            get { return _currentItem; }
            set
            {
                
                SetProperty(ref this._currentItem, value);
                SetImage();
                OnCurrentItemChanged();
            }
        }

        private object _peopleImage;
        public object PeopleImage
        {
            get { return _peopleImage; }
            set { SetProperty(ref this._peopleImage, value); }
        }

        private void OnCurrentItemChanged()
        {
            CanGetImage = (CurrentItem != null);
            GetImageCommand.IsActive = CanGetImage;
            CanGetImageChanged?.Invoke(this, new EventArgs());
        }

        private bool _canGetImage = false;
        public bool CanGetImage
        {
            get { return _canGetImage; }
            set { SetProperty(ref _canGetImage, value); }
        }

        public event EventHandler CanGetImageChanged;

        private bool _canAddPeople = false;
        public bool CanAddPeople
        {
            get { return _canAddPeople; }
            set { SetProperty(ref _canAddPeople, value); }
        }

        public event EventHandler CanAddPeopleChanged;

        private bool _canDeletePeople = false;
        public bool CanDeletePeople
        {
            get { return _canDeletePeople; }
            set { SetProperty(ref _canDeletePeople, value); }
        }

        public event EventHandler CanDeletePeopleChanged;

        private bool _canSavePeople = false;
        public bool CanSavePeople
        {
            get { return _canSavePeople; }
            set { SetProperty(ref _canSavePeople, value); }
        }

        public event EventHandler CanSavePeopleChanged;

        private bool _canUndoPeople = false;
        public bool CanUndoPeople
        {
            get { return _canUndoPeople; }
            set { SetProperty(ref _canUndoPeople, value); }
        }

        public DelegateCommand<object> GetDataCommand { get; private set; }
        public DelegateCommand<object> GetImageCommand { get; private set; }
        public DelegateCommand<object> AddPeopleCommand { get; private set; }
        public DelegateCommand<object> DeletePeopleCommand { get; private set; }
        public DelegateCommand<object> SavePeopleCommand { get; private set; }
        public DelegateCommand<object> UndoPeopleCommand { get; private set; }

        public bool KeepAlive
        {
            get { return false; }
        }

        [ImportingConstructor]
        public PeopleMaintViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;

            GetDataCommand = new DelegateCommand<object>(GetData);
            GetImageCommand = new DelegateCommand<object>(GetImage).ObservesCanExecute(() => CanGetImage);
            AddPeopleCommand = new DelegateCommand<object>(AddPeople).ObservesCanExecute(() => CanAddPeople);
            DeletePeopleCommand = new DelegateCommand<object>(DeletePeople).ObservesCanExecute(() => CanDeletePeople);
            SavePeopleCommand = new DelegateCommand<object>(SavePeople).ObservesCanExecute(() => CanSavePeople);
            UndoPeopleCommand = new DelegateCommand<object>(UndoPeople).ObservesCanExecute(() => CanUndoPeople);

            _applicationCommands.GetImageCommand.RegisterCommand(GetImageCommand);
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

        private void UndoPeople(object commandArg)
        {
            throw new NotImplementedException();
        }

        private void SavePeople(object commandArg)
        {
            throw new NotImplementedException();
        }

        private void DeletePeople(object commandArg)
        {
            throw new NotImplementedException();
        }

        private void AddPeople(object commandArg)
        {
            throw new NotImplementedException();
        }

        private void GetData(object commandArg)
        {
        }

        private void GetImage(object commandArg)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
            {
                var filePath = openFileDialog.FileName;
                var fileExtension = Path.GetExtension(filePath);

                if (CurrentItem.ImagePath != null && CurrentItem.ImagePath != string.Empty)
                {
                    var oldFilePath = Path.Combine(ApplicationSettings.ImagePath, String.Format("{0}", CurrentItem.ImagePath));

                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                var newFilePath = Path.Combine(ApplicationSettings.ImagePath, String.Format("{0}.{1}", newFileName, fileExtension));
                CurrentItem.ImagePath = String.Format("{0}.{1}", newFileName, fileExtension);

                File.Copy(filePath, newFilePath);
                DataService.SaveChanges(CurrentItem);

                SetImage();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var peopleId = navigationContext.Parameters["PeopleId"];
            if (peopleId != null)

                CurrentItem = DataService.GetPeopleById(Convert.ToInt32(peopleId));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var peopleId = navigationContext.Parameters["PeopleId"] as object;
            if (CurrentItem != null)
                return CurrentItem != null && CurrentItem.PeopleId == Convert.ToInt32(peopleId);
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }

}
