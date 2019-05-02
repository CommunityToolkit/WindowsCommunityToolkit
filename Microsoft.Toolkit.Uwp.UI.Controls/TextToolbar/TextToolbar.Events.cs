// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
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
        private static void OnEditorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (InDesignMode)
            {
                return;
            }

            var bar = obj as TextToolbar;
            if (bar != null)
            {
                var oldEditor = args.OldValue as RichEditBox;
                var newEditor = args.NewValue as RichEditBox;

                if (oldEditor != null)
                {
                    oldEditor.RemoveHandler(KeyDownEvent, bar.KeyEventHandler);
                }

                if (newEditor != null)
                {
                    newEditor.AddHandler(KeyDownEvent, bar.KeyEventHandler, handledEventsToo: true);
                    bar.CreateFormatter();
                }

                var editorArgs = new EditorChangedArgs
                {
                    Old = oldEditor,
                    New = newEditor
                };

                bar.EditorChanged?.Invoke(bar, editorArgs);
            }
        }

        /// <summary>
        /// Creates a new formatter, if it is a built-in formatter.
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        private static void OnFormatTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
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
        private static void OnFormatterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var bar = obj as TextToolbar;
            if (bar != null && bar.Formatter != null)
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
        private static void OnButtonMapChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
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
                            bar.RemoveToolbarItem(item);
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
        /// Resets removed entries, and Reloads the Toolbar Entries
        /// </summary>
        /// <param name="obj">TextToolbar</param>
        /// <param name="args">Property Changed Args</param>
        private static void OnDefaultButtonModificationsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var bar = obj as TextToolbar;
            if (bar != null)
            {
                var oldSource = args.OldValue as DefaultButtonModificationList;
                var newSource = args.NewValue as DefaultButtonModificationList;
                var root = bar.GetTemplateChild(RootControl) as CommandBar;

                if (oldSource != null)
                {
                    oldSource.CollectionChanged -= bar.OnDefaultButtonModificationListChanged;
                }

                if (newSource != null)
                {
                    newSource.CollectionChanged += bar.OnDefaultButtonModificationListChanged;

                    foreach (DefaultButton item in newSource)
                    {
                        var element = bar.GetDefaultButton(item.Type);
                        item.Button = element;
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
                            RemoveToolbarItem(item);

                            var button = item as ToolbarButton;
                            if (button != null)
                            {
                                button.PropertyChanged -= ToolbarItemPropertyChanged;
                            }
                        }

                        break;

                    case NotifyCollectionChangedAction.Reset:
                        BuildBar();
                        break;
                }
            }
        }

        /// <summary>
        /// Default Button Modification Instances
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="e">Property Changed Args</param>
        private void OnDefaultButtonModificationListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (DefaultButton item in e.NewItems)
                    {
                        var element = GetDefaultButton(item.Type);
                        item.Button = element;
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

        /// <summary>
        /// Checks if a Shortcut Combination is pressed, by going through the list of attached buttons.
        /// </summary>
        /// <param name="sender">RichEditBox</param>
        /// <param name="e">Key args</param>
        private void Editor_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (InDesignMode)
            {
                return;
            }

            LastKeyPress = e.Key;

            var root = GetTemplateChild(RootControl) as CommandBar;
            if (root != null)
            {
                if (ControlKeyDown && e.Key != VirtualKey.Control)
                {
                    var key = FindBestAlternativeKey(e.Key);

                    var matchingButtons = root.PrimaryCommands.OfType<ToolbarButton>().Where(item => item.ShortcutKey == key);
                    if (matchingButtons.Any())
                    {
                        if (e.Handled)
                        {
                            Editor.Document.Undo();
                            if (string.IsNullOrWhiteSpace(Editor.Document.Selection.Text))
                            {
                                Editor.Document.Redo();
                            }
                        }

                        var args = new ShortcutKeyRequestArgs(key, ShiftKeyDown, e);
                        foreach (var button in matchingButtons)
                        {
                            if (button != null && !args.Handled)
                            {
                                button.ShortcutRequested(ref args);
                            }
                        }

                        ShortcutRequested?.Invoke(this, args);
                        if (args.Handled)
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private KeyEventHandler KeyEventHandler { get; set; }

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
        /// Fired when the RichEditBox Instance Changes.
        /// </summary>
        public event EventHandler<EditorChangedArgs> EditorChanged;
    }
}