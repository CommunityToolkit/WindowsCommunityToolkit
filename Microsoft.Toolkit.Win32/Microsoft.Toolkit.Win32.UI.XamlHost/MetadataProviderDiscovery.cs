// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

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
        internal static List<Windows.UI.Xaml.Markup.IXamlMetadataProvider> DiscoverMetadataProviders(List<Type> filteredTypes)
        {
            // List of discovered UWP XAML metadata providers
            List<Windows.UI.Xaml.Markup.IXamlMetadataProvider> metadataProviders = null;
            metadataProviders = new List<Windows.UI.Xaml.Markup.IXamlMetadataProvider>();

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

                    LoadTypesFromAssembly(assembly, ref metadataProviders, ref filteredTypes);
                }
                catch (System.IO.FileLoadException)
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
        private static void LoadTypesFromAssembly(Assembly assembly, ref List<Windows.UI.Xaml.Markup.IXamlMetadataProvider> metadataProviders, ref List<Type> filteredTypes)
        {
            // Load types inside the executing assembly
            foreach (var type in assembly.GetTypes())
            {
                if (filteredTypes.Contains(type))
                {
                    continue;
                }

                if (typeof(Windows.UI.Xaml.Markup.IXamlMetadataProvider).IsAssignableFrom(type))
                {
                    var provider = (Windows.UI.Xaml.Markup.IXamlMetadataProvider)Activator.CreateInstance(type);
                    metadataProviders.Add(provider);
                }
            }
        }
    }
}