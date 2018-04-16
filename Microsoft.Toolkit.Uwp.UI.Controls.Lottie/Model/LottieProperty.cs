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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    /// <summary>
    /// Property values are the same type as the generic type of their corresponding
    /// <see cref="Value.ILottieValueCallback{T}"/>. With this, we can use generics to maintain type safety
    /// of the callbacks.
    ///
    /// Supported properties:
    /// Transform:
    ///    <see cref="TransformAnchorPoint"/>
    ///    <see cref="TransformPosition"/>
    ///    <see cref="TransformOpacity"/>
    ///    <see cref="TransformScale"/>
    ///    <see cref="TransformRotation"/>
    ///
    /// Fill:
    ///    <see cref="Color"/> (non-gradient)
    ///    <see cref="Opacity"/>
    ///    <see cref="Lottie.ColorFilter"/>
    ///
    /// Stroke:
    ///    <see cref="Color"/> (non-gradient)
    ///    <see cref="StrokeWidth"/>
    ///    <see cref="Opacity"/>
    ///    <see cref="Lottie.ColorFilter"/>
    ///
    /// Ellipse:
    ///    <see cref="Position"/>
    ///    <see cref="EllipseSize"/>
    ///
    /// Polystar:
    ///    <see cref="PolystarPoints"/>
    ///    <see cref="PolystarRotation"/>
    ///    <see cref="Position"/>
    ///    <see cref="PolystarInnerRadius"/> (star)
    ///    <see cref="PolystarOuterRadius"/>
    ///    <see cref="PolystarInnerRoundedness"/> (star)
    ///    <see cref="PolystarOuterRoundedness"/>
    ///
    /// Repeater:
    ///    All transform properties
    ///    <see cref="RepeaterCopies"/>
    ///    <see cref="RepeaterOffset"/>
    ///    <see cref="TransformRotation"/>
    ///    <see cref="TransformStartOpacity"/>
    ///    <see cref="TransformEndOpacity"/>
    ///
    /// Layers:
    ///    All transform properties
    ///    <see cref="TimeRemap"/> (composition layers only)
    /// </summary>
    public enum LottieProperty
    {
        /// <summary>
        /// ColorInt
        /// </summary>
        Color,

        /// <summary>
        /// StrokeColor
        /// </summary>
        StrokeColor,

        /// <summary>
        /// Opacity value are 0-100 to match after effects
        /// </summary>
        TransformOpacity,

        /// <summary>
        /// [0,100]
        /// </summary>
        Opacity,

        /// <summary>
        /// In Px
        /// </summary>
        TransformAnchorPoint,

        /// <summary>
        /// In Px
        /// </summary>
        TransformPosition,

        /// <summary>
        /// In Px
        /// </summary>
        EllipseSize,

        /// <summary>
        /// In Px
        /// </summary>
        Position,

        /// <summary>
        /// 1.0 = 100%
        /// </summary>
        TransformScale,

        /// <summary>
        /// In degrees
        /// </summary>
        TransformRotation,

        /// <summary>
        /// In Px
        /// </summary>
        StrokeWidth,

        /// <summary>
        /// Offset added to document text tracking
        /// </summary>
        TextTracking,

        /// <summary>
        /// Number of copies
        /// </summary>
        RepeaterCopies,

        /// <summary>
        /// Offset of between copy
        /// </summary>
        RepeaterOffset,

        /// <summary>
        /// Number of points in the polystar
        /// </summary>
        PolystarPoints,

        /// <summary>
        /// In degrees
        /// </summary>
        PolystarRotation,

        /// <summary>
        /// In Px
        /// </summary>
        PolystarInnerRadius,

        /// <summary>
        /// In Px
        /// </summary>
        PolystarOuterRadius,

        /// <summary>
        /// [0,100]
        /// </summary>
        PolystarInnerRoundedness,

        /// <summary>
        /// [0,100]
        /// </summary>
        PolystarOuterRoundedness,

        /// <summary>
        /// [0,100]
        /// </summary>
        TransformStartOpacity,

        /// <summary>
        /// [0,100]
        /// </summary>
        TransformEndOpacity,

        /// <summary>
        /// The time value in seconds
        /// </summary>
        TimeRemap,

        /// <summary>
        /// A <see cref="Lottie.ColorFilter"/> implementation
        /// </summary>
        ColorFilter
    }
}