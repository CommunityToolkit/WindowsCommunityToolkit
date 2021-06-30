

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// The base class for attached shadows.
    /// </summary>
    public abstract class AttachedShadowBase : DependencyObject
    {
        public static AttachedShadowBase GetShadow(DependencyObject obj)
        {
            return (AttachedShadowBase)obj.GetValue(ShadowProperty);
        }

        public static void SetShadow(DependencyObject obj, AttachedShadowBase value)
        {
            obj.SetValue(ShadowProperty, value);
        }

        public static readonly DependencyProperty ShadowProperty =
            DependencyProperty.RegisterAttached("Shadow", typeof(AttachedShadowBase), typeof(AttachedShadowBase), new PropertyMetadata(null, OnShadowChanged));

        private static void OnShadowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
            {
                return;
            }

            if (e.OldValue is AttachedShadowBase oldShadow)
            {
                oldShadow.DisconnectElement(element);
            }

            if (e.NewValue is AttachedShadowBase newShadow)
            {
                newShadow.ConnectElement(element);
            }
        }

        public static readonly DependencyProperty BlurRadiusProperty =
            DependencyProperty.Register(nameof(BlurRadius), typeof(double), typeof(AttachedShadowBase), new PropertyMetadata(12d, OnDependencyPropertyChanged));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(AttachedShadowBase), new PropertyMetadata(Colors.Black, OnDependencyPropertyChanged));

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register(
                nameof(Offset),
                typeof(Vector3),
                typeof(AttachedShadowBase),
                new PropertyMetadata(Vector3.Zero, OnDependencyPropertyChanged));

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(AttachedShadowBase), new PropertyMetadata(1d, OnDependencyPropertyChanged));

        /// <summary>
        /// Returns whether or not this <see cref="AttachedShadowBase"/> implementation is supported on the current platform
        /// </summary>
        public abstract bool IsSupported { get; }

        /// <summary>
        /// Returns the collection of <see cref="AttachedShadowElementContext"/> for each element this <see cref="AttachedShadowBase"/> is connected to
        /// </summary>
        protected ConditionalWeakTable<FrameworkElement, AttachedShadowElementContext> ShadowElementContextTable { get; private set; }

        /// <summary>
        /// Gets or set the blur radius of the shadow
        /// </summary>
        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the opacity of the shadow
        /// </summary>
        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        /// <summary>
        /// Gets or sets the offset of the shadow
        /// </summary>
        public Vector3 Offset
        {
            get => (Vector3)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        /// <summary>
        /// Gets or sets the color of the shadow
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Returns whether or not OnSizeChanged should be called when <see cref="FrameworkElement.SizeChanged"/> is fired
        /// </summary>
        public abstract bool SupportsOnSizeChangedEvent { get; }

        private void ConnectElement(FrameworkElement element)
        {
            if (!IsSupported)
            {
                return;
            }

            ShadowElementContextTable = ShadowElementContextTable ?? new ConditionalWeakTable<FrameworkElement, AttachedShadowElementContext>();
            if (ShadowElementContextTable.TryGetValue(element, out var context))
            {
                return;
            }

            context = new AttachedShadowElementContext();
            context.ConnectToElement(this, element);
            ShadowElementContextTable.Add(element, context);
        }

        private void DisconnectElement(FrameworkElement element)
        {
            if (ShadowElementContextTable == null)
            {
                return;
            }

            if (ShadowElementContextTable.TryGetValue(element, out var context))
            {
                context.DisconnectFromElement();
                ShadowElementContextTable.Remove(element);
            }
        }

        /// <summary>
        /// Override to handle when the <see cref="AttachedShadowElementContext"/> for an element is being initialized.
        /// </summary>
        /// <param name="context">The <see cref="AttachedShadowElementContext"/> that is being initialized.</param>
        protected internal virtual void OnElementContextInitialized(AttachedShadowElementContext context)
        {
            OnPropertyChanged(context, OpacityProperty, Opacity, Opacity);
            OnPropertyChanged(context, BlurRadiusProperty, BlurRadius, BlurRadius);
            OnPropertyChanged(context, ColorProperty, Color, Color);
            OnPropertyChanged(context, OffsetProperty, Offset, Offset);
            UpdateShadowClip(context);
            UpdateShadowMask(context);
            SetElementChildVisual(context);
        }

        /// <summary>
        /// Override to handle when the <see cref="AttachedShadowElementContext"/> for an element is being uninitialized.
        /// </summary>
        /// <param name="context">The <see cref="AttachedShadowElementContext"/> that is being uninitialized.</param>
        protected internal virtual void OnElementContextUninitialized(AttachedShadowElementContext context)
        {
            ElementCompositionPreview.SetElementChildVisual(context.Element, null);
        }

        /// <summary>
        /// Get the associated <see cref="AttachedShadowElementContext"/> for the specified <see cref="FrameworkElement"/>
        /// </summary>
        public AttachedShadowElementContext GetElementContext(FrameworkElement element)
        {
            if (ShadowElementContextTable != null && ShadowElementContextTable.TryGetValue(element, out var context))
            {
                return context;
            }

            return null;
        }

        /// <summary>
        /// Sets <see cref="AttachedShadowElementContext.SpriteVisual"/> as a child visual on <see cref="AttachedShadowElementContext.Element"/>
        /// </summary>
        /// <param name="context">The <see cref="AttachedShadowElementContext"/> this operaiton will be performed on.</param>
        protected virtual void SetElementChildVisual(AttachedShadowElementContext context)
        {
            ElementCompositionPreview.SetElementChildVisual(context.Element, context.SpriteVisual);
        }

        /// <summary>
        /// Use this method as the <see cref="PropertyChangedCallback"/> for <see cref="DependencyProperty">DependencyProperties</see> in derived classes
        /// </summary>
        protected static void OnDependencyPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AttachedShadowBase)?.CallPropertyChangedForEachElement(args.Property, args.OldValue, args.NewValue);
        }

        private void CallPropertyChangedForEachElement(DependencyProperty property, object oldValue, object newValue)
        {
            if (ShadowElementContextTable == null)
            {
                return;
            }

            foreach (var context in ShadowElementContextTable)
            {
                if (context.Value.IsInitialized)
                {
                    OnPropertyChanged(context.Value, property, oldValue, newValue);
                }
            }
        }

        /// <summary>
        /// Get a <see cref="CompositionBrush"/> in the shape of the element that is casting the shadow
        /// </summary>
        protected virtual CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// Get the <see cref="CompositionClip"/> for the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        protected virtual CompositionClip GetShadowClip(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// Update the mask that gives the shadow its shape
        /// </summary>
        protected void UpdateShadowMask(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            context.Shadow.Mask = GetShadowMask(context);
        }

        /// <summary>
        /// Update the clipping on the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        protected void UpdateShadowClip(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            context.SpriteVisual.Clip = GetShadowClip(context);
        }

        /// <summary>
        /// This method is called when a DependencyProperty is changed
        /// </summary>
        protected virtual void OnPropertyChanged(AttachedShadowElementContext context, DependencyProperty property, object oldValue, object newValue)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            if (property == BlurRadiusProperty)
            {
                context.Shadow.BlurRadius = (float)(double)newValue;
            }
            else if (property == OpacityProperty)
            {
                context.Shadow.Opacity = (float)(double)newValue;
            }
            else if (property == ColorProperty)
            {
                context.Shadow.Color = (Color)newValue;
            }
            else if (property == OffsetProperty)
            {
                context.Shadow.Offset = (Vector3)newValue;
            }
        }

        /// <summary>
        /// This method is called when the element size changes, and <see cref="SupportsOnSizeChangedEvent"/> = <see cref="bool">true</see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newSize"></param>
        /// <param name="previousSize"></param>
        protected internal virtual void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
        }
    }
}