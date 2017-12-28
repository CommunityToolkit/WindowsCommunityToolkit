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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Specifies what type of access the app is granted
    /// </summary>
    [Flags]
    public enum OneDriveScopes
    {
        /// <summary>
        /// Authentication Provider does not use scopes (.ie ADAL)
        /// </summary>
        None = 0,

        /// <summary>
        /// Allow to get a refresh token
        /// </summary>
        OfflineAccess = 1,

        /// <summary>
        /// Grants read-only permission
        /// </summary>
        ReadOnly = 2,

        /// <summary>
        /// Grants read/write permission
        /// </summary>
        ReadWrite = 4,

        /// <summary>
        /// Grants read/write permission to a specific folder for your application
        /// </summary>
        AppFolder = 8,

        /// <summary>
        /// Single sign-in behavior.
        /// </summary>
        WlSignin = 16
    }
}
