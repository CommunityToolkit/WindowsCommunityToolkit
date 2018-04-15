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

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// TextBoxRegex allows text validation using a regular expression.
    /// </summary>
    /// <remarks>
    /// If <see cref="ValidationMode"> is set to Normal then IsValid will be set according to whether the regex is valid.</see>
    /// If <see cref="ValidationMode"> is set to Forced then IsValid will be set according to whether the regex is valid, when TextBox lose focus and in case the textbox is invalid clear its value. </see>
    /// If <see cref="ValidationMode"> is set to Dynamic then IsValid will be set according to whether the regex is valid. If the newest charachter is invalid, only invalid character of the Textbox will be deleted.</see>
    /// </remarks>
    public partial class TextBoxRegex
    {
        /// <summary>
        /// Regex validation mode
        /// </summary>
        public enum ValidationMode
        {
            /// <summary>
            /// Update IsValid property with validation result at text changed
            /// </summary>
            Normal,

            /// <summary>
            /// Update IsValid property with validation result and in case the textbox is not valid clear its value when the TextBox lose focus
            /// </summary>
            Forced,

            /// <summary>
            /// Update IsValid property with validation result at text changed and clear the newest character at input which is not valid
            /// </summary>
            Dynamic
        }

        /// <summary>
        /// Specify the type of validation required
        /// </summary>
        public enum ValidationType
        {
            /// <summary>
            /// The default validation that required property Regex to be setted
            /// </summary>
            Custom,

            /// <summary>
            /// Email validation
            /// </summary>
            Email,

            /// <summary>
            /// Number validation
            /// </summary>
            Number,

            /// <summary>
            /// Decimal validation
            /// </summary>
            Decimal,

            /// <summary>
            /// Text only validation
            /// </summary>
            Characters,

            /// <summary>
            /// Phone number validation
            /// </summary>
            PhoneNumber
        }
    }
}