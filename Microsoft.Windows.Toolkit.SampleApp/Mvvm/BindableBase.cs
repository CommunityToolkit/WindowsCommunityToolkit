using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Microsoft.Windows.Toolkit.SampleApp.Mvvm
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Set<T>(ref T storage, T value, [CallerMemberName()]string propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                this.RaisePropertyChanged(propertyName);
            }
        }
    }
}
