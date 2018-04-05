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

using System;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// A selection box item (an item that the user can select from the drop down list).
    /// </summary>
    public sealed class ToastSelectionBoxItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToastSelectionBoxItem"/> class.
        /// Constructs a new Toast SelectionBoxItem with the required elements.
        /// </summary>
        /// <param name="id">Developer-provided ID that the developer uses later to retrieve input when the Toast is interacted with.</param>
        /// <param name="content">String that is displayed on the selection item. This is what the user sees.</param>
        public ToastSelectionBoxItem(string id, string content)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        /// <summary>
        /// Gets the required ID property used so that developers can retrieve user input once the app is activated.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the required string that is displayed on the selection item.
        /// </summary>
        public string Content { get; private set; }

        internal Element_ToastSelection ConvertToElement()
        {
            return new Element_ToastSelection()
            {
                Id = Id,
                Content = Content
            };
        }
    }
}