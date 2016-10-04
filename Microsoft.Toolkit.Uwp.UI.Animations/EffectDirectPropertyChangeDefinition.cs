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

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Defines an <see cref="EffectDirectPropertyChangeDefinition"/> which is used by
    /// <see cref="AnimationSet"/> to link effect property Changes to Visuals
    /// </summary>
    internal class EffectDirectPropertyChangeDefinition
    {
        /// <summary>
        /// Gets or sets <see cref="CompositionEffectBrush"/> that will be animated
        /// </summary>
        public CompositionEffectBrush EffectBrush { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="float"/> value for the property
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Gets or sets the property name that will be animated on the <see cref="CompositionEffectBrush"/>
        /// </summary>
        public string PropertyName { get; set; }
    }
}