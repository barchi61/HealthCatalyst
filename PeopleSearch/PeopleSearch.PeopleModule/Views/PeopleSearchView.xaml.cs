using PeopleSearch.PeopleModule.ViewModels;
using Prism.Regions;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using PeopleSearch.Infrastructure.Behaviors;

namespace PeopleSearch.PeopleModule.Views
{
    /// <summary>
    /// Interaction logic for PeopleSearch.xaml
    /// </summary>
    [Export("PeopleSearchView")]
    public partial class PeopleSearchView : UserControl, IRegionMemberLifetime
    {
        public PeopleSearchView()
        {
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
