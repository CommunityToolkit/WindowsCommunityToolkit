// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]

// Dependencies

// Common
[assembly: Dependency("System", LoadHint.Always)]
[assembly: Dependency("System.Core", LoadHint.Always)]
[assembly: Dependency("System.Xml", LoadHint.Always)]

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

[assembly: SecurityRules(SecurityRuleSet.Level2)]
#if ALLOW_PARTIALLY_TRUSTED_CALLERS
[assembly: System.Security.AllowPartiallyTrustedCallers]
#endif
