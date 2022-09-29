// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UITests.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ColorPickerButtonTestPage : Page, INotifyPropertyChanged
    {
        private Color _theColor = Colors.Green;

        public Color TheColor
        {
            get => _theColor;
            set
            {
                if (_theColor != value)
                {
                    _theColor = value;
                    OnPropertyChanged(nameof(TheColor));
                }
            }
        }

        public ColorPickerButtonTestPage()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TheColor = Colors.Red;
        }
    }
}