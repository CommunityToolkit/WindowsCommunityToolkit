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

namespace Microsoft.Toolkit.Uwp.Connectivity
{
    /// <summary>
    /// Enumeration denoting connection type.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Connected to wired network
        /// </summary>
        Ethernet,

        /// <summary>
        /// Connected to wireless network
        /// </summary>
        WiFi,

        /// <summary>
        /// Connected to mobile data connection
        /// </summary>
        Data,

        /// <summary>
        /// Connection type not identified
        /// </summary>
        Unknown,
    }
}
