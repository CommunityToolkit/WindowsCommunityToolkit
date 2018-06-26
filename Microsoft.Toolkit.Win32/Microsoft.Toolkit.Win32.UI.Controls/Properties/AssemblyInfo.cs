// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Windows Community Toolkit Controls for Windows Forms and WPF")]
[assembly: AssemblyDescription("Wndows Forms and WPF controls for Windows Community Toolkit")]

// Make sure this is the same in the VisualToolsManifest.xml file
[assembly: AssemblyProduct("Windows Community Toolkit")]
[assembly: AssemblyCopyright("\x00a9 Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyCompany("Microsoft Corporation")]

[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// Dependencies

// Common
[assembly: Dependency("System", LoadHint.Always)]
[assembly: Dependency("System.Core", LoadHint.Always)]
[assembly: Dependency("System.Xml", LoadHint.Always)]

// WinForms
[assembly: Dependency("System.Windows.Forms", LoadHint.Sometimes)]

[assembly: Dependency("System.Drawing", LoadHint.Sometimes)]
[assembly: Dependency("System.Configuration", LoadHint.Sometimes)]

[assembly: Dependency("Windows.Web.winmd", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime.InteropServices.WindowsRuntime", LoadHint.Sometimes)]
[assembly: Dependency("Windows.Foundation.winmd", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime.WindowsRuntime", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime", LoadHint.Sometimes)]

// WPF
[assembly: Dependency("PresentationFramework", LoadHint.Sometimes)]
[assembly: Dependency("PresentationCore", LoadHint.Sometimes)]
[assembly: Dependency("WindowsBase", LoadHint.Sometimes)]
[assembly: Dependency("System.Xaml", LoadHint.Sometimes)]
[assembly: Dependency("System.Configuration", LoadHint.Sometimes)]
[assembly: Dependency("PresentationFramework.Aero2", LoadHint.Sometimes)]
[assembly: Dependency("Windows.Foundation.winmd", LoadHint.Sometimes)]
[assembly: Dependency("Windows.Web.winmd", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime.InteropServices.WindowsRuntime", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime.Serialization", LoadHint.Sometimes)]
[assembly: Dependency("SMDiagnostics", LoadHint.Sometimes)]
[assembly: Dependency("System.ServiceModel.Internals", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime.WindowsRuntime", LoadHint.Sometimes)]
[assembly: Dependency("UIAutomationProvider", LoadHint.Sometimes)]
[assembly: Dependency("System.Windows.Forms", LoadHint.Sometimes)]
[assembly: Dependency("System.Drawing", LoadHint.Sometimes)]
[assembly: Dependency("System.Runtime", LoadHint.Sometimes)]
[assembly: Dependency("Windows.Globalization.winmd", LoadHint.Sometimes)]
[assembly: Dependency("Windows.Storage.winmd", LoadHint.Sometimes)]

// InternalsVisableTo
[assembly: InternalsVisibleTo("Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView")]
[assembly: InternalsVisibleTo("Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared")]

[assembly: SecurityRules(SecurityRuleSet.Level2)]
#if ALLOW_PARTIALLY_TRUSTED_CALLERS
[assembly: System.Security.AllowPartiallyTrustedCallers]
#endif
