using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class SerializablePoint
    {
        public Point Position { get; set; }

        public float Pressure { get; set; }

        public float TiltX { get; set; }

        public float TiltY { get; set; }

        public ulong Timestamp { get; set; }
    }
}