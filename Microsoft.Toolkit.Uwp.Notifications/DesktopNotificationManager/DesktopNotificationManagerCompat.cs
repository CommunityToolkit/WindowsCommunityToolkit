// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Helper for .NET Framework applications to display toast notifications and respond to toast events
    /// </summary>
    public class DesktopNotificationManagerCompat
    {
        /// <summary>
        /// Event that is triggered when a notification or notification button is clicked.
        /// </summary>
        public static event OnActivated OnActivated;

        internal static void OnActivatedInternal(string args, Internal.NotificationActivator.NOTIFICATION_USER_INPUT_DATA[] input, string aumid)
        {
            ValueSet userInput = new ValueSet();

            if (input != null)
            {
                foreach (var val in input)
                {
                    userInput.Add(val.Key, val.Value);
                }
            }

            try
            {
                OnActivated?.Invoke(new DesktopNotificationActivatedEventArgs()
                {
                    Argument = args,
                    UserInput = userInput
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// A constant that is used as the launch arg when your EXE is launched from a toast notification.
        /// </summary>
        private const string TOAST_ACTIVATED_LAUNCH_ARG = "-ToastActivated";

        private const int CLASS_E_NOAGGREGATION = -2147221232;
        private const int E_NOINTERFACE = -2147467262;
        private const int CLSCTX_LOCAL_SERVER = 4;
        private const int REGCLS_MULTIPLEUSE = 1;
        private const int S_OK = 0;
        private static readonly Guid IUnknownGuid = new Guid("00000000-0000-0000-C000-000000000046");

        private static bool _registeredAumidAndComServer;
        private static string _aumid;

        /// <summary>
        /// If you're not using UWP, MSIX, or sparse packages, you must call this method to register your AUMID
        /// and display assets with the Compat library. Feel free to call this regardless, it will no-op if running
        /// under MSIX/sparse/UWP. Call this upon application startup (every time), before calling any other APIs.
        /// Note that the display name and icon will NOT update if changed until either all toasts are cleared,
        /// or the system is rebooted.
        /// </summary>
        /// <param name="aumid">An AUMID that uniquely identifies your application.</param>
        /// <param name="displayName">Your app's display name, which will appear on toasts and within Action Center.</param>
        /// <param name="iconPath">Your app's icon, which will appear on toasts and within Action Center.</param>
        public static void RegisterApplication(
            string aumid,
            string displayName,
            string iconPath)
        {
            RegisterApplication(aumid, displayName, iconPath, Colors.LightGray);
        }

        /// <summary>
        /// If you're not using UWP, MSIX, or sparse packages, you must call this method to register your AUMID
        /// and display assets with the Compat library. Feel free to call this regardless, it will no-op if running
        /// under MSIX/sparse/UWP. Call this upon application startup (every time), before calling any other APIs.
        /// Note that the display name and icon will NOT update if changed until either all toasts are cleared,
        /// or the system is rebooted.
        /// </summary>
        /// <param name="aumid">An AUMID that uniquely identifies your application.</param>
        /// <param name="displayName">Your app's display name, which will appear on toasts and within Action Center.</param>
        /// <param name="iconPath">Your app's icon, which will appear on toasts and within Action Center.</param>
        /// <param name="iconBackgroundColor">Your app's icon background color, which only appears in the system notification settings page (everywhere else your icon will have a transparent background). If you use the method without this parameter, we'll use light gray (which should look good on most icons). To use the accent color, pass in null for this parameter.</param>
        public static void RegisterApplication(
            string aumid,
            string displayName,
            string iconPath,
            Color iconBackgroundColor)
        {
            if (string.IsNullOrWhiteSpace(aumid))
            {
                throw new ArgumentException("You must provide an AUMID.", nameof(aumid));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("You must provide a display name.", nameof(displayName));
            }

            if (string.IsNullOrWhiteSpace(iconPath))
            {
                throw new ArgumentException("You must provide an icon path.", nameof(iconPath));
            }

            // If running as Desktop Bridge
            if (DesktopBridgeHelpers.IsRunningAsUwp())
            {
                // Clear the AUMID since Desktop Bridge doesn't use it, and then we're done.
                // Desktop Bridge apps are registered with platform through their manifest.
                // Their LocalServer32 key is also registered through their manifest.
                _aumid = null;
                _registeredAumidAndComServer = true;
                return;
            }

            _aumid = aumid;

            using (var rootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\AppUserModelId\" + aumid))
            {
                rootKey.SetValue("DisplayName", displayName);
                rootKey.SetValue("IconUri", iconPath);

                // Background color only appears in the settings page, format is
                // hex without leading #, like "FFDDDDDD"
                if (iconBackgroundColor == null)
                {
                    rootKey.DeleteValue("IconBackgroundColor");
                }
                else
                {
                    rootKey.SetValue("IconBackgroundColor", $"{iconBackgroundColor.A:X2}{iconBackgroundColor.R:X2}{iconBackgroundColor.G:X2}{iconBackgroundColor.B:X2}");
                }
            }

            _registeredAumidAndComServer = true;

            // https://stackoverflow.com/questions/24069352/c-sharp-typebuilder-generate-class-with-function-dynamically
            // For .NET Core we're going to need https://stackoverflow.com/questions/36937276/is-there-any-replace-of-assemblybuilder-definedynamicassembly-in-net-core
            AssemblyName aName = new AssemblyName("DynamicComActivator");
            AssemblyBuilder aBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);

            // For a single-module assembly, the module name is usually the assembly name plus an extension.
            ModuleBuilder mb = aBuilder.DefineDynamicModule(aName.Name);

            // Create class which extends NotificationActivator
            TypeBuilder tb = mb.DefineType(
                name: "MyNotificationActivator",
                attr: TypeAttributes.Public,
                parent: typeof(Internal.NotificationActivator),
                interfaces: new Type[0]);

            tb.SetCustomAttribute(new CustomAttributeBuilder(
                con: typeof(GuidAttribute).GetConstructor(new Type[] { typeof(string) }),
                constructorArgs: new object[] { GenerateGuid(aumid) }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(
                con: typeof(ComVisibleAttribute).GetConstructor(new Type[] { typeof(bool) }),
                constructorArgs: new object[] { true }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(
#pragma warning disable CS0618 // Type or member is obsolete
                con: typeof(ComSourceInterfacesAttribute).GetConstructor(new Type[] { typeof(Type) }),
#pragma warning restore CS0618 // Type or member is obsolete
                constructorArgs: new object[] { typeof(Internal.NotificationActivator.INotificationActivationCallback) }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(
                con: typeof(ClassInterfaceAttribute).GetConstructor(new Type[] { typeof(ClassInterfaceType) }),
                constructorArgs: new object[] { ClassInterfaceType.None }));

            var activatorType = tb.CreateType();

            RegisterActivator(activatorType);
        }

        /// <summary>
        /// From https://stackoverflow.com/a/41622689/1454643
        /// Generates Guid based on String. Key assumption for this algorithm is that name is unique (across where it it's being used)
        /// and if name byte length is less than 16 - it will be fetched directly into guid, if over 16 bytes - then we compute sha-1
        /// hash from string and then pass it to guid.
        /// </summary>
        /// <param name="name">Unique name which is unique across where this guid will be used.</param>
        /// <returns>For example "706C7567-696E-7300-0000-000000000000" for "plugins"</returns>
        private static string GenerateGuid(string name)
        {
            byte[] buf = Encoding.UTF8.GetBytes(name);
            byte[] guid = new byte[16];
            if (buf.Length < 16)
            {
                Array.Copy(buf, guid, buf.Length);
            }
            else
            {
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hash = sha1.ComputeHash(buf);

                    // Hash is 20 bytes, but we need 16. We loose some of "uniqueness", but I doubt it will be fatal
                    Array.Copy(hash, guid, 16);
                }
            }

            // Don't use Guid constructor, it tends to swap bytes. We want to preserve original string as hex dump.
            string guidS = $"{guid[0]:X2}{guid[1]:X2}{guid[2]:X2}{guid[3]:X2}-{guid[4]:X2}{guid[5]:X2}-{guid[6]:X2}{guid[7]:X2}-{guid[8]:X2}{guid[9]:X2}-{guid[10]:X2}{guid[11]:X2}{guid[12]:X2}{guid[13]:X2}{guid[14]:X2}{guid[15]:X2}";

            return guidS;
        }

        /// <summary>
        /// Registers the activator type as a COM server client so that Windows can launch your activator. If not using UWP/MSIX/sparse, you must call <see cref="RegisterApplication(string, string, string)"/> first.
        /// </summary>
        private static void RegisterActivator(Type activatorType)
        {
            if (!DesktopBridgeHelpers.IsRunningAsUwp())
            {
                if (_aumid == null)
                {
                    throw new InvalidOperationException("You must call RegisterApplication first.");
                }

                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                RegisterComServer(activatorType, exePath);

                using (var rootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\AppUserModelId\" + _aumid))
                {
                    rootKey.SetValue("CustomActivator", string.Format("{{{0}}}", activatorType.GUID));
                }
            }

            // Big thanks to FrecherxDachs for figuring out the following code which works in .NET Core 3: https://github.com/FrecherxDachs/UwpNotificationNetCoreTest
            var uuid = activatorType.GUID;
            CoRegisterClassObject(
                uuid,
                new NotificationActivatorClassFactory(activatorType),
                CLSCTX_LOCAL_SERVER,
                REGCLS_MULTIPLEUSE,
                out _);
        }

        private static void RegisterComServer(Type activatorType, string exePath)
        {
            // We register the EXE to start up when the notification is activated
            string regString = string.Format("SOFTWARE\\Classes\\CLSID\\{{{0}}}\\LocalServer32", activatorType.GUID);
            var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(regString);

            // Include a flag so we know this was a toast activation and should wait for COM to process
            // We also wrap EXE path in quotes for extra security
            key.SetValue(null, '"' + exePath + '"' + " " + TOAST_ACTIVATED_LAUNCH_ARG);
        }

        /// <summary>
        /// Gets whether the current process was activated due to a toast activation. If so, the OnActivated event will be triggered soon after process launch.
        /// </summary>
        /// <returns>True if the current process was activated due to a toast activation, otherwise false.</returns>
        public static bool WasProcessToastActivated()
        {
            return Environment.GetCommandLineArgs().Contains(TOAST_ACTIVATED_LAUNCH_ARG);
        }

        [ComImport]
        [Guid("00000001-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IClassFactory
        {
            [PreserveSig]
            int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject);

            [PreserveSig]
            int LockServer(bool fLock);
        }

        private class NotificationActivatorClassFactory : IClassFactory
        {
            private Type _activatorType;

            public NotificationActivatorClassFactory(Type activatorType)
            {
                _activatorType = activatorType;
            }

            public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
            {
                ppvObject = IntPtr.Zero;

                if (pUnkOuter != IntPtr.Zero)
                {
                    Marshal.ThrowExceptionForHR(CLASS_E_NOAGGREGATION);
                }

                if (riid == _activatorType.GUID || riid == IUnknownGuid)
                {
                    // Create the instance of the .NET object
                    ppvObject = Marshal.GetComInterfaceForObject(
                        Activator.CreateInstance(_activatorType),
                        typeof(Internal.NotificationActivator.INotificationActivationCallback));
                }
                else
                {
                    // The object that ppvObject points to does not support the
                    // interface identified by riid.
                    Marshal.ThrowExceptionForHR(E_NOINTERFACE);
                }

                return S_OK;
            }

            public int LockServer(bool fLock)
            {
                return S_OK;
            }
        }

        [DllImport("ole32.dll")]
        private static extern int CoRegisterClassObject(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
            [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            uint dwClsContext,
            uint flags,
            out uint lpdwRegister);

        /// <summary>
        /// Creates a toast notifier. If you're a Win32 non-MSIX/sparse app, you must have called <see cref="RegisterApplication(string, string, string)"/> first), or this will throw an exception.
        /// </summary>
        /// <returns><see cref="ToastNotifier"/></returns>
        public static ToastNotifier CreateToastNotifier()
        {
            EnsureRegistered();

            if (_aumid != null)
            {
                // Non-Desktop Bridge
                return ToastNotificationManager.CreateToastNotifier(_aumid);
            }
            else
            {
                // Desktop Bridge
                return ToastNotificationManager.CreateToastNotifier();
            }
        }

        /// <summary>
        /// Gets the <see cref="DesktopNotificationHistoryCompat"/> object. If you're a Win32 non-MISX/sparse app, you must call <see cref="RegisterApplication(string, string, string)"/> first, or this will throw an exception.
        /// </summary>
        public static DesktopNotificationHistoryCompat History
        {
            get
            {
                EnsureRegistered();

                return new DesktopNotificationHistoryCompat(_aumid);
            }
        }

        private static void EnsureRegistered()
        {
            // If not registered AUMID yet
            if (!_registeredAumidAndComServer)
            {
                // Check if Desktop Bridge
                if (DesktopBridgeHelpers.IsRunningAsUwp())
                {
                    // Implicitly registered, all good!
                    _registeredAumidAndComServer = true;
                }
                else
                {
                    // Otherwise, incorrect usage
                    throw new Exception($"You must call {nameof(RegisterApplication)} first.");
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether http images can be used within toasts. This is true if running with package identity (MSIX or sparse package).
        /// </summary>
        public static bool CanUseHttpImages
        {
            get { return DesktopBridgeHelpers.IsRunningAsUwp(); }
        }

        /// <summary>
        /// Code from https://github.com/qmatteoq/DesktopBridgeHelpers/edit/master/DesktopBridge.Helpers/Helpers.cs
        /// </summary>
        private class DesktopBridgeHelpers
        {
            private const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

            private static bool? _isRunningAsUwp;

            public static bool IsRunningAsUwp()
            {
                if (_isRunningAsUwp == null)
                {
                    if (IsWindows7OrLower)
                    {
                        _isRunningAsUwp = false;
                    }
                    else
                    {
                        int length = 0;
                        var sb = new StringBuilder(0);
                        GetCurrentPackageFullName(ref length, sb);

                        sb = new StringBuilder(length);
                        int error = GetCurrentPackageFullName(ref length, sb);

                        _isRunningAsUwp = error != APPMODEL_ERROR_NO_PACKAGE;
                    }
                }

                return _isRunningAsUwp.Value;
            }

            private static bool IsWindows7OrLower
            {
                get
                {
                    int versionMajor = Environment.OSVersion.Version.Major;
                    int versionMinor = Environment.OSVersion.Version.Minor;
                    double version = versionMajor + ((double)versionMinor / 10);
                    return version <= 6.1;
                }
            }
        }
    }
}
