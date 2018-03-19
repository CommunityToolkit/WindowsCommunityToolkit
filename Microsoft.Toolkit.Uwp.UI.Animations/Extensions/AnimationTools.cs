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

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Internal tool to link composite transforms to elements
    /// </summary>
    internal class AnimationTools : DependencyObject
    {
        /// <summary>
        /// Attached property used to link composite transform with UIElement
        /// </summary>
        public static readonly DependencyProperty AnimationCompositeTransformIndexProperty = DependencyProperty.RegisterAttached(
            "AnimationCompositeTransformIndex",
            typeof(int),
            typeof(AnimationTools),
            new PropertyMetadata(-2));

        /// <summary>
        /// Attach a composite transform index to an UIElement.
        /// </summary>
        /// <param name="element">UIElement to use</param>
        /// <param name="value">Composite transform index</param>
        public static void SetAnimationCompositeTransformIndex(UIElement element, int value)
        {
            element.SetValue(AnimationCompositeTransformIndexProperty, value);
        }

        /// <summary>
        /// Get the composite transform index attached to an UIElement.
        /// </summary>
        /// <param name="element">UIElement to use</param>
        /// <returns>Composite transform index.</returns>
        public static int GetAnimationCompositeTransformIndex(UIElement element)
        {
            return (int)element.GetValue(AnimationCompositeTransformIndexProperty);
        }
    }
}
