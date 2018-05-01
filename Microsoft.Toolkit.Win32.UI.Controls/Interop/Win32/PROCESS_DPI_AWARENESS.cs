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
    /// <summary>
    /// Identifies dots per inch (dpi) awareness values.
    /// </summary>
    internal enum PROCESS_DPI_AWARENESS
    {
        PROCESS_DPI_UNINITIALIZED = -1,

        /// <summary>
        /// DPI unaware. This app does not scale for DPI changes and is always assumed to have a scale factor of 100% (96 DPI). It will be automatically scaled by the system on any other DPI setting.
        /// </summary>
        PROCESS_DPI_UNAWARE = 0,

        /// <summary>
        /// System DPI aware. This app does not scale for DPI changes. It will query for the DPI once and use that value for the lifetime of the app. If the DPI changes, the app will not adjust to the new DPI value. It will be automatically scaled up or down by the system when the DPI changes from the system value.
        /// </summary>
        PROCESS_SYSTEM_DPI_AWARE = 1,

        /// <summary>
        /// Per monitor DPI aware. This app checks for the DPI when it is created and adjusts the scale factor whenever the DPI changes. These applications are not automatically scaled by the system.
        /// </summary>
        PROCESS_PER_MONITOR_DPI_AWARE = 2
    }
}
