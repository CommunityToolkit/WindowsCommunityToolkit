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
    public class XamlApplication : Windows.UI.Xaml.Application, Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
        // Metadata provider identified by the root metadata provider
        private List<Windows.UI.Xaml.Markup.IXamlMetadataProvider> _metadataProviders;

        /// <summary>
        /// Loads all types from the specified assembly and caches metadata providers
        /// </summary>
        /// <param name="assembly">Target assembly to load types from</param>
        private void LoadTypesFromAssembly(Assembly assembly)
        {
            // Load types inside the executing assembly
            var thisType = GetType();

            foreach (var type in assembly.GetTypes())
            {
                if (type == thisType)
                {
                    continue;
                }

                if (typeof(Windows.UI.Xaml.Markup.IXamlMetadataProvider).IsAssignableFrom(type))
                {
                    var provider = (Windows.UI.Xaml.Markup.IXamlMetadataProvider)Activator.CreateInstance(type);
                    _metadataProviders.Add(provider);
                }
            }
        }

        /// <summary>
        /// Probes working directory for all available metadata providers
        /// </summary>
        private void EnsureMetadataProviders()
        {
            if (_metadataProviders != null)
            {
                return;
            }

            _metadataProviders = new List<Windows.UI.Xaml.Markup.IXamlMetadataProvider>();

            // Reflection-based runtime metadata probing
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var exes = System.IO.Directory.GetFiles(currentDirectory, "*.exe");
            var dlls = System.IO.Directory.GetFiles(currentDirectory, "*.dll");

            var files = new string[exes.Length + dlls.Length];
            Array.Copy(exes, files, exes.Length);
            Array.Copy(dlls, 0, files, exes.Length, dlls.Length);

            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);

                    LoadTypesFromAssembly(assembly);
                }
                catch
                {
                     // XamlApplication::EnsureMetadataProviders: Non-.NET assembly. Expected.
                }
            }

            // Load any types from this assembly
            LoadTypesFromAssembly(Assembly.GetExecutingAssembly());
        }

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
    }
}
