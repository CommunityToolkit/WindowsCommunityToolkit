// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements
{
    internal sealed class Element_AdaptiveText : IElement_TileBindingChild, IElement_AdaptiveSubgroupChild, IElement_ToastBindingChild, IHaveXmlName, IHaveXmlNamedProperties, IHaveXmlText
    {
        internal const AdaptiveTextStyle DEFAULT_STYLE = AdaptiveTextStyle.Default;
        internal const AdaptiveTextAlign DEFAULT_ALIGN = AdaptiveTextAlign.Default;
        internal const AdaptiveTextPlacement DEFAULT_PLACEMENT = AdaptiveTextPlacement.Inline;

        public string Text { get; set; }

        public int? Id { get; set; }

        public string Lang { get; set; }

        public AdaptiveTextAlign Align { get; set; } = DEFAULT_ALIGN;

        private int? _maxLines;

        public int? MaxLines
        {
            get
            {
                return _maxLines;
            }

            set
            {
                if (value != null)
                {
                    CheckMaxLinesValue(value.Value);
                }

                _maxLines = value;
            }
        }

        internal static void CheckMaxLinesValue(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("MaxLines must be between 1 and int.MaxValue, inclusive.");
            }
        }

        private int? _minLines;

        public int? MinLines
        {
            get
            {
                return _minLines;
            }

            set
            {
                if (value != null)
                {
                    CheckMinLinesValue(value.Value);
                }

                _minLines = value;
            }
        }

        internal static void CheckMinLinesValue(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("MinLines must be between 1 and int.MaxValue, inclusive.");
            }
        }

        public AdaptiveTextStyle Style { get; set; } = DEFAULT_STYLE;

        public bool? Wrap { get; set; }

        public AdaptiveTextPlacement Placement { get; set; } = DEFAULT_PLACEMENT;

        /// <inheritdoc/>
        string IHaveXmlName.Name => "text";

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("id", Id);
            yield return new("lang", Lang);

            if (Align != DEFAULT_ALIGN)
            {
                yield return new("hint-align", Align.ToPascalCaseString());
            }

            yield return new("hint-maxLines", MaxLines);
            yield return new("hint-minLines", MinLines);

            if (Style != DEFAULT_STYLE)
            {
                yield return new("hint-style", Style.ToPascalCaseString());
            }

            yield return new("hint-wrap", Wrap);

            if (Placement != DEFAULT_PLACEMENT)
            {
                yield return new("placement", Placement.ToPascalCaseString());
            }
        }
    }
}