using Microsoft.Windows.Toolkit.SampleApp.Common;

namespace Microsoft.Windows.Toolkit.SampleApp.Models
{
    public class Item: BindableBase
    {
        private string _Title = default(string);
        public string Title { get { return _Title; } set { Set(ref _Title, value); } }

        private bool? _IsFavorite = default(bool);
        public bool? IsFavorite { get { return _IsFavorite; } set { Set(ref _IsFavorite, value); } }

        DelegateCommand _ToggleFavorite = default(DelegateCommand);
        public DelegateCommand ToggleFavorite { get { return _ToggleFavorite ?? (_ToggleFavorite = new DelegateCommand(ExecuteToggleFavoriteCommand, CanExecuteToggleFavoriteCommand)); } }
        private bool CanExecuteToggleFavoriteCommand() { return true; }
        private void ExecuteToggleFavoriteCommand()
        {
            IsFavorite = !IsFavorite;
        }
    }
}
