// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINDOWS_UWP
using Microsoft.UI.Xaml.Data;
#else
using System.ComponentModel;
#endif

namespace Microsoft.Toolkit.Uwp.SampleApp.Models
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
            get
            {
                return _value;
            }

            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
    }
}
