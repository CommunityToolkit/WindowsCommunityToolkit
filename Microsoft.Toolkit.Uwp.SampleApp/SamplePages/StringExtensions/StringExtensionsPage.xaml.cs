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

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValidEmail.Text = $"IsEmail(): {InputTextBox.Text.IsEmail()}";
            IsValidNumber.Text = $"IsNumeric(): {InputTextBox.Text.IsNumeric()}";
            IsValidDecimal.Text = $"IsDecimal(): {InputTextBox.Text.IsDecimal()}";
            IsValidString.Text = $"IsCharacterString(): {InputTextBox.Text.IsCharacterString()}";
            IsValidPhoneNumber.Text = $"IsPhoneNumber(): {InputTextBox.Text.IsPhoneNumber()}";
        }
    }
}
