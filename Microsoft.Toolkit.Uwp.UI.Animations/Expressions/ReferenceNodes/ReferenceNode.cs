namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    using System;
    using Windows.UI.Composition;

    ///---------------------------------------------------------------------------------------------------------------------
    /// 
    /// class ReferenceNode
    ///    ToDo: Add description after docs written
    /// 
    ///---------------------------------------------------------------------------------------------------------------------

    public abstract class ReferenceNode : ExpressionNode
    {
        internal ReferenceNode(string paramName, CompositionObject compObj = null)
        {
            _reference = compObj;
            _nodeType = ExpressionNodeType.Reference;
            _paramName = paramName;
        }


        //
        // Property set accessor functions
        //

        /// <summary> Create a reference to the specified boolean property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public BooleanNode    GetBooleanProperty(string propertyName)    { return ReferenceProperty<BooleanNode>(propertyName);    }

        /// <summary> Create a reference to the specified float property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public ScalarNode     GetScalarProperty(string propertyName)     { return ReferenceProperty<ScalarNode>(propertyName);     }

        /// <summary> Create a reference to the specified Vector2 property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public Vector2Node    GetVector2Property(string propertyName)    { return ReferenceProperty<Vector2Node>(propertyName);    }

        /// <summary> Create a reference to the specified Vector3 property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public Vector3Node    GetVector3Property(string propertyName)    { return ReferenceProperty<Vector3Node>(propertyName);    }

        /// <summary> Create a reference to the specified Vector4 property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public Vector4Node    GetVector4Property(string propertyName)    { return ReferenceProperty<Vector4Node>(propertyName);    }

        /// <summary> Create a reference to the specified Color property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public ColorNode      GetColorProperty(string propertyName)      { return ReferenceProperty<ColorNode>(propertyName);      }

        /// <summary> Create a reference to the specified Quaternion property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public QuaternionNode GetQuaternionProperty(string propertyName) { return ReferenceProperty<QuaternionNode>(propertyName); }

        /// <summary> Create a reference to the specified Matrix3x2 property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public Matrix3x2Node  GetMatrix3x2Property(string propertyName)  { return ReferenceProperty<Matrix3x2Node>(propertyName);  }

        /// <summary> Create a reference to the specified Matrix4x4 property. This maybe be a property on the CompositionObject directly, or on the its PropertySet. </summary>
        /// <param name="propertyName">The name of the property to reference.</param>
        public Matrix4x4Node  GetMatrix4x4Property(string propertyName)  { return ReferenceProperty<Matrix4x4Node>(propertyName);  }

        
        //
        // Helper functions
        //
        
        internal protected T ReferenceProperty<T>(string propertyName) where T : class
        {
            T newNode = ExpressionNode.CreateExpressionNode<T>();

            (newNode as ExpressionNode)._nodeType = ExpressionNodeType.ReferenceProperty;
            (newNode as ExpressionNode)._children.Add(this);
            (newNode as ExpressionNode)._propertyName = propertyName;

            return newNode;
        }

        internal string GetReferenceParamString()
        {
            if (_nodeType == ExpressionNodeType.TargetReference)
            {
                return "this.target";
            }
            else
            {
                return _paramName;
            }
        }
        
        internal protected override string GetValue()
        {
            throw new NotImplementedException("GetValue is not implemented for ReferenceNode and shouldn't be called");
        }

        public CompositionObject Reference { get { return _reference; } }
        
        
        //
        // Data
        //
        
        private CompositionObject _reference;
    }
}