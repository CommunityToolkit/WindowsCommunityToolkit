using Microsoft.Windows.Toolkit.SampleApp.Common;

namespace Microsoft.Windows.Toolkit.SampleApp.Models
{
    public class Item : BindableBase
    {
        private string _title = default(string);

        public string Title
        {
            get { return _title; } set { Set(ref _title, value); }
        }

        private bool? _isFavorite = default(bool);

        public bool? IsFavorite
        {
            get { return _isFavorite; } set { Set(ref _isFavorite, value); }
        }

        private DelegateCommand _toggleFavorite = default(DelegateCommand);

        public DelegateCommand ToggleFavorite => _toggleFavorite ?? (_toggleFavorite = new DelegateCommand(ExecuteToggleFavoriteCommand, CanExecuteToggleFavoriteCommand));

        private bool CanExecuteToggleFavoriteCommand()
        {
            return true;
        }

        private void ExecuteToggleFavoriteCommand()
        {
            IsFavorite = !IsFavorite;
        }
    }
}
