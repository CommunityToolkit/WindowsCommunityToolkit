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

using System;
using Microsoft.Xaml.Interactivity;
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
    public class QuickReturnHeaderBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// The UIElement that will be faded.
        /// </summary>
        public static readonly DependencyProperty HeaderElementProperty = DependencyProperty.Register(
            nameof(HeaderElement), typeof(UIElement), typeof(QuickReturnHeaderBehavior), new PropertyMetadata(null, PropertyChangedCallback));

        private ScrollViewer _scrollViewer;
        private double _previousVerticalScrollOffset;
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
                _previousVerticalScrollOffset = _scrollViewer.VerticalOffset;

                _animationProperties.InsertScalar("OffsetY", 0.0f);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssignAnimation();
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            RemoveAnimation();
            AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
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
        /// Called when the associated object is loaded.
        /// </summary>
        /// <param name="sender">The associated object</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data</param>
        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AssignAnimation();
        }

        /// <summary>
        /// Uses Composition API to get the UIElement and sets an ExpressionAnimation
        /// The ExpressionAnimation uses the height of the UIElement to calculate an opacity value
        /// for the Header as it is scrolling off-screen. The opacity reaches 0 when the Header
        /// is entirely scrolled off.
        /// </summary>
        private void AssignAnimation()
        {
            StopAnimation();

            // Confirm that Windows.UI.Xaml.Hosting.ElementCompositionPreview is available (Windows 10 10586 or later).
            if (!ApiInformation.IsMethodPresent("Windows.UI.Xaml.Hosting.ElementCompositionPreview", nameof(ElementCompositionPreview.GetScrollViewerManipulationPropertySet)))
            {
                return;
            }

            if (AssociatedObject == null)
            {
                return;
            }

            if (_scrollViewer == null)
            {
                _scrollViewer = AssociatedObject as ScrollViewer ?? AssociatedObject.FindDescendant<ScrollViewer>();
            }

            if (_scrollViewer == null)
            {
                return;
            }

            if (_scrollProperties == null)
            {
                _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
            }

            if (_scrollProperties == null)
            {
                return;
            }

            // Implicit operation: Find the Header object of the control if it uses ListViewBase
            if (HeaderElement == null)
            {
                var listElement = AssociatedObject as ListViewBase ?? AssociatedObject.FindDescendant<ListViewBase>();

                if (listElement != null)
                {
                    HeaderElement = listElement.Header as UIElement;
                }
            }

            if (!(HeaderElement is FrameworkElement))
            {
                return;
            }

            if (_headerVisual == null)
            {
                _headerVisual = ElementCompositionPreview.GetElementVisual(HeaderElement);
            }

            if (_headerVisual == null)
            {
                return;
            }

            _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            (HeaderElement as FrameworkElement).SizeChanged -= ScrollHeader_SizeChanged;
            (HeaderElement as FrameworkElement).SizeChanged += ScrollHeader_SizeChanged;

            var compositor = _scrollProperties.Compositor;

            if (_animationProperties == null)
            {
                _animationProperties = compositor.CreatePropertySet();
                _animationProperties.InsertScalar("OffsetY", 0.0f);
            }

            _previousVerticalScrollOffset = _scrollViewer.VerticalOffset;

            ExpressionAnimation expressionAnimation = compositor.CreateExpressionAnimation($"max(animationProperties.OffsetY - ScrollingProperties.Translation.Y, 0)");
            expressionAnimation.SetReferenceParameter("ScrollingProperties", _scrollProperties);
            expressionAnimation.SetReferenceParameter("animationProperties", _animationProperties);

            _headerVisual.StartAnimation("Offset.Y", expressionAnimation);
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
                float oldOffsetY;
                _animationProperties.TryGetScalar("OffsetY", out oldOffsetY);

                var delta = _scrollViewer.VerticalOffset - _previousVerticalScrollOffset;
                _previousVerticalScrollOffset = _scrollViewer.VerticalOffset;

                var newOffsetY = oldOffsetY - (float)delta;

                // Keep values within negative header size and 0
                FrameworkElement header = (FrameworkElement)HeaderElement;
                newOffsetY = Math.Max((float)-header.ActualHeight, newOffsetY);
                newOffsetY = Math.Min(0, newOffsetY);

                if (oldOffsetY != newOffsetY)
                {
                    _animationProperties.InsertScalar("OffsetY", newOffsetY);
                }
            }
        }
    }
}
