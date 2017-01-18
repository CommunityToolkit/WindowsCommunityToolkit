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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Textbox regex extension helps developer to validate a textbox with a regex using the Regex property, IsValid property is updated with the regex validation result, if ValidationMode is Normal only IsValid property is setted if ValidationMode is Forced and the input is not valid the textbox text will be cleared
    /// </summary>
    public partial class TextBoxRegexEx
    {
        /// <summary>
        /// Regex extension validation mode
        /// </summary>
        public enum ValidationMode
        {
            /// <summary>
            /// Update IsValid property with validation result
            /// </summary>
            Normal,

            /// <summary>
            /// Update IsValid proeprty with validation result and in case the textbox is not valid clear its value
            /// </summary>
            Forced
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
            /// Integer validation
            /// </summary>
            Integer,

            /// <summary>
            /// Decimal validation
            /// </summary>
            Decimal,

            /// <summary>
            /// Text only validation
            /// </summary>
            CharOnly,

            /// <summary>
            /// Phone number validation
            /// </summary>
            PhoneNumber
        }
    }
}