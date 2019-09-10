using PeopleSearch.PeopleModule.ViewModels;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace PeopleSearch.PeopleModule.Views
{
    /// <summary>
    /// Interaction logic for ModuleSelector.xaml
    /// </summary>
    [Export("PeopleSearchModuleSelectorView")]
    public partial class PeopleSearchModuleSelectorView : UserControl
    {
        public PeopleSearchModuleSelectorView()
        {
            InitializeComponent();
        }

        [Import(AllowRecomposition = false)]
        public PeopleSearchModuleSelectorViewModel ViewModel
        {
            get { return this.DataContext as PeopleSearchModuleSelectorViewModel; }
            set { this.DataContext = value; }
        }
    }
}
