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

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    internal class Matrix3X3
    {
        public float M11 { get; internal set; }

        public float M12 { get; internal set; }

        public float M13 { get; internal set; }

        public float M21 { get; internal set; }

        public float M22 { get; internal set; }

        public float M23 { get; internal set; }

        public float M31 { get; internal set; }

        public float M32 { get; internal set; }

        public float M33 { get; internal set; }

        public static Matrix3X3 CreateIdentity() => new Matrix3X3
        {
            M11 = 1,
            M12 = 0,
            M13 = 0,
            M21 = 0,
            M22 = 1,
            M23 = 0,
            M31 = 0,
            M32 = 0,
            M33 = 1
        };

        public void Set(Matrix3X3 m)
        {
            M11 = m.M11;
            M12 = m.M12;
            M13 = m.M13;
            M21 = m.M21;
            M22 = m.M22;
            M23 = m.M23;
            M31 = m.M31;
            M32 = m.M32;
            M33 = m.M33;
        }

        public void Reset()
        {
            M11 = 1;
            M12 = 0;
            M13 = 0;
            M21 = 0;
            M22 = 1;
            M23 = 0;
            M31 = 0;
            M32 = 0;
            M33 = 1;
        }

        public static Matrix3X3 operator *(Matrix3X3 m1, Matrix3X3 m2)
        {
            return new Matrix3X3
            {
                M11 = (m1.M11 * m2.M11) + (m1.M12 * m2.M21) + (m1.M13 * m2.M31),
                M12 = (m1.M11 * m2.M12) + (m1.M12 * m2.M22) + (m1.M13 * m2.M32),
                M13 = (m1.M11 * m2.M13) + (m1.M12 * m2.M23) + (m1.M13 * m2.M33),
                M21 = (m1.M21 * m2.M11) + (m1.M22 * m2.M21) + (m1.M23 * m2.M31),
                M22 = (m1.M21 * m2.M12) + (m1.M22 * m2.M22) + (m1.M23 * m2.M32),
                M23 = (m1.M21 * m2.M13) + (m1.M22 * m2.M23) + (m1.M23 * m2.M33),
                M31 = (m1.M31 * m2.M11) + (m1.M32 * m2.M21) + (m1.M33 * m2.M31),
                M32 = (m1.M31 * m2.M12) + (m1.M32 * m2.M22) + (m1.M33 * m2.M32),
                M33 = (m1.M31 * m2.M13) + (m1.M32 * m2.M23) + (m1.M33 * m2.M33)
            };
        }

        public Vector2 Transform(Vector2 v)
        {
            return new Vector2(
                (v.X * M11) + (v.Y * M12) + M13,
                (v.X * M21) + (v.Y * M22) + M23);
        }
    }
}
