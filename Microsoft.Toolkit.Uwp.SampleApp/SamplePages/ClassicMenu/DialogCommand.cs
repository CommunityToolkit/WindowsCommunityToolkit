using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ClassicMenu
{
    public class DialogCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var dialog = new MessageDialog("Your message here");
            await dialog.ShowAsync();
        }

        public event EventHandler CanExecuteChanged;
    }
}
