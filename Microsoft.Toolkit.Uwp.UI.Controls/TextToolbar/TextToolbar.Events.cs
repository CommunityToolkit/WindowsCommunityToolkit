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
    using System.Linq;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    public partial class TextToolbar
    {
        /// <summary>
        /// Collects Default Button Set from the Formatter, or loads a Custom Set.
        /// </summary>
        /// <param name="root">Root Control</param>
        private void LoadDefaultButtonMap(CommandBar root)
        {
            if (DefaultButtons == null)
            {
                DefaultButtons = Formatter.DefaultButtons;
            }

            AttachButtonMap(DefaultButtons, root);
        }

        /// <summary>
        /// Adds all of the ToolbarButtons to the Root Control
        /// </summary>
        /// <param name="map">Collection of Buttons to add</param>
        /// <param name="root">Root Control</param>
        private void AttachButtonMap(ButtonMap map, CommandBar root)
        {
            foreach (var item in map)
            {
                if (!root.PrimaryCommands.Contains(item))
                {
                    if (item.Position != -1)
                    {
                        root.PrimaryCommands.Insert(item.Position, item);
                    }
                    else
                    {
                        root.PrimaryCommands.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Removes one of the Default Buttons from the Toolbar
        /// </summary>
        /// <param name="button">Button to Remove</param>
        public void RemoveDefaultButton(DefaultButton button)
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                var element = root.PrimaryCommands.FirstOrDefault(item => ((FrameworkElement)item).Name == button.ToString());
                root.PrimaryCommands.Remove(element);
            }

            if (!RemoveDefaultButtons.Contains(button))
            {
                RemoveDefaultButtons.Add(button);
            }
        }
    }
}