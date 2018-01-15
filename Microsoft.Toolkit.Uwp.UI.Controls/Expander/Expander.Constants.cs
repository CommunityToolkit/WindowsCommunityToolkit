﻿// ******************************************************************
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
        /// Key of the VisualStateGroup that trigger display mode (visible/collapsed) and direction content
        /// </summary>
        private const string DisplayModeAndDirectionStatesGroupStateContent = "DisplayModeAndDirectionStates";

        /// <summary>
        /// Key of the VisualState when expander is visible and expander direction is set to Left
        /// </summary>
        private const string StateContentVisibleLeft = "VisibleLeft";

        /// <summary>
        /// Key of the VisualState when expander is visible and expander direction is set to Down
        /// </summary>
        private const string StateContentVisibleDown = "VisibleDown";

        /// <summary>
        /// Key of the VisualState when expander is visible and expander direction is set to Right
        /// </summary>
        private const string StateContentVisibleRight = "VisibleRight";

        /// <summary>
        /// Key of the VisualState when expander is visible and expander direction is set to Up
        /// </summary>
        private const string StateContentVisibleUp = "VisibleUp";

        /// <summary>
        /// Key of the VisualState when expander is collapsed and expander direction is set to Left
        /// </summary>
        private const string StateContentCollapsedLeft = "CollapsedLeft";

        /// <summary>
        /// Key of the VisualState when expander is collapsed and expander direction is set to Down
        /// </summary>
        private const string StateContentCollapsedDown = "CollapsedDown";

        /// <summary>
        /// Key of the VisualState when expander is collapsed and expander direction is set to Right
        /// </summary>
        private const string StateContentCollapsedRight = "CollapsedRight";

        /// <summary>
        /// Key of the VisualState when expander is collapsed and expander direction is set to Up
        /// </summary>
        private const string StateContentCollapsedUp = "CollapsedUp";

        /// <summary>
        /// Key of the UI Element that toggle IsExpanded property
        /// </summary>
        private const string ExpanderToggleButtonPart = "PART_ExpanderToggleButton";

        /// <summary>
        /// Key of the UI Element that contains the content of the control that is expanded/collapsed
        /// </summary>
        private const string MainContentPart = "PART_MainContent";

        /// <summary>
        /// Key of the VisualStateGroup that set expander direction of the control
        /// </summary>
        private const string ExpandDirectionGroupStateContent = "ExpandDirectionStates";

        /// <summary>
        /// Key of the VisualState when expander direction is set to Left
        /// </summary>
        private const string StateContentLeftDirection = "LeftDirection";

        /// <summary>
        /// Key of the VisualState when expander direction is set to Down
        /// </summary>
        private const string StateContentDownDirection = "DownDirection";

        /// <summary>
        /// Key of the VisualState when expander direction is set to Right
        /// </summary>
        private const string StateContentRightDirection = "RightDirection";

        /// <summary>
        /// Key of the VisualState when expander direction is set to Up
        /// </summary>
        private const string StateContentUpDirection = "UpDirection";

        /// <summary>
        /// Key of the UI Element that contains the content of the entire control
        /// </summary>
        private const string RootGridPart = "PART_RootGrid";

        /// <summary>
        /// Key of the UI Element that contains the content of the LayoutTransformer (of the expander button)
        /// </summary>
        private const string LayoutTransformerPart = "PART_LayoutTransformer";

        /// <summary>
        /// Key of the UI Element that contains the content of the control that is visible in Overlay mode
        /// </summary>
        private const string ContentOverlayPart = "PART_ContentOverlay";
    }
}