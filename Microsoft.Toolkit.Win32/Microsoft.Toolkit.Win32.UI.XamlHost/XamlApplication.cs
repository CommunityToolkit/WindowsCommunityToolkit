// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Toolkit.Win32.UI.XamlHost
{
    /// <summary>
    /// XamlApplication is a custom Windows.UI.Xaml.Applicaiton that implements IXamlMetadataProvider. The
    /// metadata provider implemented on the application is known as the 'root metadata provider'.  This provider
    /// has the responsibility of loading all other metadata for custom UWP XAML types.  In this implementation,
    /// reflection is used at runtime to probe for metadata providers in the working directory, allowing any
    /// type that includes metadata (compiled in to a .NET framework assembly) to be used without explicit
    /// metadata handling by the developer.
    /// </summary>
    internal class XamlApplication : Windows.UI.Xaml.Application, Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
        // Metadata provider identified by the root metadata provider
        private List<Windows.UI.Xaml.Markup.IXamlMetadataProvider> _metadataProviders = null;

        /// <summary>
        /// Gets XAML IXamlType interface from all cached metadata providers by Type
        /// </summary>
        /// <param name="type">Type of requested type</param>
        /// <returns>IXamlType interface or null if type is not found</returns>
        public Windows.UI.Xaml.Markup.IXamlType GetXamlType(Type type)
        {
            EnsureMetadataProviders();

            foreach (var provider in _metadataProviders)
            {
                var result = provider.GetXamlType(type);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets XAML IXamlType interface from all cached metadata providers by full type name
        /// </summary>
        /// <param name="fullName">Full name of requested type</param>
        /// <returns>IXamlTypeInterface or null if type is not found</returns>
        public Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            EnsureMetadataProviders();

            foreach (var provider in _metadataProviders)
            {
                var result = provider.GetXamlType(fullName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all XAML namespace definitions from metadata providers
        /// </summary>
        /// <returns>Array of namspace definitions</returns>
        public Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            EnsureMetadataProviders();

            var definitions = new List<Windows.UI.Xaml.Markup.XmlnsDefinition>();
            foreach (var provider in _metadataProviders)
            {
                definitions.AddRange(provider.GetXmlnsDefinitions());
            }

            return definitions.ToArray();
        }

        /// <summary>
        /// Probes file system for UWP XAML metadata providers
        /// </summary>
        private void EnsureMetadataProviders()
        {
            if (_metadataProviders == null)
            {
                _metadataProviders = MetadataProviderDiscovery.DiscoverMetadataProviders(new List<Type> { typeof(XamlApplication) });
            }
        }

        /// <summary>
        /// Gets and returns the current UWP XAML Application instance in a reference parameter.
        /// If the current XAML Application instance has not been created for the process (is null),
        /// a new Microsoft.Toolkit.Win32.UI.XamlHost.XamlApplication instance is created and returned.
        /// </summary>
        internal static void GetOrCreateXamlApplicationInstance(ref Windows.UI.Xaml.Application application)
        {
            // Instantiation of the application object must occur before creating the DesktopWindowXamlSource instance.
            // DesktopWindowXamlSource will create a generic Application object unable to load custom UWP XAML metadata.
            if (application == null)
            {
                try
                {
                    // global::Windows.UI.Xaml.Application.Current may throw if DXamlCore has not been initialized.
                    // Treat the exception as an uninitialized global::Windows.UI.Xaml.Application condition.
                    application = Windows.UI.Xaml.Application.Current;
                }
                catch
                {
                    // Create a custom UWP XAML Application object that implements reflection-based XAML metdata probing.
                    application = new XamlApplication();
                }
            }
        }
    }
}
