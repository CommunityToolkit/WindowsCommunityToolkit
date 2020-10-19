// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UITests.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TextBoxMask : Page, INotifyPropertyChanged
    {
        private const string INITIAL_VALUE = "12:50:59";
        private const string NEW_VALUE = "00:00:00";

        private string _value = INITIAL_VALUE;

        public string InitialValue => INITIAL_VALUE;

        public string NewValue => NEW_VALUE;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public TextBoxMask()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Value = NEW_VALUE;
        }
    }
}
