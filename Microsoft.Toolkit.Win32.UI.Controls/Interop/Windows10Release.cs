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

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop
{
    /// <summary>
    /// Identifies Windows 10 release IDs
    /// </summary>
    internal enum Windows10Release
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 10.0.10240.0
        /// </summary>
        Threshold1 = 1507,

        /// <summary>
        /// 10.0.10586
        /// </summary>
        Threshold2 = 1511,

        /// <summary>
        /// 10.0.14393.0 (Redstone 1)
        /// </summary>
        Anniversary = 1607,

        /// <summary>
        /// 10.0.15063.0 (Redstone 2)
        /// </summary>
        Creators = 1703,

        /// <summary>
        /// 10.0.16299.0 (Redstone 3)
        /// </summary>
        FallCreators = 1709,

        /// <summary>
        /// 10.0.17134.0 (Redstone 4)
        /// </summary>
        April2018 = 1803,
    }
}