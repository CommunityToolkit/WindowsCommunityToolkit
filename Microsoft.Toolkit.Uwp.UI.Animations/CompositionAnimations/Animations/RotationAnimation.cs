// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
