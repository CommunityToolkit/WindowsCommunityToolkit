// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Toolkit.Uwp.UI.Lottie
{
    /// <summary>
    /// Diagnostics information about a Lottie and its translation.
    /// </summary>
    public sealed class LottieVisualDiagnostics
    {
        static readonly Issue[] s_emptyIssues = new Issue[0];
        static readonly KeyValuePair<string, double>[] s_emptyMarkers = new KeyValuePair<string, double>[0];

        public string FileName { get; internal set; } = "";

        public string SuggestedFileName => 
            string.IsNullOrWhiteSpace(FileName) 
                ? "MyComposition" 
                : Path.GetFileNameWithoutExtension(FileName);
            
        public string SuggestedClassName
        {
            get
            {
                string result = null;
                if (LottieComposition != null)
                {
                    result = InstantiatorGeneratorBase.TrySynthesizeClassName(LottieComposition.Name);
                }
                return result ?? InstantiatorGeneratorBase.TrySynthesizeClassName(SuggestedFileName);
            }
        }

        /// <summary>
        /// True if the Lottie is compatible with the current operating system.
        /// </summary>
        public bool IsCompatibleWithCurrentOS { get; internal set; }

        public TimeSpan Duration => LottieComposition?.Duration ?? TimeSpan.Zero;

        public TimeSpan ReadTime { get; internal set; }

        public TimeSpan ParseTime { get; internal set; }

        public TimeSpan ValidationTime { get; internal set; }

        public TimeSpan OptimizationTime { get; internal set; }

        public TimeSpan TranslationTime { get; internal set; }

        public TimeSpan InstantiationTime { get; internal set; }

        public IEnumerable<Issue> JsonParsingIssues { get; internal set; } = s_emptyIssues;

        public IEnumerable<Issue> LottieValidationIssues { get; internal set; } = s_emptyIssues;

        public IEnumerable<Issue> TranslationIssues { get; internal set; } = s_emptyIssues;

        public double LottieWidth => LottieComposition?.Width ?? 0;

        public double LottieHeight => LottieComposition?.Height ?? 0;

        public string LottieDetails => DescribeLottieComposition();
        public string LottieVersion => LottieComposition?.Version.ToString() ?? "";

        /// <summary>
        /// The options that were set on the <see cref="LottieVisualSource"/> when it 
        /// produced this diagnostics object.
        /// </summary>
        public LottieVisualOptions Options { get; internal set; }

        public string GenerateLottieXml()
        {
            if (LottieComposition == null) { return null; }
            return LottieData.Tools.LottieCompositionXmlSerializer.ToXml(LottieComposition).ToString();
        }

        public string GenerateWinCompXml()
        {
            return WinCompData.Tools.CompositionObjectXmlSerializer.ToXml(RootVisual).ToString();
        }

        public string GenerateCSharpCode()
        {
            if (LottieComposition == null) { return null; }

            return
                CSharpInstantiatorGenerator.CreateFactoryCode(
                    SuggestedClassName,
                    RootVisual,
                    (float)LottieComposition.Width,
                    (float)LottieComposition.Height,
                    LottieComposition.Duration);
        }

        public void GenerateCxCode(string headerFileName, out string cppText, out string hText)
        {
            if (LottieComposition == null) {
                cppText = null;
                hText = null;
                return;
            }

            CxInstantiatorGenerator.CreateFactoryCode(
                SuggestedClassName,
                RootVisual,
                (float)LottieComposition.Width,
                (float)LottieComposition.Height,
                LottieComposition.Duration,
                headerFileName,
                out cppText,
                out hText);
        }

        public KeyValuePair<string, double>[] Markers { get; internal set; } = s_emptyMarkers;

        // Holds the parsed LottieComposition. Only used if one of the codegen or XML options was selected.
        internal LottieComposition LottieComposition { get; set; }

        // Holds the translated Visual. Only used if one of the codgen or XML options was selected.
        internal WinCompData.Visual RootVisual { get; set; }

        internal LottieVisualDiagnostics Clone() =>
            new LottieVisualDiagnostics
            {
                FileName = FileName,
                InstantiationTime = InstantiationTime,
                JsonParsingIssues = JsonParsingIssues,
                LottieComposition = LottieComposition,
                LottieValidationIssues = LottieValidationIssues,
                Markers = Markers,
                Options = Options,
                ParseTime = ParseTime,
                ReadTime = ReadTime,
                RootVisual = RootVisual,
                TranslationTime = TranslationTime,
                ValidationTime = ValidationTime,
                TranslationIssues = TranslationIssues,
            };

        // Creates a string that describes the Lottie.
        string DescribeLottieComposition()
        {
            if (LottieComposition == null) { return null; }

            var stats = new LottieData.Tools.Stats(LottieComposition);

            return $"LottieVisualSource w={LottieComposition.Width} h={LottieComposition.Height} " +
                $"layers: precomp={stats.PreCompLayerCount} solid={stats.SolidLayerCount} " +
                $"image={stats.ImageLayerCount} null={stats.NullLayerCount} " +
                $"shape={stats.ShapeLayerCount} text={stats.TextLayerCount}";
        }
    }
}
