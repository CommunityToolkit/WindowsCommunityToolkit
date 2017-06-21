///---------------------------------------------------------------------------------------------------------------------
/// <copyright company="Microsoft">
///     Copyright (c) Microsoft Corporation.  All rights reserved.
/// </copyright>
///---------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    ///---------------------------------------------------------------------------------------------------------------------
    /// 
    /// class ExpressionValues.StartingValue
    ///    ToDo: Add description after docs written
    /// 
    ///---------------------------------------------------------------------------------------------------------------------

    // ExpressionValues is a static class instead of a namespace to improve intellisense discoverablity and consistency with the other helper classes.
    public static partial class ExpressionValues
    {        
        /// <summary> Refer to the value of the property this expression is connected to, sampled during the first frame of execution. </summary>
        public static class StartingValue
        {
            /// <summary> Create a reference to the starting value of the boolean property that this expression will be connected to. </summary>
            public static BooleanNode    CreateBooleanStartingValue()    { return ExpressionNode.CreateValueKeyword<BooleanNode>(ValueKeywordKind.StartingValue);    }
            
            /// <summary> Create a reference to the starting value of the float property that this expression will be connected to. </summary>
            public static ScalarNode     CreateScalarStartingValue()     { return ExpressionNode.CreateValueKeyword<ScalarNode>(ValueKeywordKind.StartingValue);     }
            
            /// <summary> Create a reference to the starting value of the Vector2 property that this expression will be connected to. </summary>
            public static Vector2Node    CreateVector2StartingValue()    { return ExpressionNode.CreateValueKeyword<Vector2Node>(ValueKeywordKind.StartingValue);    }
            
            /// <summary> Create a reference to the starting value of the Vector3 property that this expression will be connected to. </summary>
            public static Vector3Node    CreateVector3StartingValue()    { return ExpressionNode.CreateValueKeyword<Vector3Node>(ValueKeywordKind.StartingValue);    }
            
            /// <summary> Create a reference to the starting value of the Vector4 property that this expression will be connected to. </summary>
            public static Vector4Node    CreateVector4StartingValue()    { return ExpressionNode.CreateValueKeyword<Vector4Node>(ValueKeywordKind.StartingValue);    }
            
            /// <summary> Create a reference to the starting value of the Color property that this expression will be connected to. </summary>
            public static ColorNode      CreateColorStartingValue()      { return ExpressionNode.CreateValueKeyword<ColorNode>(ValueKeywordKind.StartingValue);      }
            
            /// <summary> Create a reference to the starting value of the Quaternion property that this expression will be connected to. </summary>
            public static QuaternionNode CreateQuaternionStartingValue() { return ExpressionNode.CreateValueKeyword<QuaternionNode>(ValueKeywordKind.StartingValue); }
            
            /// <summary> Create a reference to the starting value of the Matrix3x2 property that this expression will be connected to. </summary>
            public static Matrix3x2Node  CreateMatrix3x2StartingValue()  { return ExpressionNode.CreateValueKeyword<Matrix3x2Node>(ValueKeywordKind.StartingValue);  }
            
            /// <summary> Create a reference to the starting value of the Matrix4x4 property that this expression will be connected to. </summary>
            public static Matrix4x4Node  CreateMatrix4x4StartingValue()  { return ExpressionNode.CreateValueKeyword<Matrix4x4Node>(ValueKeywordKind.StartingValue);  }
        }
    }
}