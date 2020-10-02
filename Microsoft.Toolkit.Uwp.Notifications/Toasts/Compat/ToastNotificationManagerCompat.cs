// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINDOWS_UWP

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using Windows.ApplicationModel;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Provides access to sending and managing toast notifications. Works for all types of apps, even Win32 non-MSIX/sparse apps.
    /// </summary>
    public static class ToastNotificationManagerCompat
    {
#if WIN32
        private const string TOAST_ACTIVATED_LAUNCH_ARG = "-ToastActivated";

        private const int CLASS_E_NOAGGREGATION = -2147221232;
        private const int E_NOINTERFACE = -2147467262;
        private const int CLSCTX_LOCAL_SERVER = 4;
        private const int REGCLS_MULTIPLEUSE = 1;
        private const int S_OK = 0;
        private static readonly Guid IUnknownGuid = new Guid("00000000-0000-0000-C000-000000000046");

        private static bool _registeredOnActivated;
        private static List<OnActivated> _onActivated = new List<OnActivated>();

        /// <summary>
        /// Event that is triggered when a notification or notification button is clicked. Subscribe to this event in your app's initial startup code.
        /// </summary>
        public static event OnActivated OnActivated
        {
            add
            {
                lock (_onActivated)
                {
                    if (!_registeredOnActivated)
                    {
                        // Desktop Bridge apps will dynamically register upon first subscription to event
                        CreateAndRegisterActivator();
                    }

                    _onActivated.Add(value);
                }
            }

            remove
            {
                lock (_onActivated)
                {
                    _onActivated.Remove(value);
                }
            }
        }

        internal static void OnActivatedInternal(string args, Internal.InternalNotificationActivator.NOTIFICATION_USER_INPUT_DATA[] input, string aumid)
        {
            ValueSet userInput = new ValueSet();

            if (input != null)
            {
                foreach (var val in input)
                {
                    userInput.Add(val.Key, val.Value);
                }
            }

            var e = new ToastNotificationActivatedEventArgsCompat()
            {
                Argument = args,
                UserInput = userInput
            };

            OnActivated[] listeners;
            lock (_onActivated)
            {
                listeners = _onActivated.ToArray();
            }

            foreach (var listener in listeners)
            {
                listener(e);
            }
        }

        private static string _win32Aumid;
        private static string _clsid;

        static ToastNotificationManagerCompat()
        {
            Initialize();
        }

        private static void Initialize()
        {
            // If containerized
            if (DesktopBridgeHelpers.IsContainerized())
            {
                // No need to do anything additional, already registered through manifest
                return;
            }

            // If sparse
            if (DesktopBridgeHelpers.HasIdentity())
            {
                _win32Aumid = GetAumidFromPackageManifest();
            }
            else
            {
                // Win32 apps are uniquely identified based on their process name
                _win32Aumid = GetAumidFromCurrentProcess(Process.GetCurrentProcess());
            }

            // Create and register activator
            var activatorType = CreateAndRegisterActivator();

            // Register via registry
            var currProcess = Process.GetCurrentProcess();

            using (var rootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\AppUserModelId\" + _win32Aumid))
            {
                // If they don't have identity, we need to specify the display assets
                if (!DesktopBridgeHelpers.HasIdentity())
                {
                    IShellItem shortcutItem = null;
                    try
                    {
                        IApplicationResolver appResolver = (IApplicationResolver)new CAppResolver();
                        appResolver.GetBestShortcutForAppID(_win32Aumid, out shortcutItem);
                    }
                    catch
                    {
                    }

                    string displayName = null;
                    string iconPath = null;

                    // First we attempt to use display assets from the shortcut itself
                    if (shortcutItem != null)
                    {
                        try
                        {
                            shortcutItem.GetDisplayName(0, out displayName);

                            ((IShellItemImageFactory)shortcutItem).GetImage(new SIZE(48, 48), SIIGBF.IconOnly | SIIGBF.BiggerSizeOk, out IntPtr nativeHBitmap);

                            if (nativeHBitmap != IntPtr.Zero)
                            {
                                try
                                {
                                    System.Drawing.Bitmap bmp = System.Drawing.Bitmap.FromHbitmap(nativeHBitmap);

                                    if (IsAlphaBitmap(bmp, out var bmpData))
                                    {
                                        var alphaBitmap = GetAlphaBitmapFromBitmapData(bmpData);
                                        iconPath = SaveIconToAppPath(alphaBitmap);
                                    }
                                    else
                                    {
                                        iconPath = SaveIconToAppPath(bmp);
                                    }
                                }
                                catch
                                {
                                }

                                DeleteObject(nativeHBitmap);
                            }
                        }
                        catch
                        {
                        }
                    }

                    // If we didn't get a display name from shortcut
                    if (string.IsNullOrWhiteSpace(displayName))
                    {
                        // We use the one from the process
                        displayName = GetDisplayNameFromCurrentProcess(currProcess);
                    }

                    // If we didn't get an icon from shortcut
                    if (string.IsNullOrWhiteSpace(iconPath))
                    {
                        // We use the one from the process
                        iconPath = ExtractAndObtainIconFromCurrentProcess(currProcess);
                    }

                    // Set the display name and icon uri
                    rootKey.SetValue("DisplayName", displayName);

                    if (iconPath != null)
                    {
                        rootKey.SetValue("IconUri", iconPath);
                    }
                    else
                    {
                        if (rootKey.GetValue("IconUri") != null)
                        {
                            rootKey.DeleteValue("IconUri");
                        }
                    }

                    // Background color only appears in the settings page, format is
                    // hex without leading #, like "FFDDDDDD"
                    rootKey.SetValue("IconBackgroundColor", "FFDDDDDD");
                }

                rootKey.SetValue("CustomActivator", string.Format("{{{0}}}", activatorType.GUID));
            }
        }

        // From https://stackoverflow.com/a/9291151
        private static System.Drawing.Bitmap GetAlphaBitmapFromBitmapData(System.Drawing.Imaging.BitmapData bmpData)
        {
            return new System.Drawing.Bitmap(
                    bmpData.Width,
                    bmpData.Height,
                    bmpData.Stride,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    bmpData.Scan0);
        }

        // From https://stackoverflow.com/a/9291151
        private static bool IsAlphaBitmap(System.Drawing.Bitmap bmp, out System.Drawing.Imaging.BitmapData bmpData)
        {
            var bmBounds = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);

            bmpData = bmp.LockBits(bmBounds, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            try
            {
                for (int y = 0; y <= bmpData.Height - 1; y++)
                {
                    for (int x = 0; x <= bmpData.Width - 1; x++)
                    {
                        var pixelColor = System.Drawing.Color.FromArgb(
                            Marshal.ReadInt32(bmpData.Scan0, (bmpData.Stride * y) + (4 * x)));

                        if (pixelColor.A > 0 & pixelColor.A < 255)
                        {
                            return true;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }

            return false;
        }

        /// <summary>Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.</summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        ///   <para>If the function succeeds, the return value is nonzero.</para>
        ///   <para>If the specified handle is not valid or is currently selected into a DC, the return value is zero.</para>
        /// </returns>
        /// <remarks>
        ///   <para>Do not delete a drawing object (pen or brush) while it is still selected into a DC.</para>
        ///   <para>When a pattern brush is deleted, the bitmap associated with the brush is not deleted. The bitmap must be deleted independently.</para>
        /// </remarks>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);

        private static string GetAumidFromCurrentProcess(Process process)
        {
            IApplicationResolver appResolver = (IApplicationResolver)new CAppResolver();
            appResolver.GetAppIDForProcess(Convert.ToUInt32(process.Id), out string appId, out bool pinningPrevented, out bool explicitAppId, out bool embeddedShortcutValid);
            return appId;
        }

        private static string GetDisplayNameFromCurrentProcess(Process process)
        {
            // If AssemblyTitle is set, use that
            var assemblyTitleAttr = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyTitleAttribute>();
            if (assemblyTitleAttr != null)
            {
                return assemblyTitleAttr.Title;
            }

            // Otherwise, fall back to process name
            return process.ProcessName;
        }

        private static string ExtractAndObtainIconFromCurrentProcess(Process process)
        {
            return ExtractAndObtainIconFromPath(process.MainModule.FileName);
        }

        private static string ExtractAndObtainIconFromPath(string pathToExtract)
        {
            try
            {
                // Extract the icon
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(pathToExtract);

                using (var bmp = icon.ToBitmap())
                {
                    return SaveIconToAppPath(bmp);
                }
            }
            catch
            {
                return null;
            }
        }

        private static string SaveIconToAppPath(System.Drawing.Bitmap bitmap)
        {
            try
            {
                var path = Path.Combine(GetAppDataFolderPath(), "Icon.png");

                // Ensure the directories exist
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                return path;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the app data folder path within the ToastNotificationManagerCompat folder, used for storing icon assets and any additional data.
        /// </summary>
        /// <returns>Returns a string of the absolute folder path.</returns>
        private static string GetAppDataFolderPath()
        {
            string conciseAumid = _win32Aumid.Contains('\\') ? GenerateGuid(_win32Aumid) : _win32Aumid;

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToastNotificationManagerCompat", "Apps", conciseAumid);
        }

        private static Type CreateActivatorType()
        {
            // https://stackoverflow.com/questions/24069352/c-sharp-typebuilder-generate-class-with-function-dynamically
            // For .NET Core we use https://stackoverflow.com/questions/36937276/is-there-any-replace-of-assemblybuilder-definedynamicassembly-in-net-core
            AssemblyName aName = new AssemblyName("DynamicComActivator");
            AssemblyBuilder aBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);

            // For a single-module assembly, the module name is usually the assembly name plus an extension.
            ModuleBuilder mb = aBuilder.DefineDynamicModule(aName.Name);

            // Create class which extends NotificationActivator
            TypeBuilder tb = mb.DefineType(
                name: "MyNotificationActivator",
                attr: TypeAttributes.Public,
                parent: typeof(Internal.InternalNotificationActivator),
                interfaces: new Type[0]);

            if (DesktopBridgeHelpers.IsContainerized())
            {
                _clsid = GetClsidFromPackageManifest();
            }
            else
            {
                _clsid = GenerateGuid(_win32Aumid);
            }

            tb.SetCustomAttribute(new CustomAttributeBuilder(
                con: typeof(GuidAttribute).GetConstructor(new Type[] { typeof(string) }),
                constructorArgs: new object[] { _clsid }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(
                con: typeof(ComVisibleAttribute).GetConstructor(new Type[] { typeof(bool) }),
                constructorArgs: new object[] { true }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(
#pragma warning disable CS0618 // Type or member is obsolete
                con: typeof(ComSourceInterfacesAttribute).GetConstructor(new Type[] { typeof(Type) }),
#pragma warning restore CS0618 // Type or member is obsolete
                constructorArgs: new object[] { typeof(Internal.InternalNotificationActivator.INotificationActivationCallback) }));

            tb.SetCustomAttribute(new CustomAttributeBuilder(
                con: typeof(ClassInterfaceAttribute).GetConstructor(new Type[] { typeof(ClassInterfaceType) }),
                constructorArgs: new object[] { ClassInterfaceType.None }));

            return tb.CreateType();
        }

        private static string GetAumidFromPackageManifest()
        {
            var appxManifestPath = Path.Combine(Package.Current.InstalledLocation.Path, "AppxManifest.xml");
            var doc = new System.Xml.XmlDocument();
            doc.Load(appxManifestPath);

            var namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("default", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            namespaceManager.AddNamespace("desktop", "http://schemas.microsoft.com/appx/manifest/desktop/windows10");
            namespaceManager.AddNamespace("com", "http://schemas.microsoft.com/appx/manifest/com/windows10");

            var appNode = doc.SelectSingleNode("/default:Package/default:Applications/default:Application[1]", namespaceManager);

            if (appNode == null)
            {
                throw new InvalidOperationException("Your MSIX app manifest must have an <Application> entry.");
            }

            return Package.Current.Id.FamilyName + "!" + appNode.Attributes["Id"].Value;
        }

        private static string GetClsidFromPackageManifest()
        {
            var appxManifestPath = Path.Combine(Package.Current.InstalledLocation.Path, "AppxManifest.xml");
            var doc = new System.Xml.XmlDocument();
            doc.Load(appxManifestPath);

            var namespaceManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("default", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            namespaceManager.AddNamespace("desktop", "http://schemas.microsoft.com/appx/manifest/desktop/windows10");
            namespaceManager.AddNamespace("com", "http://schemas.microsoft.com/appx/manifest/com/windows10");

            var activatorClsidNode = doc.SelectSingleNode("/default:Package/default:Applications/default:Application[1]/default:Extensions/desktop:Extension[@Category='windows.toastNotificationActivation']/desktop:ToastNotificationActivation/@ToastActivatorCLSID", namespaceManager);

            if (activatorClsidNode == null)
            {
                throw new InvalidOperationException("Your app manifest must have a toastNotificationActivation extension with a valid ToastActivatorCLSID specified.");
            }

            var clsid = activatorClsidNode.Value;

            // Make sure they have a COM class registration matching the CLSID
            var comClassNode = doc.SelectSingleNode($"/default:Package/default:Applications/default:Application[1]/default:Extensions/com:Extension[@Category='windows.comServer']/com:ComServer/com:ExeServer/com:Class[@Id='{clsid}']", namespaceManager);

            if (comClassNode == null)
            {
                throw new InvalidOperationException("Your app manifest must have a comServer extension with a class ID matching your toastNotificationActivator's CLSID.");
            }

            // Make sure they have a COM class registration matching their current process executable
            var comExeServerNode = comClassNode.ParentNode;

            var specifiedExeRelativePath = comExeServerNode.Attributes["Executable"].Value;
            var specifiedExeFullPath = Path.Combine(Package.Current.InstalledLocation.Path, specifiedExeRelativePath);
            var actualExeFullPath = Process.GetCurrentProcess().MainModule.FileName;

            if (specifiedExeFullPath != actualExeFullPath)
            {
                var correctExeRelativePath = actualExeFullPath.Substring(Package.Current.InstalledLocation.Path.Length + 1);
                throw new InvalidOperationException($"Your app manifest's comServer extension's Executable value is incorrect. It should be \"{correctExeRelativePath}\".");
            }

            // Make sure their arguments are set correctly
            var argumentsNode = comExeServerNode.Attributes.GetNamedItem("Arguments");
            if (argumentsNode == null || argumentsNode.Value != "-ToastActivated")
            {
                throw new InvalidOperationException("Your app manifest's comServer extension for toast activation must have its Arguments set exactly to \"-ToastActivated\"");
            }

            return clsid;
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

        private static Type CreateAndRegisterActivator()
        {
            var activatorType = CreateActivatorType();
            RegisterActivator(activatorType);
            _registeredOnActivated = true;
            return activatorType;
        }

        private static void RegisterActivator(Type activatorType)
        {
            if (!DesktopBridgeHelpers.IsContainerized())
            {
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                RegisterComServer(activatorType, exePath);
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
        public static bool WasCurrentProcessToastActivated()
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
                        typeof(Internal.InternalNotificationActivator.INotificationActivationCallback));
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
#endif

        /// <summary>
        /// Creates a toast notifier.
        /// </summary>
        /// <returns><see cref="ToastNotifier"/></returns>
        public static ToastNotifier CreateToastNotifier()
        {
#if WIN32
            if (DesktopBridgeHelpers.HasIdentity())
            {
                return ToastNotificationManager.CreateToastNotifier();
            }
            else
            {
                return ToastNotificationManager.CreateToastNotifier(_win32Aumid);
            }
#else
            return ToastNotificationManager.CreateToastNotifier();
#endif
        }

        /// <summary>
        /// Gets the <see cref="ToastNotificationHistoryCompat"/> object.
        /// </summary>
        public static ToastNotificationHistoryCompat History
        {
            get
            {
#if WIN32
                return new ToastNotificationHistoryCompat(DesktopBridgeHelpers.HasIdentity() ? null : _win32Aumid);
#else
                return new ToastNotificationHistoryCompat(null);
#endif
            }
        }

        /// <summary>
        /// Gets a value indicating whether http images can be used within toasts. This is true if running with package identity (UWP, MSIX, or sparse package).
        /// </summary>
        public static bool CanUseHttpImages
        {
            get
            {
#if WIN32
                return DesktopBridgeHelpers.HasIdentity();
#else
                return true;
#endif
            }
        }

#if WIN32
        /// <summary>
        /// If you're not using MSIX, call this when your app is being uninstalled to properly clean up all notifications and notification-related resources. Note that this must be called from your app's main EXE (the one that you used notifications for) and not a separate uninstall EXE. If called from a MSIX app, this method no-ops.
        /// </summary>
        public static void Uninstall()
        {
            if (DesktopBridgeHelpers.IsContainerized())
            {
                // Packaged containerized apps automatically clean everything up already
                return;
            }

            if (!DesktopBridgeHelpers.HasIdentity())
            {
                try
                {
                    // Remove all scheduled notifications (do this first before clearing current notifications)
                    var notifier = CreateToastNotifier();
                    foreach (var scheduled in CreateToastNotifier().GetScheduledToastNotifications())
                    {
                        try
                        {
                            notifier.RemoveFromSchedule(scheduled);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    // Clear all current notifications
                    History.Clear();
                }
                catch
                {
                }
            }

            try
            {
                // Remove registry key
                if (_win32Aumid != null)
                {
                    Registry.CurrentUser.DeleteSubKey(@"Software\Classes\AppUserModelId\" + _win32Aumid);
                }
            }
            catch
            {
            }

            try
            {
                if (_clsid != null)
                {
                    Registry.CurrentUser.DeleteSubKey(string.Format("SOFTWARE\\Classes\\CLSID\\{{{0}}}\\LocalServer32", _clsid));
                }
            }
            catch
            {
            }

            if (!DesktopBridgeHelpers.HasIdentity())
            {
                try
                {
                    // Delete any of the app files
                    var appDataFolderPath = GetAppDataFolderPath();
                    if (Directory.Exists(appDataFolderPath))
                    {
                        Directory.Delete(appDataFolderPath, recursive: true);
                    }
                }
                catch
                {
                }
            }
        }
#endif
    }
}

#endif