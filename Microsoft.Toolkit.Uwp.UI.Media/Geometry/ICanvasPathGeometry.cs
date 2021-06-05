using Microsoft.Graphics.Canvas.Geometry;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry
{
    internal interface ICanvasPathGeometry
    {
        /// <summary>
        /// Gets the associate CanvasGeometry
        /// </summary>
        CanvasGeometry Geometry { get; }
    }
}
