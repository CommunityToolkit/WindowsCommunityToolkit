namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    ///---------------------------------------------------------------------------------------------------------------------
    /// 
    /// class ExpressionValues
    ///    ToDo: Add description after docs written
    /// 
    ///---------------------------------------------------------------------------------------------------------------------

    // ExpressionValues is a static class instead of a namespace to improve intellisense discoverablity and consistency with the other helper classes.
    public static partial class ExpressionValues
    {
        /// <summary> Create a reference to a CompositionObject. </summary>
        public static class Reference
        {
            /// <summary> Creates a named reference parameter to an AmbientLight. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static AmbientLightReferenceNode CreateAmbientLightReference(string parameterName)
            {
                return new AmbientLightReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a ColorBrush. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static ColorBrushReferenceNode CreateColorBrushReference(string parameterName)
            {
                return new ColorBrushReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a DistantLight. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static DistantLightReferenceNode CreateDistantLightReference(string parameterName)
            {
                return new DistantLightReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a DropShadow. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static DropShadowReferenceNode CreateDropShadowReference(string parameterName)
            {
                return new DropShadowReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to an InsetClip. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static InsetClipReferenceNode CreateInsetClipReference(string parameterName)
            {
                return new InsetClipReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to an InteractionTracker. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static InteractionTrackerReferenceNode CreateInteractionTrackerReference(string parameterName)
            {
                return new InteractionTrackerReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a NineGridBrush. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static NineGridBrushReferenceNode CreateNineGridBrushReference(string parameterName)
            {
                return new NineGridBrushReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a PointLight. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static PointLightReferenceNode CreatePointLightReference(string parameterName)
            {
                return new PointLightReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a PropertySet. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static PropertySetReferenceNode CreatePropertySetReference(string parameterName)
            {
                return new PropertySetReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a SpotLight. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static SpotLightReferenceNode CreateSpotLightReference(string parameterName)
            {
                return new SpotLightReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a SurfaceBrush. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static SurfaceBrushReferenceNode CreateSurfaceBrushReference(string parameterName)
            {
                return new SurfaceBrushReferenceNode(parameterName);
            }

            /// <summary> Creates a named reference parameter to a Visual. </summary>
            /// <param name="paramName">The name that will be used to refer to the parameter at a later time.</param>
            public static VisualReferenceNode CreateVisualReference(string parameterName)
            {
                return new VisualReferenceNode(parameterName);
            }
        }
    }
}