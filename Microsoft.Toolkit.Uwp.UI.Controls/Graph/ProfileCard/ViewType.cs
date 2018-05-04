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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The view type.
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// Only the user photo is shown.
        /// </summary>
        PictureOnly = 0,

        /// <summary>
        /// Only the user email is shown.
        /// </summary>
        EmailOnly = 1,

        /// <summary>
        /// A basic user profile is shown, and the user photo is place on the left side.
        /// </summary>
        LargeProfilePhotoLeft = 2,

        /// <summary>
        /// A basic user profile is shown, and the user photo is place on the right side.
        /// </summary>
        LargeProfilePhotoRight = 3
    }
}
