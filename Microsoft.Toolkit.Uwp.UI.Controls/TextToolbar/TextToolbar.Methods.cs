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
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    public partial class TextToolbar
    {
        /// <summary>
        /// Creates one of the Default formatters.
        /// </summary>
        private void CreateFormatter()
        {
            if (Format.HasValue)
            {
                switch (Format.Value)
                {
                    case TextToolbarFormats.Format.MarkDown:
                        Formatter = new MarkDownFormatter(this);
                        break;

                    case TextToolbarFormats.Format.RichText:
                        Formatter = new RichTextFormatter(this);
                        break;
                }
            }
        }

        /// <summary>
        /// Attaches all of the Default Buttons, Removing any that are to be removed, and inserting Custom buttons.
        /// </summary>
        private void BuildBar()
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                root.PrimaryCommands.Clear();

                AttachButtonMap(DefaultButtons, root);

                foreach (var button in RemoveDefaultButtons)
                {
                    RemoveDefaultButton(button);
                }

                if (CustomButtons != null)
                {
                    AttachButtonMap(CustomButtons, root);
                }
            }
            else
            {
                formatterLoadedBeforeTemplate = true;
            }
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
                AddToolbarItem(item, root);
            }
        }

        /// <summary>
        /// Adds an element to the Toolbar
        /// </summary>
        /// <param name="item">Item to add to the Toolbar</param>
        /// <param name="root">Root Control</param>
        private void AddToolbarItem(IToolbarItem item, CommandBar root)
        {
            if (item == null)
            {
                return;
            }

            if (!root.PrimaryCommands.Contains(item))
            {
                if (item is ToolbarButton button)
                {
                    button.Model = this;
                }

                if (item.Position != -1 && item.Position <= root.PrimaryCommands.Count)
                {
                    root.PrimaryCommands.Insert(item.Position, item);
                }
                else
                {
                    item.Position = root.PrimaryCommands.Count;
                    root.PrimaryCommands.Add(item);
                }
            }
        }

        /// <summary>
        /// Moves a Toolbar element to a new location on the Toolbar
        /// </summary>
        /// <param name="item">Item to Move</param>
        /// <param name="root">Root Control</param>
        private void MoveToolbarItem(IToolbarItem item, CommandBar root)
        {
            if (root.PrimaryCommands.Contains(item))
            {
                root.PrimaryCommands.Remove(item);
                root.PrimaryCommands.Insert(item.Position, item);
            }
        }

        /// <summary>
        /// Removes an Element from the Toolbar
        /// </summary>
        /// <param name="item">Item to Remove</param>
        /// <param name="root">Root Control</param>
        private void RemoveToolbarItem(IToolbarItem item, CommandBar root)
        {
            if (root.PrimaryCommands.Contains(item))
            {
                root.PrimaryCommands.Remove(item);
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
                var element = root.PrimaryCommands.FirstOrDefault(item => ((FrameworkElement)item).Name == button.ToString()) as IToolbarItem;
                button.Button = element;
                root.PrimaryCommands.Remove(element);
            }

            if (!RemoveDefaultButtons.Contains(button))
            {
                RemoveDefaultButtons.Add(button);
            }
        }

        /// <summary>
        /// Checks if the key is pressed down.
        /// </summary>
        /// <param name="state">Key State</param>
        /// <returns>Is Key pressed down?</returns>
        private static bool IsKeyActive(CoreVirtualKeyStates state)
        {
            var downAndLocked = CoreVirtualKeyStates.Down | CoreVirtualKeyStates.Locked;
            return state == CoreVirtualKeyStates.Down || state == downAndLocked;
        }
    }
}