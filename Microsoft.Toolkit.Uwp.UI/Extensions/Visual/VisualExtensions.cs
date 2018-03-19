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
using System.Linq;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Extension methods and attached properties for <see cref="Visual"/> objects
    /// </summary>
    public static class VisualExtensions
    {
        /// <summary>
        /// Converts a <see cref="string"/> to <see cref="Vector2"/>
        /// </summary>
        /// <param name="str">A string in the format of "float, float"</param>
        /// <returns><see cref="Vector2"/></returns>
        public static Vector2 ToVector2(this string str)
        {
            try
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
                Vector2 vector;

                if (count == 1)
                {
                    vector = new Vector2(float.Parse(values[0]));
                }
                else if (count == 2)
                {
                    vector = new Vector2(float.Parse(values[0]), float.Parse(values[1]));
                }
                else
                {
                    throw new Exception();
                }

                return vector;
            }
            catch (Exception)
            {
                throw new FormatException($"Cannot convert {str} to Vector2. Use format \"float, float\"");
            }
        }

        /// <summary>
        /// Converts a <see cref="string"/> to <see cref="Vector3"/>
        /// </summary>
        /// <param name="str">A string in the format of "float, float, float"</param>
        /// <returns><see cref="Vector3"/></returns>
        public static Vector3 ToVector3(this string str)
        {
            try
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
                Vector3 vector;

                if (count == 1)
                {
                    vector = new Vector3(float.Parse(values[0]));
                }
                else if (count == 3)
                {
                    vector = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                }
                else
                {
                    throw new Exception();
                }

                return vector;
            }
            catch (Exception)
            {
                throw new FormatException($"Cannot convert {str} to Vector3. Use format \"float, float, float\"");
            }
        }

        /// <summary>
        /// Converts a <see cref="string"/> to <see cref="Vector4"/>
        /// </summary>
        /// <param name="str">A string in the format of "float, float, float, float"</param>
        /// <returns><see cref="Vector4"/></returns>
        public static Vector4 ToVector4(this string str)
        {
            try
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
                Vector4 vector;

                if (count == 1)
                {
                    vector = new Vector4(float.Parse(values[0]));
                }
                else if (count == 4)
                {
                    vector = new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                }
                else
                {
                    throw new Exception();
                }

                return vector;
            }
            catch (Exception)
            {
                throw new FormatException($"Cannot convert {str} to Vector4. Use format \"float, float, float, float\"");
            }
        }

        /// <summary>
        /// Retrieves the <see cref="Visual"/> object of a <see cref="UIElement"/>
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/></param>
        /// <returns>The <see cref="Visual"/> backing the <see cref="UIElement"/></returns>
        public static Visual GetVisual(UIElement element)
        {
            return ElementCompositionPreview.GetElementVisual(element);
        }

        /// <summary>
        /// Gets the <see cref="Visual.AnchorPoint"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>A <see cref="Vector2"/> string representation of the <see cref="Visual.AnchorPoint"/></returns>
        public static string GetAnchorPoint(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetAnchorPointForElement(element);
            }

            return (string)obj.GetValue(AnchorPointProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.AnchorPoint"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The string representation of the <see cref="Vector2"/> to be set</param>
        public static void SetAnchorPoint(DependencyObject obj, string value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetAnchorPointForElement(value, element);
            }

            obj.SetValue(AnchorPointProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.CenterPoint"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>A <see cref="Vector3"/> string representation of the <see cref="Visual.CenterPoint"/></returns>
        public static string GetCenterPoint(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetCenterPointForElement(element);
            }

            return (string)obj.GetValue(CenterPointProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.CenterPoint"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The string representation of the <see cref="Vector3"/> to be set</param>
        public static void SetCenterPoint(DependencyObject obj, string value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetCenterPointForElement(value, element);
            }

            obj.SetValue(CenterPointProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.Offset"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>A <see cref="Vector3"/> string representation of the <see cref="Visual.Offset"/></returns>
        public static string GetOffset(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetOffsetForElement(element);
            }

            return (string)obj.GetValue(OffsetProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.Offset"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The string representation of the <see cref="Vector3"/> to be set</param>
        public static void SetOffset(DependencyObject obj, string value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetOffsetForElement(value, element);
            }

            obj.SetValue(OffsetProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.Opacity"/> of a UIElement
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>The <see cref="Visual.Opacity"/> of the <see cref="UIElement"/></returns>
        public static double GetOpacity(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetOpacityForElement(element);
            }

            return (double)obj.GetValue(OpacityProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.Opacity"/> of a UIElement
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The opacity to be set between 0.0 and 1.0</param>
        public static void SetOpacity(DependencyObject obj, double value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetOpacityForElement(value, element);
            }

            obj.SetValue(OpacityProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.RotationAngle"/> of a UIElement in radians
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>The <see cref="Visual.RotationAngle"/> of the <see cref="UIElement"/></returns>
        public static double GetRotationAngle(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetRotationAngleForElement(element);
            }

            return (double)obj.GetValue(RotationAngleProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.RotationAngle"/> of a UIElement in radians
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The rotation in radians</param>
        public static void SetRotationAngle(DependencyObject obj, double value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetRotationAngleForElement(value, element);
            }

            obj.SetValue(RotationAngleProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.RotationAngleInDegrees"/> of a UIElement in degrees
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>The <see cref="Visual.RotationAngleInDegrees"/> of the <see cref="UIElement"/></returns>
        public static double GetRotationAngleInDegrees(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetRotationAngleInDegreesForElement(element);
            }

            return (double)obj.GetValue(RotationAngleInDegreesProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.RotationAngleInDegrees"/> of a UIElement in radians
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The rotation in degrees</param>
        public static void SetRotationAngleInDegrees(DependencyObject obj, double value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetRotationAngleInDegreesForElement(value, element);
            }

            obj.SetValue(RotationAngleInDegreesProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.RotationAxis"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>A <see cref="Vector3"/> string representation of the <see cref="Visual.RotationAxis"/></returns>
        public static string GetRotationAxis(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetRotationAxisForElement(element);
            }

            return (string)obj.GetValue(RotationAxisProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.RotationAxis"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The string representation of the <see cref="Vector3"/> to be set</param>
        public static void SetRotationAxis(DependencyObject obj, string value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetRotationAxisForElement(value, element);
            }

            obj.SetValue(RotationAxisProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.Scale"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>A <see cref="Vector3"/> string representation of the <see cref="Visual.Scale"/></returns>
        public static string GetScale(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetScaleForElement(element);
            }

            return (string)obj.GetValue(ScaleProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.Scale"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The string representation of the <see cref="Vector3"/> to be set</param>
        public static void SetScale(DependencyObject obj, string value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetScaleForElement(value, element);
            }

            obj.SetValue(ScaleProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.Size"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>A <see cref="Vector2"/> string representation of the <see cref="Visual.Size"/></returns>
        public static string GetSize(DependencyObject obj)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                return GetSizeForElement(element);
            }

            return (string)obj.GetValue(SizeProperty);
        }

        /// <summary>
        /// Sets the <see cref="Visual.Size"/> of a UIElement in a string form
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">The string representation of the <see cref="Vector2"/> to be set</param>
        public static void SetSize(DependencyObject obj, string value)
        {
            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode && obj is UIElement element)
            {
                SetSizeForElement(value, element);
            }

            obj.SetValue(SizeProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Visual.CenterPoint"/> of the <see cref="UIElement"/> normalized between 0.0 and 1.0
        /// is centered even when the visual is resized
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>a string representing Vector3 as the normalized <see cref="Visual.CenterPoint"/></returns>
        public static string GetNormalizedCenterPoint(DependencyObject obj)
        {
            return (string)obj.GetValue(NormalizedCenterPointProperty);
        }

        /// <summary>
        /// Sets the normalized <see cref="Visual.CenterPoint"/> of the <see cref="UIElement"/>
        /// is centered even when the visual is resized
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <param name="value">A string representing a Vector3 normalized between 0.0 and 1.0</param>
        public static void SetNormalizedCenterPoint(DependencyObject obj, string value)
        {
            obj.SetValue(NormalizedCenterPointProperty, value);
        }

        /// <summary>
        /// Identifies the AnchorPointProperty attached property.
        /// </summary>
        public static readonly DependencyProperty AnchorPointProperty =
            DependencyProperty.RegisterAttached("AnchorPoint", typeof(string), typeof(VisualExtensions), new PropertyMetadata(null, OnAnchorPointChanged));

        /// <summary>
        /// Identifies the CenterPoint attached property.
        /// </summary>
        public static readonly DependencyProperty CenterPointProperty =
            DependencyProperty.RegisterAttached("CenterPoint", typeof(string), typeof(VisualExtensions), new PropertyMetadata(null, OnCenterPointChanged));

        /// <summary>
        /// Identifies the Offset attached property.
        /// </summary>
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.RegisterAttached("Offset", typeof(string), typeof(VisualExtensions), new PropertyMetadata(null, OnOffsetChanged));

        /// <summary>
        /// Identifies the Opacity attached property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(VisualExtensions), new PropertyMetadata(double.NaN, OnOpacityChanged));

        /// <summary>
        /// Identifies the RotationAngle attached property.
        /// </summary>
        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.RegisterAttached("RotationAngle", typeof(double), typeof(VisualExtensions), new PropertyMetadata(double.NaN, OnRotationAngleChanged));

        /// <summary>
        /// Identifies the RotationAngleInDegrees attached property.
        /// </summary>
        public static readonly DependencyProperty RotationAngleInDegreesProperty =
            DependencyProperty.RegisterAttached("RotationAngleInDegrees", typeof(double), typeof(VisualExtensions), new PropertyMetadata(double.NaN, OnRotationAngleInDegreesChanged));

        /// <summary>
        /// Identifies the RotationAxis attached property.
        /// </summary>
        public static readonly DependencyProperty RotationAxisProperty =
            DependencyProperty.RegisterAttached("RotationAxis", typeof(string), typeof(VisualExtensions), new PropertyMetadata(null, OnRotationAxisChanged));

        /// <summary>
        /// Identifies the Scale attached property.
        /// </summary>
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached("Scale", typeof(string), typeof(VisualExtensions), new PropertyMetadata(null, OnScaleChanged));

        /// <summary>
        /// Identifies the Size attached property.
        /// </summary>
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size", typeof(string), typeof(VisualExtensions), new PropertyMetadata(null, OnSizeChanged));

        /// <summary>
        /// Identifies the NormalizedCenterPoint attached property.
        /// </summary>
        public static readonly DependencyProperty NormalizedCenterPointProperty =
            DependencyProperty.RegisterAttached("NormalizedCenterPoint", typeof(string), typeof(VisualExtensions), new PropertyMetadata(false, OnNormalizedCenterPointChanged));

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
                SetCenterPoint(d, str);
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

        private static void OnNormalizedCenterPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && !DesignTimeHelpers.IsRunningInLegacyDesignerMode)
            {
                SetupNormalizedCenterPoint(e, element);
            }
        }

        private static void SetupNormalizedCenterPoint(DependencyPropertyChangedEventArgs e, FrameworkElement element)
        {
            element.SizeChanged -= KeepCenteredElementSizeChanged;

            if (e.NewValue is string normalizedValue)
            {
                var vectorValue = normalizedValue.ToVector3();
                var visual = GetVisual(element);
                visual.CenterPoint = new Vector3((float)element.ActualWidth * vectorValue.X, (float)element.ActualHeight * vectorValue.Y, 0);

                element.SizeChanged += KeepCenteredElementSizeChanged;
            }
        }

        private static void KeepCenteredElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var element = sender as FrameworkElement;

            var normalizedValue = GetNormalizedCenterPoint(element);
            var vectorValue = normalizedValue.ToVector3();
            var visual = GetVisual(element);
            visual.CenterPoint = new Vector3((float)element.ActualWidth * vectorValue.X, (float)element.ActualHeight * vectorValue.Y, 0);
        }

        private static string GetAnchorPointForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.AnchorPoint.ToString();
        }

        private static void SetAnchorPointForElement(string value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.AnchorPoint = value.ToVector2();
        }

        private static string GetCenterPointForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.CenterPoint.ToString();
        }

        private static void SetCenterPointForElement(string value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.CenterPoint = value.ToVector3();
        }

        private static string GetOffsetForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.Offset.ToString();
        }

        private static void SetOffsetForElement(string value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.Offset = value.ToVector3();
        }

        private static double GetOpacityForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.Opacity;
        }

        private static void SetOpacityForElement(double value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.Opacity = (float)value;
        }

        private static double GetRotationAngleForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.RotationAngle;
        }

        private static void SetRotationAngleForElement(double value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.RotationAngle = (float)value;
        }

        private static double GetRotationAngleInDegreesForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.RotationAngleInDegrees;
        }

        private static void SetRotationAngleInDegreesForElement(double value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.RotationAngleInDegrees = (float)value;
        }

        private static string GetRotationAxisForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.RotationAxis.ToString();
        }

        private static void SetRotationAxisForElement(string value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.RotationAxis = value.ToVector3();
        }

        private static string GetScaleForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.Scale.ToString();
        }

        private static void SetScaleForElement(string value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.Scale = value.ToVector3();
        }

        private static string GetSizeForElement(UIElement element)
        {
            var visual = GetVisual(element);
            return visual.Size.ToString();
        }

        private static void SetSizeForElement(string value, UIElement element)
        {
            var visual = GetVisual(element);
            visual.Size = value.ToVector2();
        }
    }
}
