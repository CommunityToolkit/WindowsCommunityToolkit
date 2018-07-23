// <copyright file="WindowsXamlHost.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>Microsoft</author>

namespace Microsoft.Toolkit.Win32.UI.Interop.WPF
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public static class UWPTypeFactory
    {
        #region Methods

        /// <summary>
        /// Create XAML content by type name
        /// XAML type name should be specified as: namespace.class
        /// ex: MyClassLibrary.MyCustomType
        /// </summary>
        /// <param name="xamlTypeName">XAML type name</param>
        public static global::Windows.UI.Xaml.FrameworkElement CreateXamlContentByType(string xamlTypeName)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType = null;
            Type systemType = null;

            // If a root metatadata provider has been defined on the application object, 
            // use it to probe for custom UWP XAML type metadata.  If the root metadata
            // provider has not been implemented on the current application object, assume
            // the caller wants a built-in UWP XAML type, not a custom UWP XAML type. 
            global::Windows.UI.Xaml.Markup.IXamlMetadataProvider xamlRootMetadataProvider = global::Windows.UI.Xaml.Application.Current as global::Windows.UI.Xaml.Markup.IXamlMetadataProvider;
            if (xamlRootMetadataProvider != null)
            {
                xamlType = xamlRootMetadataProvider.GetXamlType(xamlTypeName);
            }

            systemType = UWPTypeFactory.FindBuiltInType(xamlTypeName);

            if (xamlType != null)
            {
                // Create custom UWP XAML type
                return (global::Windows.UI.Xaml.FrameworkElement)xamlType.ActivateInstance();
            }
            else if (systemType != null)
            {
                // Create built-in UWP XAML type
                return (global::Windows.UI.Xaml.FrameworkElement)Activator.CreateInstance(systemType);
            }

            throw new InvalidOperationException("Microsoft.Windows.Interop.UWPTypeFactory: Could not create type: " + xamlTypeName);
        }

        /// <summary>
        /// Searches for a built-in type by iterating through all types in
        /// all assemblies loaded in the current AppDomain
        /// </summary>
        /// <param name="typeName">Full type name, with namespace, without assembly</param>
        /// <returns>System.Type</returns>
        private static Type FindBuiltInType(string typeName)
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
