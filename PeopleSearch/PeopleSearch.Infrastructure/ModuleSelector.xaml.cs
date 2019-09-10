using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PeopleSearch.Infrastructure
{
    /// <summary>
    /// Interaction logic for ModuleSelectorUserControl.xaml
    /// </summary>
    public sealed partial class ModuleSelector : UserControl
	{
		public ModuleSelector()
		{
			InitializeComponent();
		}

		public string ModuleName
		{
			get { return (string)GetValue(ModuleNameProperty); }
			set { SetValue(ModuleNameProperty, value); }
		}

		public static readonly DependencyProperty ModuleNameProperty =
				DependencyProperty.Register("ModuleName", typeof(string), typeof(ModuleSelector), new PropertyMetadata("Textblock"));


		public ImageSource ModuleIcon
		{
			get { return (ImageSource)GetValue(ModuleIconProperty); }
			set { SetValue(ModuleIconProperty, value); }
		}

		public static readonly DependencyProperty ModuleIconProperty =
				DependencyProperty.Register("ModuleIcon", typeof(ImageSource), typeof(ModuleSelector), new PropertyMetadata(null));


		public string ModuleToolTip
		{
			get { return (string)GetValue(ModuleToolTipProperty); }
			set { SetValue(ModuleToolTipProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ModuleToolTip.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ModuleToolTipProperty =
				DependencyProperty.Register("ModuleToolTip", typeof(string), typeof(ModuleSelector), new PropertyMetadata("ToolTip"));


		public ICommand ModuleExecuteCommand
		{
			get { return (DelegateCommand<object>)GetValue(ExecuteModuleCommandProperty); }
			set { SetValue(ExecuteModuleCommandProperty, value); }

		}
		
		// Using a DependencyProperty as the backing store for ExecuteModuleCommand.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ExecuteModuleCommandProperty =
				DependencyProperty.Register("ModuleExecuteCommand", typeof(ICommand), typeof(ModuleSelector));
	}
}

