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

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
	internal class LottieAnimationViewMetadata : AttributeTableBuilder
	{
        public LottieAnimationViewMetadata()
			: base()
		{
			AddCallback(typeof(LottieAnimationView),
				b =>
				{   
					b.AddCustomAttributes(nameof(LottieAnimationView.FileName),
						new CategoryAttribute(Properties.Resources.CategoryCommon)
					);
                    b.AddCustomAttributes(nameof(LottieAnimationView.AutoPlay),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.RepeatCount),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.Progress),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.Scale),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.FrameRate),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.Speed),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.RepeatMode),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.MinFrame),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(LottieAnimationView.MaxFrame),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}
