using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Core.DropShadowPanel
{
    /// <summary>
    /// Any user control can implement this interface to provide a custom alpha mask to it's parent <see cref="DropShadowPanel"/>
    /// </summary>
    public interface IAlphaMaskProvider
    {
        /// <summary>
        /// This method should return the appropiate alpha mask to be used in the shadow of this control
        /// </summary>
        /// <returns>The alpha mask as a composition brush</returns>
        CompositionBrush GetAlphaMask();
    }
}
