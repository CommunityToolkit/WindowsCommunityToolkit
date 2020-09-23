// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See the LICENSE file in the project root
// for the license information.
//
// Author(s):
//  - Laurent Sansonetti (native Xamarin flex https://github.com/xamarin/flex)
//  - Stephane Delcroix (.NET port)
//  - Ben Askren (UWP/Uno port)
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.FlexPanelExtensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// FlexPanel is a panel based on the CSS Flexible Box Layout Module, commonly known as flex layout or flex-box,
    /// so called because it includes many flexible options to arrange children within the layout.  It can arrange
    /// its children horizontally and vertically in a stack, with options to reverse the order of items as well as
    /// align and justify content and individual items. More importantly, FlexPanel is capable of wrapping its children
    /// if there are too many to fit in a single row or column, and also has many options for orientation, alignment,
    /// and adapting to various screen sizes.
    /// </summary>
    public partial class FlexPanel : Panel
    {
        /// <summary>
        /// Dependency Property for the FlexPanel.AlignContent property
        /// </summary>
        public static readonly DependencyProperty AlignContentProperty = DependencyProperty.Register(
            nameof(AlignContent),
            typeof(FlexAlignContent),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.AlignContentDefault, new PropertyChangedCallback(OnAlignContentChanged)));

        private static void OnAlignContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FlexPanel flexPanel && flexPanel.root is FlexItem root)
            {
                root.AlignContent = (FlexAlignContent)e.NewValue;
                flexPanel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets how the FlexPanel will distribute space between and around child elements that have been
        /// laid out on multiple lines. This property is ignored if the root item does not have its
        /// <see cref="FlexItem.Wrap" /> property set to Wrap or WrapReverse.
        /// </summary>
        /// <remarks>The default value for this property is Stretch.</remarks>
        public FlexAlignContent AlignContent
        {
            get => (FlexAlignContent)GetValue(AlignContentProperty);
            set => this?.SetNewValue(AlignContentProperty, value);
        }

        /// <summary>
        /// Dependency Property for the FlexPanel.AlignItems property
        /// </summary>
        public static readonly DependencyProperty AlignItemsProperty = DependencyProperty.Register(
            nameof(AlignItems),
            typeof(FlexAlignItems),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.AlignItemsDefault, new PropertyChangedCallback(OnAlignItemsChanged)));

        private static void OnAlignItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FlexPanel flexPanel && flexPanel.root is FlexItem root)
            {
                root.AlignItems = (FlexAlignItems)e.NewValue;
                flexPanel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets how the FlexPanel will distribute space between and around child elements along the
        /// cross-axis.
        /// </summary>
        /// <remarks>The default value for this property is Stretch.</remarks>
        public FlexAlignItems AlignItems
        {
            get => (FlexAlignItems)GetValue(AlignItemsProperty);
            set => this?.SetNewValue(AlignItemsProperty, value);
        }

        /// <summary>
        /// DependencyProperty for the FlexPanel.Direction property
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            nameof(Direction),
            typeof(FlexDirection),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.DirectionDefault, new PropertyChangedCallback(OnDirectionChanged)));

        private static void OnDirectionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FlexPanel flexPanel && flexPanel.root is FlexItem root)
            {
                root.Direction = (FlexDirection)e.NewValue;
                flexPanel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets the direction and main-axis of child elements. If set to Column (or ColumnReverse),
        /// the main-axis will be the y-axis and items will be stacked vertically. If set to Row (or RowReverse),
        /// the main-axis will be the x-axis and items will be stacked horizontally.
        /// </summary>
        /// <remarks>The default value for this property is Column.</remarks>
        public FlexDirection Direction
        {
            get => (FlexDirection)GetValue(DirectionProperty);
            set => this?.SetNewValue(DirectionProperty, value);
        }

        /// <summary>
        /// Dependency Property for the FlexPanel.JustifyContent property
        /// </summary>
        public static readonly DependencyProperty JustifyContentProperty = DependencyProperty.Register(
            nameof(JustifyContent),
            typeof(FlexJustify),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.JustifyContentDefault, new PropertyChangedCallback(OnJustifyContentChanged)));

        private static void OnJustifyContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FlexPanel flexPanel && flexPanel.root is FlexItem root)
            {
                root.JustifyContent = (FlexJustify)e.NewValue;
                flexPanel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets how the FlexPanel will distribute space between and around child items
        /// along the main-axis.
        /// </summary>
        /// <remarks>The default value for this property is Start.</remarks>
        public FlexJustify JustifyContent
        {
            get => (FlexJustify)GetValue(JustifyContentProperty);
            set => this?.SetNewValue(JustifyContentProperty, value);
        }

        /// <summary>
        /// The Dependency Property for the FlexPanel.Wrap property
        /// </summary>
        public static readonly DependencyProperty WrapProperty = DependencyProperty.Register(
            nameof(Wrap),
            typeof(FlexWrap),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.WrapDefault, new PropertyChangedCallback(OnWrapChanged)));

        private static void OnWrapChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FlexPanel flexPanel && flexPanel.root is FlexItem root)
            {
                root.Wrap = (FlexWrap)e.NewValue;
                flexPanel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets or sets whether child elements should be laid out in a single line(NoWrap)
        /// or multiple lines(Wrap or WrapReverse). If this property is set to Wrap or WrapReverse,
        /// <see cref = "P:Microsoft.Toolkit.Uwp.UI.Controls.FlexItem.AlignContent" /> can then be
        /// used to specify how the lines should be distributed.
        /// </summary>
        /// <remarks>The default value for this property is NoWrap.</remarks>
        public FlexWrap Wrap
        {
            get => (FlexWrap)GetValue(WrapProperty);
            set => this?.SetNewValue(WrapProperty, value);
        }

        /// <summary>
        /// The Attached Dependency Property for the FlexLayout.Order attached property
        /// </summary>
        public static readonly DependencyProperty OrderProperty = DependencyProperty.RegisterAttached(
            "Order",
            typeof(int),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.OrderDefault, OnOrderChanged));

        private static void OnOrderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && GetFlexItem(element) is FlexItem item)
            {
                item.Order = (int)e.NewValue;
                InternalInvalidateArrange(element);
            }
        }

        /// <summary>
        /// This attached property specifies whether this UIElement should be laid out before or after other items
        /// in the FlexPanel.  Items are laid out based on the ascending value of this property. Items that
        /// have the same value for this property will be laid out in the order they were inserted.
        /// </summary>
        /// <value>The item order (can be a negative, 0, or positive value).</value>
        /// <remarks>The default value for this property is 0.</remarks>
        /// <returns>Element's FlexPanel order value</returns>
        public static int GetOrder(UIElement element)
            => (int)element?.GetValue(OrderProperty);

        /// <summary>
        /// This attached property specifies whether this UIElement should be laid out before or after other items
        /// in the FlexPanel.  Items are laid out based on the ascending value of this property. Items that
        /// have the same value for this property will be laid out in the order they were inserted.
        /// </summary>
        /// <value>The item order (can be a negative, 0, or positive value).</value>
        /// <remarks>The default value for this property is 0.</remarks>
        public static void SetOrder(UIElement element, int value)
            => element?.SetNewValue(OrderProperty, value);

        /// <summary>
        /// The Attached Dependency Property for the FlexLayout.Grow attached property
        /// </summary>
        public static readonly DependencyProperty GrowProperty = DependencyProperty.RegisterAttached(
            "Grow",
            typeof(double),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.GrowDefault, OnGrowChanged));

        private static void OnGrowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && GetFlexItem(element) is FlexItem item)
            {
                item.Grow = (double)e.NewValue;
                InternalInvalidateArrange(element);
            }
        }

        /// <summary>
        /// This attached property defines the grow factor of the UIElement; the amount of available space it
        /// should use on the main-axis. If this property is set to 0, the item will not grow.
        /// </summary>
        /// <value>The item grow factor.</value>
        /// <remarks>The default value for this property is 0 (does not take any available space).</remarks>
        /// <returns>Element's FlexPanel Grow value</returns>
        public static double GetGrow(UIElement element)
            => (double)element?.GetValue(GrowProperty);

        /// <summary>
        /// This attached property defines the grow factor of the UIElement; the amount of available space it
        /// should use on the main-axis. If this property is set to 0, the item will not grow.
        /// </summary>
        /// <value>The item grow factor.</value>
        /// <remarks>The default value for this property is 0 (does not take any available space).</remarks>
        public static void SetGrow(UIElement element, double value)
            => element?.SetNewValue(GrowProperty, value);

        /// <summary>
        /// The Attached Dependency Property for the FlexLayout.Shrink attached property;
        /// </summary>
        public static readonly DependencyProperty ShrinkProperty = DependencyProperty.RegisterAttached(
            "Shrink",
            typeof(double),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.ShrinkDefault, OnShrinkChanged));

        private static void OnShrinkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && GetFlexItem(element) is FlexItem item)
            {
                item.Shrink = (double)e.NewValue;
                InternalInvalidateArrange(element);
            }
        }

        /// <summary>
        /// This attached property defines the shrink factor of the UIElement.  In case the child items overflow
        /// the main-axis of the container, this factor will be used to determine how individual items
        /// should shrink so that all items can fill inside the container.If this property is set to 0,
        /// the item will not shrink.
        /// </summary>
        /// <value>The item shrink factor.</value>
        /// <remarks>The default value for this property is 1 (all items will shrink equally).</remarks>
        /// <returns>Element's FlexPanel shrink value</returns>
        public static double GetShrink(UIElement element)
            => (double)element?.GetValue(ShrinkProperty);

        /// <summary>
        /// This attached property defines the shrink factor of the UIElement.  In case the child items overflow
        /// the main-axis of the container, this factor will be used to determine how individual items
        /// should shrink so that all items can fill inside the container.If this property is set to 0,
        /// the item will not shrink.
        /// </summary>
        /// <value>The item shrink factor.</value>
        /// <remarks>The default value for this property is 1 (all items will shrink equally).</remarks>
        public static void SetShrink(UIElement element, double value)
            => element?.SetNewValue(ShrinkProperty, value);

        /// <summary>
        /// The Attached Dependency Property for the FlexPanel.AlignSelf attached property
        /// </summary>
        public static readonly DependencyProperty AlignSelfProperty = DependencyProperty.RegisterAttached(
            "AlignSelf",
            typeof(FlexAlignSelf),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.AlignSelfDefault, OnAlignSelfChanged));

        private static void OnAlignSelfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && GetFlexItem(element) is FlexItem item)
            {
                item.AlignSelf = (FlexAlignSelf)e.NewValue;
                InternalInvalidateArrange(element);
            }
        }

        /// <summary>
        /// This attached property defines how the FlexPanel will distribute space between and around child
        /// element for a specific element along the cross-axis. If this property is set to FlexAlignSelf.Auto
        /// on a child element, the parent's value for <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.FlexItem.AlignItems" />
        /// will be used instead.
        /// </summary>
        /// <remarks>The default value for this property FlexAlignSelf.Auto.</remarks>
        /// <returns>Element's Self Alignment</returns>
        public static FlexAlignSelf GetAlignSelf(UIElement element)
            => (FlexAlignSelf)element?.GetValue(AlignSelfProperty);

        /// <summary>
        /// This attached property defines how the FlexPanel will distribute space between and around child
        /// element for a specific element along the cross-axis. If this property is set to FlexAlignSelf.Auto
        /// on a child element, the parent's value for <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.FlexItem.AlignItems" />
        /// will be used instead.
        /// </summary>
        /// <remarks>The default value for this property FlexAlignSelf.Auto.</remarks>
        public static void SetAlignSelf(UIElement element, FlexAlignSelf value)
            => element?.SetNewValue(AlignSelfProperty, value);

        /// <summary>
        /// The Attached Dependency Property for the FlexLayout.Basis attached property
        /// </summary>
        public static readonly DependencyProperty BasisProperty = DependencyProperty.RegisterAttached(
            "Basis",
            typeof(string),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.BasisDefault.ToString(), OnBasisChanged));

        private static void OnBasisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && GetFlexItem(element) is FlexItem item)
            {
                if (e.NewValue is string value)
                {
                    item.Basis = FlexBasis.Parse(value);
                }
                else
                {
                    item.Basis = FlexBasis.Auto;
                }

                InternalInvalidateArrange(element);
            }
        }

        /// <summary>
        /// Gets or sets the initial main-axis dimension of the UIElement in the FlexLayout or if that value
        /// calculated by FlexPanel (FlexBasis.Auto).  If FlexBasis.IsRelative is false, then this child element's
        /// main-axis dimension will the FlexBasis.Length, in pixels.  Any remaining space will be portioned among all the child
        /// elements with a FlexBasis.IsRelstive set to true.
        /// </summary>
        /// <remarks>The default value for this property is Auto.</remarks>
        /// <returns>string representation of the element's basis</returns>
        public static string GetBasis(UIElement element)
        {
            if (element?.GetValue(BasisProperty) is string value)
            {
                return value;
            }

            return "auto";
        }

        /// <summary>
        /// Gets or sets the initial main-axis dimension of the UIElement in the FlexLayout or if that value
        /// calculated by FlexPanel (FlexBasis.Auto).  If FlexBasis.IsRelative is false, then this child element's
        /// main-axis dimension will the FlexBasis.Length, in pixels.  Any remaining space will be portioned among all the child
        /// elements with a FlexBasis.IsRelstive set to true.
        /// </summary>
        /// <remarks>The default value for this property is Auto.</remarks>
        public static void SetBasis(UIElement element, string value)
            => element?.SetNewValue(BasisProperty, value);

        /// <summary>
        /// Gets or sets the initial main-axis dimension of the UIElement in the FlexLayout or if that value
        /// calculated by FlexPanel (FlexBasis.Auto).  If FlexBasis.IsRelative is false, then this child element will be
        /// main-axis dimension will the FlexBasis.Length.  Any remaining space will be portioned among all the child
        /// elements with a FlexBasis.IsRelstive set to true.
        /// </summary>
        /// <remarks>The default value for this property is Auto.</remarks>
        public static void SetFlexBasis(UIElement element, FlexBasis value)
            => SetBasis(element, value.ToString());

        /// <summary>
        /// Gets or sets the initial main-axis dimension of the UIElement in the FlexLayout or if that value
        /// calculated by FlexPanel (FlexBasis.Auto).  If FlexBasis.IsRelative is false, then this child element will be
        /// main-axis dimension will the FlexBasis.Length.  Any remaining space will be portioned among all the child
        /// elements with a FlexBasis.IsRelstive set to true.
        /// </summary>
        /// <remarks>The default value for this property is Auto.</remarks>
        /// <returns>FlexBasis</returns>
        public static FlexBasis GetFlexBasis(UIElement element)
        {
            if (GetBasis(element) is string basisString)
            {
                return FlexBasis.Parse(basisString);
            }

            return FlexBasis.Auto;
        }

        private static readonly DependencyProperty FlexItemProperty = DependencyProperty.RegisterAttached(
            "FlexItem",
            typeof(object),
            typeof(FlexPanel),
            new PropertyMetadata(null));

        private static FlexItem GetFlexItem(UIElement element)
        {
            if (element is null)
            {
                return null;
            }

            if (element is FlexPanel flexPanel)
            {
                return flexPanel.root;
            }

            FlexItem item = null;
            try
            {
                item = (FlexItem)element.GetValue(FlexItemProperty);
            }
            catch (Exception)
            {
            }

            if (item is null)
            {
                item = new FlexItem();
                element.SetValue(FlexItemProperty, item);
            }

            return item;
        }

        private static void SetFlexItem(UIElement element, FlexItem value)
        {
            element?.SetValue(FlexItemProperty, value);
            UpdateItemProperties(element, value);
        }

        private static void InternalInvalidateArrange(UIElement element)
        {
            if (element is FlexPanel)
            {
                element.InvalidateArrange();
            }
            else if (element is FrameworkElement frameworkElement && frameworkElement.Parent is FlexPanel flexPanel)
            {
                flexPanel.InvalidateArrange();
            }
        }

        private static void UpdateItemProperties(UIElement view, FlexItem item)
        {
            item.IsVisible = view.Visibility == Visibility.Visible;

            if (view is FrameworkElement element)
            {
                item.MarginLeft = (double)element.Margin.Left;
                item.MarginTop = (double)element.Margin.Top;
                item.MarginRight = (double)element.Margin.Right;
                item.MarginBottom = (double)element.Margin.Bottom;
            }

            if (view is Control control)
            {
                item.SetPadding(control.Padding);
            }
            else if (view is Border border)
            {
                item.SetPadding(border.Padding);
            }
            else if (view is TextBlock textBlock)
            {
                item.SetPadding(textBlock.Padding);
            }
#if __ANDROID__ || __WASM__
#else
            else if (view is RichTextBlock richTextBlock)
            {
                item.SetPadding(richTextBlock.Padding);
            }
#endif
        }

        private readonly FlexItem root = new FlexItem();

        private FlexItem AddChild(FrameworkElement view)
        {
            if (root == null)
            {
                return null;
            }

            view.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var item = (view as FlexPanel)?.root ?? new FlexItem();
            InitItemProperties(view, item);
            if (!(view is FlexPanel))
            {
                // inner layouts don't get measured
                item.SelfSizing = (FlexItem it, ref double w, ref double h) =>
                {
                    UpdateItemProperties(view, item);

                    var sizeConstraints = item.GetConstraints();
                    sizeConstraints.Width = (measuring && sizeConstraints.Width == 0) ? double.PositiveInfinity : sizeConstraints.Width;
                    sizeConstraints.Height = (measuring && sizeConstraints.Height == 0) ? double.PositiveInfinity : sizeConstraints.Height;

                    if (!measuring || (sizeConstraints.Width >= view.DesiredSize.Width && sizeConstraints.Height >= view.DesiredSize.Height))
                    {
                        if (view.DesiredSize.Width > 0 && view.DesiredSize.Height > 0)
                        {
                            w = (double)view.DesiredSize.Width;
                            h = (double)view.DesiredSize.Height;
                            return;
                        }
                    }

                    view.Measure(sizeConstraints);
                    w = (double)view.DesiredSize.Width;
                    h = (double)view.DesiredSize.Height;
                };
            }

            root.InsertAt(Children.IndexOf(view), item);
            SetFlexItem(view, item);

            return item;
        }

        private void InitItemProperties(FrameworkElement view, FlexItem item)
        {
            item.Order = GetOrder(view);
            item.Grow = GetGrow(view);
            item.Shrink = GetShrink(view);
            item.Basis = GetBasis(view);
            item.AlignSelf = GetAlignSelf(view);

            var margin = (Thickness)view.GetValue(MarginProperty);
            item.MarginLeft = (double)margin.Left;
            item.MarginTop = (double)margin.Top;
            item.MarginRight = (double)margin.Right;
            item.MarginBottom = (double)margin.Bottom;

            var width = view.Width;
            item.Width = width <= 0 ? double.NaN : (double)width;

            var height = view.Height;
            item.Height = height <= 0 ? double.NaN : (double)height;

            item.IsVisible = (Visibility)view.GetValue(VisibilityProperty) == Visibility.Visible;
        }

        /// <summary>
        /// UWP defined method
        /// </summary>
        /// <param name="finalSize">Size allocated to this element</param>
        /// <returns>Size consumed by this element</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var width = finalSize.Width;
            var height = finalSize.Height;

            Layout(width, height);

            foreach (var child in Children)
            {
                if (GetFlexItem(child) is FlexItem item)
                {
                    var frame = item.GetFrame();
                    if (double.IsNaN(frame.X)
                        || double.IsNaN(frame.Y)
                        || double.IsNaN(frame.Width)
                        || double.IsNaN(frame.Height))
                    {
                        ThrowHelper.ThrowInvalidDataException("child frame contains invalid value");
                    }

                    child.Arrange(frame);
                }
            }

            return finalSize;
        }

        private bool measuring;

        /// <summary>
        /// UWP defined
        /// </summary>
        /// <param name="availableSize">how much space is available to this element</param>
        /// <returns>size desired to render this element</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var widthConstraint = availableSize.Width;
            var heightConstraint = availableSize.Height;

            if (root == null)
            {
                return new Size(widthConstraint, heightConstraint);
            }

            measuring = true;

            // 1. Keep track of missing layout items
            var deleteCandidates = root.CopyChildrenAsList();

            // 2. Set Shrink to 0, set align-self to start (to avoid stretching)
            //    Set Image.Aspect to Fill to get the value we expect in measuring
            foreach (var child in Children)
            {
                if (GetFlexItem(child) is FlexItem item && item.Parent != null)
                {
                    deleteCandidates.Remove(item);
                }
                else if (child is FrameworkElement frameworkElement)
                {
                    item = AddChild(frameworkElement);
                }
                else
                {
                    continue;
                }

                item.Shrink = 0;
                item.AlignSelf = FlexAlignSelf.Start;
            }

            // 3. Remove missing layout items
            foreach (var item in deleteCandidates)
            {
                root.Remove(item);
            }

            Layout(widthConstraint, heightConstraint);

            // 4. look at the children location
            if (double.IsPositiveInfinity(widthConstraint))
            {
                widthConstraint = 0;
                foreach (var item in root)
                {
                    widthConstraint = Math.Max(widthConstraint, item.Frame[0] + item.Frame[2] + item.MarginRight);
                }
            }

            if (double.IsPositiveInfinity(heightConstraint))
            {
                heightConstraint = 0;
                foreach (var item in root)
                {
                    heightConstraint = Math.Max(heightConstraint, item.Frame[1] + item.Frame[3] + item.MarginBottom);
                }
            }

            // 5. reset Shrink, algin-self, and image.aspect
            foreach (var child in Children)
            {
                if (GetFlexItem(child) is FlexItem item)
                {
                    item.Shrink = (double)child.GetValue(ShrinkProperty);
                    item.AlignSelf = (FlexAlignSelf)child.GetValue(AlignSelfProperty);
                }
            }

            measuring = false;

            var result = new Size(widthConstraint, heightConstraint);
            return result;
        }

        private void Layout(double width, double height)
        {
            // Layout is only computed at root level
            if (root.Parent != null)
            {
                return;
            }

            root.Width = !double.IsPositiveInfinity(width) ? (double)width : 0;
            root.Height = !double.IsPositiveInfinity(height) ? (double)height : 0;
            root.Layout();
        }

        /// <summary>
        /// An item with flexbox properties. Items can also contain other items and be enumerated.
        /// </summary>
        internal class FlexItem
        {
            internal const FlexAlignContent AlignContentDefault = FlexAlignContent.Stretch;
            internal const FlexAlignItems AlignItemsDefault = FlexAlignItems.Stretch;
            internal const FlexAlignSelf AlignSelfDefault = FlexAlignSelf.Auto;
            internal const FlexDirection DirectionDefault = FlexDirection.Row;
            internal const FlexJustify JustifyContentDefault = FlexJustify.Start;
            internal const FlexPosition PositionDefault = FlexPosition.Relative;
            internal const FlexWrap WrapDefault = FlexWrap.NoWrap;

            internal const int OrderDefault = 0;
            internal const double GrowDefault = 0;
            internal const double ShrinkDefault = 1;

            internal static readonly FlexBasis BasisDefault = FlexBasis.Auto;

            /// <summary>
            /// Gets the frame (x, y, w, h).
            /// </summary>
            /// <value>The frame.</value>
            public double[] Frame { get; } = new double[4];

            /// <summary>Gets the parent item.</summary>
            /// <value>The parent item, or null if the item is a root item.</value>
            public FlexItem Parent { get; private set; }

            private List<FlexItem> children;

            private bool ShouldOrderChildren { get; set; }

            /// <summary>
            /// Gets or sets how the layout engine will distribute space between and around child items that have been laid out on multiple lines. This property is ignored if the root item does not have its <see cref="FlexItem.Wrap" /> property set to Wrap or WrapReverse.
            /// </summary>
            /// <remarks>The default value for this property is Stretch.</remarks>
            /// <value>The content of the align.</value>
            public FlexAlignContent AlignContent { get; set; } = AlignContentDefault;

            /// <summary>Gets or sets how the layout engine will distribute space between and around child items along the cross-axis.</summary>
            /// <value>The align items.</value>
            /// <remarks>The default value for this property is Stretch.</remarks>
            public FlexAlignItems AlignItems { get; set; } = AlignItemsDefault;

            /// <summary>Gets or sets  how the layout engine will distribute space between and around child items for a specific child along the cross-axis. If this property is set to Auto on a child item, the parent's value for <see cref="FlexItem.AlignItems" /> will be used instead.</summary>
            /// <value>The align self.</value>
            public FlexAlignSelf AlignSelf { get; set; } = AlignSelfDefault;

            /// <summary>Gets or sets  the initial main-axis dimension of the item. If <see cref="FlexItem.Direction" /> is row-based (horizontal), it will be used instead of <see cref="FlexItem.Width" />, and if it's column-based (vertical), it will be used instead of <see cref="FlexItem.Height" />.</summary>
            /// <value>The basis.</value>
            /// <remarks>The default value for this property is Auto.</remarks>
            public FlexBasis Basis { get; set; } = BasisDefault;

            /// <summary>Gets or sets  the bottom edge absolute position of the item. It also defines the item's height if <see cref="FlexItem.Top" /> is also set and if <see cref="FlexItem.Height" /> isn't set. It is ignored if <see cref="FlexItem.Position" /> isn't set to Absolute.</summary>
            /// <value>The value for the property.</value>
            /// <remarks>The default value for this property is NaN.</remarks>
            public double Bottom { get; set; } = double.NaN;

            /// <summary>Gets or sets  the direction and main-axis of child items. If set to Column (or ColumnReverse), the main-axis will be the y-axis and items will be stacked vertically. If set to Row (or RowReverse), the main-axis will be the x-axis and items will be stacked horizontally.</summary>
            /// <value>Any value part of the<see cref="FlexDirection" /> enumeration.</value>
            /// <remarks>The default value for this property is Column.</remarks>
            public FlexDirection Direction { get; set; } = DirectionDefault;

            /// <summary>Gets or sets  the grow factor of the item; the amount of available space it should use on the main-axis. If this property is set to 0, the item will not grow.</summary>
            /// <value>The item grow factor.</value>
            /// <remarks>The default value for this property is 0 (does not take any available space).</remarks>
            public double Grow { get; set; } = GrowDefault;

            /// <summary>Gets or sets  the height size dimension of the item.</summary>
            /// <value>The height size dimension.</value>
            /// <remarks>The default value for this property is NaN.</remarks>
            public double Height { get; set; } = double.NaN;

            public bool IsVisible { get; set; } = true;

            /// <summary>Gets or sets  how the layout engine will distribute space between and around child items along the main-axis.</summary>
            /// <value>Any value part of the<see cref="FlexJustify" />.</value>
            /// <remarks>The default value for this property is Start.</remarks>
            public FlexJustify JustifyContent { get; set; } = JustifyContentDefault;

            /// <summary>Gets or sets  the left edge absolute position of the item.It also defines the item's width if <see cref="FlexItem.Right" /> is also set and if <see cref="FlexItem.Width" /> isn't set.It is ignored if <see cref = "FlexItem.Position" /> isn't set to Absolute.</summary>
            /// <value>The value for the property.</value>
            /// <remarks>The default value for this property is NaN.</remarks>
            public double Left { get; set; } = double.NaN;

            /// <summary>Gets or sets  the margin space required on the bottom edge of the item.</summary>
            /// <value>The top edge margin space (negative values are allowed).</value>
            /// <remarks>The default value for this property is 0.</remarks>
            public double MarginBottom { get; set; } = 0f;

            /// <summary>Gets or sets  the margin space required on the left edge of the item.</summary>
            /// <value>The top edge margin space (negative values are allowed).</value>
            /// <remarks>The default value for this property is 0.</remarks>
            public double MarginLeft { get; set; } = 0f;

            /// <summary>Gets or sets  the margin space required on the right edge of the item.</summary>
            /// <value>The top edge margin space (negative values are allowed).</value>
            /// <remarks>The default value for this property is 0.</remarks>
            public double MarginRight { get; set; } = 0f;

            /// <summary>Gets or sets  the margin space required on the top edge of the item.</summary>
            /// <value>The top edge margin space (negative values are allowed).</value>
            /// <remarks>The default value for this property is 0.</remarks>
            public double MarginTop { get; set; } = 0f;

            private int order = OrderDefault;

            /// <summary>Gets or sets whether this item should be laid out before or after other items in the container.Items are laid out based on the ascending value of this property.Items that have the same value for this property will be laid out in the order they were inserted.</summary>
            /// <value>The item order (can be a negative, 0, or positive value).</value>
            /// <remarks>The default value for this property is 0.</remarks>
            public int Order
            {
                get => order;
                set
                {
                    if ((order = value) != 0 && Parent != null)
                    {
                        Parent.ShouldOrderChildren = true;
                    }
                }
            }

            /// <summary>Gets or sets the height of the item's bottom edge padding space that should be used when laying out child items.</summary>
            /// <value>The bottom edge padding space.Negative values are not allowed.</value>
            public double PaddingBottom { get; set; } = 0f;

            /// <summary>Gets or sets  the height of the item's left edge padding space that should be used when laying out child items.</summary>
            /// <value>The bottom edge padding space.Negative values are not allowed.</value>
            public double PaddingLeft { get; set; } = 0f;

            /// <summary>Gets or sets  the height of the item's right edge padding space that should be used when laying out child items.</summary>
            /// <value>The bottom edge padding space.Negative values are not allowed.</value>
            public double PaddingRight { get; set; } = 0f;

            /// <summary>Gets or sets  the height of the item's top edge padding space that should be used when laying out child items.</summary>
            /// <value>The bottom edge padding space.Negative values are not allowed.</value>
            public double PaddingTop { get; set; } = 0f;

            /// <summary>Gets or sets  whether the item should be positioned by the flexbox rules of the layout engine(Relative) or have an absolute fixed position (Absolute). If this property is set to Absolute, the<see cref="FlexItem.Left" />, <see cref = "FlexItem.Right" />, <see cref = "FlexItem.Top" /> and <see cref= "FlexItem.Bottom" /> properties will then be used to determine the item's fixed position in its container.</summary>
            /// <value>Any value part of the<see cref="FlexPosition" /> enumeration.</value>
            /// <remarks>The default value for this property is Relative</remarks>
            public FlexPosition Position { get; set; } = PositionDefault;

            /// <summary>Gets or sets  the right edge absolute position of the item.It also defines the item's width if <see cref="FlexItem.Left" /> is also set and if <see cref="FlexItem.Width" /> isn't set.It is ignored if <see cref = "FlexItem.Position" /> isn't set to Absolute.</summary>
            /// <value>The value for the property.</value>
            /// <remarks>The default value for this property is NaN.</remarks>
            public double Right { get; set; } = double.NaN;

            /// <summary>Gets or sets  the shrink factor of the item.In case the child items overflow the main-axis of the container, this factor will be used to determine how individual items should shrink so that all items can fill inside the container.If this property is set to 0, the item will not shrink.</summary>
            /// <value>The item shrink factor.</value>
            /// <remarks>The default value for this property is 1 (all items will shrink equally).</remarks>
            public double Shrink { get; set; } = ShrinkDefault;

            /// <summary>Gets or sets  the top edge absolute position of the item. It also defines the item's height if <see cref="FlexItem.Bottom" /> is also set and if <see cref="FlexItem.Height" /> isn't set. It is ignored if <see cref="FlexItem.Position" /> isn't set to Absolute.</summary>
            /// <value>The value for the property.</value>
            /// <remarks>The default value for this property is NaN.</remarks>
            public double Top { get; set; } = double.NaN;

            /// <summary>Gets or sets  the width size dimension of the item.</summary>
            /// <value>The width size dimension.</value>
            /// <remarks>The default value for this property is NaN.</remarks>
            public double Width { get; set; } = double.NaN;

            /// <summary>Gets or sets  whether child items should be laid out in a single line(NoWrap) or multiple lines(Wrap or WrapReverse). If this property is set to Wrap or WrapReverse, <see cref = "FlexAlignContent" /> can then be used to specify how the lines should be distributed.</summary>
            /// <value>Any value part of the<see cref="FlexWrap" /> enumeration.</value>
            /// <remarks>The default value for this property is NoWrap.</remarks>
            public FlexWrap Wrap { get; set; } = WrapDefault;

            /// <summary>
            /// Initializes a new instance of the <see cref="FlexItem"/> class.
            /// </summary>
            public FlexItem()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FlexItem"/> class.
            /// </summary>
            /// <param name="width">Width.</param>
            /// <param name="height">Height.</param>
            public FlexItem(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public void Add(FlexItem child)
            {
                Guard.IsNotNull(child, nameof(child));
                Guard.IsNull(child.Parent, "child.Parent");
                (children ??= new List<FlexItem>()).Add(child);
                child.Parent = this;
                ShouldOrderChildren |= child.Order != 0;
            }

            public void InsertAt(int index, FlexItem child)
            {
                Guard.IsNotNull(child, nameof(child));
                Guard.IsNull(child.Parent, "child.Parent");
                (children ??= new List<FlexItem>()).Insert(index, child);
                child.Parent = this;
                ShouldOrderChildren |= child.Order != 0;
            }

            public FlexItem RemoveAt(uint index)
            {
                var child = children[(int)index];
                child.Parent = null;
                children.RemoveAt((int)index);
                return child;
            }

            public int Count =>
                children?.Count ?? 0;

            public FlexItem ItemAt(int index) =>
                children?[index];

            public FlexItem this[int index]
                => ItemAt(index);

            public FlexItem Root
            {
                get
                {
                    var root = this;
                    while (root.Parent != null)
                    {
                        root = root.Parent;
                    }

                    return root;
                }
            }

            public void Layout()
            {
                Guard.IsNull(Parent, nameof(Parent));
                Guard.IsNull(SelfSizing, nameof(SelfSizing));

                if (double.IsNaN(Width) || double.IsNaN(Height))
                {
                    ThrowHelper.ThrowInvalidOperationException("Layout() must be called on an item that has proper values for the Width and Height properties");
                }

                LayoutItem(this, Width, Height);
            }

            public delegate void SelfSizingDelegate(FlexItem item, ref double width, ref double height);

            public SelfSizingDelegate SelfSizing { get; set; }

            private static void LayoutItem(FlexItem item, double width, double height)
            {
                if (item.children == null || item.children.Count == 0)
                {
                    return;
                }

                var layout = default(FlexLayout);
                layout.Init(item, width, height);
                layout.Reset();

                int lastLayoutChild = 0;
                int relativeChildrenCount = 0;

                for (int i = 0; i < item.Count; i++)
                {
                    var child = layout.ChildAt(item, i);
                    if (!child.IsVisible)
                    {
                        continue;
                    }

                    // Items with an absolute position have their frames determined
                    // directly and are skipped during layout.
                    if (child.Position == FlexPosition.Absolute)
                    {
                        child.Frame[2] = AbsoluteSize(child.Width, child.Left, child.Right, width);
                        child.Frame[3] = AbsoluteSize(child.Height, child.Top, child.Bottom, height);
                        child.Frame[0] = AbsolutePosition(child.Left, child.Right, child.Frame[2], width);
                        child.Frame[1] = AbsolutePosition(child.Top, child.Bottom, child.Frame[3], height);

                        // Now that the item has a frame, we can layout its children.
                        LayoutItem(child, child.Frame[2], child.Frame[3]);
                        continue;
                    }

                    // Initialize frame.
                    child.Frame[0] = 0;
                    child.Frame[1] = 0;
                    child.Frame[2] = child.Width + child.MarginThickness(false);
                    child.Frame[3] = child.Height + child.MarginThickness(true);

                    // Main axis size defaults to 0.
                    if (double.IsNaN(child.Frame[layout.MainAxisSize]))
                    {
                        child.Frame[layout.MainAxisSize] = 0;
                    }

                    // Cross axis size defaults to the parent's size (or line size in wrap
                    // mode, which is calculated later on).
                    if (double.IsNaN(child.Frame[layout.CrossAxisSize]))
                    {
                        if (layout.Wrap)
                        {
                            layout.NeedLines = true;
                        }
                        else
                        {
                            child.Frame[layout.CrossAxisSize] = (layout.Vertical ? width : height) - child.MarginThickness(!layout.Vertical);
                        }
                    }

                    // Call the SelfSizing callback if provided. Only non-NAN values
                    // are taken into account. If the item's cross-axis align property
                    // is set to stretch, ignore the value returned by the callback.
                    if (child.SelfSizing != null)
                    {
                        double[] size = { child.Frame[2], child.Frame[3] };
                        child.SelfSizing(child, ref size[0], ref size[1]);
                        for (int j = 0; j < 2; j++)
                        {
                            int sizeOff = j + 2;
                            if (sizeOff == layout.CrossAxisSize && ChildAlign(child, item) == FlexAlignItems.Stretch)
                            {
                                continue;
                            }

                            double val = size[j];
                            if (!double.IsNaN(val))
                            {
                                child.Frame[sizeOff] = val;
                            }
                        }
                    }

                    // Honor the `basis' property which overrides the main-axis size.
                    if (!child.Basis.IsAuto)
                    {
                        Guard.IsGreaterThanOrEqualTo(child.Basis.Length, 0, nameof(child.Basis.Length));
                        if (child.Basis.IsRelative)
                        {
                            Guard.IsLessThanOrEqualTo(child.Basis.Length, 1, nameof(child.Basis.Length));
                        }

                        double basis = child.Basis.Length;
                        if (child.Basis.IsRelative)
                        {
                            basis *= layout.Vertical ? height : width;
                        }

                        child.Frame[layout.MainAxisSize] = basis - child.MarginThickness(layout.Vertical);
                    }

                    double mainAxisChildSize = child.Frame[layout.MainAxisSize];
                    if (layout.Wrap)
                    {
                        if (layout.LineFlexibleMainAxisSize < mainAxisChildSize)
                        {
                            // Not enough space for this child on this line, layout the
                            // remaining items and move it to a new line.
                            LayoutItems(item, lastLayoutChild, i, relativeChildrenCount, ref layout);

                            layout.Reset();
                            lastLayoutChild = i;
                            relativeChildrenCount = 0;
                        }

                        double crossAxisChildSize = child.Frame[layout.CrossAxisSize];
                        if (!double.IsNaN(crossAxisChildSize) && crossAxisChildSize + child.MarginThickness(!layout.Vertical) > layout.LineCrossAxisSize)
                        {
                            layout.LineCrossAxisSize = crossAxisChildSize + child.MarginThickness(!layout.Vertical);
                        }
                    }

                    Guard.IsGreaterThanOrEqualTo(child.Grow, 0, nameof(child.Grow));
                    Guard.IsGreaterThanOrEqualTo(child.Shrink, 0, nameof(child.Shrink));

                    layout.LineFlexGrows += child.Grow;
                    layout.LineFlexShrinks += child.Shrink;

                    layout.LineFlexibleMainAxisSize -= mainAxisChildSize + child.MarginThickness(layout.Vertical);

                    relativeChildrenCount++;

                    if (mainAxisChildSize > 0 && child.Grow > 0)
                    {
                        layout.LineExtraFlexDim += mainAxisChildSize;
                    }
                }

                // Layout remaining items in wrap mode, or everything otherwise.
                LayoutItems(item, lastLayoutChild, item.Count, relativeChildrenCount, ref layout);

                // In wrap mode we may need to tweak the position of each line according to
                // the AlignContent property as well as the cross-axis size of items that
                // haven't been set yet.
                if (layout.NeedLines && (layout.Lines?.Length ?? 0) > 0)
                {
                    double pos = 0;
                    double spacing = 0;
                    double flexDim = layout.CrossAxisParentSize - layout.LinesSizes;

                    if (flexDim > 0)
                    {
                        LayoutAlign(item.AlignContent, flexDim, (uint)(layout.Lines?.Length ?? 0), ref pos, ref spacing);
                    }

                    double oldPos = 0;
                    if (layout.IsCrossAxisReversed)
                    {
                        pos = layout.CrossAxisParentSize - pos;
                        oldPos = layout.CrossAxisParentSize;
                    }

                    for (uint i = 0; i < (layout.Lines?.Length ?? 0); i++)
                    {
                        FlexLayout.FlexLayoutLine line = layout.Lines[i];

                        if (layout.IsCrossAxisReversed)
                        {
                            pos -= line.Size;
                            pos -= spacing;
                            oldPos -= line.Size;
                        }

                        // Re-position the children of this line, honoring any child
                        // alignment previously set within the line.
                        for (int j = line.ChildBegin; j < line.ChildEnd; j++)
                        {
                            FlexItem child = layout.ChildAt(item, j);
                            if (child.Position == FlexPosition.Absolute)
                            {
                                // Should not be re-positioned.
                                continue;
                            }

                            if (double.IsNaN(child.Frame[layout.CrossAxisSize]))
                            {
                                // If the child's cross axis size hasn't been set it, it
                                // defaults to the line size.
                                child.Frame[layout.CrossAxisSize] = line.Size
                                    + (item.AlignContent == FlexAlignContent.Stretch
                                       ? spacing : 0);
                            }

                            child.Frame[layout.CrossAxisPosition] = pos + (child.Frame[layout.CrossAxisPosition] - oldPos);
                        }

                        if (!layout.IsCrossAxisReversed)
                        {
                            pos += line.Size;
                            pos += spacing;
                            oldPos += line.Size;
                        }
                    }
                }

                layout.Cleanup();
            }

            private static void LayoutAlign1(FlexJustify align, double flexDim, int childrenCount, ref double posP, ref double spacingP)
            {
                Guard.IsGreaterThanOrEqualTo(flexDim, 0, nameof(flexDim));

                posP = 0;
                spacingP = 0;

                switch (align)
                {
                    case FlexJustify.Start:
                        return;
                    case FlexJustify.End:
                        posP = flexDim;
                        return;
                    case FlexJustify.Center:
                        posP = flexDim / 2;
                        return;
                    case FlexJustify.SpaceBetween:
                        if (childrenCount > 0)
                        {
                            spacingP = flexDim / (childrenCount - 1);
                        }

                        return;
                    case FlexJustify.SpaceAround:
                        if (childrenCount > 0)
                        {
                            spacingP = flexDim / childrenCount;
                            posP = spacingP / 2;
                        }

                        return;
                    case FlexJustify.SpaceEvenly:
                        if (childrenCount > 0)
                        {
                            spacingP = flexDim / (childrenCount + 1);
                            posP = spacingP;
                        }

                        return;
                }
            }

            private static void LayoutAlign(FlexAlignContent align, double flexDim, uint childrenCount, ref double posP, ref double spacingP)
            {
                Guard.IsGreaterThanOrEqualTo(flexDim, 0, nameof(flexDim));

                posP = 0;
                spacingP = 0;

                switch (align)
                {
                    case FlexAlignContent.Start:
                        return;
                    case FlexAlignContent.End:
                        posP = flexDim;
                        return;
                    case FlexAlignContent.Center:
                        posP = flexDim / 2;
                        return;
                    case FlexAlignContent.SpaceBetween:
                        if (childrenCount > 0)
                        {
                            spacingP = flexDim / (childrenCount - 1);
                        }

                        return;
                    case FlexAlignContent.SpaceAround:
                        if (childrenCount > 0)
                        {
                            spacingP = flexDim / childrenCount;
                            posP = spacingP / 2;
                        }

                        return;
                    case FlexAlignContent.SpaceEvenly:
                        if (childrenCount > 0)
                        {
                            spacingP = flexDim / (childrenCount + 1);
                            posP = spacingP;
                        }

                        return;
                    case FlexAlignContent.Stretch:
                        spacingP = flexDim / childrenCount;
                        return;
                }
            }

            private static void LayoutItems(FlexItem item, int childBegin, int childEnd, int childrenCount, ref FlexLayout layout)
            {
                Guard.IsGreaterThan(childrenCount, 0, nameof(childrenCount));
                Guard.IsLessThanOrEqualTo(childrenCount, childEnd - childBegin, nameof(childrenCount));

                if (layout.LineFlexibleMainAxisSize > 0 && layout.LineExtraFlexDim > 0)
                {
                    // If the container has a positive flexible space, let's add to it
                    // the sizes of all flexible children.
                    layout.LineFlexibleMainAxisSize += layout.LineExtraFlexDim;
                }

                // Determine the main axis initial position and optional spacing.
                double pos = 0;
                double spacing = 0;
                if (layout.LineFlexGrows == 0 && layout.LineFlexibleMainAxisSize > 0)
                {
                    LayoutAlign1(item.JustifyContent, layout.LineFlexibleMainAxisSize, childrenCount, ref pos, ref spacing);
                }

                if (layout.IsMainAxisReversed)
                {
                    pos = layout.MainAxisParentSize - pos;
                }

                if (layout.IsMainAxisReversed)
                {
                    pos -= layout.Vertical ? item.PaddingBottom : item.PaddingRight;
                }
                else
                {
                    pos += layout.Vertical ? item.PaddingTop : item.PaddingLeft;
                }

                if (layout.Wrap && layout.IsCrossAxisReversed)
                {
                    layout.LineCrossAxisPosition -= layout.LineCrossAxisSize;
                }

                for (int i = childBegin; i < childEnd; i++)
                {
                    FlexItem child = layout.ChildAt(item, i);
                    if (!child.IsVisible)
                    {
                        continue;
                    }

                    if (child.Position == FlexPosition.Absolute)
                    {
                        // Already positioned.
                        continue;
                    }

                    // Grow or shrink the main axis item size if needed.
                    double flexSize = 0;
                    if (layout.LineFlexibleMainAxisSize > 0)
                    {
                        if (child.Grow != 0)
                        {
                            child.Frame[layout.MainAxisSize] = 0; // Ignore previous size when growing.
                            flexSize = (layout.LineFlexibleMainAxisSize / layout.LineFlexGrows) * child.Grow;
                        }
                    }
                    else if (layout.LineFlexibleMainAxisSize < 0)
                    {
                        if (child.Shrink != 0)
                        {
                            flexSize = (layout.LineFlexibleMainAxisSize / layout.LineFlexShrinks) * child.Shrink;
                        }
                    }

                    child.Frame[layout.MainAxisSize] += flexSize;

                    // Set the cross axis position (and stretch the cross axis size if
                    // needed).
                    double alignSize = child.Frame[layout.CrossAxisSize];
                    double alignPos = layout.LineCrossAxisPosition + 0;
                    switch (ChildAlign(child, item))
                    {
                        case FlexAlignItems.End:
                            alignPos += layout.LineCrossAxisSize - alignSize - (layout.Vertical ? child.MarginRight : child.MarginBottom);
                            break;

                        case FlexAlignItems.Center:
                            alignPos += (layout.LineCrossAxisSize / 2) - (alignSize / 2)
                                + ((layout.Vertical ? child.MarginLeft : child.MarginTop)
                                   - (layout.Vertical ? child.MarginRight : child.MarginBottom));
                            break;

                        case FlexAlignItems.Stretch:
                            if (alignSize == 0)
                            {
                                child.Frame[layout.CrossAxisSize] = layout.LineCrossAxisSize
                                    - ((layout.Vertical ? child.MarginLeft : child.MarginTop)
                                       + (layout.Vertical ? child.MarginRight : child.MarginBottom));
                            }

                            alignPos += layout.Vertical ? child.MarginLeft : child.MarginTop;
                            break;
                        case FlexAlignItems.Start:
                            alignPos += layout.Vertical ? child.MarginLeft : child.MarginTop;
                            break;
                    }

                    child.Frame[layout.CrossAxisPosition] = alignPos;

                    // Set the main axis position.
                    if (layout.IsMainAxisReversed)
                    {
                        pos -= layout.Vertical ? child.MarginBottom : child.MarginRight;
                        pos -= child.Frame[layout.MainAxisSize];
                        child.Frame[layout.MainAxisPosition] = pos;
                        pos -= spacing;
                        pos -= layout.Vertical ? child.MarginTop : child.MarginLeft;
                    }
                    else
                    {
                        pos += layout.Vertical ? child.MarginTop : child.MarginLeft;
                        child.Frame[layout.MainAxisPosition] = pos;
                        pos += child.Frame[layout.MainAxisSize];
                        pos += spacing;
                        pos += layout.Vertical ? child.MarginBottom : child.MarginRight;
                    }

                    // Now that the item has a frame, we can layout its children.
                    LayoutItem(child, child.Frame[2], child.Frame[3]);
                }

                if (layout.Wrap && !layout.IsCrossAxisReversed)
                {
                    layout.LineCrossAxisPosition += layout.LineCrossAxisSize;
                }

                if (layout.NeedLines)
                {
                    Array.Resize(ref layout.Lines, (layout.Lines?.Length ?? 0) + 1);

                    ref FlexLayout.FlexLayoutLine line = ref layout.Lines[layout.Lines.Length - 1];

                    line.ChildBegin = childBegin;
                    line.ChildEnd = childEnd;
                    line.Size = layout.LineCrossAxisSize;

                    layout.LinesSizes += line.Size;
                }
            }

            private static double AbsoluteSize(double val, double pos1, double pos2, double dim) =>
                !double.IsNaN(val) ? val : (!double.IsNaN(pos1) && !double.IsNaN(pos2) ? dim - pos2 - pos1 : 0);

            private static double AbsolutePosition(double pos1, double pos2, double size, double dim) =>
                !double.IsNaN(pos1) ? pos1 : (!double.IsNaN(pos2) ? dim - size - pos2 : 0);

            private static FlexAlignItems ChildAlign(FlexItem child, FlexItem parent) =>
                child.AlignSelf == FlexAlignSelf.Auto ? parent.AlignItems : (FlexAlignItems)child.AlignSelf;

            private struct FlexLayout
            {
                // Set during init.
                public bool Wrap;
                public bool IsMainAxisReversed;
                public bool IsCrossAxisReversed;
                public bool Vertical;
                public double MainAxisParentSize;
                public double CrossAxisParentSize;
                public uint MainAxisPosition;
                public uint CrossAxisPosition;
                public uint MainAxisSize;
                public uint CrossAxisSize;
                private int[] orderedIndices;

                // Set for each line layout.
                public double LineCrossAxisSize;              // the cross axis size
                public double LineFlexibleMainAxisSize;              // the flexible part of the main axis size
                public double LineExtraFlexDim;        // sizes of flexible items
                public double LineFlexGrows;
                public double LineFlexShrinks;
                public double LineCrossAxisPosition;                  // cross axis position

                // Calculated layout lines - only tracked when needed:
                //   - if the root's AlignContent property isn't set to FlexAlign.Start
                //   - or if any child item doesn't have a cross-axis size set
                public bool NeedLines;

                public struct FlexLayoutLine
                {
                    public int ChildBegin;
                    public int ChildEnd;
                    public double Size;
                }

                public FlexLayoutLine[] Lines;
                public double LinesSizes;

                public void Reset()
                {
                    LineCrossAxisSize = Wrap ? 0 : CrossAxisParentSize;
                    LineFlexibleMainAxisSize = MainAxisParentSize;
                    LineExtraFlexDim = 0;
                    LineFlexGrows = 0;
                    LineFlexShrinks = 0;
                }

                public void Init(FlexItem item, double width, double height)
                {
                    Guard.IsGreaterThanOrEqualTo(item.PaddingLeft, 0, nameof(item.PaddingLeft));
                    Guard.IsGreaterThanOrEqualTo(item.PaddingRight, 0, nameof(item.PaddingRight));
                    Guard.IsGreaterThanOrEqualTo(item.PaddingTop, 0, nameof(item.PaddingTop));
                    Guard.IsGreaterThanOrEqualTo(item.PaddingBottom, 0, nameof(item.PaddingBottom));

                    width = Math.Max(0, width - item.PaddingLeft + item.PaddingRight);
                    height = Math.Max(0, height - item.PaddingTop + item.PaddingBottom);

                    IsMainAxisReversed = item.Direction == FlexDirection.RowReverse || item.Direction == FlexDirection.ColumnReverse;
                    Vertical = true;
                    switch (item.Direction)
                    {
                        case FlexDirection.Row:
                        case FlexDirection.RowReverse:
                            Vertical = false;
                            MainAxisParentSize = width;
                            CrossAxisParentSize = height;
                            MainAxisPosition = 0;
                            CrossAxisPosition = 1;
                            MainAxisSize = 2;
                            CrossAxisSize = 3;
                            break;
                        case FlexDirection.Column:
                        case FlexDirection.ColumnReverse:
                            MainAxisParentSize = height;
                            CrossAxisParentSize = width;
                            MainAxisPosition = 1;
                            CrossAxisPosition = 0;
                            MainAxisSize = 3;
                            CrossAxisSize = 2;
                            break;
                    }

                    orderedIndices = null;
                    if (item.ShouldOrderChildren && item.Count > 0)
                    {
                        var indices = new int[item.Count];

                        // Creating a list of item indices sorted using the children's `order'
                        // attribute values. We are using a simple insertion sort as we need
                        // stability (insertion order must be preserved) and cross-platform
                        // support. We should eventually switch to merge sort (or something
                        // else) if the number of items becomes significant enough.
                        for (int i = 0; i < item.Count; i++)
                        {
                            indices[i] = i;
                            for (int j = i; j > 0; j--)
                            {
                                int prev = indices[j - 1];
                                int curr = indices[j];
                                if (item.children[prev].Order <= item.children[curr].Order)
                                {
                                    break;
                                }

                                indices[j - 1] = curr;
                                indices[j] = prev;
                            }
                        }

                        orderedIndices = indices;
                    }

                    LineFlexibleMainAxisSize = 0;
                    LineFlexGrows = 0;
                    LineFlexShrinks = 0;

                    IsCrossAxisReversed = false;
                    Wrap = item.Wrap != FlexWrap.NoWrap;
                    if (Wrap)
                    {
                        if (item.Wrap == FlexWrap.Reverse)
                        {
                            IsCrossAxisReversed = true;
                            LineCrossAxisPosition = CrossAxisParentSize;
                        }
                    }
                    else
                    {
                        LineCrossAxisPosition = Vertical ? item.PaddingLeft : item.PaddingTop;
                    }

                    NeedLines = Wrap && item.AlignContent != FlexAlignContent.Start;
                    Lines = null;
                    LinesSizes = 0;
                }

                public FlexItem ChildAt(FlexItem item, int i) =>
                    item.children[orderedIndices?[i] ?? i];

                public void Cleanup()
                {
                    orderedIndices = null;
                    Lines = null;
                }
            }

            private static readonly List<FlexItem> Empty = new List<FlexItem>();

            public List<FlexItem>.Enumerator GetEnumerator() =>
                (children ?? Empty).GetEnumerator();

            public List<FlexItem> CopyChildrenAsList() => children is null
                ? new List<FlexItem>()
                : new List<FlexItem>(children);

            private double MarginThickness(bool vertical) =>
                vertical ? MarginTop + MarginBottom : MarginLeft + MarginRight;
        }
    }
}
