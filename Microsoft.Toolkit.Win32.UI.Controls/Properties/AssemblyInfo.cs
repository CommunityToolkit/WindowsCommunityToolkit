using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Toolkit.Win32.UI.Controls.dll")]
[assembly: AssemblyDescription("Microsoft.Toolkit.Win32.UI.Controls.dll")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("392734a6-6ccb-49d5-bca2-44a484008fb8")]

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
[assembly: InternalsVisibleTo("Microsoft.Toolkit.Win32.UI.Controls.WinForms.Tests")]

#if ALLOW_PARTIALLY_TRUSTED_CALLERS
[assembly: System.Security.AllowPartiallyTrustedCallers]
#endif
