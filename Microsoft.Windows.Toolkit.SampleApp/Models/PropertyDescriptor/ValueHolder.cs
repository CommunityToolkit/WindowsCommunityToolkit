using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.SampleApp.Models
{
    // Need to use this class as ExpandoObject does not raise PropertyChanged event
    public class ValueHolder : INotifyPropertyChanged
    {
        public ValueHolder(object v)
        {
            _value = v;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
    }
}
