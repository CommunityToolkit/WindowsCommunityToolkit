using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;

namespace AnimationsDebugging
{
    /// <summary>
    /// Creates a <see cref="Visual"/> that displays the value of animated properties.
    /// </summary>
    public sealed class PropertyMeter
    {
        const float c_digitHeight = 8;
        const float c_digitWidth = 8;

        static readonly CanvasTextFormat s_textFormat =
            new CanvasTextFormat()
            {
                FontFamily = "Arial",
                LineSpacing = c_digitHeight,
                FontSize = 10,
                HorizontalAlignment = CanvasHorizontalAlignment.Center
            };

        readonly Compositor _c;
        readonly ContainerVisual _root;

        // Caches the geometry for the number strip.
        static CanvasGeometry s_numberStripCanvasGeometry;
        CompositionPathGeometry _numberStripGeometry;

        // Where the next meter will be placed.
        float _nextMeterVerticalOffset;

        public PropertyMeter(Compositor compositor)
        {
            _c = compositor;
            _root = _c.CreateContainerVisual();
        }

        /// <summary>
        /// Returns the root <see cref="Visual"/> of the meter.
        /// </summary>
        public Visual Root => _root;

        /// <summary>
        /// Starts tracking a scalar property value.
        /// </summary>
        public void AddScalar(CompositionObject propertyOwner, string scalarPropertyAccessor, int numberOfDigits)
            => AddScalar(propertyOwner, scalarPropertyAccessor, numberOfDigits, numberOfDecimalPlaces: 0);

        /// <summary>
        /// Starts tracking a scalar property value.
        /// </summary>
        public void AddScalar(CompositionObject propertyOwner, string scalarPropertyAccessor, int numberOfDigits, int numberOfDecimalPlaces)
        {
            AddScalarProperty(propertyOwner, scalarPropertyAccessor, numberOfDigits, numberOfDecimalPlaces);
            AddGapToNextMeter();
        }

        /// <summary>
        /// Starts tracking a Vector2 property value.
        /// </summary>
        public void AddVector2(CompositionObject propertyOwner, string vector2PropertyAccessor, int numberOfDigits)
            => AddVector2(propertyOwner, vector2PropertyAccessor, numberOfDigits, numberOfDecimalPlaces: 0);

        /// <summary>
        /// Starts tracking a Vector2 property value.
        /// </summary>
        public void AddVector2(CompositionObject propertyOwner, string vector2PropertyAccessor, int numberOfDigits, int numberOfDecimalPlaces)
        {
            AddScalarProperty(propertyOwner, $"{vector2PropertyAccessor}.X", numberOfDigits, numberOfDecimalPlaces);
            AddScalarProperty(propertyOwner, $"{vector2PropertyAccessor}.Y", numberOfDigits, numberOfDecimalPlaces);
            AddGapToNextMeter();
        }

        /// <summary>
        /// Starts tracking a Vector3 property value.
        /// </summary>
        public void AddVector3(CompositionObject propertyOwner, string vector3PropertyAccessor, int numberOfDigits)
            => AddVector3(propertyOwner, vector3PropertyAccessor, numberOfDigits, numberOfDecimalPlaces: 0);

        /// <summary>
        /// Starts tracking a Vector3 property value.
        /// </summary>
        public void AddVector3(CompositionObject propertyOwner, string vector3PropertyAccessor, int numberOfDigits, int numberOfDecimalPlaces)
        {
            AddScalarProperty(propertyOwner, $"{vector3PropertyAccessor}.X", numberOfDigits, numberOfDecimalPlaces);
            AddScalarProperty(propertyOwner, $"{vector3PropertyAccessor}.Y", numberOfDigits, numberOfDecimalPlaces);
            AddScalarProperty(propertyOwner, $"{vector3PropertyAccessor}.Z", numberOfDigits, numberOfDecimalPlaces);
            AddGapToNextMeter();
        }

        // Increases the vertical offset for the next meter so that it is logically
        // separated from the meter that was just added.
        void AddGapToNextMeter()
        {
            _nextMeterVerticalOffset += c_digitHeight * 0.12F;
        }

        void AddScalarProperty(CompositionObject propertyOwner, string scalarPropertyAccessorExpression, int numberOfDigits, int numberOfDecimalPlaces)
            => AddScalarExpression(scalarPropertyAccessorExpression, new[] { new ObjectBinding("_", propertyOwner) }, numberOfDigits, numberOfDecimalPlaces);

        void AddScalarExpression(string scalarExpression, IEnumerable<ObjectBinding> objectBindings, int numberOfDigits, int numberOfDecimalPlaces)
        {
            // Binds the variables in an expression to CompositionObjects.
            void BindVariables(CompositionAnimation animation)
            {
                foreach (var binding in objectBindings)
                {
                    animation.SetReferenceParameter(binding.Name, binding.Object);
                }
            }

            var numberOfDigitsToLeftOfDecimalPlace = numberOfDigits - numberOfDecimalPlaces;

            if (numberOfDigits < 1 || numberOfDecimalPlaces < 0 || numberOfDigitsToLeftOfDecimalPlace < 0)
            {
                throw new ArgumentException();
            }

            // Create a visual to hold this meter. Each meter is contained by a ShapeVisual rather
            // than a container shape so we can clip the meter.
            var shapeVisual = _c.CreateShapeVisual();
            shapeVisual.Clip = _c.CreateInsetClip();
            shapeVisual.Size = new Vector2(c_digitWidth * numberOfDigits, c_digitHeight);
            // Offset the visual so it doesn't occlude any previous meters
            shapeVisual.Offset = new Vector3(0, _nextMeterVerticalOffset * 1.03F, 0);
            // Next meter will go below this one.
            _nextMeterVerticalOffset += c_digitHeight;
            _root.Children.InsertAtTop(shapeVisual);

            // Create a colored background.
            var backgroundRectangle = _c.CreateRectangleGeometry();
            backgroundRectangle.Size = shapeVisual.Size;
            var backgroundShape = _c.CreateSpriteShape(backgroundRectangle);
            shapeVisual.Shapes.Add(backgroundShape);
            // Black with a little bit of transparency.
            backgroundShape.FillBrush = _c.CreateColorBrush(Color.FromArgb(180, 0, 0, 0));

            const string objectReferenceName = "_";

            var rawValue = $"({objectReferenceName}.{scalarExpression})";
            var absoluteValue = $"Abs({objectReferenceName}.{scalarExpression})";
            var isNegative = $"{rawValue}<0";

            // Animate the foreground color so it changes when the value is negative.
            var foregroundBrush = _c.CreateColorBrush();
            {
                // Red or white. Note that ColorRgb constructor expressions seem to be broken, but
                // ColorHsl works. Animates between red (0,1,0.5) and white (0,0,1).
                var red = "ColorHsl(0,1,0.5)";
                var white = "ColorHsl(0,0,1)";
                var expression = $"{isNegative}?{red}:{white}";
                var brushColorAnimation = _c.CreateExpressionAnimation(expression);
                BindVariables(brushColorAnimation);
                foregroundBrush.StartAnimation(nameof(foregroundBrush.Color), brushColorAnimation);
            }

            // Create the decimal point
            var decimalPoint = _c.CreateSpriteShape();
            var dot = _c.CreateEllipseGeometry();
            dot.Radius = new Vector2(0.7F);
            decimalPoint.Geometry = dot;
            decimalPoint.FillBrush = foregroundBrush;
            decimalPoint.Offset = new Vector2(((numberOfDigits - numberOfDecimalPlaces + 0.09F) * c_digitWidth), c_digitHeight * 0.9F);
            shapeVisual.Shapes.Add(decimalPoint);

            // Create the digits
            for (var i = 0; i < numberOfDigits; i++)
            {
                // The far right digit is treated specially - it rolls over continuously and has no
                // decimal point.
                var isFarRightDigit = i == numberOfDigits - 1;


                var digit = _c.CreateSpriteShape();
                shapeVisual.Shapes.Add(digit);
                digit.Geometry = GetNumberStripGeometry();
                digit.FillBrush = foregroundBrush;

                // Animate the scale so that the digit disappears if it is a leading 0.
                if (i < numberOfDigitsToLeftOfDecimalPlace - 1)
                {
                    // The digit is a leading (and therefore unnecessary) 0 if the absolute raw value
                    // is less than this:
                    var minValueForDigit = DoubleToString(Math.Pow(10, numberOfDigitsToLeftOfDecimalPlace - i - 1));
                    // Hide the digit by setting its scale to 0 if it is a leading 0.
                    var expression = $"{absoluteValue}<{minValueForDigit}?Vector2(0,0):Vector2(1,1)";
                    var digitVisibilityAnimation = _c.CreateExpressionAnimation(expression);
                    BindVariables(digitVisibilityAnimation);
                    digit.StartAnimation(nameof(digit.Scale), digitVisibilityAnimation);
                }

                // Expression to get the value for the current digit. The evaluated value is [0..10). The further to the
                // right the digit is, the more the value is scaled up.
                var digitValue = $"{absoluteValue}*{Pow10(i - numberOfDigitsToLeftOfDecimalPlace + 1)}";

                var moddedValue = $"Mod({digitValue},10)";

                string digitValueExpression;

                if (isFarRightDigit)
                {
                    digitValueExpression = moddedValue;
                }
                else
                {
                    // Counts from numberOfDigits-1 down to 0
                    var compI = numberOfDigits - i - 1;

                    // When this digit overflows from 9 to 0, during the last 1/10th of the far right digit 
                    // (when the far right digit is rolling over 9 back to 0), allow this digit to roll as 
                    // if it is being pushed by the far right value.

                    // Evaluates to true if the digit is in its transition zone.
                    var transitionTest = $"Floor({moddedValue}+{Pow10(-compI)})!=Floor({moddedValue})";

                    // Prevent the digit from rolling when not in its transition zone. Make it snap to whole values.
                    var normalValue = $"Floor({moddedValue})";

                    // During transition, ramp up to the next value.
                    var transitionValue = $"{normalValue}+Mod({digitValue}*{Pow10(compI)}, 1)";

                    digitValueExpression = $"({transitionTest}?{transitionValue}:{normalValue})";
                }

                // Offset x to place the digit in the correct position, offset y to display the correct value in the number strip.
                var offsetExpression = $"Vector2({i * c_digitWidth},({digitValueExpression}*-{c_digitHeight}) - 1.5)";

                var digitOffsetAnimation = _c.CreateExpressionAnimation(offsetExpression);
                BindVariables(digitOffsetAnimation);

                // Animate the offset of the number string for this digit.
                digit.StartAnimation(nameof(digit.Offset), digitOffsetAnimation);
            }

            // Create a message to indicate overflow. This will sit on top
            // of the digits if there is an overflow detected.
            var overflowContainer = _c.CreateContainerShape();
            shapeVisual.Shapes.Add(overflowContainer);

            // True if the current value is too large for the configuration of the meter.
            var isOverflowing = $"{absoluteValue}>={Pow10(numberOfDigitsToLeftOfDecimalPlace)}";

            var overflowBackgroundShape = _c.CreateSpriteShape(backgroundRectangle);
            overflowBackgroundShape.FillBrush = _c.CreateColorBrush(Colors.DarkOrange);
            overflowContainer.Shapes.Add(overflowBackgroundShape);

            var overflowGeometry = GetNumberStripGeometry();
            var overflowShape = _c.CreateSpriteShape(overflowGeometry);
            overflowContainer.Shapes.Add(overflowShape);

            overflowShape.FillBrush = foregroundBrush;
            // 11 is the offset for the "E" character in the number strip.
            overflowShape.Offset = new Vector2(0, (11 * -c_digitHeight) - 1.5F);

            // Hide the overflow indicator as long as the value is within range.
            {
                var overflowScale = $"{isOverflowing}?Vector2(1,1):Vector2(0,0)";
                var overflowVisibilityAnimation = _c.CreateExpressionAnimation(overflowScale);
                BindVariables(overflowVisibilityAnimation);
                overflowContainer.StartAnimation(nameof(overflowContainer.Scale), overflowVisibilityAnimation);
            }

            // Create the minus sign. This sits on top of everything so it is visible even
            // if the meter overflows.
            var minusGeometry = _c.CreateRectangleGeometry();
            minusGeometry.Size = new Vector2(2, 1);
            var minusShape = _c.CreateSpriteShape(minusGeometry);
            minusShape.FillBrush = foregroundBrush;

            // Animate the minus sign so that it only appears if the value is negative, and it moves to
            // account for the number of digits visible to the left of the decimal place.
            // When the value is positive the minus sign is hidden by offseting it outside
            // the clip of the containing ShapeVisual.
            {
                // One less than the number of significant digits to the left of the decimal place.
                var numberOfSignificantDigitsMinus1 = $"{absoluteValue}<1?0:Floor(Log10({absoluteValue}))";

                var minusSignPositionX = $"{c_digitWidth}*({numberOfDigitsToLeftOfDecimalPlace - 1}-({numberOfSignificantDigitsMinus1}))";
                var minusSignPositionY = $"{DoubleToString((c_digitHeight - minusGeometry.Size.Y) / 2)}";
                var expression = $"Vector2({isNegative}?({isOverflowing}?0:{minusSignPositionX}):-100,{minusSignPositionY})";
                var minusSignVisibilityAnimation = _c.CreateExpressionAnimation(expression);
                BindVariables(minusSignVisibilityAnimation);
                minusShape.StartAnimation(nameof(minusShape.Offset), minusSignVisibilityAnimation);
            }

            // Add the minus sign.
            shapeVisual.Shapes.Add(minusShape);
        }

        CompositionPathGeometry GetNumberStripGeometry()
        {
            if (_numberStripGeometry == null)
            {
                _numberStripGeometry = _c.CreatePathGeometry(new CompositionPath(GetNumberStripCanvaGeometry()));
            }
            return _numberStripGeometry;
        }

        static CanvasGeometry GetNumberStripCanvaGeometry()
        {
            if (s_numberStripCanvasGeometry == null)
            {
                s_numberStripCanvasGeometry = CreateText("9\n0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n0\nE");
            }
            return s_numberStripCanvasGeometry;
        }

        static CanvasGeometry CreateText(string text)
        {
            const float size = 10;

            var textLayout = new CanvasTextLayout(
                resourceCreator: CanvasDevice.GetSharedDevice(),
                textString: text,
                textFormat: s_textFormat,
                requestedWidth: size,
                requestedHeight: size);
            return CanvasGeometry.CreateText(textLayout);
        }

        // Returns the value of 10 raised to the given value, as a string that can be used in a Composition animation expression.
        static string Pow10(double value) => DoubleToString(Math.Pow(10, value));

        // Converts a floating point value to a string suitable for a Composition animation expression.
        static string DoubleToString(double value)
        {
            var fValue = (float)value;
            return Math.Floor(fValue) == fValue
                ? fValue.ToString("0")
                : fValue.ToString("0.0####################");
        }

        // This is used instead of a ValueTuple so that the app that the meter is used in
        // doesn't depend on the ValueTuple NuGet.
        readonly struct ObjectBinding
        {
            public readonly string Name;
            public readonly CompositionObject Object;
            internal ObjectBinding(string name, CompositionObject @object)
            {
                Name = name;
                Object = @object;
            }
        }
    }
}
