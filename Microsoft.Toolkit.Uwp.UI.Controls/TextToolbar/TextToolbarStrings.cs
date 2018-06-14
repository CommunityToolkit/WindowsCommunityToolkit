// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Strings for TextToolbar Tooltips and UI.
    /// </summary>
    public partial class TextToolbarStrings : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="BoldLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BoldLabelProperty =
            DependencyProperty.Register(nameof(BoldLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Bold"));

        /// <summary>
        /// Identifies the <see cref="ItalicsLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItalicsLabelProperty =
            DependencyProperty.Register(nameof(ItalicsLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Italics"));

        /// <summary>
        /// Identifies the <see cref="StrikethroughLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrikethroughLabelProperty =
            DependencyProperty.Register(nameof(StrikethroughLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Strikethrough"));

        /// <summary>
        /// Identifies the <see cref="QuoteLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty QuoteLabelProperty =
            DependencyProperty.Register(nameof(QuoteLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Quote"));

        /// <summary>
        /// Identifies the <see cref="CodeLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CodeLabelProperty =
            DependencyProperty.Register(nameof(CodeLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Code"));

        /// <summary>
        /// Identifies the <see cref="ListLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListLabelProperty =
            DependencyProperty.Register(nameof(ListLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("List"));

        /// <summary>
        /// Identifies the <see cref="OrderedListLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrderedListLabelProperty =
            DependencyProperty.Register(nameof(OrderedListLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Ordered List"));

        /// <summary>
        /// Identifies the <see cref="LinkLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkLabelProperty =
            DependencyProperty.Register(nameof(LinkLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Link"));

        /// <summary>
        /// Identifies the <see cref="CreateLinkLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreateLinkLabelProperty =
            DependencyProperty.Register(nameof(CreateLinkLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Create Link"));

        /// <summary>
        /// Identifies the <see cref="UrlLabelProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UrlLabelProperty =
            DependencyProperty.Register(nameof(UrlLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Url"));

        /// <summary>
        /// Identifies the <see cref="LabelLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelLabelProperty =
            DependencyProperty.Register(nameof(LabelLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Label"));

        /// <summary>
        /// Identifies the <see cref="OkLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OkLabelProperty =
            DependencyProperty.Register(nameof(OkLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("OK"));

        /// <summary>
        /// Identifies the <see cref="CancelLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelLabelProperty =
            DependencyProperty.Register(nameof(CancelLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Cancel"));

        /// <summary>
        /// Identifies the <see cref="UnderlineLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnderlineLabelProperty =
            DependencyProperty.Register(nameof(UnderlineLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Underline"));

        /// <summary>
        /// Identifies the <see cref="HeaderLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderLabelProperty =
            DependencyProperty.Register(nameof(HeaderLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Header"));

        /// <summary>
        /// Identifies the <see cref="EmptyTextLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EmptyTextLabelProperty =
            DependencyProperty.Register(nameof(EmptyTextLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Label cannot be Empty"));

        /// <summary>
        /// Identifies the <see cref="LinkInvalidLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkInvalidLabelProperty =
            DependencyProperty.Register(nameof(LinkInvalidLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Link Format is Invaild"));

        /// <summary>
        /// Identifies the <see cref="WarningLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WarningLabelProperty =
            DependencyProperty.Register(nameof(WarningLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Warning"));

        /// <summary>
        /// Identifies the <see cref="RelativeLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeLabelProperty =
            DependencyProperty.Register(nameof(RelativeLabel), typeof(string), typeof(TextToolbarStrings), new PropertyMetadata("Relative"));

        /// <summary>
        /// Gets or sets the label for Bold
        /// </summary>
        public string BoldLabel
        {
            get { return (string)GetValue(BoldLabelProperty); }
            set { SetValue(BoldLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Italics
        /// </summary>
        public string ItalicsLabel
        {
            get { return (string)GetValue(ItalicsLabelProperty); }
            set { SetValue(ItalicsLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Strikethrough
        /// </summary>
        public string StrikethroughLabel
        {
            get { return (string)GetValue(StrikethroughLabelProperty); }
            set { SetValue(StrikethroughLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Quote
        /// </summary>
        public string QuoteLabel
        {
            get { return (string)GetValue(QuoteLabelProperty); }
            set { SetValue(QuoteLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Code
        /// </summary>
        public string CodeLabel
        {
            get { return (string)GetValue(CodeLabelProperty); }
            set { SetValue(CodeLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for List
        /// </summary>
        public string ListLabel
        {
            get { return (string)GetValue(ListLabelProperty); }
            set { SetValue(ListLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for OrderedList
        /// </summary>
        public string OrderedListLabel
        {
            get { return (string)GetValue(OrderedListLabelProperty); }
            set { SetValue(OrderedListLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Link
        /// </summary>
        public string LinkLabel
        {
            get { return (string)GetValue(LinkLabelProperty); }
            set { SetValue(LinkLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for CreateLink
        /// </summary>
        public string CreateLinkLabel
        {
            get { return (string)GetValue(CreateLinkLabelProperty); }
            set { SetValue(CreateLinkLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Url
        /// </summary>
        public string UrlLabel
        {
            get { return (string)GetValue(UrlLabelProperty); }
            set { SetValue(UrlLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Label
        /// </summary>
        public string LabelLabel
        {
            get { return (string)GetValue(LabelLabelProperty); }
            set { SetValue(LabelLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for OK
        /// </summary>
        public string OkLabel
        {
            get { return (string)GetValue(OkLabelProperty); }
            set { SetValue(OkLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Cancel
        /// </summary>
        public string CancelLabel
        {
            get { return (string)GetValue(CancelLabelProperty); }
            set { SetValue(CancelLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Underline
        /// </summary>
        public string UnderlineLabel
        {
            get { return (string)GetValue(UnderlineLabelProperty); }
            set { SetValue(UnderlineLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Header
        /// </summary>
        public string HeaderLabel
        {
            get { return (string)GetValue(HeaderLabelProperty); }
            set { SetValue(HeaderLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for EmptyTextLabel
        /// </summary>
        public string EmptyTextLabel
        {
            get { return (string)GetValue(EmptyTextLabelProperty); }
            set { SetValue(EmptyTextLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for LinkInvalid
        /// </summary>
        public string LinkInvalidLabel
        {
            get { return (string)GetValue(LinkInvalidLabelProperty); }
            set { SetValue(LinkInvalidLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Warning
        /// </summary>
        public string WarningLabel
        {
            get { return (string)GetValue(WarningLabelProperty); }
            set { SetValue(WarningLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for Relative
        /// </summary>
        public string RelativeLabel
        {
            get { return (string)GetValue(RelativeLabelProperty); }
            set { SetValue(RelativeLabelProperty, value); }
        }
    }
}