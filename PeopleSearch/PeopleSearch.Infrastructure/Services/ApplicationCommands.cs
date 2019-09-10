using PeopleSearch.Infrastructure.Interfaces;
using Prism.Commands;

namespace PeopleSearch.Infrastructure.Services
{
    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _saveSettingCommand = new CompositeCommand();
        private CompositeCommand _getSettingCommand = new CompositeCommand();
        private CompositeCommand _getDataCommand = new CompositeCommand();
        private CompositeCommand _editPeopleCommand = new CompositeCommand();
        private CompositeCommand _getImageCommand = new CompositeCommand();

        public CompositeCommand GetDataCommand
        {
            get { return _getDataCommand; }
        }

        public CompositeCommand SaveSettingCommand
        {
            get { return _saveSettingCommand; }
        }

        public CompositeCommand GetSettingCommand
        {
            get { return _getSettingCommand; }
        }

        public CompositeCommand EditPeopleCommand
        {
            get { return _editPeopleCommand; }
        }

        public CompositeCommand GetImageCommand
        {
            get { return _getImageCommand; }
        }
    }
}
