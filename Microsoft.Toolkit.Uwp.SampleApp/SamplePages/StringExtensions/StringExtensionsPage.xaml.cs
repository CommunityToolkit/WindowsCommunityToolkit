// ******************************************************************
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

using Microsoft.Toolkit.Extensions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Page that shows how to use StringExtensions
    /// </summary>
    public sealed partial class StringExtensionsPage : Page
    {
        public StringExtensionsPage()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string returnText = string.Empty;
            switch (int.Parse((sender as Button).Tag.ToSafeString()))
            {
                case 0:
                    returnText = string.Format("IsValid: {0}", InputTextBox.Text.IsEmail());
                    break;
                case 1:
                    returnText = string.Format("IsValid: {0}", InputTextBox.Text.IsNumeric());
                    break;
                case 2:
                    returnText = string.Format("IsValid: {0}", InputTextBox.Text.IsDecimal());
                    break;
                case 3:
                    returnText = string.Format("IsValid: {0}", InputTextBox.Text.IsCharacterString());
                    break;
                case 4:
                    returnText = string.Format("IsValid: {0}", InputTextBox.Text.IsPhoneNumber());
                    break;
            }

            IsValid.Text = returnText;
        }
    }
}
