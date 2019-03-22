// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Strings for TextToolbar Tooltips and UI.
    /// </summary>
    public partial class TextToolbarStrings
    {
        /// <summary>
        /// Gets or sets the label for <see cref="BoldLabel"/> .
        /// </summary>
        public string BoldLabel
        {
            get => _boldLabel;
            set => _boldLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="ItalicsLabel"/>
        /// </summary>
        public string ItalicsLabel
        {
            get => _italicsLabel;
            set => _italicsLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="StrikethroughLabel"/>.
        /// </summary>
        public string StrikethroughLabel
        {
            get => _strikethroughLabel;
            set => _strikethroughLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="QuoteLabel"/>.
        /// </summary>
        public string QuoteLabel
        {
            get => _quoteLabel;
            set => _quoteLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="CodeLabel"/>
        /// </summary>
        public string CodeLabel
        {
            get => _codeLabel;
            set => _codeLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="ListLabel"/>
        /// </summary>
        public string ListLabel
        {
            get => _listLabel;
            set => _listLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="OrderedListLabel"/>
        /// </summary>
        public string OrderedListLabel
        {
            get => _orderedListlabel;
            set => _orderedListlabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="LinkLabel"/>
        /// </summary>
        public string LinkLabel
        {
            get => _linkLabel;
            set => _linkLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="CreateLinkLabel"/>
        /// </summary>
        public string CreateLinkLabel
        {
            get => _createLinkLabel;
            set => _createLinkLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="UrlLabel"/>
        /// </summary>
        public string UrlLabel
        {
            get => _urlLabel;
            set => _urlLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="LabelLabel"/>
        /// </summary>
        public string LabelLabel
        {
            get => _labelLabel;
            set => _labelLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="OkLabel"/>
        /// </summary>
        public string OkLabel
        {
            get => _okLabel;

            set => _okLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="CancelLabel"/>
        /// </summary>
        public string CancelLabel
        {
            get => _cancelLabel;
            set => _cancelLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="UnderlineLabel"/>
        /// </summary>
        public string UnderlineLabel
        {
            get => _underlineLabel;
            set => _underlineLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="HeaderLabel"/>
        /// </summary>
        public string HeaderLabel
        {
            get => _headerLabel;
            set => _headerLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="EmptyTextLabel"/>
        /// </summary>
        public string EmptyTextLabel
        {
            get => _emptyTextLabel;
            set => _emptyTextLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="LinkInvalidLabel"/>
        /// </summary>
        public string LinkInvalidLabel
        {
            get => _linkInvalidLabel;
            set => _linkInvalidLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="WarningLabel"/>
        /// </summary>
        public string WarningLabel
        {
            get => _warningLabel;
            set => _warningLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="RelativeLabel"/>
        /// </summary>
        public string RelativeLabel
        {
            get => _relativeLabel;
            set => _relativeLabel = value;
        }

        private string _okLabel = "Ok";
        private string _boldLabel = "Bold";
        private string _italicsLabel = "Italics";
        private string _strikethroughLabel = "Strikethrough";
        private string _quoteLabel = "Quote";
        private string _codeLabel = "Code";
        private string _listLabel = "List";
        private string _orderedListlabel = "Ordered List";
        private string _linkLabel = "Link";
        private string _createLinkLabel = "Create Link";
        private string _urlLabel = "Url";
        private string _labelLabel = "Label";
        private string _cancelLabel = "Cancel";
        private string _underlineLabel = "Underline";
        private string _headerLabel = "Header";
        private string _emptyTextLabel = "Empty Text";
        private string _linkInvalidLabel = "Link invalid";
        private string _warningLabel = "Warning";
        private string _relativeLabel = "Relative";
    }
}