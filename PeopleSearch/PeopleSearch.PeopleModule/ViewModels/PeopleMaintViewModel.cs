using PeopleSearch.Data.EDM;
using PeopleSearch.Infrastructure.Interfaces;
using PeopleSearch.Infrastructure.Services;
using PeopleSearch.PeopleModule.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PeopleSearch.PeopleModule.ViewModels
{
    [Export]
    public class PeopleMaintViewModel : BindableBase, INavigationAware, IRegionMemberLifetime, INotifyPropertyChanged, IConfirmNavigationRequest, INotifyDataErrorInfo
    {
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private IApplicationCommands _applicationCommands;

        #region Properties
        private People _currentItem;
        public People CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (_currentItem != value)
                {
                    if (_currentItem != null)
                    {
                        CurrentItem.PropertyChanged -= CurrentItem_PropertyChanged;
                    }

                    SetProperty(ref this._currentItem, value);
                    _undoImagePath = string.Empty;
                    SetImage();

                    if (_currentItem != null)
                    {
                        CurrentItem.PropertyChanged += CurrentItem_PropertyChanged;
                    }

                    RaisePropertyChanged(nameof(CurrentItem));
                    SetToolbarState();
                }
            }
        }

        private object _peopleImage;
        public object PeopleImage
        {
            get { return _peopleImage; }
            set { SetProperty(ref this._peopleImage, value); }
        }

        private string _undoImagePath;

        private bool _canGetImage = false;
        public bool CanGetImage
        {
            get { return _canGetImage; }
            set { SetProperty(ref _canGetImage, value); }
        }

        private bool _canAddPeople = false;
        public bool CanAddPeople
        {
            get { return _canAddPeople; }
            set { SetProperty(ref _canAddPeople, value); }
        }

        private bool _canDeletePeople = false;
        public bool CanDeletePeople
        {
            get { return _canDeletePeople; }
            set { SetProperty(ref _canDeletePeople, value); }
        }

        private bool _canSavePeople = false;
        public bool CanSavePeople
        {
            get { return _canSavePeople; }
            set { SetProperty(ref _canSavePeople, value); }
        }

        private bool _canUndoPeople = false;
        public bool CanUndoPeople
        {
            get { return _canUndoPeople; }
            set { SetProperty(ref _canUndoPeople, value); }
        }

        public bool KeepAlive
        {
            get { return false; }
        }

        public bool HasErrors { get; set; } = false;
        #endregion Properties

        #region EventHanlders
        public event EventHandler CanGetImageChanged;
        public event EventHandler CanAddPeopleChanged;
        public event EventHandler CanDeletePeopleChanged;
        public event EventHandler CanSavePeopleChanged;
        public event EventHandler CanUndoPeopleChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion EventHandlers

        #region DelegateCommands
        public DelegateCommand<object> GetDataCommand { get; private set; }
        public DelegateCommand<object> GetImageCommand { get; private set; }
        public DelegateCommand<object> AddPeopleCommand { get; private set; }
        public DelegateCommand<object> DeletePeopleCommand { get; private set; }
        public DelegateCommand<object> SavePeopleCommand { get; private set; }
        public DelegateCommand<object> UndoPeopleCommand { get; private set; }
        #endregion DelegateCommands


        [ImportingConstructor]
        public PeopleMaintViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;

            GetImageCommand = new DelegateCommand<object>(GetImage).ObservesCanExecute(() => CanGetImage);
            AddPeopleCommand = new DelegateCommand<object>(AddPeople).ObservesCanExecute(() => CanAddPeople);
            DeletePeopleCommand = new DelegateCommand<object>(DeletePeople).ObservesCanExecute(() => CanDeletePeople);
            SavePeopleCommand = new DelegateCommand<object>(SavePeople).ObservesCanExecute(() => CanSavePeople);
            UndoPeopleCommand = new DelegateCommand<object>(UndoPeople).ObservesCanExecute(() => CanUndoPeople);

            _applicationCommands.GetImageCommand.RegisterCommand(GetImageCommand);
        }

        private void CurrentItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetToolbarState();
        }

        private void SetToolbarState()
        {
            switch (DataService.GetEntityState(CurrentItem))
            {
                case EntityState.Modified:
                    CanAddPeople = false;
                    CanDeletePeople = false;
                    CanSavePeople = true;
                    CanUndoPeople = true;
                    CanGetImage = true;
                    break;
                case EntityState.Added:
                    CanAddPeople = false;
                    CanDeletePeople = false;
                    CanSavePeople = true;
                    CanUndoPeople = true;
                    CanGetImage = true;
                    break;
                case EntityState.Deleted:
                    CanAddPeople = true;
                    CanDeletePeople = true;
                    CanSavePeople = false;
                    CanUndoPeople = false;
                    CanGetImage = false;
                    break;
                case EntityState.Unchanged:
                    CanAddPeople = true;
                    CanDeletePeople = true;
                    CanSavePeople = false;
                    CanUndoPeople = false;
                    CanGetImage = true;
                    break;
                case EntityState.Detached:
                    CanAddPeople = true;
                    CanDeletePeople = false;
                    CanSavePeople = false;
                    CanUndoPeople = false;
                    CanGetImage = false;
                    break;
            }

            CanAddPeopleChanged?.Invoke(this, new EventArgs());
            CanDeletePeopleChanged?.Invoke(this, new EventArgs());
            CanSavePeopleChanged?.Invoke(this, new EventArgs());
            CanUndoPeopleChanged?.Invoke(this, new EventArgs());
            CanGetImageChanged?.Invoke(this, new EventArgs());
        }

        private void SetImage()
        {
            if (CurrentItem != null &&!string.IsNullOrEmpty(CurrentItem.ImagePath))
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
                    _undoImagePath = CurrentItem.ImagePath;
                }

                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                var newFilePath = Path.Combine(ApplicationSettings.ImagePath, String.Format("{0}.{1}", newFileName, fileExtension));
                CurrentItem.ImagePath = String.Format("{0}.{1}", newFileName, fileExtension);

                File.Copy(filePath, newFilePath);

                SetImage();
            }
        }

        private void UndoPeople(object commandArg)
        {
            if (!string.IsNullOrEmpty(_undoImagePath))
               DeleteImage(CurrentItem.ImagePath);

            HasErrors = false;
            CurrentItem = DataService.UndoChanges(CurrentItem.PeopleId);
        }

        private void SavePeople(object commandArg)
        {
            if (ValidateData())
            {
                DataService.SaveChanges(CurrentItem);
                CurrentItem = DataService.GetPeopleById(CurrentItem.PeopleId);
                HasErrors = false;
            }
        }

        private void DeletePeople(object commandArg)
        {
            if (System.Windows.MessageBox.Show("Ok to Delete?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DeleteImage(CurrentItem.ImagePath);
                DataService.Delete(CurrentItem.PeopleId);
                HasErrors = false;
                CurrentItem = null;
            }
        }

        private void AddPeople(object commandArg)
        {
            HasErrors = false;
            CurrentItem = DataService.AddPeople();
        }

        private void DeleteImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                var path = Path.Combine(ApplicationSettings.ImagePath, imagePath);

                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || (!HasErrors))
                return null;
            return new List<string>() { "Invalid Entries." };
        }

        public bool ValidateData()
        {
            HasErrors = (CurrentItem != null && (string.IsNullOrEmpty(CurrentItem.FirstName) || string.IsNullOrEmpty(CurrentItem.LastName)));
            if (HasErrors)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("FirstName"));
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("LastName"));
            }
            else
            {
                return true;
            }
            return false;
        }

        #region NavigationMethods
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var peopleId = navigationContext.Parameters["PeopleId"];
            if (peopleId != null)
                CurrentItem = DataService.GetPeopleById(Convert.ToInt32(peopleId));
            else
                SetToolbarState();
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

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            bool result = true;

            if (DataService.GetEntityState(CurrentItem) == EntityState.Modified || DataService.GetEntityState(CurrentItem) == EntityState.Added)
            {
                var messageResult = System.Windows.MessageBox.Show("Save changes before leaving?", "Save Changes?", MessageBoxButton.YesNo);
                if (messageResult == MessageBoxResult.No)
                    result = false;
                else
                {
                    SavePeople(null);
                }
            }
            else
                result = true;

            continuationCallback(result);
        }
        #endregion NavigationMethods


    }
}
