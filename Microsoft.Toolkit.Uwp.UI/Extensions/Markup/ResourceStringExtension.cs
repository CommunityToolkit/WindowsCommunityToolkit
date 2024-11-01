// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Xaml extension to return a <see cref="string"/> value from resource file associated with a resource key
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public sealed class ResourceStringExtension : MarkupExtension
    {
        private static readonly ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse();

        /// <summary>
        /// Gets or sets associated ID from resource strings.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the associated language resource to use (ie: "es-ES").
        /// Defaults to the OS language of current view.
        /// </summary>
        public string Language { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return GetValue(this.Name, this.Language);
        }

        /// <summary>
        /// Gets a string value from resource file associated with a resource key.
        /// </summary>
        /// <param name="name">Resource key name.</param>
        /// <returns>A string value from resource file associated with a resource key.</returns>
        public static string GetValue(string name)
        {
            // This function is needed to accomodate compiled function usage without second paramater,
            // which doesn't work with optional values.
            return resourceLoader.GetString(name);
        }

        /// <summary>
        /// Gets a string value from resource file associated with a resource key.
        /// </summary>
        /// <param name="name">Resource key name.</param>
        /// <param name="language">Optional language of the associated resource to use (ie: "es-ES").
        /// default is the OS language of current view.</param>
        /// <returns>A string value from resource file associated with a resource key.</returns>
        public static string GetValue(string name, string language = "")
        {
            if (string.IsNullOrEmpty(language))
            {
                return GetValue(name);
            }

            // Not using ResourceContext.GetForCurrentView nor GetForViewIndependentUse
            ResourceContext resourceContext = new() { Languages = new[] { language } };
            var resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

            return resourceMap.GetValue(name, resourceContext).ValueAsString;
        }

    }
}
