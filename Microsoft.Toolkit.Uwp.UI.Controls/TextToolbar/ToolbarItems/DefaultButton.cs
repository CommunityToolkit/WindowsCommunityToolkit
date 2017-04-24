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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    /// <summary>
    /// Specifies a DefaultButton, in order to disable it.
    /// </summary>
    public class DefaultButton
    {
        /// <summary>
        /// Specifies the Type of DefaultButton in order to remove it.
        /// </summary>
        /// <param name="type">Type of Default Button</param>
        /// <returns>Removal Object</returns>
        public static DefaultButton OfType(ButtonType type)
        {
            return new DefaultButton { Type = type };
        }

        /// <summary>
        /// Gets or sets the type of Default Button to remove.
        /// </summary>
        public ButtonType Type { get; set; }

        /// <summary>
        /// Gets or sets the instance of button that is removed, in order to preserve any modifications when re-attaching to the Toolbar.
        /// </summary>
        internal IToolbarItem Button { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DefaultButton other && other.Type == Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        public enum ButtonType
        {
            /// <summary>
            /// Bold Button
            /// </summary>
            Bold,

            /// <summary>
            /// Italics Button
            /// </summary>
            Italics,

            /// <summary>
            /// Strikethrough Button
            /// </summary>
            Strikethrough,

            /// <summary>
            /// Code button
            /// </summary>
            Code,

            /// <summary>
            /// Quote Button
            /// </summary>
            Quote,

            /// <summary>
            /// Link Button
            /// </summary>
            Link,

            /// <summary>
            /// List Button
            /// </summary>
            List,

            /// <summary>
            /// Ordered List Button
            /// </summary>
            OrderedList,

            /// <summary>
            /// Header Selector
            /// </summary>
            Headers
        }
    }
}