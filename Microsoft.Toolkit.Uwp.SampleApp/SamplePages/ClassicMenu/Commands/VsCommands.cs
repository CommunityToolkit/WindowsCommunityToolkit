using System;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Microsoft.Toolkit.Uwp.SampleApp.ClassicMenu.Commands
{
    internal class NewProjectCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var dialog = new MessageDialog("Create New Project");
            await dialog.ShowAsync();
        }

        public event EventHandler CanExecuteChanged;
    }

    internal class NewFileCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var dialog = new MessageDialog("Create New File");
            await dialog.ShowAsync();
        }

        public event EventHandler CanExecuteChanged;
    }

    internal class GenericCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var dialog = new MessageDialog(parameter.ToString());
            await dialog.ShowAsync();
        }

        public event EventHandler CanExecuteChanged;
    }
}
