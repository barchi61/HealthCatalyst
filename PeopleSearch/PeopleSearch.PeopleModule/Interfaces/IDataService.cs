using PeopleSearch.Data.EDM;
using System.Collections.ObjectModel;

namespace PeopleSearch.PeopleModule.Interfaces
{
    interface IDataService
    {
        ObservableCollection<People> GetData(string searchContext);
    }
}
