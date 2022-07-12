// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements
{
    internal sealed class Element_AdaptiveSubgroup : IElementWithDescendants, IHaveXmlName, IHaveXmlNamedProperties, IHaveXmlChildren
    {
        internal const AdaptiveSubgroupTextStacking DEFAULT_TEXT_STACKING = AdaptiveSubgroupTextStacking.Default;

        public AdaptiveSubgroupTextStacking TextStacking { get; set; } = DEFAULT_TEXT_STACKING;

        private int? _weight;

        public int? Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                CheckWeight(value);

                _weight = value;
            }
        }

        internal static void CheckWeight(int? weight)
        {
            if (weight != null && weight.Value < 1)
            {
                throw new ArgumentOutOfRangeException("Weight must be between 1 and int.MaxValue, inclusive (or null)");
            }
        }

        public IList<IElement_AdaptiveSubgroupChild> Children { get; private set; } = new List<IElement_AdaptiveSubgroupChild>();

        public IEnumerable<object> Descendants()
        {
            foreach (IElement_AdaptiveSubgroupChild child in Children)
            {
                // Return each child (we know there's no further descendants)
                yield return child;
            }
        }

        /// <inheritdoc/>
        string IHaveXmlName.Name => "subgroup";

        /// <inheritdoc/>
        IEnumerable<object> IHaveXmlChildren.Children => Children;

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            if (TextStacking != DEFAULT_TEXT_STACKING)
            {
                yield return new("hint-textStacking", TextStacking.ToPascalCaseString());
            }

            yield return new("hint-weight", Weight);
        }
    }

    internal interface IElement_AdaptiveSubgroupChild
    {
    }
}