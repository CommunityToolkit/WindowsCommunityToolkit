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
    /// Defines a visual header for the toast notification.
    /// </summary>
    public sealed class ToastHeader
    {
        /// <summary>
        /// Constructs a toast header with all the required properties.
        /// </summary>
        /// <param name="id">A developer-created identifier that uniquely identifies this header. If two notifications have the same header id, they will be displayed underneath the same header in Action Center.</param>
        /// <param name="title">A title for the header.</param>
        /// <param name="arguments">A developer-defined string of arguments that is returned to the app when the user clicks this header.</param>
        public ToastHeader(string id, string title, string arguments)
        {
            Id = id;
            Title = title;
            Arguments = arguments;
        }

        /// <summary>
        /// Gets or sets a developer-created identifier that uniquely identifies this header. If two notifications have the same header id, they will be displayed underneath the same header in Action Center.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a title for the header.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a developer-defined string of arguments that is returned to the app when the user clicks this header.
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Gets or sets the type of activation this header will use when clicked. Defaults to Foreground.
        /// </summary>
        public ToastActivationType ActivationType { get; set; } = ToastActivationType.Foreground;

        internal Element_ToastHeader ConvertToElement()
        {
            if (Id == null)
            {
                throw new NullReferenceException("Id on ToastHeader must be provided.");
            }

            if (Title == null)
            {
                throw new NullReferenceException("Title on ToastHeader must be provided.");
            }

            if (Arguments == null)
            {
                throw new NullReferenceException("Arguments on ToastHeader must be provided.");
            }

            return new Element_ToastHeader()
            {
                Id = Id,
                Title = Title,
                Arguments = Arguments,
                ActivationType = ActivationType
            };
        }
    }
}
