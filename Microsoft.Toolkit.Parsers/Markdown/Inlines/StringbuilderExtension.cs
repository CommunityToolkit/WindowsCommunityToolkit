// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{

    internal static class StringbuilderExtension
    {
        public static void Append(this StringBuilder stringBuilder, ReadOnlySpan<char> txt)
        {
            var buffer = ArrayPool<char>.Shared.Rent(txt.Length);
            txt.CopyTo(buffer);
            stringBuilder.Append(buffer, 0, txt.Length);
            ArrayPool<char>.Shared.Return(buffer, false);
        }
    }
}