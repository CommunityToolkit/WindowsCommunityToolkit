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

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// Enum indicating result of Camera Helper initialization.
    /// </summary>
    public enum CameraHelperResult
    {
        /// <summary>
        /// Initialization is Successful
        /// </summary>
        Success,

        /// <summary>
        /// Initialization failed, Frame Reader Creation failed
        /// </summary>
        CreateFrameReaderFailed,

        /// <summary>
        /// Initialization failed, Unable to start Frame Reader
        /// </summary>
        StartFrameReaderFailed,

        /// <summary>
        /// Initialization failed, Frame Source Group is null
        /// </summary>
        NoFrameSourceGroupAvailable,

        /// <summary>
        /// Initialization failed, Frame Source is null
        /// </summary>
        NoFrameSourceAvailable,

        /// <summary>
        /// Access to camera is denied.
        /// </summary>
        CameraAccessDenied,

        /// <summary>
        /// Initialization failed due to an exception.
        /// </summary>
        InitializationFailed_UnknownError,

        /// <summary>
        /// Initialization failed, no comprible frame format are exposed via the frame source
        /// </summary>
        NoCompatibleFrameFormatAvailable
    }
}
