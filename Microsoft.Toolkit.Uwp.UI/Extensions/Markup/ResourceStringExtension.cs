// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Xaml extension to return a string value from resource file associated with a resource key
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public sealed class ResourceStringExtension : MarkupExtension
    {
        private static ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();

        /// <summary>
        /// Gets or sets associated ID from resource strings
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the associated language resource to use (ie: "es-ES")
        /// default is the OS language of current view
        /// </summary>
        public string Language { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            if (string.IsNullOrEmpty(Language))
            {
                return resourceLoader.GetString(this.Name);
            }
            else
            {
                // not using ResourceContext.GetForCurrentView
                var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext
                {
                    Languages = new string[] { Language }
                };
                var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
                return resourceMap.GetValue(this.Name, resourceContext).ValueAsString;
            }
        }
    }
}
