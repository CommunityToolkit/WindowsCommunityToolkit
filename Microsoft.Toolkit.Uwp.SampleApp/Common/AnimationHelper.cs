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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class AnimationHelper
    {
        private static float _defaultShowAnimationDuration = 300;
        private static float _defaultHideAnimationDiration = 150;

        private static bool? _isImpicitHideShowSupported;

        public static bool IsImplicitHideShowSupported => (bool)(_isImpicitHideShowSupported ??
            (_isImpicitHideShowSupported = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4)));

        public static void SetTopLevelShowHideAnimation(FrameworkElement element)
        {
            if (!IsImplicitHideShowSupported)
            {
                return;
            }

            var compositor = ElementCompositionPreview.GetElementVisual(element).Compositor;
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);

            // var hideAnimationGroup = _compositor.CreateAnimationGroup();
            // hideAnimationGroup.Add(GetOpacityAnimation(0, _defaultHideAnimationDiration));
            // hideAnimationGroup.Add(GetYOffsetAnimation(-(float)element.Height, _defaultHideAnimationDiration));
            // ElementCompositionPreview.SetImplicitHideAnimation(element, hideAnimationGroup);
            var showAnimationGroup = compositor.CreateAnimationGroup();
            showAnimationGroup.Add(GetOpacityAnimation(compositor, 1, 0, _defaultShowAnimationDuration));
            showAnimationGroup.Add(GetYOffsetAnimation(compositor, 0, -(float)element.Height, _defaultShowAnimationDuration));

            ElementCompositionPreview.SetImplicitShowAnimation(element, showAnimationGroup);
        }

        public static void SetSecondLevelShowHideAnimation(FrameworkElement element)
        {
            if (!IsImplicitHideShowSupported)
            {
                return;
            }

            var compositor = ElementCompositionPreview.GetElementVisual(element).Compositor;

            // ElementCompositionPreview.SetImplicitHideAnimation(element, GetOpacityAnimation(0, 1, _defaultHideAnimationDiration));
            ElementCompositionPreview.SetImplicitShowAnimation(element, GetOpacityAnimation(compositor, 1, 0, 200, 200));
        }

        public static CompositionAnimation GetYOffsetAnimation(Compositor compositor, float y, float from, float duration, float delay = 0)
        {
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Target = "Offset.Y";
            animation.InsertKeyFrame(0, from);
            animation.InsertKeyFrame(1, y);
            animation.Duration = TimeSpan.FromMilliseconds(duration);
            animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            animation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;

            return animation;
        }

        public static CompositionAnimation GetOpacityAnimation(Compositor compositor, float opacity, float from, float duration, float delay = 0)
        {
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Target = "Opacity";
            animation.InsertKeyFrame(0, from);
            animation.InsertKeyFrame(1, opacity);
            animation.Duration = TimeSpan.FromMilliseconds(duration);
            animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            animation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;

            return animation;
        }
    }
}
