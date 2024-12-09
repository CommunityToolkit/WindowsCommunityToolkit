// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// Based on: https://github.com/microsoft/XamlBehaviorsWpf/blob/master/src/Microsoft.Xaml.Behaviors/Layout/MouseDragElementBehavior.cs

#nullable enable

using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI;

#pragma warning disable SA1124 // I'm not sure about this, this is a pretty controversial rule.

/// <summary>
/// Provides attached dependency properties for the <see cref="FrameworkElement"/> type.
/// </summary>
public static partial class FrameworkElementExtensions
{
    #region Public properties

    #region CanDragElement

    /// <summary>
    /// Attached <see cref="DependencyProperty"/> for repositions the element
    /// in response to mouse drag gestures on the element.
    /// </summary>
    public static readonly DependencyProperty CanDragElementProperty =
        DependencyProperty.RegisterAttached(
            nameof(CanDragElementProperty).Replace("Property", string.Empty),
            typeof(bool),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(false, OnCanDragElementChanged));

    /// <summary>
    /// Gets the <see cref="bool"/> that enables/disables repositions the element
    /// in response to mouse drag gestures on the element.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="bool"/> from</param>
    /// <returns>The <see cref="bool"/> associated with the <see cref="FrameworkElement"/></returns>
    public static bool GetCanDragElement(FrameworkElement obj)
    {
        return (bool)obj.GetValue(CanDragElementProperty);
    }

    /// <summary>
    /// Sets the <see cref="bool"/> that enables/disables repositions the element
    /// in response to mouse drag gestures on the element.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="bool"/> with</param>
    /// <param name="value">The <see cref="bool"/> for binding to the <see cref="FrameworkElement"/></param>
    public static void SetCanDragElement(FrameworkElement obj, bool value)
    {
        obj.SetValue(CanDragElementProperty, value);
    }

    private static void OnCanDragElementChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not FrameworkElement frameworkElement)
        {
            throw new ArgumentException($"Element should be {nameof(FrameworkElement)}.");
        }

        if (args.OldValue is true)
        {
            frameworkElement.RemoveHandler(UIElement.PointerPressedEvent, new PointerEventHandler(OnPointerPressed));
        }

        if (args.NewValue is true)
        {
            frameworkElement.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(OnPointerPressed), handledEventsToo: false);
        }
    }

    #endregion

    #region DragX

    /// <summary>
    /// Dependency property for the X position of the dragged element, relative to the left of the root element.
    /// </summary>
    public static readonly DependencyProperty DragXProperty =
        DependencyProperty.RegisterAttached(
            nameof(DragXProperty).Replace("Property", string.Empty),
            typeof(double),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(double.NaN, OnDragXChanged));

    /// <summary>
    /// Gets the X position of the dragged element, relative to the left of the root element. This is a dependency property.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="double"/> from</param>
    /// <returns>The <see cref="double"/> associated with the <see cref="FrameworkElement"/></returns>
    public static double GetDragX(FrameworkElement obj)
    {
        return (double)obj.GetValue(DragXProperty);
    }

    /// <summary>
    /// Sets the X position of the dragged element, relative to the left of the root element. This is a dependency property.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="double"/> with</param>
    /// <param name="value">The <see cref="double"/> for binding to the <see cref="FrameworkElement"/></param>
    public static void SetDragX(FrameworkElement obj, double value)
    {
        obj.SetValue(DragXProperty, value);
    }

    private static void OnDragXChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not FrameworkElement frameworkElement)
        {
            throw new ArgumentException($"Element should be {nameof(FrameworkElement)}.");
        }

        UpdatePosition(frameworkElement, new Point((double)args.NewValue, GetDragY(frameworkElement)));
    }

    #endregion

    #region DragY

    /// <summary>
    /// Dependency property for the Y position of the dragged element, relative to the top of the root element.
    /// </summary>
    public static readonly DependencyProperty DragYProperty =
        DependencyProperty.RegisterAttached(
            nameof(DragYProperty).Replace("Property", string.Empty),
            typeof(double),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(double.NaN, OnDragYChanged));

    /// <summary>
    /// Gets or sets the Y position of the dragged element, relative to the top of the root element. This is a dependency property.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="double"/> from</param>
    /// <returns>The <see cref="double"/> associated with the <see cref="FrameworkElement"/></returns>
    public static double GetDragY(FrameworkElement obj)
    {
        return (double)obj.GetValue(DragYProperty);
    }

    /// <summary>
    /// Gets or sets the Y position of the dragged element, relative to the top of the root element. This is a dependency property.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="double"/> with</param>
    /// <param name="value">The <see cref="double"/> for binding to the <see cref="FrameworkElement"/></param>
    public static void SetDragY(FrameworkElement obj, double value)
    {
        obj.SetValue(DragYProperty, value);
    }

    private static void OnDragYChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not FrameworkElement frameworkElement)
        {
            throw new ArgumentException($"Element should be {nameof(FrameworkElement)}.");
        }

        UpdatePosition(frameworkElement, new Point(GetDragX(frameworkElement), (double)args.NewValue));
    }

    #endregion

    #region ConstrainDragToParentBounds

    /// <summary>
    /// Dependency property for the ConstrainDragToParentBounds property. If true, the dragged element will be constrained to stay within the bounds of its parent container.
    /// </summary>
    public static readonly DependencyProperty ConstrainDragToParentBoundsProperty =
        DependencyProperty.RegisterAttached(
            nameof(ConstrainDragToParentBoundsProperty).Replace("Property", string.Empty),
            typeof(bool),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(false, OnConstrainDragToParentBoundsChanged));

    /// <summary>
    /// Gets or sets a value indicating whether the dragged element is constrained to stay within the bounds of its parent container. This is a dependency property.
    /// </summary>
    /// <value>
    ///     <c>True</c> if the dragged element should be constrained to its parents bounds; otherwise, <c>False</c>.
    /// </value>
    /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="bool"/> from</param>
    /// <returns>The <see cref="bool"/> associated with the <see cref="FrameworkElement"/></returns>
    public static bool GetConstrainDragToParentBounds(FrameworkElement obj)
    {
        return (bool)obj.GetValue(ConstrainDragToParentBoundsProperty);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the dragged element is constrained to stay within the bounds of its parent container. This is a dependency property.
    /// </summary>
    /// <value>
    ///     <c>True</c> if the dragged element should be constrained to its parents bounds; otherwise, <c>False</c>.
    /// </value>
    /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="bool"/> with</param>
    /// <param name="value">The <see cref="bool"/> for binding to the <see cref="FrameworkElement"/></param>
    public static void SetConstrainDragToParentBounds(FrameworkElement obj, bool value)
    {
        obj.SetValue(ConstrainDragToParentBoundsProperty, value);
    }

    private static void OnConstrainDragToParentBoundsChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not FrameworkElement frameworkElement)
        {
            throw new ArgumentException($"Element should be {nameof(FrameworkElement)}.");
        }

        UpdatePosition(frameworkElement, new Point(GetDragX(frameworkElement), GetDragY(frameworkElement)));
    }

    #endregion

    #endregion

    #region Private properties

    #region SettingPosition

    private static readonly DependencyProperty SettingPositionProperty =
        DependencyProperty.RegisterAttached(
            nameof(SettingPositionProperty).Replace("Property", string.Empty),
            typeof(bool),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(false));

    private static bool GetSettingPosition(FrameworkElement element)
    {
        return (bool)element.GetValue(SettingPositionProperty);
    }

    private static void SetSettingPosition(FrameworkElement element, bool value)
    {
        element.SetValue(SettingPositionProperty, value);
    }

    #endregion

    #region RelativePosition

    private static readonly DependencyProperty RelativePositionProperty =
        DependencyProperty.RegisterAttached(
            nameof(RelativePositionProperty).Replace("Property", string.Empty),
            typeof(Point),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(default(Point)));

    private static Point GetRelativePosition(FrameworkElement element)
    {
        return (Point)element.GetValue(RelativePositionProperty);
    }

    private static void SetRelativePosition(FrameworkElement element, Point value)
    {
        element.SetValue(RelativePositionProperty, value);
    }

    #endregion

    #region CachedRenderTransform

    private static readonly DependencyProperty CachedRenderTransformProperty =
        DependencyProperty.RegisterAttached(
            nameof(CachedRenderTransformProperty).Replace("Property", string.Empty),
            typeof(Transform),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(null));

    private static Transform GetCachedRenderTransform(FrameworkElement element)
    {
        return (Transform)element.GetValue(CachedRenderTransformProperty);
    }

    private static void SetCachedRenderTransform(FrameworkElement element, Transform value)
    {
        element.SetValue(CachedRenderTransformProperty, value);
    }

    #endregion

    /// <summary>
    /// Gets the on-screen position of the associated element in root coordinates.
    /// </summary>
    /// <value>The on-screen position of the associated element in root coordinates.</value>
    private static Point GetActualPosition(FrameworkElement element)
    {
        GeneralTransform elementToRoot = element.TransformToVisual(GetRootElement(element));
        Point translation = GetTransformOffset(elementToRoot);
        return new Point(translation.X, translation.Y);
    }

    /// <summary>
    /// Gets the element bounds in element coordinates.
    /// </summary>
    /// <value>The element bounds in element coordinates.</value>
    private static Rect GetElementBounds(FrameworkElement element)
    {
        Rect layoutRect = GetLayoutRect(element);
        return new Rect(new Point(0, 0), new Size(layoutRect.Width, layoutRect.Height));
    }

    /// <summary>
    /// Get the layout rectangle of an element, by getting the layout slot and then computing which portion of the slot is being used.
    /// </summary>
    /// <param name="element">The element whose layout Rect will be retrieved.</param>
    /// <returns>The layout Rect of that element.</returns>
    private static Rect GetLayoutRect(FrameworkElement element)
    {
        double actualWidth = element.ActualWidth;
        double actualHeight = element.ActualHeight;

        // Use RenderSize here because that works for SL Image and MediaElement - the other uses fo ActualWidth/Height are correct even for these element types
        if (element is Image)
        {
            if (element.Parent is Canvas)
            {
                actualWidth = double.IsNaN(element.Width) ? actualWidth : element.Width;
                actualHeight = double.IsNaN(element.Height) ? actualHeight : element.Height;
            }
            else
            {
                actualWidth = element.RenderSize.Width;
                actualHeight = element.RenderSize.Height;
            }
        }

        actualWidth = element.Visibility == Visibility.Collapsed ? 0 : actualWidth;
        actualHeight = element.Visibility == Visibility.Collapsed ? 0 : actualHeight;
        Thickness margin = element.Margin;

        Rect slotRect = LayoutInformation.GetLayoutSlot(element);

        double left = 0.0;
        double top = 0.0;

        switch (element.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
                left = slotRect.Left + margin.Left;
                break;

            case HorizontalAlignment.Center:
                left = ((((slotRect.Left + margin.Left) + slotRect.Right) - margin.Right) / 2.0) - (actualWidth / 2.0);
                break;

            case HorizontalAlignment.Right:
                left = (slotRect.Right - margin.Right) - actualWidth;
                break;

            case HorizontalAlignment.Stretch:
                left = Math.Max((double)(slotRect.Left + margin.Left), (double)(((((slotRect.Left + margin.Left) + slotRect.Right) - margin.Right) / 2.0) - (actualWidth / 2.0)));
                break;
        }

        switch (element.VerticalAlignment)
        {
            case VerticalAlignment.Top:
                top = slotRect.Top + margin.Top;
                break;

            case VerticalAlignment.Center:
                top = ((((slotRect.Top + margin.Top) + slotRect.Bottom) - margin.Bottom) / 2.0) - (actualHeight / 2.0);
                break;

            case VerticalAlignment.Bottom:
                top = (slotRect.Bottom - margin.Bottom) - actualHeight;
                break;

            case VerticalAlignment.Stretch:
                top = Math.Max((double)(slotRect.Top + margin.Top), (double)(((((slotRect.Top + margin.Top) + slotRect.Bottom) - margin.Bottom) / 2.0) - (actualHeight / 2.0)));
                break;
        }

        return new Rect(left, top, actualWidth, actualHeight);
    }

    /// <summary>
    /// Gets the parent element of the associated object.
    /// </summary>
    /// <value>The parent element of the associated object.</value>
    private static FrameworkElement? GetParentElement(FrameworkElement element)
    {
        return element.Parent as FrameworkElement;
    }

    /// <summary>
    /// Gets the root element of the scene in which the associated object is located.
    /// </summary>
    /// <value>The root element of the scene in which the associated object is located.</value>
    private static UIElement GetRootElement(FrameworkElement element)
    {
        DependencyObject child = element;
        DependencyObject parent = child;
        while (parent != null)
        {
            child = parent;
            parent = VisualTreeHelper.GetParent(child);
        }

        return (UIElement)child;
    }

    /// <summary>
    /// Gets and sets the RenderTransform of the associated element.
    /// </summary>
    private static Transform GetRenderTransform(FrameworkElement element)
    {
        var cachedRenderTransform = GetCachedRenderTransform(element);
        if (cachedRenderTransform == null ||
            !object.ReferenceEquals(cachedRenderTransform, element.RenderTransform))
        {
            Transform clonedTransform = CloneTransform(element.RenderTransform);
            SetRenderTransform(element, clonedTransform);
            cachedRenderTransform = clonedTransform;
        }

        return cachedRenderTransform;
    }

    /// <summary>
    /// Gets and sets the RenderTransform of the associated element.
    /// </summary>
    private static void SetRenderTransform(FrameworkElement element, Transform value)
    {
        var cachedRenderTransform = GetCachedRenderTransform(element);
        if (cachedRenderTransform != value)
        {
            SetCachedRenderTransform(element, value);
            element.RenderTransform = value;
        }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Attempts to update the position of the associated element to the specified coordinates.
    /// </summary>
    /// <param name="element">The associated element.</param>
    /// <param name="point">The desired position of the element in root coordinates.</param>
    private static void UpdatePosition(FrameworkElement element, Point point)
    {
        if (!GetSettingPosition(element) && element != null)
        {
            GeneralTransform elementToRoot = element.TransformToVisual(GetRootElement(element));
            Point translation = GetTransformOffset(elementToRoot);
            double xChange = double.IsNaN(point.X) ? 0 : point.X - translation.X;
            double yChange = double.IsNaN(point.Y) ? 0 : point.Y - translation.Y;
            ApplyTranslation(element, xChange, yChange);
        }
    }

    /// <summary>
    /// Applies a relative position translation to the associated element.
    /// </summary>
    /// <param name="element">The associated element.</param>
    /// <param name="x">The X component of the desired translation in root coordinates.</param>
    /// <param name="y">The Y component of the desired translation in root coordinates.</param>
    private static void ApplyTranslation(FrameworkElement element, double x, double y)
    {
        var parentElement = GetParentElement(element);
        if (parentElement != null)
        {
            GeneralTransform rootToParent = GetRootElement(element).TransformToVisual(parentElement);
            Point transformedPoint = TransformAsVector(rootToParent, x, y);
            x = transformedPoint.X;
            y = transformedPoint.Y;

            if (GetConstrainDragToParentBounds(element))
            {
                Rect parentBounds = new Rect(0, 0, parentElement.ActualWidth, parentElement.ActualHeight);

                GeneralTransform objectToParent = element.TransformToVisual(parentElement);
                Rect objectBoundingBox = GetElementBounds(element);
                objectBoundingBox = objectToParent.TransformBounds(objectBoundingBox);

                Rect endPosition = objectBoundingBox;
                endPosition.X += x;
                endPosition.Y += y;

                if (!RectContainsRect(parentBounds, endPosition))
                {
                    if (endPosition.X < parentBounds.Left)
                    {
                        double diff = endPosition.X - parentBounds.Left;
                        x -= diff;
                    }
                    else if (endPosition.Right > parentBounds.Right)
                    {
                        double diff = endPosition.Right - parentBounds.Right;
                        x -= diff;
                    }

                    if (endPosition.Y < parentBounds.Top)
                    {
                        double diff = endPosition.Y - parentBounds.Top;
                        y -= diff;
                    }
                    else if (endPosition.Bottom > parentBounds.Bottom)
                    {
                        double diff = endPosition.Bottom - parentBounds.Bottom;
                        y -= diff;
                    }
                }
            }

            ApplyTranslationTransform(element, x, y);
        }
    }

    /// <summary>
    /// Applies the given translation to the RenderTransform of the selected element.
    /// </summary>
    /// <param name="element">The associated element.</param>
    /// <param name="x">The X component of the translation in parent coordinates.</param>
    /// <param name="y">The Y component of the translation in parent coordinates.</param>
    private static void ApplyTranslationTransform(FrameworkElement element, double x, double y)
    {
        Transform renderTransform = GetRenderTransform(element);

        // todo jekelly: what if its frozen?
        TranslateTransform? translateTransform = renderTransform as TranslateTransform;

        if (translateTransform == null)
        {
            TransformGroup? renderTransformGroup = renderTransform as TransformGroup;
            MatrixTransform? renderMatrixTransform = renderTransform as MatrixTransform;
            if (renderTransformGroup != null)
            {
                if (renderTransformGroup.Children.Count > 0)
                {
                    translateTransform = renderTransformGroup.Children[renderTransformGroup.Children.Count - 1] as TranslateTransform;
                }

                if (translateTransform == null)
                {
                    translateTransform = new TranslateTransform();
                    renderTransformGroup.Children.Add(translateTransform);
                }
            }
            else if (renderMatrixTransform != null)
            {
                Matrix matrix = renderMatrixTransform.Matrix;
                matrix.OffsetX += x;
                matrix.OffsetY += y;
                MatrixTransform matrixTransform = new MatrixTransform();
                matrixTransform.Matrix = matrix;
                SetRenderTransform(element, matrixTransform);
                return;
            }
            else
            {
                TransformGroup transformGroup = new TransformGroup();
                translateTransform = new TranslateTransform();

                // this will break multi-step animations that target the render transform
                if (renderTransform != null)
                {
                    transformGroup.Children.Add(renderTransform);
                }

                transformGroup.Children.Add(translateTransform);
                SetRenderTransform(element, transformGroup);
            }
        }

        Debug.Assert(translateTransform != null, "TranslateTransform should not be null by this point.");
        if (translateTransform != null)
        {
            translateTransform.X += x;
            translateTransform.Y += y;
        }
    }

    /// <summary>
    /// Does a recursive deep copy of the specified transform.
    /// </summary>
    /// <param name="transform">The transform to clone.</param>
    /// <returns>A deep copy of the specified transform, or null if the specified transform is null.</returns>
    /// <exception cref="System.ArgumentException">Thrown if the type of the Transform is not recognized.</exception>
    private static Transform CloneTransform(Transform transform)
    {
        transform = transform ?? throw new ArgumentNullException(nameof(transform));

        ScaleTransform? scaleTransform = null;
        RotateTransform? rotateTransform = null;
        SkewTransform? skewTransform = null;
        TranslateTransform? translateTransform = null;
        MatrixTransform? matrixTransform = null;
        TransformGroup? transformGroup = null;

        Type transformType = transform.GetType();
        if ((scaleTransform = transform as ScaleTransform) != null)
        {
            return new ScaleTransform()
            {
                CenterX = scaleTransform.CenterX,
                CenterY = scaleTransform.CenterY,
                ScaleX = scaleTransform.ScaleX,
                ScaleY = scaleTransform.ScaleY,
            };
        }
        else if ((rotateTransform = transform as RotateTransform) != null)
        {
            return new RotateTransform()
            {
                Angle = rotateTransform.Angle,
                CenterX = rotateTransform.CenterX,
                CenterY = rotateTransform.CenterY,
            };
        }
        else if ((skewTransform = transform as SkewTransform) != null)
        {
            return new SkewTransform()
            {
                AngleX = skewTransform.AngleX,
                AngleY = skewTransform.AngleY,
                CenterX = skewTransform.CenterX,
                CenterY = skewTransform.CenterY,
            };
        }
        else if ((translateTransform = transform as TranslateTransform) != null)
        {
            return new TranslateTransform()
            {
                X = translateTransform.X,
                Y = translateTransform.Y,
            };
        }
        else if ((matrixTransform = transform as MatrixTransform) != null)
        {
            return new MatrixTransform()
            {
                Matrix = matrixTransform.Matrix,
            };
        }
        else if ((transformGroup = transform as TransformGroup) != null)
        {
            TransformGroup group = new TransformGroup();
            foreach (Transform childTransform in transformGroup.Children)
            {
                group.Children.Add(CloneTransform(childTransform));
            }

            return group;
        }

        throw new InvalidOperationException("Unexpected Transform type encountered");
    }

    /// <summary>
    /// Updates the X and Y properties based on the current rendered position of the associated element.
    /// </summary>
    private static void UpdatePosition(FrameworkElement element)
    {
        GeneralTransform elementToRoot = element.TransformToVisual(GetRootElement(element));
        Point translation = GetTransformOffset(elementToRoot);
        SetDragX(element, translation.X);
        SetDragY(element, translation.Y);
    }

    private static void StartDrag(FrameworkElement element, PointerRoutedEventArgs e)
    {
        SetRelativePosition(element, e.GetCurrentPoint(element).Position);
        element.CapturePointer(e.Pointer);

        element.PointerMoved += OnPointerMoved;
        element.PointerCaptureLost += OnPointerCaptureLost;
        element.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(OnPointerReleased), handledEventsToo: false);
    }

    private static void HandleDrag(FrameworkElement element, Point newPositionInElementCoordinates)
    {
        var relativePosition = GetRelativePosition(element);
        double relativeXDiff = newPositionInElementCoordinates.X - relativePosition.X;
        double relativeYDiff = newPositionInElementCoordinates.Y - relativePosition.Y;

        GeneralTransform elementToRoot = element.TransformToVisual(GetRootElement(element));
        Point relativeDifferenceInRootCoordinates = TransformAsVector(elementToRoot, relativeXDiff, relativeYDiff);

        SetSettingPosition(element, true);
        ApplyTranslation(element, relativeDifferenceInRootCoordinates.X, relativeDifferenceInRootCoordinates.Y);
        UpdatePosition(element);
        SetSettingPosition(element, false);
    }

    private static void EndDrag(FrameworkElement element)
    {
        element.PointerMoved -= OnPointerMoved;
        element.PointerCaptureLost -= OnPointerCaptureLost;
        element.RemoveHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(OnPointerReleased));
    }

    private static void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        StartDrag((FrameworkElement)sender, e);
    }

    private static void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
    {
        EndDrag((FrameworkElement)sender);
    }

    private static void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        (sender as FrameworkElement)?.ReleasePointerCapture(e.Pointer);
    }

    private static void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var relativePosition = e.GetCurrentPoint(sender as UIElement).Position;
        HandleDrag((FrameworkElement)sender, relativePosition);
    }

    #endregion

    #region Linear algebra helper methods

    /// <summary>
    /// Check if one Rect is contained by another.
    /// </summary>
    /// <param name="rect1">The containing Rect.</param>
    /// <param name="rect2">The contained Rect.</param>
    /// <returns><c>True</c> if rect1 contains rect2; otherwise, <c>False</c>.</returns>
    private static bool RectContainsRect(Rect rect1, Rect rect2)
    {
        if (rect1.IsEmpty || rect2.IsEmpty)
        {
            return false;
        }

        return
            (rect1.X <= rect2.X) &&
            (rect1.Y <= rect2.Y) &&
            ((rect1.X + rect1.Width) >= (rect2.X + rect2.Width)) &&
            ((rect1.Y + rect1.Height) >= (rect2.Y + rect2.Height));
    }

    /// <summary>
    /// Transforms as vector.
    /// </summary>
    /// <param name="transform">The transform.</param>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    /// <returns>A point containing the values of X and Y transformed by transform as a vector.</returns>
    private static Point TransformAsVector(GeneralTransform transform, double x, double y)
    {
        Point origin = transform.TransformPoint(new Point(0, 0));
        Point transformedPoint = transform.TransformPoint(new Point(x, y));

        return new Point(transformedPoint.X - origin.X, transformedPoint.Y - origin.Y);
    }

    /// <summary>
    /// Gets the transform offset.
    /// </summary>
    /// <param name="transform">The transform.</param>
    /// <returns>The offset of the transform.</returns>
    private static Point GetTransformOffset(GeneralTransform transform)
    {
        return transform.TransformPoint(new Point(0, 0));
    }

    #endregion
}