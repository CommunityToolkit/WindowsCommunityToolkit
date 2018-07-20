// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.ResourceDictionary"/>
    /// </summary>
    public class ResourceDictionary
    {
        internal global::Windows.UI.Xaml.ResourceDictionary UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDictionary"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.ResourceDictionary"/>
        /// </summary>
        public ResourceDictionary(global::Windows.UI.Xaml.ResourceDictionary instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.ResourceDictionary.Source"/>
        /// </summary>
        public System.Uri Source
        {
            get => UwpInstance.Source;
            set => UwpInstance.Source = value;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.ResourceDictionary.MergedDictionaries"/>
        /// </summary>
        public System.Collections.Generic.IList<Microsoft.Toolkit.Win32.UI.Controls.WPF.ResourceDictionary> MergedDictionaries
        {
            get => UwpInstance.MergedDictionaries.Cast<Microsoft.Toolkit.Win32.UI.Controls.WPF.ResourceDictionary>().ToList();
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.ResourceDictionary.ThemeDictionaries"/>
        /// </summary>
        public System.Collections.Generic.IDictionary<object, object> ThemeDictionaries
        {
            get => UwpInstance.ThemeDictionaries;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.ResourceDictionary"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.ResourceDictionary"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.ResourceDictionary"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ResourceDictionary(
            global::Windows.UI.Xaml.ResourceDictionary args)
        {
            return FromResourceDictionary(args);
        }

        /// <summary>
        /// Creates a <see cref="ResourceDictionary"/> from <see cref="global::Windows.UI.Xaml.ResourceDictionary"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.ResourceDictionary"/> instance containing the event data.</param>
        /// <returns><see cref="ResourceDictionary"/></returns>
        public static ResourceDictionary FromResourceDictionary(global::Windows.UI.Xaml.ResourceDictionary args)
        {
            return new ResourceDictionary(args);
        }
    }
}