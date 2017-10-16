using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    public static class Visual
    {
        public static Vector2 ToVector2(this string str)
        {
            var strLength = str.Count();
            if (strLength < 1)
            {
                throw new Exception();
            }
            else if (str[0] == '<' && str[strLength - 1] == '>')
            {
                str = str.Substring(1, strLength - 2);
            }

            string[] values = str.Split(',');

            var count = values.Count();
            Vector2 vector = Vector2.Zero;

            try
            {
                if (count == 1)
                {
                    vector = new Vector2(float.Parse(values[0]));
                }
                else if (count == 2)
                {
                    vector = new Vector2(float.Parse(values[0]),
                                         float.Parse(values[1]));
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new FormatException($"Cannot convert {str} to Vector2. Use format \"float, float\"");
            }

            return vector;
        }

        public static Vector3 ToVector3(this string str)
        {
            var strLength = str.Count();
            if (strLength < 1)
            {
                throw new Exception();
            }
            else if (str[0] == '<' && str[strLength - 1] == '>')
            {
                str = str.Substring(1, strLength - 2);
            }

            string[] values = str.Split(',');

            var count = values.Count();
            Vector3 vector = Vector3.Zero;

            try
            {
                if (count == 1)
                {
                    vector = new Vector3(float.Parse(values[0]));
                }
                else if (count == 3)
                {
                    vector = new Vector3(float.Parse(values[0]),
                                         float.Parse(values[1]),
                                         float.Parse(values[2]));
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new FormatException($"Cannot convert {str} to Vector3. Use format \"float, float, float\"");
            }

            return vector;
        }

        public static Vector4 ToVector4(this string str)
        {
            var strLength = str.Count();
            if (strLength < 1)
            {
                throw new Exception();
            }
            else if (str[0] == '<' && str[strLength - 1] == '>')
            {
                str = str.Substring(1, strLength - 2);
            }

            string[] values = str.Split(',');

            var count = values.Count();
            Vector4 vector = Vector4.Zero;

            try
            {
                if (count == 1)
                {
                    vector = new Vector4(float.Parse(values[0]));
                }
                else if (count == 4)
                {
                    vector = new Vector4(float.Parse(values[0]),
                                         float.Parse(values[1]),
                                         float.Parse(values[2]),
                                         float.Parse(values[3]));
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new FormatException($"Cannot convert {str} to Vector4. Use format \"float, float, float, float\"");
            }

            return vector;
        }

        public static Windows.UI.Composition.Visual GetVisual(UIElement element)
        {
            return ElementCompositionPreview.GetElementVisual(element);
        }

        public static string GetAnchorPoint(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.AnchorPoint.ToString();
            }
            return (string)obj.GetValue(AnchorPointProperty);
        }

        public static void SetAnchorPoint(DependencyObject obj, string value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.AnchorPoint = value.ToVector2();
            }
            obj.SetValue(AnchorPointProperty, value);
        }

        public static string GetCenterPoint(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.CenterPoint.ToString();
            }

            return (string)obj.GetValue(CenterPointProperty);
        }

        public static void SetCenterPoint(DependencyObject obj, string value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.CenterPoint = value.ToVector3();
            }
            obj.SetValue(CenterPointProperty, value);
        }

        public static string GetOffset(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.Offset.ToString();
            }
            return (string)obj.GetValue(OffsetProperty);
        }

        public static void SetOffset(DependencyObject obj, string value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.Offset = value.ToVector3();
            }
            obj.SetValue(OffsetProperty, value);
        }

        public static double GetOpacity(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.Opacity;
            }

            return (double)obj.GetValue(OpacityProperty);
        }

        public static void SetOpacity(DependencyObject obj, double value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.Opacity = (float)value;
            }

            obj.SetValue(OpacityProperty, value);
        }

        public static double GetRotationAngle(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.RotationAngle;
            }
            return (double)obj.GetValue(RotationAngleProperty);
        }

        public static void SetRotationAngle(DependencyObject obj, double value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.RotationAngle = (float)value;
            }
            obj.SetValue(RotationAngleProperty, value);
        }

        public static double GetRotationAngleInDegrees(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.RotationAngleInDegrees;
            }
            return (double)obj.GetValue(RotationAngleInDegreesProperty);
        }

        public static void SetRotationAngleInDegrees(DependencyObject obj, double value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.RotationAngleInDegrees = (float)value;
            }
            obj.SetValue(RotationAngleInDegreesProperty, value);
        }

        public static string GetRotationAxis(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.RotationAxis.ToString();
            }
            return (string)obj.GetValue(RotationAxisProperty);
        }

        public static void SetRotationAxis(DependencyObject obj, string value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.RotationAxis = value.ToVector3();
            }
            obj.SetValue(RotationAxisProperty, value);
        }

        public static string GetScale(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.Scale.ToString();
            }
            return (string)obj.GetValue(ScaleProperty);
        }

        public static void SetScale(DependencyObject obj, string value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.Scale = value.ToVector3();
            }
            obj.SetValue(ScaleProperty, value);
        }

        public static string GetSize(DependencyObject obj)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                return visual.Size.ToString();
            }
            return (string)obj.GetValue(SizeProperty);
        }

        public static void SetSize(DependencyObject obj, string value)
        {
            if (obj is UIElement element)
            {
                var visual = GetVisual(obj as UIElement);
                visual.Size = value.ToVector2();
            }
            obj.SetValue(SizeProperty, value);
        }

        public static bool GetKeepCenterPointCentered(DependencyObject obj)
        {
            return (bool)obj.GetValue(KeepCenterPointCenteredProperty);
        }

        public static void SetKeepCenterPointCentered(DependencyObject obj, bool value)
        {
            obj.SetValue(KeepCenterPointCenteredProperty, value);
        }

        public static readonly DependencyProperty AnchorPointProperty =
            DependencyProperty.RegisterAttached("AnchorPoint",
                                                typeof(string),
                                                typeof(Visual),
                                                new PropertyMetadata(null, OnAnchorPointChanged));

        public static readonly DependencyProperty CenterPointProperty =
            DependencyProperty.RegisterAttached("CenterPoint",
                                                typeof(string),
                                                typeof(Visual),
                                                new PropertyMetadata(null, OnCenterPointChanged));

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.RegisterAttached("Offset",
                                                typeof(string),
                                                typeof(Visual),
                                                new PropertyMetadata(null, OnOffsetChanged));

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.RegisterAttached("Opacity",
                                                typeof(double),
                                                typeof(Visual),
                                                new PropertyMetadata(double.NaN, OnOpacityChanged));

        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.RegisterAttached("RotationAngle",
                                                typeof(double),
                                                typeof(Visual),
                                                new PropertyMetadata(double.NaN, OnRotationAngleChanged));

        public static readonly DependencyProperty RotationAngleInDegreesProperty =
            DependencyProperty.RegisterAttached("RotationAngleInDegrees",
                                                typeof(double),
                                                typeof(Visual),
                                                new PropertyMetadata(double.NaN, OnRotationAngleInDegreesChanged));

        public static readonly DependencyProperty RotationAxisProperty =
            DependencyProperty.RegisterAttached("RotationAxis",
                                                typeof(string),
                                                typeof(Visual),
                                                new PropertyMetadata(null, OnRotationAxisChanged));

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached("Scale",
                                                typeof(string),
                                                typeof(Visual),
                                                new PropertyMetadata(null, OnScaleChanged));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size",
                                                typeof(string),
                                                typeof(Visual),
                                                new PropertyMetadata(null, OnSizeChanged));

        public static readonly DependencyProperty KeepCenterPointCenteredProperty =
            DependencyProperty.RegisterAttached("KeepCenterPointCentered",
                                                typeof(bool),
                                                typeof(Visual),
                                                new PropertyMetadata(false, OnKeepCenterPointCenteredChanged));

        private static void OnAnchorPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str)
            {
                SetAnchorPoint(d, str);
            }
        }

        private static void OnCenterPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str)
            {
                SetCenterPoint(d, e.NewValue as string);
            }
        }

        private static void OnOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str)
            {
                SetOffset(d, str);
            }
        }

        private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double dbl)
            {
                SetOpacity(d, dbl);
            }
        }

        private static void OnRotationAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double dbl)
            {
                SetRotationAngle(d, dbl);
            }
        }

        private static void OnRotationAngleInDegreesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double dbl)
            {
                SetRotationAngleInDegrees(d, dbl);
            }
        }

        private static void OnRotationAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str)
            {
                SetRotationAxis(d, str);
            }
        }

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str)
            {
                SetScale(d, str);
            }
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string str)
            {
                SetSize(d, str);
            }
        }

        private static void OnKeepCenterPointCenteredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
            {
                return;
            }

            element.SizeChanged -= KeepCenteredElementSizeChanged;

            if (e.NewValue is bool keepCentered && keepCentered)
            {
                element.SizeChanged += KeepCenteredElementSizeChanged;
                var visual = GetVisual(element);
                visual.CenterPoint = new Vector3((float)element.ActualWidth / 2, (float)element.ActualHeight / 2, 0);
            }
        }

        private static void KeepCenteredElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var visual = GetVisual(element);
            visual.CenterPoint = new Vector3((float)element.ActualWidth / 2, (float)element.ActualHeight / 2, 0);
        }
    }
}
