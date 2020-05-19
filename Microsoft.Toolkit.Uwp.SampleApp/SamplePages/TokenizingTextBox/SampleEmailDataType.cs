// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample of strongly-typed email address simulated data for <see cref="TokenizingTextBox"/>.
    /// </summary>
    public class SampleEmailDataType
    {
        /// <summary>
        /// Gets or sets symbol to display.
        /// </summary>
        public Symbol Icon { get; set; }

        /// <summary>
        /// Gets or sets the first name .
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the family name .
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, FamilyName);
            }
        }

        /// <summary>
        /// Gets the formatted email address
        /// </summary>
        public string EmailAddress
        {
            get
            {
                return string.Format("{0} <{1}.{2}@contoso.com>", DisplayName, FirstName, FamilyName);
            }
        }

        public override string ToString()
        {
            return EmailAddress;
        }
    }
}