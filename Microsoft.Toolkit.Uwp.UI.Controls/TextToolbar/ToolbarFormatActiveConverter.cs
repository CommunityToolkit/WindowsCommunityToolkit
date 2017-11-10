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
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
<<<<<<< HEAD
    /// <summary>
    /// Compares if Formats are equal and returns bool
    /// </summary>
    public class ToolbarFormatActiveConverter : IValueConverter
    {
        /// <inheritdoc/>
=======
    public class ToolbarFormatActiveConverter : IValueConverter
    {
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Format)
            {
                CurrentFormat = (Format)value;
                return CurrentFormat == Format;
            }
            else
            {
                return value;
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (CurrentFormat != null)
            {
                return CurrentFormat;
            }

            return value;
        }

<<<<<<< HEAD
        /// <summary>
        /// Gets or sets the <see cref="Format"/> to compare
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public Format Format { get; set; }

        private Format? CurrentFormat { get; set; }
    }
}