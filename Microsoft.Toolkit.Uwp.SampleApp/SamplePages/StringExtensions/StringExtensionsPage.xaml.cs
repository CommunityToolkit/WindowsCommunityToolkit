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
using Windows.UI.Text;
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
            ValidateCurrentText();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCurrentText();
        }

        private void ValidateCurrentText()
        {
            IsValidEmailResult.Text = InputTextBox.Text.IsEmail().ToString();
            IsValidEmailResult.FontWeight = InputTextBox.Text.IsEmail() ? FontWeights.Bold : FontWeights.Normal;

            IsValidNumberResult.Text = InputTextBox.Text.IsNumeric().ToString();
            IsValidNumberResult.FontWeight = InputTextBox.Text.IsNumeric() ? FontWeights.Bold : FontWeights.Normal;

            IsValidDecimalResult.Text = InputTextBox.Text.IsDecimal().ToString();
            IsValidDecimalResult.FontWeight = InputTextBox.Text.IsDecimal() ? FontWeights.Bold : FontWeights.Normal;

            IsValidStringResult.Text = InputTextBox.Text.IsCharacterString().ToString();
            IsValidPhoneNumberResult.FontWeight = InputTextBox.Text.IsCharacterString() ? FontWeights.Bold : FontWeights.Normal;

            IsValidPhoneNumberResult.Text = InputTextBox.Text.IsPhoneNumber().ToString();
            IsValidPhoneNumberResult.FontWeight = InputTextBox.Text.IsPhoneNumber() ? FontWeights.Bold : FontWeights.Normal;
        }
    }
}