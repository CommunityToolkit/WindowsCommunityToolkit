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
    /// The <see cref="Expander"/> control allows user to show/hide content based on a boolean state
    /// </summary>
    public partial class Expander
    {
        /// <summary>
        /// Key of the VisualStateGroup that open/close content
        /// </summary>
        public const string GroupContent = "ExpandedStates";

        /// <summary>
        /// Key of the VisualState when content is expanded
        /// </summary>
        public const string StateContentExpanded = "Expanded";

        /// <summary>
        /// Key of the VisualState when content is collapsed
        /// </summary>
        public const string StateContentCollapsed = "Collapsed";

        /// <summary>
        /// Key of the UI Element that toggle IsExpanded property
        /// </summary>
        public const string ExpanderToggleButtonPart = "PART_ExpanderToggleButton";

        /// <summary>
        /// Key of the UI Element that contains the content of the control that is expanded/collapsed
        /// </summary>
        public const string MainContentPart = "PART_MainContent";
    }
}