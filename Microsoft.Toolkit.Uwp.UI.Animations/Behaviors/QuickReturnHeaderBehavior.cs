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
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs an animation on a ListView or GridView Header to make it quick return using composition.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class QuickReturnHeaderBehavior : BehaviorBase<FrameworkElement>
    {
        /// <summary>
        /// Attaches the behavior to the associated object.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if attaching succeeded; otherwise <c>false</c>.
        /// </returns>
        protected override bool Initialize()
        {
            var result = AssignAnimation();
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
            RemoveAnimation();
            return true;
        }

        /// <summary>
        /// The UIElement that will be faded.
        /// </summary>
        public static readonly DependencyProperty HeaderElementProperty = DependencyProperty.Register(
            nameof(HeaderElement), typeof(UIElement), typeof(QuickReturnHeaderBehavior), new PropertyMetadata(null, PropertyChangedCallback));

        private ScrollViewer _scrollViewer;
        private double _headerPosition;
        private CompositionPropertySet _scrollProperties;
        private CompositionPropertySet _animationProperties;
        private Visual _headerVisual;

        /// <summary>
        /// Gets or sets the target element for the ScrollHeader behavior.
        /// </summary>
        /// <remarks>
        /// Set this using the header of a ListView or GridView.
        /// </remarks>
        public UIElement HeaderElement
        {
            get { return (UIElement)GetValue(HeaderElementProperty); }
            set { SetValue(HeaderElementProperty, value); }
        }

        /// <summary>
        /// Show the header
        /// </summary>
        public void Show()
        {
            if (_headerVisual != null && _scrollViewer != null)
            {
                _animationProperties.InsertScalar("OffsetY", 0.0f);
            }
        }

        /// <summary>
        /// If any of the properties are changed then the animation is automatically started depending on the QuickReturn and IsSticky properties.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as QuickReturnHeaderBehavior;
            b?.AssignAnimation();
        }

        /// <summary>
        /// Uses Composition API to get the UIElement and sets an ExpressionAnimation
        /// The ExpressionAnimation uses the height of the UIElement to calculate an opacity value
        /// for the Header as it is scrolling off-screen. The opacity reaches 0 when the Header
        /// is entirely scrolled off.
        /// </summary>
        /// <returns><c>true</c> if the assignment was successfull; otherwise, <c>false</c>.</returns>
        private bool AssignAnimation()
        {
            StopAnimation();

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

            if (_scrollViewer == null)
            {
                _scrollViewer = AssociatedObject as ScrollViewer ?? AssociatedObject.FindDescendant<ScrollViewer>();
            }

            if (_scrollViewer == null)
            {
                return false;
            }

            if (_scrollProperties == null)
            {
                _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
            }

            if (_scrollProperties == null)
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

            var headerElement = HeaderElement as FrameworkElement;
            if (headerElement == null || headerElement.RenderSize.Height == 0)
            {
                return false;
            }

            if (_headerVisual == null)
            {
                _headerVisual = ElementCompositionPreview.GetElementVisual(headerElement);
            }

            if (_headerVisual == null)
            {
                return false;
            }

            _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            headerElement.SizeChanged -= ScrollHeader_SizeChanged;
            headerElement.SizeChanged += ScrollHeader_SizeChanged;

            var compositor = _scrollProperties.Compositor;

            if (_animationProperties == null)
            {
                _animationProperties = compositor.CreatePropertySet();
                _animationProperties.InsertScalar("OffsetY", 0.0f);
            }

            var expressionAnimation = compositor.CreateExpressionAnimation($"max(min(animationProperties.OffsetY, -ScrollingProperties.Translation.Y), 0)");
            expressionAnimation.SetReferenceParameter("ScrollingProperties", _scrollProperties);
            expressionAnimation.SetReferenceParameter("animationProperties", _animationProperties);

            _headerVisual.StartAnimation("Offset.Y", expressionAnimation);

            return true;
        }

        /// <summary>
        /// Remove the animation from the UIElement.
        /// </summary>
        private void RemoveAnimation()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            }

            var element = HeaderElement as FrameworkElement;
            if (element != null)
            {
                element.SizeChanged -= ScrollHeader_SizeChanged;
            }

            StopAnimation();
        }

        /// <summary>
        /// Stop the animation of the UIElement.
        /// </summary>
        private void StopAnimation()
        {
            _headerVisual?.StopAnimation("Offset.Y");

            _animationProperties?.InsertScalar("OffsetY", 0.0f);

            if (_headerVisual != null)
            {
                var offset = _headerVisual.Offset;
                offset.Y = 0.0f;
                _headerVisual.Offset = offset;
            }
        }

        private void ScrollHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AssignAnimation();
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (_animationProperties != null)
            {
                FrameworkElement header = (FrameworkElement)HeaderElement;
                var headerHeight = header.ActualHeight;
                if (_headerPosition + headerHeight < _scrollViewer.VerticalOffset)
                {
                    // scrolling down: move header down, so it is just above screen
                    _headerPosition = _scrollViewer.VerticalOffset - headerHeight;
                    _animationProperties.InsertScalar("OffsetY", (float)_headerPosition);
                }
                else if (_headerPosition > _scrollViewer.VerticalOffset)
                {
                    // scrolling up: move header up, align with top border.
                    // the expression animation makes sure it never relly is shown below border, so no lag effect!
                    _headerPosition = _scrollViewer.VerticalOffset;
                    _animationProperties.InsertScalar("OffsetY", (float)_headerPosition);
                }
            }
        }
    }
}
