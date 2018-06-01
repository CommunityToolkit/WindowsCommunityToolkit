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
    /// In App Notification defines a control to show local notification in the app.
    /// </summary>
    public partial class InAppNotification
    {
        /// <summary>
        /// Key of the VisualStateGroup that show/dismiss content
        /// </summary>
        private const string GroupContent = "State";

        /// <summary>
        /// Key of the VisualState when content is showed
        /// </summary>
        private const string StateContentVisible = "Visible";

        /// <summary>
        /// Key of the VisualState when content is dismissed
        /// </summary>
        private const string StateContentCollapsed = "Collapsed";

        /// <summary>
        /// Key of the UI Element that dismiss the control
        /// </summary>
        private const string DismissButtonPart = "PART_DismissButton";
    }
}
