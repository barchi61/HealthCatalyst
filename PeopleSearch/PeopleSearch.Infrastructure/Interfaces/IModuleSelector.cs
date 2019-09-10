using Prism.Commands;

namespace PeopleSearch.Infrastructure.Interfaces
{
    public interface IModuleSelector
    {
        DelegateCommand<object> ExecuteModuleCommand();
    }
}
