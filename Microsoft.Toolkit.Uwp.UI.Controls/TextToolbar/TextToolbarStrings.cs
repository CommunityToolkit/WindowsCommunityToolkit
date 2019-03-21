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
        /// Gets or sets the label for <see cref="BoldLabel"/> .
        /// </summary>
        public string BoldLabel
        {
            get => _boldLabel;
            set => _boldLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for Italics
        /// </summary>
        public string ItalicsLabel
        {
            get => _italicsLabel;
            set => _italicsLabel = value;
        }

        /// <summary>
        /// Gets or sets the label for <see cref="StrikeThroughLabel"/>.
        /// </summary>
        public string StrikeThroughLabel
        {
            get => _strikeThroughLabel;
            set => _strikeThroughLabel = value;
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
        /// Gets or sets the label for Code
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
        /// Gets or sets the label for <see cref="OrderedList"/>
        /// </summary>
        public string OrderedList
        {
            get => _orderedList;
            set => _orderedList = value;
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

        private string _okLabel;
        private string _boldLabel;
        private string _italicsLabel;
        private string _strikeThroughLabel;
        private string _quoteLabel;
        private string _codeLabel;
        private string _listLabel;
        private string _orderedList;
        private string _linkLabel;
        private string _createLinkLabel;
        private string _urlLabel;
        private string _labelLabel;
        private string _cancelLabel;
        private string _underlineLabel;
        private string _headerLabel;
        private string _emptyTextLabel;
        private string _linkInvalidLabel;
        private string _warningLabel;
        private string _relativeLabel;
    }
}