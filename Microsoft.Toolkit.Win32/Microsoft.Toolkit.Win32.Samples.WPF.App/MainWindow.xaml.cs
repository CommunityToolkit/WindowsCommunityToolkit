// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.WPF;
using System;
using System.Windows;

namespace Microsoft.Toolkit.Win32.Samples.WPF.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void inkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;
        }

        private void inkToolbar_Initialized(object sender, EventArgs e)
        {
            
        }

        private void inkToolbar_ActiveToolChanged(object sender, object e)
        {

        }

        private void inkToolbar_InkDrawingAttributesChanged(object sender, object e)
        {

        }

        private void inkToolbar_IsStencilButtonCheckedChanged(object sender, Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarIsStencilButtonCheckedChangedEventArgs e)
        {

        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ColorspacesCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TonemappersCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ScalingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void WhiteLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ScalingCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void swapChainPanel_Loaded(object sender, RoutedEventArgs e)
        {

            
        }
    }

}
