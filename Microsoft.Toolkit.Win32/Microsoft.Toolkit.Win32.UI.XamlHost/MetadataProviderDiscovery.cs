// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Win32.UI.XamlHost
{
    /// <summary>
    /// MetadataProviderDiscovery is responsible for loading all metadata providers for custom UWP XAML
    /// types.  In this implementation, reflection is used at runtime to probe for metadata providers in
    /// the working directory, allowing any type that includes metadata (compiled in to a .NET framework
    /// assembly) to be used without explicit metadata handling by the application developer.  This
    /// internal class will be amended or removed when additional type loading support is available.
    /// </summary>
    internal static class MetadataProviderDiscovery
    {
        /// <summary>
        /// Probes working directory for all available metadata providers
        /// </summary>
        /// <param name="filteredTypes">Types to ignore</param>
        /// <returns>List of UWP XAML metadata providers</returns>
        internal static List<IXamlMetadataProvider> DiscoverMetadataProviders(List<Type> filteredTypes)
        {
            // List of discovered UWP XAML metadata providers
            var metadataProviders = new List<IXamlMetadataProvider>();

            // Reflection-based runtime metadata probing
            var currentDirectory = new FileInfo(typeof(MetadataProviderDiscovery).Assembly.Location).Directory;

            foreach (var file in currentDirectory.GetFiles("*.dll").Union(currentDirectory.GetFiles("*.exe")))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file.FullName);

                    LoadTypesFromAssembly(assembly, ref metadataProviders, ref filteredTypes);
                }
                catch (FileLoadException)
                {
                    // These exceptions are expected
                }
            }

            // Load any types from this assembly
            LoadTypesFromAssembly(Assembly.GetExecutingAssembly(), ref metadataProviders, ref filteredTypes);

            return metadataProviders;
        }

        /// <summary>
        /// Loads all types from the specified assembly and caches metadata providers
        /// </summary>
        /// <param name="assembly">Target assembly to load types from</param>
        /// <param name="metadataProviders">List of metadata providers</param>
        /// <param name="filteredTypes">List of types to ignore</param>
        private static void LoadTypesFromAssembly(Assembly assembly, ref List<IXamlMetadataProvider> metadataProviders, ref List<Type> filteredTypes)
        {
            // Load types inside the executing assembly
            foreach (var type in assembly.GetTypes())
            {
                if (filteredTypes.Contains(type))
                {
                    continue;
                }

                if (typeof(IXamlMetadataProvider).IsAssignableFrom(type))
                {
                    var provider = (IXamlMetadataProvider)Activator.CreateInstance(type);
                    metadataProviders.Add(provider);
                }
            }
        }
    }
}