using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework.NetPasswordManager
{
    internal class NetPasswordManager : IPasswordManager
    {
        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr CredentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredWrite([In] ref NativeCredential userCredential, [In] UInt32 flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        static extern bool CredFree([In] IntPtr cred);

        [DllImport("advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode)]
        private static extern bool CredDelete(string target, CRED_TYPE type, int flags);

        public enum CRED_TYPE : uint
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5,
            DOMAIN_EXTENDED = 6,
            MAXIMUM = 7,      // Maximum supported cred type
            MAXIMUM_EX = (MAXIMUM + 1000),  // Allow new applications to run on old OSes
        }
        public enum CRED_PERSIST : uint
        {
            SESSION = 1,
            LOCAL_MACHINE = 2,
            ENTERPRISE = 3,
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NativeCredential
        {
            public UInt32 Flags;
            public CRED_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public IntPtr CredentialBlob;
            public UInt32 Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;

            /// <summary>
            /// This method derives a NativeCredential instance from a given Credential instance.
            /// </summary>
            /// <param name="cred">The managed Credential counterpart containing data to be stored.</param>
            /// <returns>A NativeCredential instance that is derived from the given Credential
            /// instance.</returns>
            internal static NativeCredential GetNativeCredential(Credential cred)
            {
                NativeCredential ncred = new NativeCredential();
                ncred.AttributeCount = 0;
                ncred.Attributes = IntPtr.Zero;
                ncred.Comment = IntPtr.Zero;
                ncred.TargetAlias = IntPtr.Zero;
                ncred.Type = CRED_TYPE.GENERIC;
                ncred.Persist = (UInt32)cred.Persist;
                ncred.CredentialBlobSize = (UInt32)cred.CredentialBlobSize;
                ncred.TargetName = Marshal.StringToCoTaskMemUni(cred.TargetName);
                ncred.CredentialBlob = Marshal.StringToCoTaskMemUni(cred.CredentialBlob);
                ncred.UserName = Marshal.StringToCoTaskMemUni(System.Environment.UserName);
                return ncred;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct Credential
        {
            public UInt32 Flags;
            public CRED_TYPE Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public string CredentialBlob;
            public CRED_PERSIST Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }

        #region Critical Handle Type definition
        sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            // Set the handle.
            internal CriticalCredentialHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }

            internal Credential GetCredential()
            {
                if (!IsInvalid)
                {
                    // Get the Credential from the mem location
                    NativeCredential ncred = (NativeCredential)Marshal.PtrToStructure(handle,
                          typeof(NativeCredential));

                    // Create a managed Credential type and fill it with data from the native counterpart.
                    Credential cred = new Credential();
                    cred.CredentialBlobSize = ncred.CredentialBlobSize;
                    cred.CredentialBlob = Marshal.PtrToStringUni(ncred.CredentialBlob,
                          (int)ncred.CredentialBlobSize / 2);
                    cred.UserName = Marshal.PtrToStringUni(ncred.UserName);
                    cred.TargetName = Marshal.PtrToStringUni(ncred.TargetName);
                    cred.TargetAlias = Marshal.PtrToStringUni(ncred.TargetAlias);
                    cred.Type = ncred.Type;
                    cred.Flags = ncred.Flags;
                    cred.Persist = (CRED_PERSIST)ncred.Persist;
                    return cred;
                }
                else
                {
                    throw new InvalidOperationException("Invalid CriticalHandle!");
                }
            }

            // Perform any specific actions to release the handle in the ReleaseHandle method.
            // Often, you need to use Pinvoke to make a call into the Win32 API to release the 
            // handle. In this case, however, we can use the Marshal class to release the unmanaged memory.

            override protected bool ReleaseHandle()
            {
                // If the handle was set, free it. Return success.
                if (!IsInvalid)
                {
                    // NOTE: We should also ZERO out the memory allocated to the handle, before free'ing it
                    // so there are no traces of the sensitive data left in memory.
                    CredFree(handle);
                    // Mark the handle as invalid for future users.
                    SetHandleAsInvalid();
                    return true;
                }
                // Return false. 
                return false;
            }
        }
        #endregion
        public void Store(string resource, PasswordCredential credential)
        {
            // Validations.

            byte[] byteArray = Encoding.Unicode.GetBytes(credential.Password);
            if (byteArray.Length > 512)
                throw new ArgumentOutOfRangeException("The secret message has exceeded 512 bytes.");

            // Go ahead with what we have are stuff it into the CredMan structures.
            Credential cred = new Credential();
            cred.TargetName = resource;
            cred.UserName = credential.UserName;
            cred.CredentialBlob = credential.Password;
            cred.CredentialBlobSize = (UInt32)Encoding.Unicode.GetBytes(credential.Password).Length;
            cred.AttributeCount = 0;
            cred.Attributes = IntPtr.Zero;
            cred.Comment = null;
            cred.TargetAlias = null;
            cred.Type = CRED_TYPE.DOMAIN_PASSWORD;
            cred.Persist = CRED_PERSIST.ENTERPRISE;
            NativeCredential ncred = NativeCredential.GetNativeCredential(cred);
            // Write the info into the CredMan storage.
            bool written = CredWrite(ref ncred, 0);
            int lastError = Marshal.GetLastWin32Error();
            if (!written)
            {
                string message = string.Format("CredWrite failed with the error code {0}.", lastError);
                throw new Exception(message);
            }
        }

        public PasswordCredential Get(string key)
        {
            IntPtr nCredPtr;
            PasswordCredential passCred = new PasswordCredential();

            bool read = CredRead(key, CRED_TYPE.GENERIC, 0, out nCredPtr);
            int lastError = Marshal.GetHRForLastWin32Error();

            if (read)
            {
                CriticalCredentialHandle critCred = new CriticalCredentialHandle(nCredPtr);

                Credential credential = critCred.GetCredential();
                passCred.UserName = credential.UserName;
                passCred.Password = credential.CredentialBlob;
            }
            else
            {
                throw new Exception("Error to reach the Credential");
            }

            return passCred;
        }

        public void Remove(string key)
        {
            CredDelete(key, CRED_TYPE.GENERIC, 0);
        }
    }
}
