// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static class Assert
    {
        internal static void AreEqual(object expected, object actual)
        {
            if (!expected.Equals(actual))
            {
                throw new Exception($"Assert.AreEqual failed. Expected:<{expected}>. Actual:<{actual}>.");
            }
        }
    }
}
