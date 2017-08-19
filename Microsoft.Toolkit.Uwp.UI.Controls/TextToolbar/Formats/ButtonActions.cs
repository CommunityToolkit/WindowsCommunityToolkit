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

using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    /// <summary>
    /// The Actions taken when a button is pressed. Required for Common Button actions (Unless you override both Activation and ShiftActivation)
    /// </summary>
    public abstract class ButtonActions
    {
        /// <summary>
        /// Applies Bold
        /// </summary>
        public abstract void FormatBold(ToolbarButton button);

        /// <summary>
        /// Applies Italics
        /// </summary>
        public abstract void FormatItalics(ToolbarButton button);

        /// <summary>
        /// Applies Strikethrough
        /// </summary>
        public abstract void FormatStrikethrough(ToolbarButton button);

        /// <summary>
        /// Applies Link
        /// </summary>
        public abstract void FormatLink(ToolbarButton button, string label, string formattedLabel, string link);

        /// <summary>
        /// Applies List
        /// </summary>
        public abstract void FormatList(ToolbarButton button);

        /// <summary>
        /// Applies Ordered List
        /// </summary>
        public abstract void FormatOrderedList(ToolbarButton button);
    }
}