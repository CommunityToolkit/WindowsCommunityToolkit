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

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Enumeration listing all Pin results
    /// </summary>
    public enum PinResult
    {
        /// <summary>
        ///  Unsupported Device
        /// </summary>
        UnsupportedDevice = 0,

        /// <summary>
        ///  Unsupported Windows 10 OS ( Pin supported SDK Version StartMenu >= 15063 ,TaskBar >= 16299)
        /// </summary>
        UnsupportedOs = 1,

        /// <summary>
        /// pin access is denied
        /// </summary>
        PinNotAllowed = 2,

        /// <summary>
        /// App has added startMenu or TaskBar
        /// </summary>
        PinPresent = 3,

        /// <summary>
        /// App has already is avaliable in StartMenu orTaskBar
        /// </summary>
        PinAlreadyPresent = 4,

        /// <summary>
        /// Pin Operation is Failed
        /// </summary>
        PinOperationFailed = 5
    }
}