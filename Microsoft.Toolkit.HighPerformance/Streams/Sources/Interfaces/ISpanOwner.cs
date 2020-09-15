using System;

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    internal interface ISpanOwner
    {
        int Length { get; }

        Span<byte> Span { get; }
    }
}
