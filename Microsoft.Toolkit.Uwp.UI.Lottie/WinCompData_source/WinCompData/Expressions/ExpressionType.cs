// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
    /// <summary>
    /// Describes the type of an <see cref="Expression"/> or variable.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class ExpressionType
    {
        public ExpressionType(TypeConstraint constraints)
        {
            Constraints = constraints;
        }

        /// <summary>
        /// The set of constraints on the type of the expression.
        /// </summary>
        public TypeConstraint Constraints { get; }

        /// <summary>
        /// The expression has a generic type if there is more than one valid type in the
        /// <see cref="Constraints"/> set.
        /// </summary>
        public bool IsGeneric
        {
            get
            {
                var validTypes = (int)(Constraints & TypeConstraint.AllValidTypes);
                // If there's more than 1 bit set the type is generic.
                while (validTypes != 0)
                {
                    var bitWasSet = (validTypes & 1) != 0;
                    validTypes >>= 1;
                    if (bitWasSet && (validTypes != 0))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// True if the expression can have at least one type.
        /// </summary>
        public bool IsValid => (Constraints | TypeConstraint.AllValidTypes) != TypeConstraint.NoType;

        public override string ToString()
        {
            if (!IsValid)
            {
                return "Invalid";
            }

            if (!IsGeneric)
            {
                return SingleTypeConstraintToString(Constraints);
            }
            if (Constraints == TypeConstraint.AllValidTypes)
            {
                return "Any";
            }
            var constraints = Constraints;
            var constraintList = new List<string>();
            var mask = 1;
            while (constraints != 0)
            {
                if ((constraints & (TypeConstraint)mask) != 0)
                {
                    constraintList.Add(SingleTypeConstraintToString(constraints & (TypeConstraint)mask));
                    constraints ^= (TypeConstraint)mask;
                }
                mask = mask << 1;
            }
            return string.Join("|", constraintList);
        }

        static string SingleTypeConstraintToString(TypeConstraint singleConstraint)
        {
            switch (singleConstraint)
            {
                case TypeConstraint.Boolean:
                    return "Boolean";
                case TypeConstraint.Scalar:
                    return "Scalar";
                case TypeConstraint.Vector2:
                    return "Vector2";
                case TypeConstraint.Vector3:
                    return "Vector3";
                case TypeConstraint.Vector4:
                    return "Vector4";
                case TypeConstraint.Matrix3x2:
                    return "Matrix3x2";
                case TypeConstraint.Matrix4x4:
                    return "Matrix4x4";
                case TypeConstraint.Quaternion:
                    return "Quaternion";
                case TypeConstraint.Color:
                    return "Color";
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static TypeConstraint IntersectConstraints(TypeConstraint a, TypeConstraint b)
        {
            return a & b;
        }

        internal static ExpressionType ConstrainToType(TypeConstraint allowedTypes, ExpressionType constrainer)
        {
            return new ExpressionType(IntersectConstraints(constrainer.Constraints, allowedTypes));
        }

        internal static ExpressionType ConstrainToTypes(TypeConstraint allowedTypes, ExpressionType constrainerA, ExpressionType constrainerB)
        {
            if (constrainerA.Constraints == TypeConstraint.NoType)
            {
                return ConstrainToType(allowedTypes, constrainerB);
            }

            if (constrainerB.Constraints == TypeConstraint.NoType)
            {
                return ConstrainToType(allowedTypes, constrainerA);
            }

            // Both constrainers have types. Constrain to the intersection of their constraints.
            return new ExpressionType(IntersectConstraints(allowedTypes, IntersectConstraints(constrainerA.Constraints, constrainerB.Constraints)));
        }

        internal static ExpressionType AssertMatchingTypes(TypeConstraint allowedTypes, ExpressionType constrainerA, ExpressionType constrainerB, TypeConstraint resultType)
        {
            var doTypesMatch = IntersectConstraints(allowedTypes, IntersectConstraints(constrainerA.Constraints, constrainerB.Constraints)) != TypeConstraint.NoType;
            return new ExpressionType(doTypesMatch ? resultType : TypeConstraint.NoType);
        }
    }
}
