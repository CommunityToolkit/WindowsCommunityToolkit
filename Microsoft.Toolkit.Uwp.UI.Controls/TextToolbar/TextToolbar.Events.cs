namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    public partial class TextToolbar
    {
        public void AttachHandlers()
        {
            DetermineAttach(BoldElement, MakeBold);
            DetermineAttach(ItalicsElement, MakeItalics);
            DetermineAttach(StrikethoughElement, MakeStrike);

            DetermineAttach(CodeElement, MakeCode);
            DetermineAttach(QuoteElement, MakeQuote);
            DetermineAttach(LinkElement, MakeLink);

            DetermineAttach(ListElement, MakeList);
            DetermineAttach(OrderedElement, MakeOList);

            CustomItems.CollectionChanged += CustomItems_CollectionChanged;
        }

        public void DettachHandlers()
        {
            DetermineDettach(BoldElement, MakeBold);
            DetermineDettach(ItalicsElement, MakeItalics);
            DetermineDettach(StrikethoughElement, MakeStrike);

            DetermineDettach(CodeElement, MakeCode);
            DetermineDettach(QuoteElement, MakeQuote);
            DetermineDettach(LinkElement, MakeLink);

            DetermineDettach(ListElement, MakeList);
            DetermineDettach(OrderedElement, MakeOList);

            CustomItems.CollectionChanged -= CustomItems_CollectionChanged;
        }

        private void CustomItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems)
                        {
                            AddCustomButton(root, item as CustomToolbarItem);
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                        {
                            RemoveCustomButton(root, item as CustomToolbarItem);
                        }

                        break;
                }
            }
        }

        private void AddCustomButton(CommandBar root, CustomToolbarItem item)
        {
            if (GetIfAttached(root, item) != null)
            {
                return;
            }

            ICommandBarElement element = null;
            switch (item)
            {
                case CustomToolbarSeparator separator:
                    element = new AppBarSeparator { Style = SeparatorStyle, Tag = item };
                    break;

                case CustomToolbarButton button:
                    var content = new AppBarButton { Icon = button.Icon, Style = ButtonStyle, Tag = item };
                    content.Click += (s, e) => { button.Action(); };
                    ToolTipService.SetToolTip(content, button.Label);
                    element = content;
                    break;
            }

            if (item.Position.HasValue)
            {
                root.PrimaryCommands.Insert(item.Position.Value, element);
            }
            else
            {
                root.PrimaryCommands.Add(element);
            }
        }

        private void RemoveCustomButton(CommandBar root, CustomToolbarItem item)
        {
            var control = GetIfAttached(root, item);
            if (control != null)
            {
                root.PrimaryCommands.Remove(control);
            }
        }

        private ICommandBarElement GetIfAttached(CommandBar root, CustomToolbarItem item)
        {
            return root.PrimaryCommands.FirstOrDefault(element => ((FrameworkElement)element).Tag == item);
        }

        /// <summary>
        /// Removes one of the Default Buttons from the Toolbar
        /// </summary>
        /// <param name="button">Button to Remove</param>
        public void RemoveDefaultButton(DefaultButton button)
        {
            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                var element = GetTemplateChild(button.ToString()) as ICommandBarElement;
                root.PrimaryCommands.Remove(element);
            }

            if (!removedButtons.Contains(button))
            {
                removedButtons.Add(button);
            }
        }

        private void MakeBold(object sender, RoutedEventArgs args)
        {
            Formatter.FormatBold();
        }

        private void MakeItalics(object sender, RoutedEventArgs args)
        {
            Formatter.FormatItalics();
        }

        private void MakeStrike(object sender, RoutedEventArgs args)
        {
            Formatter.FormatStrikethrough();
        }

        private void MakeCode(object sender, RoutedEventArgs args)
        {
            Formatter.FormatCode();
        }

        private void MakeQuote(object sender, RoutedEventArgs args)
        {
            Formatter.FormatQuote();
        }

        private async void MakeLink(object sender, RoutedEventArgs args)
        {
            var labelBox = new TextBox { PlaceholderText = LabelLabel, Margin = new Thickness(0, 0, 0, 5) };
            var linkBox = new TextBox { PlaceholderText = UrlLabel };

            var result = await new ContentDialog
            {
                Title = CreateLinkLabel,
                Content = new StackPanel
                {
                    Children =
                    {
                        labelBox,
                        linkBox
                    }
                },
                PrimaryButtonText = OkLabel,
                SecondaryButtonText = CancelLabel
            }.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Formatter.FormatLink(labelBox.Text, linkBox.Text);
            }
        }

        private void MakeList(object sender, RoutedEventArgs args)
        {
            Formatter.FormatList();
        }

        private void MakeOList(object sender, RoutedEventArgs args)
        {
            Formatter.FormatOrderedList();
        }

        public void DetermineAttach(string partName, RoutedEventHandler handler)
        {
            if (GetTemplateChild(partName) is ButtonBase element)
            {
                element.Click += handler;
            }
        }

        public void DetermineDettach(string partName, RoutedEventHandler handler)
        {
            if (GetTemplateChild(partName) is ButtonBase element)
            {
                element.Click -= handler;
            }
        }
    }
}