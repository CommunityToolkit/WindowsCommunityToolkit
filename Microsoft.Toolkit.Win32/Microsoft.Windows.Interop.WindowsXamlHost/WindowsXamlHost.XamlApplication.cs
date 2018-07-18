// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     A Windows Forms control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHost 
    {
        /// <summary>
        /// XamlApplication is a custom Windows.UI.Xaml.Applicaiton that implements IXamlMetadataProvider. The
        /// metadata provider implemented on the application is known as the 'root metadata provider'.  This provider
        /// has the responsibility of loading all other metadata for custom UWP XAML types.  In this implementation, 
        /// reflection is used at runtime to probe for metadata providers in the working directory, allowing any 
        /// type that includes metadata (compiled in to a .NET framework assembly) to be used without explicit 
        /// metadata handling by the developer.  
        /// </summary>
        public class XamlApplication : global::Windows.UI.Xaml.Application, global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
        {
            // Metadata provider identified by the root metadata provider
            private List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider> _metadataProviders;

            /// <summary>
            /// Loads all types from the specified assembly and caches metadata providers
            /// </summary>
            /// <param name="assembly">Target assembly to load types from</param>
            private void LoadTypesFromAssembly(Assembly assembly)
            {
                System.Diagnostics.Debug.WriteLine("XamlApplication::LoadTypesFromAssembly: Loading type from " + assembly);

                // Load types inside the executing assembly
                foreach (Type type in assembly.GetTypes())
                {
                    System.Diagnostics.Debug.WriteLine("XamlApplication::LoadTypesFromAssembly: Loading type " + type);
                    if (type == this.GetType())
                    {
                        continue;
                    }
                    if (typeof(global::Windows.UI.Xaml.Markup.IXamlMetadataProvider).IsAssignableFrom(type))
                    {
                        global::Windows.UI.Xaml.Markup.IXamlMetadataProvider provider = (global::Windows.UI.Xaml.Markup.IXamlMetadataProvider)Activator.CreateInstance(type);
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

                _metadataProviders = new List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider>();

                // Reflection-based runtime metadata probing
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string[] exes = System.IO.Directory.GetFiles(currentDirectory, "*.exe");
                string[] dlls = System.IO.Directory.GetFiles(currentDirectory, "*.dll");

                string[] files = new string[exes.Length + dlls.Length];
                Array.Copy(exes, files, exes.Length);
                Array.Copy(dlls, 0, files, exes.Length, dlls.Length);

                foreach (string file in files)
                {
                    System.Diagnostics.Debug.WriteLine("XamlApplication::EnsureMetadataProviders: Loading file " + file);

                    Assembly assembly;
                    try
                    {
                        assembly = Assembly.LoadFrom(file);
                        System.Diagnostics.Debug.WriteLine("XamlApplication::EnsureMetadataProviders: Successfully loaded " + file);

                        LoadTypesFromAssembly(assembly);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("XamlApplication::EnsureMetadataProviders: Failed to load " + file);
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
            public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(Type type)
            {
                EnsureMetadataProviders();
                foreach (global::Windows.UI.Xaml.Markup.IXamlMetadataProvider provider in _metadataProviders)
                {
                    global::Windows.UI.Xaml.Markup.IXamlType result = provider.GetXamlType(type);
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
            public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
            {
                EnsureMetadataProviders();
                foreach (global::Windows.UI.Xaml.Markup.IXamlMetadataProvider provider in _metadataProviders)
                {
                    global::Windows.UI.Xaml.Markup.IXamlType result = provider.GetXamlType(fullName);
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
            /// <returns>Array of namspace definitiona</returns>
            public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
            {
                EnsureMetadataProviders();
                List<global::Windows.UI.Xaml.Markup.XmlnsDefinition> definitions = new List<global::Windows.UI.Xaml.Markup.XmlnsDefinition>();
                foreach (global::Windows.UI.Xaml.Markup.IXamlMetadataProvider provider in _metadataProviders)
                {
                    definitions.AddRange(provider.GetXmlnsDefinitions());
                }
                return definitions.ToArray();
            }
        }
    }
}
