using PeopleSearch.PeopleModule.ViewModels;
using Prism.Mvvm;
using Prism.Regions;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace PeopleSearch.PeopleModule.Views
{
    /// <summary>
    /// Interaction logic for PeopleMaint.xaml
    /// </summary>
    [Export("PeopleMaintView")]
    public partial class PeopleMaintView : UserControl, IRegionMemberLifetime
    {
        public PeopleMaintView()
        {
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
