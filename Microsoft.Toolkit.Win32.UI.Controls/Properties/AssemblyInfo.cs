// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Toolkit.Win32.UI.Controls.dll")]
[assembly: AssemblyDescription("Microsoft.Toolkit.Win32.UI.Controls.dll")]
[assembly: AssemblyProduct("Microsoft.Toolkit.Win32.UI.Controls")]
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
