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
        public const string GroupContent = "ContentStates";
        public const string StateContentExpanded = "ContentExpanded";
        public const string StateContentCollapsed = "ContentCollapsed";

        public const string ExpanderToggleButtonPart = "PART_ExpanderToggleButton";
        public const string HeaderButtonPart = "PART_HeaderButton";
        public const string MainContentRowPart = "PART_MainContentRow";

        public const string OpenCloseStoryboardPart = "PART_OpenCloseStoryboard";
        public const string OpenCloseAnimationPart = "PART_OpenCloseAnimation";
    }
}