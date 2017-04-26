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
    using System;
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
            if (obj is TextToolbar)
            {
                var bar = obj as TextToolbar;

                var oldEditor = args.OldValue as RichEditBox;
                var newEditor = args.NewValue as RichEditBox;

                if (oldEditor != null)
                {
                    oldEditor.KeyDown -= bar.Editor_KeyDown;
                }

                if (newEditor != null)
                {
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
            var root = GetTemplateChild(RootControl) as CommandBar;
            if (root != null)
            {
                if (ControlKeyDown)
                {
                    var args = new ShortcutKeyRequestArgs(e.Key, ShiftKeyDown, e);
                    foreach (var item in root.PrimaryCommands)
                    {
                        var button = item as ToolbarButton;
                        if (button != null && !args.Handled)
                        {
                            button.ShortcutRequested(ref args);
                        }
                    }

                    ShortcutRequested?.Invoke(this, args);
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
        /// Fired when a CTRL + "Letter" combination is used inside the Editor.
        /// </summary>
        public event EventHandler<ShortcutKeyRequestArgs> ShortcutRequested;

        /// <summary>
        /// Creates a new formatter, if it is a built-in formatter.
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        public static void OnFormatTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var bar = obj as TextToolbar;
            if (bar != null)
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
            var bar = obj as TextToolbar;
            if (bar != null)
            {
                bar.DefaultButtons = bar.Formatter.DefaultButtons;
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
            var bar = obj as TextToolbar;
            if (bar != null)
            {
                var oldSource = args.OldValue as ButtonMap;
                var newSource = args.NewValue as ButtonMap;
                var root = bar.GetTemplateChild(RootControl) as CommandBar;

                if (oldSource != null)
                {
                    oldSource.CollectionChanged -= bar.OnButtonMapModified;

                    if (root != null)
                    {
                        foreach (IToolbarItem item in oldSource)
                        {
                            bar.RemoveToolbarItem(item, root);
                        }
                    }
                }

                if (newSource != null)
                {
                    newSource.CollectionChanged += bar.OnButtonMapModified;

                    if (root != null)
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
            var root = GetTemplateChild(RootControl) as CommandBar;
            if (root != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IToolbarItem item in e.NewItems)
                        {
                            AddToolbarItem(item, root);

                            var button = item as ToolbarButton;
                            if (button != null)
                            {
                                button.PropertyChanged += ToolbarItemPropertyChanged;
                            }
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (IToolbarItem item in e.OldItems)
                        {
                            RemoveToolbarItem(item, root);

                            var button = item as ToolbarButton;
                            if (button != null)
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
            var bar = obj as TextToolbar;
            if (bar != null)
            {
                var oldSource = args.OldValue as ObservableCollection<DefaultButton>;
                var newSource = args.NewValue as ObservableCollection<DefaultButton>;
                var root = bar.GetTemplateChild(RootControl) as CommandBar;

                if (oldSource != null)
                {
                    oldSource.CollectionChanged -= bar.OnRemoveButtonsModified;

                    if (root != null)
                    {
                        foreach (DefaultButton item in oldSource)
                        {
                            bar.AddToolbarItem(item.Button, root);
                        }
                    }
                }

                if (newSource != null)
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
                    var root = GetTemplateChild(RootControl) as CommandBar;
                    if (root != null)
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
            var root = GetTemplateChild(RootControl) as CommandBar;
            if (root != null)
            {
                if (e.PropertyName == nameof(IToolbarItem.Position))
                {
                    MoveToolbarItem(sender as IToolbarItem, root);
                }
            }
        }
    }
}