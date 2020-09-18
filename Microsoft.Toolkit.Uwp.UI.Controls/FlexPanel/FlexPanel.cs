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
            new PropertyMetadata(FlexItem.AlignContentDefault, new PropertyChangedCallback((d, e) => ((FlexPanel)d).OnAlignContentChanged(e))));

        private void OnAlignContentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_root != null)
            {
                _root.AlignContent = (FlexAlignContent)e.NewValue;
                InvalidateMeasure();
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
            set => this.SetNewValue(AlignContentProperty, value);
        }

        /// <summary>
        /// Dependency Property for the FlexPanel.AlignItems property
        /// </summary>
        public static readonly DependencyProperty AlignItemsProperty = DependencyProperty.Register(
            nameof(AlignItems),
            typeof(FlexAlignItems),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.AlignItemsDefault, new PropertyChangedCallback((d, e) => ((FlexPanel)d).OnAlignItemsChanged(e))));

        private void OnAlignItemsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_root != null)
            {
                _root.AlignItems = (FlexAlignItems)e.NewValue;
                InvalidateMeasure();
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
            set => this.SetNewValue(AlignItemsProperty, value);
        }

        /// <summary>
        /// DependencyProperty for the FlexPanel.Direction property
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            nameof(Direction),
            typeof(FlexDirection),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.DirectionDefault, new PropertyChangedCallback((d, e) => ((FlexPanel)d).OnDirectionChanged(e))));

        private void OnDirectionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_root != null)
            {
                _root.Direction = (FlexDirection)e.NewValue;
                InvalidateMeasure();
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
            set => this.SetNewValue(DirectionProperty, value);
        }

        /// <summary>
        /// Dependency Property for the FlexPanel.JustifyContent property
        /// </summary>
        public static readonly DependencyProperty JustifyContentProperty = DependencyProperty.Register(
            nameof(JustifyContent),
            typeof(FlexJustify),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.JustifyContentDefault, new PropertyChangedCallback((d, e) => ((FlexPanel)d).OnJustifyContentChanged(e))));

        private void OnJustifyContentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_root != null)
            {
                _root.JustifyContent = (FlexJustify)e.NewValue;
                InvalidateMeasure();
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
            set => this.SetNewValue(JustifyContentProperty, value);
        }

        /// <summary>
        /// The Dependency Property for the FlexPanel.Wrap property
        /// </summary>
        public static readonly DependencyProperty WrapProperty = DependencyProperty.Register(
            nameof(Wrap),
            typeof(FlexWrap),
            typeof(FlexPanel),
            new PropertyMetadata(FlexItem.WrapDefault, new PropertyChangedCallback((d, e) => ((FlexPanel)d).OnWrapChanged(e))));

        private void OnWrapChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_root != null)
            {
                _root.Wrap = (FlexWrap)e.NewValue;
                InvalidateMeasure();
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
            set => this.SetNewValue(WrapProperty, value);
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
            => (int)element.GetValue(OrderProperty);

        /// <summary>
        /// This attached property specifies whether this UIElement should be laid out before or after other items
        /// in the FlexPanel.  Items are laid out based on the ascending value of this property. Items that
        /// have the same value for this property will be laid out in the order they were inserted.
        /// </summary>
        /// <value>The item order (can be a negative, 0, or positive value).</value>
        /// <remarks>The default value for this property is 0.</remarks>
        public static void SetOrder(UIElement element, int value)
            => element.SetNewValue(OrderProperty, value);

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
            => (double)element.GetValue(GrowProperty);

        /// <summary>
        /// This attached property defines the grow factor of the UIElement; the amount of available space it
        /// should use on the main-axis. If this property is set to 0, the item will not grow.
        /// </summary>
        /// <value>The item grow factor.</value>
        /// <remarks>The default value for this property is 0 (does not take any available space).</remarks>
        public static void SetGrow(UIElement element, double value)
            => element.SetNewValue(GrowProperty, value);

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
            => (double)element.GetValue(ShrinkProperty);

        /// <summary>
        /// This attached property defines the shrink factor of the UIElement.  In case the child items overflow
        /// the main-axis of the container, this factor will be used to determine how individual items
        /// should shrink so that all items can fill inside the container.If this property is set to 0,
        /// the item will not shrink.
        /// </summary>
        /// <value>The item shrink factor.</value>
        /// <remarks>The default value for this property is 1 (all items will shrink equally).</remarks>
        public static void SetShrink(UIElement element, double value)
            => element.SetNewValue(ShrinkProperty, value);

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
            => (FlexAlignSelf)element.GetValue(AlignSelfProperty);

        /// <summary>
        /// This attached property defines how the FlexPanel will distribute space between and around child
        /// element for a specific element along the cross-axis. If this property is set to FlexAlignSelf.Auto
        /// on a child element, the parent's value for <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.FlexItem.AlignItems" />
        /// will be used instead.
        /// </summary>
        /// <remarks>The default value for this property FlexAlignSelf.Auto.</remarks>
        public static void SetAlignSelf(UIElement element, FlexAlignSelf value)
            => element.SetNewValue(AlignSelfProperty, value);

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
        public static void SetBasis(UIElement element, FlexBasis value)
            => SetBasis(element, value.ToString());

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
                return flexPanel._root;
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
            element.SetNewValue(FlexItemProperty, value);
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

        private readonly FlexItem _root = new FlexItem();

        private FlexItem AddChild(FrameworkElement view)
        {
            if (_root == null)
            {
                return null;
            }

            view.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var item = (view as FlexPanel)?._root ?? new FlexItem();
            InitItemProperties(view, item);
            if (!(view is FlexPanel))
            {
                // inner layouts don't get measured
                item.SelfSizing = (FlexItem it, ref double w, ref double h) =>
                {
                    UpdateItemProperties(view, item);

                    if (view.DesiredSize.Width > 0 && view.DesiredSize.Height > 0)
                    {
                        w = (double)view.DesiredSize.Width;
                        h = (double)view.DesiredSize.Height;
                        return;
                    }

                    var sizeConstraints = item.GetConstraints();
                    sizeConstraints.Width = (_measuring && sizeConstraints.Width == 0) ? double.PositiveInfinity : sizeConstraints.Width;
                    sizeConstraints.Height = (_measuring && sizeConstraints.Height == 0) ? double.PositiveInfinity : sizeConstraints.Height;
                    view.Measure(sizeConstraints);
                    w = (double)view.DesiredSize.Width;
                    h = (double)view.DesiredSize.Height;
                };
            }

            _root.InsertAt(Children.IndexOf(view), item);
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
                        throw new Exception("something is deeply wrong");
                    }

                    child.Arrange(frame);
                }
            }

            return finalSize;
        }

        private bool _measuring;

        /// <summary>
        /// UWP defined
        /// </summary>
        /// <param name="availableSize">how much space is available to this element</param>
        /// <returns>size desired to render this element</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var widthConstraint = availableSize.Width;
            var heightConstraint = availableSize.Height;

            if (_root == null)
            {
                return new Size(widthConstraint, heightConstraint);
            }

            _measuring = true;

            // 1. Keep track of missing layout items
            var deleteCandidates = _root.ToList();

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
                _root.Remove(item);
            }

            Layout(widthConstraint, heightConstraint);

            // 4. look at the children location
            if (double.IsPositiveInfinity(widthConstraint))
            {
                widthConstraint = 0;
                foreach (var item in _root)
                {
                    widthConstraint = Math.Max(widthConstraint, item.Frame[0] + item.Frame[2] + item.MarginRight);
                }
            }

            if (double.IsPositiveInfinity(heightConstraint))
            {
                heightConstraint = 0;
                foreach (var item in _root)
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

            _measuring = false;

            var result = new Size(widthConstraint, heightConstraint);
            return result;
        }

        private void Layout(double width, double height)
        {
            // Layout is only computed at root level
            if (_root.Parent != null)
            {
                return;
            }

            _root.Width = !double.IsPositiveInfinity(width) ? (double)width : 0;
            _root.Height = !double.IsPositiveInfinity(height) ? (double)height : 0;
            _root.Layout();
        }

        /// <summary>
        /// An item with flexbox properties. Items can also contain other items and be enumerated.
        /// </summary>
        internal class FlexItem : IEnumerable<FlexItem>
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

            private IList<FlexItem> Children { get; set; }

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
                ValidateChild(child);
                (Children ?? (Children = new List<FlexItem>())).Add(child);
                child.Parent = this;
                ShouldOrderChildren |= child.Order != 0;
            }

            public void InsertAt(int index, FlexItem child)
            {
                ValidateChild(child);
                (Children ?? (Children = new List<FlexItem>())).Insert(index, child);
                child.Parent = this;
                ShouldOrderChildren |= child.Order != 0;
            }

            public FlexItem RemoveAt(uint index)
            {
                var child = Children[(int)index];
                child.Parent = null;
                Children.RemoveAt((int)index);
                return child;
            }

            public int Count =>
                Children?.Count ?? 0;

            public FlexItem ItemAt(int index) =>
                Children?[index];

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
                if (Parent != null)
                {
                    throw new InvalidOperationException("Layout() must be called on a root item (that hasn't been added to another item)");
                }

                if (double.IsNaN(Width) || double.IsNaN(Height))
                {
                    throw new InvalidOperationException("Layout() must be called on an item that has proper values for the Width and Height properties");
                }

                if (SelfSizing != null)
                {
                    throw new InvalidOperationException("Layout() cannot be called on an item that has the SelfSizing property set");
                }

                Layout_item(this, Width, Height);
            }

            public delegate void SelfSizingDelegate(FlexItem item, ref double width, ref double height);

            public SelfSizingDelegate SelfSizing { get; set; }

            private static void Layout_item(FlexItem item, double width, double height)
            {
                if (item.Children == null || item.Children.Count == 0)
                {
                    return;
                }

                var layout = default(Flex_layout);
                layout.Init(item, width, height);
                layout.Reset();

                int last_layout_child = 0;
                int relative_children_count = 0;

                for (int i = 0; i < item.Count; i++)
                {
                    var child = layout.Child_at(item, i);
                    if (!child.IsVisible)
                    {
                        continue;
                    }

                    // Items with an absolute position have their frames determined
                    // directly and are skipped during layout.
                    if (child.Position == FlexPosition.Absolute)
                    {
                        child.Frame[2] = Absolute_size(child.Width, child.Left, child.Right, width);
                        child.Frame[3] = Absolute_size(child.Height, child.Top, child.Bottom, height);
                        child.Frame[0] = Absolute_pos(child.Left, child.Right, child.Frame[2], width);
                        child.Frame[1] = Absolute_pos(child.Top, child.Bottom, child.Frame[3], height);

                        // Now that the item has a frame, we can layout its children.
                        Layout_item(child, child.Frame[2], child.Frame[3]);
                        continue;
                    }

                    // Initialize frame.
                    child.Frame[0] = 0;
                    child.Frame[1] = 0;
                    child.Frame[2] = child.Width + child.MarginThickness(false);
                    child.Frame[3] = child.Height + child.MarginThickness(true);

                    // Main axis size defaults to 0.
                    if (double.IsNaN(child.Frame[layout.Frame_size_i]))
                    {
                        child.Frame[layout.Frame_size_i] = 0;
                    }

                    // Cross axis size defaults to the parent's size (or line size in wrap
                    // mode, which is calculated later on).
                    if (double.IsNaN(child.Frame[layout.Frame_size2_i]))
                    {
                        if (layout.Wrap)
                        {
                            layout.Need_lines = true;
                        }
                        else
                        {
                            child.Frame[layout.Frame_size2_i] = (layout.Vertical ? width : height) - child.MarginThickness(!layout.Vertical);
                        }
                    }

                    // Call the self_sizing callback if provided. Only non-NAN values
                    // are taken into account. If the item's cross-axis align property
                    // is set to stretch, ignore the value returned by the callback.
                    if (child.SelfSizing != null)
                    {
                        double[] size = { child.Frame[2], child.Frame[3] };
                        child.SelfSizing(child, ref size[0], ref size[1]);
                        for (int j = 0; j < 2; j++)
                        {
                            int size_off = j + 2;
                            if (size_off == layout.Frame_size2_i && Child_align(child, item) == FlexAlignItems.Stretch)
                            {
                                continue;
                            }

                            double val = size[j];
                            if (!double.IsNaN(val))
                            {
                                child.Frame[size_off] = val;
                            }
                        }
                    }

                    // Honor the `basis' property which overrides the main-axis size.
                    if (!child.Basis.IsAuto)
                    {
                        if (child.Basis.Length < 0)
                        {
                            throw new Exception("basis should >=0");
                        }

                        if (child.Basis.IsRelative && child.Basis.Length > 1)
                        {
                            throw new Exception("relative basis should be <=1");
                        }

                        double basis = child.Basis.Length;
                        if (child.Basis.IsRelative)
                        {
                            basis *= layout.Vertical ? height : width;
                        }

                        child.Frame[layout.Frame_size_i] = basis - child.MarginThickness(layout.Vertical);
                    }

                    double child_size = child.Frame[layout.Frame_size_i];
                    if (layout.Wrap)
                    {
                        if (layout.Flex_dim < child_size)
                        {
                            // Not enough space for this child on this line, layout the
                            // remaining items and move it to a new line.
                            Layout_items(item, last_layout_child, i, relative_children_count, ref layout);

                            layout.Reset();
                            last_layout_child = i;
                            relative_children_count = 0;
                        }

                        double child_size2 = child.Frame[layout.Frame_size2_i];
                        if (!double.IsNaN(child_size2) && child_size2 + child.MarginThickness(!layout.Vertical) > layout.Line_dim)
                        {
                            layout.Line_dim = child_size2 + child.MarginThickness(!layout.Vertical);
                        }
                    }

                    if (child.Grow < 0 || child.Shrink < 0)
                    {
                        throw new Exception("shrink and grow should be >= 0");
                    }

                    layout.Flex_grows += child.Grow;
                    layout.Flex_shrinks += child.Shrink;

                    layout.Flex_dim -= child_size + child.MarginThickness(layout.Vertical);

                    relative_children_count++;

                    if (child_size > 0 && child.Grow > 0)
                    {
                        layout.Extra_flex_dim += child_size;
                    }
                }

                // Layout remaining items in wrap mode, or everything otherwise.
                Layout_items(item, last_layout_child, item.Count, relative_children_count, ref layout);

                // In wrap mode we may need to tweak the position of each line according to
                // the align_content property as well as the cross-axis size of items that
                // haven't been set yet.
                if (layout.Need_lines && (layout.Lines?.Length ?? 0) > 0)
                {
                    double pos = 0;
                    double spacing = 0;
                    double flex_dim = layout.Align_dim - layout.Lines_sizes;

                    if (flex_dim > 0)
                    {
                        Layout_align(item.AlignContent, flex_dim, (uint)(layout.Lines?.Length ?? 0), ref pos, ref spacing);
                    }

                    double old_pos = 0;
                    if (layout.Reverse2)
                    {
                        pos = layout.Align_dim - pos;
                        old_pos = layout.Align_dim;
                    }

                    for (uint i = 0; i < (layout.Lines?.Length ?? 0); i++)
                    {
                        Flex_layout.Flex_layout_line line = layout.Lines[i];

                        if (layout.Reverse2)
                        {
                            pos -= line.Size;
                            pos -= spacing;
                            old_pos -= line.Size;
                        }

                        // Re-position the children of this line, honoring any child
                        // alignment previously set within the line.
                        for (int j = line.Child_begin; j < line.Child_end; j++)
                        {
                            FlexItem child = layout.Child_at(item, j);
                            if (child.Position == FlexPosition.Absolute)
                            {
                                // Should not be re-positioned.
                                continue;
                            }

                            if (double.IsNaN(child.Frame[layout.Frame_size2_i]))
                            {
                                // If the child's cross axis size hasn't been set it, it
                                // defaults to the line size.
                                child.Frame[layout.Frame_size2_i] = line.Size
                                    + (item.AlignContent == FlexAlignContent.Stretch
                                       ? spacing : 0);
                            }

                            child.Frame[layout.Frame_pos2_i] = pos + (child.Frame[layout.Frame_pos2_i] - old_pos);
                        }

                        if (!layout.Reverse2)
                        {
                            pos += line.Size;
                            pos += spacing;
                            old_pos += line.Size;
                        }
                    }
                }

                layout.Cleanup();
            }

            private static void Layout_align1(FlexJustify align, double flex_dim, int children_count, ref double pos_p, ref double spacing_p)
            {
                if (flex_dim < 0)
                {
                    throw new ArgumentException();
                }

                pos_p = 0;
                spacing_p = 0;

                switch (align)
                {
                    case FlexJustify.Start:
                        return;
                    case FlexJustify.End:
                        pos_p = flex_dim;
                        return;
                    case FlexJustify.Center:
                        pos_p = flex_dim / 2;
                        return;
                    case FlexJustify.SpaceBetween:
                        if (children_count > 0)
                        {
                            spacing_p = flex_dim / (children_count - 1);
                        }

                        return;
                    case FlexJustify.SpaceAround:
                        if (children_count > 0)
                        {
                            spacing_p = flex_dim / children_count;
                            pos_p = spacing_p / 2;
                        }

                        return;
                    case FlexJustify.SpaceEvenly:
                        if (children_count > 0)
                        {
                            spacing_p = flex_dim / (children_count + 1);
                            pos_p = spacing_p;
                        }

                        return;
                    default:
                        throw new ArgumentException();
                }
            }

            private static void Layout_align(FlexAlignContent align, double flex_dim, uint children_count, ref double pos_p, ref double spacing_p)
            {
                if (flex_dim < 0)
                {
                    throw new ArgumentException();
                }

                pos_p = 0;
                spacing_p = 0;

                switch (align)
                {
                    case FlexAlignContent.Start:
                        return;
                    case FlexAlignContent.End:
                        pos_p = flex_dim;
                        return;
                    case FlexAlignContent.Center:
                        pos_p = flex_dim / 2;
                        return;
                    case FlexAlignContent.SpaceBetween:
                        if (children_count > 0)
                        {
                            spacing_p = flex_dim / (children_count - 1);
                        }

                        return;
                    case FlexAlignContent.SpaceAround:
                        if (children_count > 0)
                        {
                            spacing_p = flex_dim / children_count;
                            pos_p = spacing_p / 2;
                        }

                        return;
                    case FlexAlignContent.SpaceEvenly:
                        if (children_count > 0)
                        {
                            spacing_p = flex_dim / (children_count + 1);
                            pos_p = spacing_p;
                        }

                        return;
                    case FlexAlignContent.Stretch:
                        spacing_p = flex_dim / children_count;
                        return;
                    default:
                        throw new ArgumentException();
                }
            }

            private static void Layout_items(FlexItem item, int child_begin, int child_end, int children_count, ref Flex_layout layout)
            {
                if (children_count > (child_end - child_begin))
                {
                    throw new ArgumentException();
                }

                if (children_count <= 0)
                {
                    return;
                }

                if (layout.Flex_dim > 0 && layout.Extra_flex_dim > 0)
                {
                    // If the container has a positive flexible space, let's add to it
                    // the sizes of all flexible children.
                    layout.Flex_dim += layout.Extra_flex_dim;
                }

                // Determine the main axis initial position and optional spacing.
                double pos = 0;
                double spacing = 0;
                if (layout.Flex_grows == 0 && layout.Flex_dim > 0)
                {
                    Layout_align1(item.JustifyContent, layout.Flex_dim, children_count, ref pos, ref spacing);
                }

                if (layout.Reverse)
                {
                    pos = layout.Size_dim - pos;
                }

                if (layout.Reverse)
                {
                    pos -= layout.Vertical ? item.PaddingBottom : item.PaddingRight;
                }
                else
                {
                    pos += layout.Vertical ? item.PaddingTop : item.PaddingLeft;
                }

                if (layout.Wrap && layout.Reverse2)
                {
                    layout.Pos2 -= layout.Line_dim;
                }

                for (int i = child_begin; i < child_end; i++)
                {
                    FlexItem child = layout.Child_at(item, i);
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
                    double flex_size = 0;
                    if (layout.Flex_dim > 0)
                    {
                        if (child.Grow != 0)
                        {
                            child.Frame[layout.Frame_size_i] = 0; // Ignore previous size when growing.
                            flex_size = (layout.Flex_dim / layout.Flex_grows) * child.Grow;
                        }
                    }
                    else if (layout.Flex_dim < 0)
                    {
                        if (child.Shrink != 0)
                        {
                            flex_size = (layout.Flex_dim / layout.Flex_shrinks) * child.Shrink;
                        }
                    }

                    child.Frame[layout.Frame_size_i] += flex_size;

                    // Set the cross axis position (and stretch the cross axis size if
                    // needed).
                    double align_size = child.Frame[layout.Frame_size2_i];
                    double align_pos = layout.Pos2 + 0;
                    switch (Child_align(child, item))
                    {
                        case FlexAlignItems.End:
                            align_pos += layout.Line_dim - align_size - (layout.Vertical ? child.MarginRight : child.MarginBottom);
                            break;

                        case FlexAlignItems.Center:
                            align_pos += (layout.Line_dim / 2) - (align_size / 2)
                                + ((layout.Vertical ? child.MarginLeft : child.MarginTop)
                                   - (layout.Vertical ? child.MarginRight : child.MarginBottom));
                            break;

                        case FlexAlignItems.Stretch:
                            if (align_size == 0)
                            {
                                child.Frame[layout.Frame_size2_i] = layout.Line_dim
                                    - ((layout.Vertical ? child.MarginLeft : child.MarginTop)
                                       + (layout.Vertical ? child.MarginRight : child.MarginBottom));
                            }

                            align_pos += layout.Vertical ? child.MarginLeft : child.MarginTop;
                            break;
                        case FlexAlignItems.Start:
                            align_pos += layout.Vertical ? child.MarginLeft : child.MarginTop;
                            break;

                        default:
                            throw new Exception();
                    }

                    child.Frame[layout.Frame_pos2_i] = align_pos;

                    // Set the main axis position.
                    if (layout.Reverse)
                    {
                        pos -= layout.Vertical ? child.MarginBottom : child.MarginRight;
                        pos -= child.Frame[layout.Frame_size_i];
                        child.Frame[layout.Frame_pos_i] = pos;
                        pos -= spacing;
                        pos -= layout.Vertical ? child.MarginTop : child.MarginLeft;
                    }
                    else
                    {
                        pos += layout.Vertical ? child.MarginTop : child.MarginLeft;
                        child.Frame[layout.Frame_pos_i] = pos;
                        pos += child.Frame[layout.Frame_size_i];
                        pos += spacing;
                        pos += layout.Vertical ? child.MarginBottom : child.MarginRight;
                    }

                    // Now that the item has a frame, we can layout its children.
                    Layout_item(child, child.Frame[2], child.Frame[3]);
                }

                if (layout.Wrap && !layout.Reverse2)
                {
                    layout.Pos2 += layout.Line_dim;
                }

                if (layout.Need_lines)
                {
                    Array.Resize(ref layout.Lines, (layout.Lines?.Length ?? 0) + 1);

                    ref Flex_layout.Flex_layout_line line = ref layout.Lines[layout.Lines.Length - 1];

                    line.Child_begin = child_begin;
                    line.Child_end = child_end;
                    line.Size = layout.Line_dim;

                    layout.Lines_sizes += line.Size;
                }
            }

            private static double Absolute_size(double val, double pos1, double pos2, double dim) =>
                !double.IsNaN(val) ? val : (!double.IsNaN(pos1) && !double.IsNaN(pos2) ? dim - pos2 - pos1 : 0);

            private static double Absolute_pos(double pos1, double pos2, double size, double dim) =>
                !double.IsNaN(pos1) ? pos1 : (!double.IsNaN(pos2) ? dim - size - pos2 : 0);

            private static FlexAlignItems Child_align(FlexItem child, FlexItem parent) =>
                child.AlignSelf == FlexAlignSelf.Auto ? parent.AlignItems : (FlexAlignItems)child.AlignSelf;

            private struct Flex_layout
            {
                // Set during init.
                public bool Wrap;
                public bool Reverse;                // whether main axis is reversed
                public bool Reverse2;               // whether cross axis is reversed (wrap only)
                public bool Vertical;
                public double Size_dim;              // main axis parent size
                public double Align_dim;             // cross axis parent size
                public uint Frame_pos_i;            // main axis position
                public uint Frame_pos2_i;           // cross axis position
                public uint Frame_size_i;           // main axis size
                public uint Frame_size2_i;          // cross axis size
                private int[] _ordered_indices;

                // Set for each line layout.
                public double Line_dim;              // the cross axis size
                public double Flex_dim;              // the flexible part of the main axis size
                public double Extra_flex_dim;        // sizes of flexible items
                public double Flex_grows;
                public double Flex_shrinks;
                public double Pos2;                  // cross axis position

                // Calculated layout lines - only tracked when needed:
                //   - if the root's align_content property isn't set to FLEX_ALIGN_START
                //   - or if any child item doesn't have a cross-axis size set
                public bool Need_lines;

                public struct Flex_layout_line
                {
                    public int Child_begin;
                    public int Child_end;
                    public double Size;
                }

                public Flex_layout_line[] Lines;
                public double Lines_sizes;

                // LAYOUT_RESET
                public void Reset()
                {
                    Line_dim = Wrap ? 0 : Align_dim;
                    Flex_dim = Size_dim;
                    Extra_flex_dim = 0;
                    Flex_grows = 0;
                    Flex_shrinks = 0;
                }

                // layout_init
                public void Init(FlexItem item, double width, double height)
                {
                    if (item.PaddingLeft < 0
                        || item.PaddingRight < 0
                        || item.PaddingTop < 0
                        || item.PaddingBottom < 0)
                    {
                        throw new ArgumentException();
                    }

                    width = Math.Max(0, width - item.PaddingLeft + item.PaddingRight);
                    height = Math.Max(0, height - item.PaddingTop + item.PaddingBottom);

                    Reverse = item.Direction == FlexDirection.RowReverse || item.Direction == FlexDirection.ColumnReverse;
                    Vertical = true;
                    switch (item.Direction)
                    {
                        case FlexDirection.Row:
                        case FlexDirection.RowReverse:
                            Vertical = false;
                            Size_dim = width;
                            Align_dim = height;
                            Frame_pos_i = 0;
                            Frame_pos2_i = 1;
                            Frame_size_i = 2;
                            Frame_size2_i = 3;
                            break;
                        case FlexDirection.Column:
                        case FlexDirection.ColumnReverse:
                            Size_dim = height;
                            Align_dim = width;
                            Frame_pos_i = 1;
                            Frame_pos2_i = 0;
                            Frame_size_i = 3;
                            Frame_size2_i = 2;
                            break;
                    }

                    _ordered_indices = null;
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
                                if (item.Children[prev].Order <= item.Children[curr].Order)
                                {
                                    break;
                                }

                                indices[j - 1] = curr;
                                indices[j] = prev;
                            }
                        }

                        _ordered_indices = indices;
                    }

                    Flex_dim = 0;
                    Flex_grows = 0;
                    Flex_shrinks = 0;

                    Reverse2 = false;
                    Wrap = item.Wrap != FlexWrap.NoWrap;
                    if (Wrap)
                    {
                        if (item.Wrap == FlexWrap.Reverse)
                        {
                            Reverse2 = true;
                            Pos2 = Align_dim;
                        }
                    }
                    else
                    {
                        Pos2 = Vertical ? item.PaddingLeft : item.PaddingTop;
                    }

                    Need_lines = Wrap && item.AlignContent != FlexAlignContent.Start;
                    Lines = null;
                    Lines_sizes = 0;
                }

                public FlexItem Child_at(FlexItem item, int i) =>
                    item.Children[_ordered_indices?[i] ?? i];

                public void Cleanup()
                {
                    _ordered_indices = null;
                    Lines = null;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() =>
                ((IEnumerable<FlexItem>)this).GetEnumerator();

            IEnumerator<FlexItem> IEnumerable<FlexItem>.GetEnumerator() =>
                (Children ?? System.Linq.Enumerable.Empty<FlexItem>()).GetEnumerator();

            private void ValidateChild(FlexItem child)
            {
                if (this == child)
                {
                    throw new ArgumentException("cannot add item into self");
                }

                if (child.Parent != null)
                {
                    throw new ArgumentException("child already has a parent");
                }
            }

            private double MarginThickness(bool vertical) =>
                vertical ? MarginTop + MarginBottom : MarginLeft + MarginRight;
        }
    }
}
