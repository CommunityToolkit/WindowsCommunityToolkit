// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public partial class WindowsXamlHost
    {
        #region Fields

        /// <summary>
        /// Stores custom and built-in UWP XAML types that have been successfully looked up.
        /// The key is the full name of the type, including the namespace, without the
        /// assembly name. The value is the associated System.Type or IXamlType.  For custom
        /// types, cache prevents root metadata provider having probe for assemblies in the 
        /// working directory, load assemblies, and iterate through types.  For built-in 
        /// types, cache prevents having to iterate through types in all currently loaded assemblies.
        /// </summary>
        static IDictionary<string, object> CacheForUWPTypes = new ConcurrentDictionary<string, object>();

        #endregion

        #region Methods

        /// <summary>
        /// Create XAML content by type name
        /// XAML type name should be specified as: namespace.class
        /// ex: MyClassLibrary.MyCustomType
        /// </summary>
        /// <param name="xamlTypeName">XAML type name</param>
        /// <returns>an instance of the requested type</returns>
        public global::Windows.UI.Xaml.FrameworkElement CreateXamlContentByType(string xamlTypeName)
        {
            // TODO: This needs to be significantly cleaned up.
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "UWPTypeFactory::CreateXamlContentByType: {0}", xamlTypeName);

            global::Windows.UI.Xaml.Markup.IXamlType xamlType = null;
            Type systemType = null;

            object systemTypeOrXamlType = null;
            if (CacheForUWPTypes.TryGetValue(xamlTypeName, out systemTypeOrXamlType))
            {
                systemType = systemTypeOrXamlType as Type;
                xamlType = systemTypeOrXamlType as global::Windows.UI.Xaml.Markup.IXamlType;

                System.Diagnostics.Debug.Assert(systemType != null || xamlType != null, "UWPTypeFactory::CreateXamlContentByType: Invalid cache value");
            }
            else
            {
                // If a root metatadata provider has been defined on the application object, 
                // use it to probe for custom UWP XAML type metadata.  If the root metadata
                // provider has not been implemented on the current application object, assume
                // the caller wants a built-in UWP XAML type, not a custom UWP XAML type. 
                global::Windows.UI.Xaml.Markup.IXamlMetadataProvider xamlRootMetadataProvider = application as global::Windows.UI.Xaml.Markup.IXamlMetadataProvider;
                if (xamlRootMetadataProvider != null)
                {
                    xamlType = xamlRootMetadataProvider.GetXamlType(xamlTypeName);
                }

                systemType = this.FindBuiltInType(xamlTypeName);
            }

            if (xamlType != null)
            {
                CacheForUWPTypes[xamlTypeName] = xamlType;

                // Create custom UWP XAML type
                return (global::Windows.UI.Xaml.FrameworkElement)xamlType.ActivateInstance();
            }
            else if (systemType != null)
            {
                CacheForUWPTypes[xamlTypeName] = systemType;

                // Create built-in UWP XAML type
                return (global::Windows.UI.Xaml.FrameworkElement)Activator.CreateInstance(systemType);
            }

            throw new InvalidOperationException("WindowsXamlHost: UWPTypeFactory: Could not create type: " + xamlTypeName);
        }

        /// <summary>
        /// Searches for a built-in type by iterating through all types in
        /// all assemblies loaded in the current AppDomain
        /// </summary>
        /// <param name="typeName">Full type name, with namespace, without assembly</param>
        /// <returns>System.Type</returns>
        private Type FindBuiltInType(string typeName)
        {
            AppDomain currentAppDomain = AppDomain.CurrentDomain;
            Assembly[] appDomainLoadedAssemblies = currentAppDomain.GetAssemblies();

            foreach (Assembly loadedAssembly in appDomainLoadedAssemblies)
            {
                Type currentType = loadedAssembly.GetType(typeName);
                if (currentType != null)
                {
                    return currentType;
                }
            }

            return null;
        }

        #endregion
    }
}