// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MarkdownParserPage : Page
    {
        public MarkdownParserViewmodel Viewmodel { get; }

        public MarkdownParserPage()
        {
            this.Viewmodel = new MarkdownParserViewmodel();
            this.InitializeComponent();
            MarkdownResult.Text = this.Viewmodel.Output;
            this.Viewmodel.PropertyChanged += this.Viewmodel_PropertyChanged;
        }

        private void Viewmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Viewmodel.Output))
            {
                MarkdownResult.Text = this.Viewmodel.Output;
            }
        }
    }

    public class MarkdownParserViewmodel : BindableBase
    {
        private string input;

        public string Input
        {
            get { return input; }
            set { Set(ref input, value); }
        }

        private string output;

        public string Output
        {
            get { return output; }
            private set { Set(ref output, value); }
        }

        private bool useTables = true;

        public bool UseTables
        {
            get { return useTables; }
            set { Set(ref useTables, value); }
        }

        public MarkdownParserViewmodel()
        {
            this.PropertyChanged += this.MarkdownParserViewmodel_PropertyChanged;
            this.Input = @"This is **Markdown**

| With | an | Table |
|-----:|:--:|:------|";
        }

        private void MarkdownParserViewmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Output))
            {
                return;
            }

            var document = ParseText(this.Input);

            var json = JsonConvert.SerializeObject(document, Formatting.Indented, new StringEnumConverter());
            Output = json;
        }

        private MarkdownDocument ParseText(string input)
        {
            var documentBuilder = new MarkdownDocument().GetBuilder();

            if (!this.UseTables)
            {
                documentBuilder.RemoveBlockParser<Parsers.Markdown.Blocks.TableBlock.Parser>();
            }

            var document = documentBuilder.Build();
            document.Parse(input ?? string.Empty);
            return document;
        }
    }
}