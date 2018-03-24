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

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base class for information about stacked notification
    /// </summary>
    public abstract class StackedNotificationInfo
    {
        /// <summary>
        /// Gets or sets duration of the stacked notification
        /// </summary>
        public int Duration { get; set; }
    }

    /// <summary>
    /// Information about stacked notification (using text only)
    /// </summary>
    public class TextStackedNotificationInfo : StackedNotificationInfo
    {
        /// <summary>
        /// Gets or sets text of the stacked notification
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Information about stacked notification (using a UIElement)
    /// </summary>
    public class UIElementStackedNotificationInfo : StackedNotificationInfo
    {
        /// <summary>
        /// Gets or sets UIElement of the stacked notification
        /// </summary>
        public UIElement Element { get; set; }
    }

    /// <summary>
    /// Information about stacked notification (using a DataTemplate)
    /// </summary>
    public class DataTemplateStackedNotificationInfo : StackedNotificationInfo
    {
        /// <summary>
        /// Gets or sets DataTemplate of the stacked notification
        /// </summary>
        public DataTemplate DataTemplate { get; set; }
    }
}
