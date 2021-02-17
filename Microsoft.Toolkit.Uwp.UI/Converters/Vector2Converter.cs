using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// Converter for converting string to <see cref="Vector2"/>
    /// </summary>
    public class Vector2Converter : TypeConverter
    {
        private const int MaxCount = 2;

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw GetConvertFromException(value);
            }

            if (value is string valStr)
            {
                var tokens = valStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (tokens?.Count() == MaxCount)
                {
                    var result = new float[MaxCount];
                    bool isValid = true;
                    for (int i = 0; i < MaxCount; i++)
                    {
                        if (!float.TryParse(tokens[i], out result[i]))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        return new Vector2(result[0], result[1]);
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != null && value is Vector2 instance)
            {
                if (destinationType == typeof(string))
                {
                    return $"{instance.X}, {instance.Y}";
                }
            }

            // Pass unhandled cases to base class (which will throw exceptions for null value or destinationType.)
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
