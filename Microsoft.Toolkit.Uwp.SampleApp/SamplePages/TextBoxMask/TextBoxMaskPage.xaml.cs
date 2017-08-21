﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Textbox Mask sample page
    /// </summary>
    public sealed partial class TextBoxMaskPage : Page
    {
        public TextBoxMaskPage()
        {
            InitializeComponent();
        }

        private void ApplyFullMaskButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AlphaTextBox.Text = "7b1y--x4a5";
        }

        private void ApplyPartialMaskButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AlphaTextBox.Text = "7b1yZW";
        }
    }
}
