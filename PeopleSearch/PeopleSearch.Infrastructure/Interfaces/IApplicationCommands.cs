using Prism.Commands;

namespace PeopleSearch.Infrastructure.Interfaces
{
    public interface IApplicationCommands
    {
        //object this[string propertyName] { get; set; }

        CompositeCommand SaveSettingCommand { get; }
        CompositeCommand GetSettingCommand { get; }
        CompositeCommand GetDataCommand { get; }
        CompositeCommand EditPeopleCommand { get; }
        CompositeCommand GetImageCommand { get; }
    }
}
