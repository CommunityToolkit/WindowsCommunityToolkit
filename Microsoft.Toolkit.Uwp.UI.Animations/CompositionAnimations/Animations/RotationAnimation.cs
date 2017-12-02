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
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// ScalarAnimation that animates the <see cref="Visual.RotationAngle"/> property
    /// </summary>
    public class RotationAnimation : ScalarAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RotationAnimation"/> class.
        /// </summary>
        public RotationAnimation()
        {
            Target = nameof(Visual.RotationAngle);
        }
    }
}
