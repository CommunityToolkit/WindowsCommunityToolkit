namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    [TemplatePart(Name = RootControl, Type = typeof(CommandBar))]
    [TemplatePart(Name = BoldElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = ItalicsElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = StrikethoughElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = CodeElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = QuoteElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = LinkElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = ListElement, Type = typeof(AppBarButton))]
    [TemplatePart(Name = OrderedElement, Type = typeof(AppBarButton))]
    public partial class TextToolbar : Control
    {
        private const string RootControl = "Root";
        private const string BoldElement = "Bold";
        private const string ItalicsElement = "Italics";
        private const string StrikethoughElement = "Strikethrough";
        private const string CodeElement = "Code";
        private const string QuoteElement = "Quote";
        private const string LinkElement = "Link";
        private const string ListElement = "List";
        private const string OrderedElement = "OrderedList";

        public Style SeparatorStyle { get; set; }

        public Style ButtonStyle { get; set; }

        public TextToolbar()
        {
            this.DefaultStyleKey = typeof(TextToolbar);
        }

        /// <summary>
        /// Gets the default Style Dictionary to get the Button Styles.
        /// </summary>
        private ResourceDictionary DefaultDictionary
        {
            get { return new ResourceDictionary { Source = new Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/TextToolbar/TextToolbar.xaml") }; }
        }

        protected override void OnApplyTemplate()
        {
            if (Formatter == null)
            {
                switch (Formatting)
                {
                    case Format.MarkDown:
                        Formatter = new MarkDownFormatter(this);
                        break;

                    case Format.RichText:
                        Formatter = new RichTextFormatter(this);
                        break;

                    case Format.Custom:
                        throw new NullReferenceException("Custom Formatter was null");
                }
            }

            if (GetTemplateChild(RootControl) is CommandBar root)
            {
                foreach (var button in removedButtons)
                {
                    RemoveDefaultButton(button);
                }

                if (CustomItems != null)
                {
                    SeparatorStyle = DefaultDictionary["ToolbarSeparator"] as Style;
                    ButtonStyle = DefaultDictionary["ToolbarButton"] as Style;

                    foreach (var item in CustomItems)
                    {
                        AddCustomButton(root, item);
                    }
                }
            }

            AttachHandlers();

            base.OnApplyTemplate();
        }
    }
}