// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace UITests.Tests.TAEF
{
    public static class MainClass
    {
        public static void Main()
        {
            // Empty main just to force the compiler to copy the DotNet Runtime on the folder as the output (since OutputType = Exe).
            // The .exe is unused, but we do use the dll generated by this compilation
        }
    }
}