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

using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class MaskParser
    {
        internal static Mask Parse(JsonReader reader, LottieComposition composition)
        {
            Mask.MaskMode maskMode = Mask.MaskMode.MaskModeAdd;
            AnimatableShapeValue maskPath = null;
            AnimatableIntegerValue opacity = null;

            reader.BeginObject();
            while (reader.HasNext())
            {
                string mode = reader.NextName();
                switch (mode)
                {
                    case "mode":
                        switch (reader.NextString())
                        {
                            case "a":
                                maskMode = Mask.MaskMode.MaskModeAdd;
                                break;
                            case "s":
                                maskMode = Mask.MaskMode.MaskModeSubtract;
                                break;
                            case "i":
                                maskMode = Mask.MaskMode.MaskModeIntersect;
                                break;
                            default:
                                Debug.WriteLine($"Unknown mask mode {mode}. Defaulting to Add.", LottieLog.Tag);
                                maskMode = Mask.MaskMode.MaskModeAdd;
                                break;
                        }

                        break;
                    case "pt":
                        maskPath = AnimatableValueParser.ParseShapeData(reader, composition);
                        break;
                    case "o":
                        opacity = AnimatableValueParser.ParseInteger(reader, composition);
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            return new Mask(maskMode, maskPath, opacity);
        }
    }
}
