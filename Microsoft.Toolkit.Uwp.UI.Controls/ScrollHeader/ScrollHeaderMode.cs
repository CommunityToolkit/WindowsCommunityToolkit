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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Define mode available for  <see cref="ScrollHeader"/>
    /// </summary>
    public enum ScrollHeaderMode
    {
        /// <summary>
        /// No change. This is the default value.
        /// </summary>
        None,

        /// <summary>
        /// Enable quick return mode.
        /// </summary>
        QuickReturn,

        /// <summary>
        /// Enable sticky mode.
        /// </summary>
        Sticky,

        /// <summary>
        /// Enable fade mode.
        /// </summary>
        Fade
    }
}
