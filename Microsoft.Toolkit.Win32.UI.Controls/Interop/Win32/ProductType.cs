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

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    internal enum ProductType : byte
    {
        /// <summary>
        /// The operating system is Windows 10, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional
        /// </summary>
        VER_NT_WORKSTATION = 0x0000001,

        /// <summary>
        /// The system is a domain controller and the operating system is Windows Server 2016, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server
        /// </summary>
        VER_NT_DOMAIN_CONTROLLER = 0x0000002,

        /// <summary>
        /// The operating system is Windows Server 2016, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        /// </summary>
        /// <remarks>
        /// Note that a server that is also a domain controller is reported as <see cref="VER_NT_DOMAIN_CONTROLLER"/>, not <see cref="VER_NT_SERVER"/>
        /// </remarks>
        VER_NT_SERVER = 0x0000003
    }
}