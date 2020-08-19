// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry.Common
{
    /// <summary>
    /// Class which can be used to encapsulate code statement(s)
    /// so that they are executed in a specific culture.
    /// <para />
    /// Usage example:
    /// <para />
    /// The following code block will be executed using the French culture.
    /// <para />
    /// using (new CultureShield("fr-FR"))
    /// <para />
    /// {
    /// <para />
    ///    ...
    /// <para />
    /// }
    /// </summary>
    public class CultureShield : IDisposable
    {
        private readonly CultureInfo _prevCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureShield"/> class so that
        /// the encapsulated code statement(s) can be executed using the specified culture.
        /// <para />
        /// Usage example:
        /// <para />
        /// The following code block will be executed using the French culture.
        /// <para />
        /// using (new CultureShield("fr-FR"))
        /// <para />
        /// {
        /// <para />
        ///   ...
        /// <para />
        /// }
        /// </summary>
        /// <param name="culture">The culture in which the encapsulated code statement(s) are to be executed.</param>
        public CultureShield(string culture)
        {
            _prevCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo(culture);
        }

        /// <summary>
        /// Disposes the CultureShield object.
        /// </summary>
        public void Dispose()
        {
            CultureInfo.CurrentCulture = _prevCulture;
        }
    }
}
