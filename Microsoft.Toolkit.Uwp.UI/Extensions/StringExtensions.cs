// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
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
                    vector = new Vector2(float.Parse(values[0], CultureInfo.InvariantCulture));
                }
                else if (count == 2)
                {
                    vector = new Vector2(float.Parse(values[0], CultureInfo.InvariantCulture), float.Parse(values[1], CultureInfo.InvariantCulture));
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
                    vector = new Vector3(float.Parse(values[0], CultureInfo.InvariantCulture));
                }
                else if (count == 3)
                {
                    vector = new Vector3(
                        float.Parse(values[0], CultureInfo.InvariantCulture),
                        float.Parse(values[1], CultureInfo.InvariantCulture),
                        float.Parse(values[2], CultureInfo.InvariantCulture));
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
                    vector = new Vector4(float.Parse(values[0], CultureInfo.InvariantCulture));
                }
                else if (count == 4)
                {
                    vector = new Vector4(
                        float.Parse(values[0], CultureInfo.InvariantCulture),
                        float.Parse(values[1], CultureInfo.InvariantCulture),
                        float.Parse(values[2], CultureInfo.InvariantCulture),
                        float.Parse(values[3], CultureInfo.InvariantCulture));
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
        /// Converts a <see cref="string"/> to <see cref="Quaternion"/>
        /// </summary>
        /// <param name="str">A string in the format of "float, float, float, float"</param>
        /// <returns><see cref="Quaternion"/></returns>
        public static unsafe Quaternion ToQuaternion(this string str)
        {
            Vector4 vector = str.ToVector4();

            return *(Quaternion*)&vector;
        }
    }
}
