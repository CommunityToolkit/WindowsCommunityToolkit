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
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.System;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    public partial class TextToolbar
    {
        /// <summary>
        /// Attaches/Detaches the Key events for Shortcuts
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        public static void OnEditorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is TextToolbar bar)
            {
                if (args.OldValue is RichEditBox oldEditor)
                {
                    oldEditor.KeyDown -= bar.Editor_KeyDown;
                }

                if (args.NewValue is RichEditBox newEditor)
                {
                    // var keydownField = newEditor.GetType().GetField("KeyDown", BindingFlags.Instance | BindingFlags.NonPublic);
                    // Figure out how to remove the default event handler from the invocation list, if a format such as Markdown is selected, so I can free Shortcuts such as CTRL + R and CTRL + L, as Markdown doesn't support Right to Left Text anyways.
                    newEditor.KeyDown += bar.Editor_KeyDown;
                }
            }
        }

        /// <summary>
        /// Checks if a Shortcut Combination is pressed, by going through the list of attached buttons.
        /// </summary>
        /// <param name="sender">RichEditBox</param>
        /// <param name="e">Key args</param>
        private void Editor_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                if (ControlKeyDown)
                {
                    var args = new ShortcutKeyRequestArgs(e.Key, ShiftKeyDown);
                    foreach (var item in root.PrimaryCommands)
                    {
                        if (item is ToolbarButton button && !args.Handled)
                        {
                            button.ShortcutRequested(ref args);
                        }
                    }

                    e.Handled = args.Handled;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether Control is pressed down
        /// </summary>
        public bool ControlKeyDown
        {
            get { return IsKeyActive(CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control)); }
        }

        /// <summary>
        /// Gets a value indicating whether Shift is pressed down
        /// </summary>
        public bool ShiftKeyDown
        {
            get { return IsKeyActive(CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift)); }
        }

        /// <summary>
        /// Creates a new formatter, if it is a built-in formatter.
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        public static void OnFormatTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is TextToolbar bar)
            {
                bar.CreateFormatter();
            }
        }

        /// <summary>
        /// Rebuilds the Toolbar if the formatter changes during operation
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        public static void OnFormatterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is TextToolbar bar)
            {
                bar.BuildBar();
            }
        }

        /// <summary>
        /// Resets removed entries, and Reloads the Toolbar Entries
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        public static void OnButtonMapChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is TextToolbar bar)
            {
                if (args.OldValue is ButtonMap oldSource)
                {
                    oldSource.CollectionChanged -= bar.OnButtonMapModified;

                    if (bar.GetTemplateChild(RootControl) is CommandBar root)
                    {
                        foreach (IToolbarItem item in oldSource)
                        {
                            bar.RemoveToolbarItem(item, root);
                        }
                    }
                }

                if (args.NewValue is ButtonMap newSource)
                {
                    newSource.CollectionChanged += bar.OnButtonMapModified;

                    if (bar.GetTemplateChild(RootControl) is CommandBar root)
                    {
                        bar.AttachButtonMap(newSource, root);
                    }
                }
            }
        }

        /// <summary>
        /// Adds new Buttons, or Removes removed buttons, if modified during operation.
        /// </summary>
        /// <param name="sender">ButtonMap</param>
        /// <param name="e">Collection Changed Args</param>
        private void OnButtonMapModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IToolbarItem item in e.NewItems)
                        {
                            AddToolbarItem(item, root);

                            if (item is ToolbarButton button)
                            {
                                button.PropertyChanged += ToolbarItemPropertyChanged;
                            }
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (IToolbarItem item in e.OldItems)
                        {
                            RemoveToolbarItem(item, root);

                            if (item is ToolbarButton button)
                            {
                                button.PropertyChanged -= ToolbarItemPropertyChanged;
                            }
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Resets removed entries, and Reloads the Toolbar Entries
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        public static void OnRemoveButtonsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is TextToolbar bar)
            {
                if (args.OldValue is ObservableCollection<DefaultButton> oldSource)
                {
                    oldSource.CollectionChanged -= bar.OnRemoveButtonsModified;

                    if (bar.GetTemplateChild(RootControl) is CommandBar root)
                    {
                        foreach (DefaultButton item in oldSource)
                        {
                            bar.AddToolbarItem(item.Button, root);
                        }
                    }
                }

                if (args.NewValue is ObservableCollection<DefaultButton> newSource)
                {
                    newSource.CollectionChanged += bar.OnRemoveButtonsModified;

                    foreach (DefaultButton item in newSource)
                    {
                        bar.RemoveDefaultButton(item);
                    }
                }
            }
        }

        /// <summary>
        /// Removes Default Buttons if Added, Adds them back if Removed.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="e">Property Changed Args</param>
        private void OnRemoveButtonsModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (DefaultButton item in e.NewItems)
                    {
                        RemoveDefaultButton(item);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (GetTemplateChild(RootControl) is CommandBar root)
                    {
                        foreach (DefaultButton item in e.OldItems)
                        {
                            AddToolbarItem(item.Button, root);
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Ensures that the Toolbar updates the position of a Toolbar Item if it's position changes.
        /// </summary>
        /// <param name="sender">Toolbar Button</param>
        /// <param name="e">Property Changed Event</param>
        private void ToolbarItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                if (e.PropertyName == nameof(IToolbarItem.Position))
                {
                    MoveToolbarItem(sender as IToolbarItem, root);
                }
            }
        }
    }
}