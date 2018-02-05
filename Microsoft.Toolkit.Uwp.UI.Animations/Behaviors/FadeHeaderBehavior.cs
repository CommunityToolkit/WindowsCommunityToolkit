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

using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs an fade animation on a ListView or GridView Header using composition.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class FadeHeaderBehavior : BehaviorBase<FrameworkElement>
    {
        /// <summary>
        /// Attaches the behavior to the associated object.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if attaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Initialize()
        {
            var result = AssignFadeAnimation();
            return result;
        }

        /// <summary>
        /// Detaches the behavior from the associated object.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if detaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Uninitialize()
        {
            RemoveFadeAnimation();
            return true;
        }

        /// <summary>
        /// If any of the properties are changed then the animation is automatically started depending on the AutomaticallyStart property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as FadeHeaderBehavior;
            b?.AssignFadeAnimation();
        }

        /// <summary>
        /// The UIElement that will be faded.
        /// </summary>
        public static readonly DependencyProperty HeaderElementProperty = DependencyProperty.Register(
            nameof(HeaderElement), typeof(UIElement), typeof(FadeHeaderBehavior), new PropertyMetadata(null, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the target element for the Fading behavior.
        /// </summary>
        /// <remarks>
        /// Set this using the header of a ListView or GridView. You can use the entire root of the header or an element within the header.
        ///
        /// Using this example Header:
        /// <ListView.Header>
        ///     <Grid Name="MyHeader">
        ///     </Grid>
        /// </ListView.Header>
        ///
        /// The behavior would be implemented like this
        /// <FadeHeaderBehavior HeaderElement="{Binding ElementName=HeaderPanel}" />
        /// </remarks>
        public UIElement HeaderElement
        {
            get { return (UIElement)GetValue(HeaderElementProperty); }
            set { SetValue(HeaderElementProperty, value); }
        }

        /// <summary>
        /// Uses Composition API to get the UIElement and sets an ExpressionAnimation
        /// The ExpressionAnimation uses the height of the UIElement to calculate an opacity value
        /// for the Header as it is scrolling off-screen. The opacity reaches 0 when the Header
        /// is entirely scrolled off.
        /// </summary>
        /// <returns><c>true</c> if the assignment was successfull; otherwise, <c>false</c>.</returns>
        private bool AssignFadeAnimation()
        {
            // Confirm that Windows.UI.Xaml.Hosting.ElementCompositionPreview is available (Windows 10 10586 or later).
            if (!ApiInformation.IsMethodPresent("Windows.UI.Xaml.Hosting.ElementCompositionPreview", nameof(ElementCompositionPreview.GetScrollViewerManipulationPropertySet)))
            {
                // Just return true since it's not supported
                return true;
            }

            if (AssociatedObject == null)
            {
                return false;
            }

            var scroller = AssociatedObject as ScrollViewer ?? AssociatedObject.FindDescendant<ScrollViewer>();
            if (scroller == null)
            {
                return false;
            }

            // Implicit operation: Find the Header object of the control if it uses ListViewBase
            if (HeaderElement == null)
            {
                var listElement = AssociatedObject as Windows.UI.Xaml.Controls.ListViewBase ?? AssociatedObject.FindDescendant<Windows.UI.Xaml.Controls.ListViewBase>();
                if (listElement != null)
                {
                    HeaderElement = listElement.Header as UIElement;
                }
            }

            // If no header is set or detected, return.
            if (HeaderElement == null || HeaderElement.RenderSize.Height == 0d)
            {
                return false;
            }

            // Get the ScrollViewer's ManipulationPropertySet
            var scrollViewerManipulationPropSet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);
            var compositor = scrollViewerManipulationPropSet.Compositor;

            // Use the ScrollViewer's Y offset and the header's height to calculate the opacity percentage. Clamp it between 0% and 100%
            var opacityExpression = compositor.CreateExpressionAnimation("Clamp(1 - (-ScrollManipulationPropSet.Translation.Y / HeaderHeight), 0, 1)");

            // Get the ScrollViewerManipulation Reference
            opacityExpression.SetReferenceParameter("ScrollManipulationPropSet", scrollViewerManipulationPropSet);

            // Pass in the height of the header as a Scalar
            opacityExpression.SetScalarParameter("HeaderHeight", (float)HeaderElement.RenderSize.Height);

            // Begin animating
            var targetElement = ElementCompositionPreview.GetElementVisual(HeaderElement);
            targetElement.StartAnimation("Opacity", opacityExpression);

            return true;
        }

        /// <summary>
        /// Remove the opacity animation from the UIElement.
        /// </summary>
        private void RemoveFadeAnimation()
        {
            if (HeaderElement != null)
            {
                var targetElement = ElementCompositionPreview.GetElementVisual(HeaderElement);
                targetElement.StopAnimation("Opacity");
                targetElement.Opacity = 1.0f;
            }
        }
    }
}
