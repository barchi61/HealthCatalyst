using PeopleSearch.PeopleModule.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace PeopleSearch.PeopleModule.Views
{
    /// <summary>
    /// Interaction logic for PeopleMaintModuleSelector.xaml
    /// </summary>
    [Export("PeopleMaintModuleSelectorView")]
    public partial class PeopleMaintModuleSelectorView : UserControl
    {
        public PeopleMaintModuleSelectorView()
        {
            InitializeComponent();
        }

        [Import(AllowRecomposition = false)]
        public PeopleMaintModuleSelectorViewModel ViewModel
        {
            get { return this.DataContext as PeopleMaintModuleSelectorViewModel; }
            set { this.DataContext = value; }
        }
    }
}
