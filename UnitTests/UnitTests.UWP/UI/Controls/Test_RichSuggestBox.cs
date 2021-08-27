// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Text;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_RichSuggestBox : VisualUITestBase
    {
        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        [DataRow("@Token1", "@Token2", "@Token3")]
        [DataRow("@Token1", "@Token2", "#Token3")]
        [DataRow("#Token1", "@Token2", "@Token3")]
        public async Task Test_RichSuggestBox_AddTokens(string tokenText1, string tokenText2, string tokenText3)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox() { Prefixes = "@#" };
                await SetTestContentAsync(rsb);
                var document = rsb.TextDocument;

                // Adding token 1
                await TestAddTokenAsync(rsb, tokenText1);

                Assert.AreEqual(1, rsb.Tokens.Count, "Token count is not 1 after committing 1 token.");

                var token1 = rsb.Tokens.Last();

                AssertToken(rsb, token1);
                var expectedStory = $"{token1} \r";
                document.GetText(TextGetOptions.None, out var actualStory);
                Assert.AreEqual(expectedStory, actualStory);

                // Adding token 2 with space between previous token
                await TestAddTokenAsync(rsb, tokenText2);

                Assert.AreEqual(2, rsb.Tokens.Count, "Token count is not 2 after committing 2 token.");

                var token2 = rsb.Tokens.Last();

                AssertToken(rsb, token2);
                expectedStory = $"{token1} {token2} \r";
                document.GetText(TextGetOptions.None, out actualStory);
                Assert.AreEqual(expectedStory, actualStory);

                // Adding token 3 without space between previous token
                rsb.TextDocument.Selection.Delete(TextRangeUnit.Character, -1);
                await TestAddTokenAsync(rsb, tokenText3);

                Assert.AreEqual(3, rsb.Tokens.Count, "Token count is not 3 after committing 3 token.");

                var token3 = rsb.Tokens.Last();

                AssertToken(rsb, token3);
                expectedStory = $"{token1} {token2}{token3} \r";
                document.GetText(TextGetOptions.None, out actualStory);
                Assert.AreEqual(expectedStory, actualStory);

                document.Selection.Delete(TextRangeUnit.Character, -1);
                Assert.AreEqual(3, rsb.Tokens.Count, "Token at the end of the document is not recognized.");
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        public async Task Test_RichSuggestBox_CustomizeToken()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox() { Prefixes = "@" };
                await SetTestContentAsync(rsb);
                var inputText = "@Placeholder";
                var expectedText = "@Token";

                rsb.SuggestionChosen += (rsb, e) =>
                {
                    e.DisplayText = expectedText.Substring(1);
                    var format = e.Format;
                    format.BackgroundColor = Windows.UI.Colors.Beige;
                    format.ForegroundColor = Windows.UI.Colors.Azure;
                    format.Bold = FormatEffect.On;
                    format.Italic = FormatEffect.On;
                    format.Size = 9;
                };

                await AddTokenAsync(rsb, inputText);

                Assert.AreEqual(1, rsb.Tokens.Count, "Token count is not 1 after committing 1 token.");

                var defaultFormat = rsb.TextDocument.GetDefaultCharacterFormat();
                var token = rsb.Tokens[0];
                var range = rsb.TextDocument.GetRange(token.RangeStart, token.RangeEnd);
                Assert.AreEqual(expectedText, token.DisplayText, "Unexpected token text.");
                Assert.AreEqual(range.Text, token.ToString());

                var prePad = range.GetClone();
                prePad.SetRange(range.StartPosition, range.StartPosition + 1);
                Assert.AreEqual(defaultFormat.BackgroundColor, prePad.CharacterFormat.BackgroundColor, "Unexpected background color for pre padding.");
                Assert.AreEqual(defaultFormat.ForegroundColor, prePad.CharacterFormat.ForegroundColor, "Unexpected foreground color for pre padding.");

                var postPad = range.GetClone();
                postPad.SetRange(range.EndPosition - 1, range.EndPosition);
                Assert.AreEqual(defaultFormat.BackgroundColor, postPad.CharacterFormat.BackgroundColor, "Unexpected background color for post padding.");
                Assert.AreEqual(defaultFormat.ForegroundColor, postPad.CharacterFormat.ForegroundColor, "Unexpected foreground color for post padding.");

                var hiddenText = $"HYPERLINK \"{token.Id}\"\u200B";
                range.SetRange(range.StartPosition + hiddenText.Length, range.EndPosition - 1);
                Assert.AreEqual(Windows.UI.Colors.Beige, range.CharacterFormat.BackgroundColor, "Unexpected token background color.");
                Assert.AreEqual(Windows.UI.Colors.Azure, range.CharacterFormat.ForegroundColor, "Unexpected token foreground color.");
                Assert.AreEqual(FormatEffect.On, range.CharacterFormat.Bold, "Token is expected to be bold.");
                Assert.AreEqual(FormatEffect.On, range.CharacterFormat.Italic, "Token is expected to be italic.");
                Assert.AreEqual(9, range.CharacterFormat.Size, "Unexpected token font size.");
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        [DataRow("@Token1", "@Token2")]
        [DataRow("@Token1", "#Token2")]
        [DataRow("#Token1", "@Token2")]
        public async Task Test_RichSuggestBox_DeleteTokens(string token1, string token2)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox() { Prefixes = "@#" };
                await SetTestContentAsync(rsb);
                var document = rsb.TextDocument;
                var selection = document.Selection;

                await AddTokenAsync(rsb, token1);
                await AddTokenAsync(rsb, token2);

                Assert.AreEqual(2, rsb.Tokens.Count, "Unexpected token count after adding.");

                // Delete token as a whole
                selection.Delete(TextRangeUnit.Character, -1);
                selection.Delete(TextRangeUnit.Link, -1);
                await Task.Delay(10);

                Assert.AreEqual(1, rsb.Tokens.Count, "Unexpected token count after deleting token 2");

                // Partially delete a token
                selection.Delete(TextRangeUnit.Character, -2);
                await Task.Delay(10);

                Assert.AreEqual(0, rsb.Tokens.Count, "Unexpected token count after deleting token 1");
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        public async Task Test_RichSuggestBox_ReplaceToken()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox() { Prefixes = "@" };
                await SetTestContentAsync(rsb);
                var document = rsb.TextDocument;
                var selection = document.Selection;

                await AddTokenAsync(rsb, "@Before");
                var tokenBefore = rsb.Tokens[0];
                AssertToken(rsb, tokenBefore);

                selection.Delete(TextRangeUnit.Character, -2);
                await Task.Delay(10);

                await AddTokenAsync(rsb, "@After");
                var tokenAfter = rsb.Tokens[0];
                AssertToken(rsb, tokenAfter);

                Assert.AreNotSame(tokenBefore, tokenAfter, "Token before and token after are the same.");
                Assert.AreNotEqual(tokenBefore.Id, tokenAfter.Id, "Token ID before and token ID after are the same.");
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        public async Task Test_RichSuggestBox_FormatReset()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox() { Prefixes = "@" };
                rsb.TokenBackground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Azure);
                await SetTestContentAsync(rsb);
                var document = rsb.TextDocument;
                var selection = document.Selection;
                var defaultFormat = document.GetDefaultCharacterFormat();

                await AddTokenAsync(rsb, "@Token1");
                selection.Delete(TextRangeUnit.Character, -1);
                var middlePosition = selection.StartPosition;
                await AddTokenAsync(rsb, "@Token2");
                selection.Delete(TextRangeUnit.Character, -1);

                await Task.Delay(10);
                selection.SetText(TextSetOptions.Unhide, "text");
                Assert.AreEqual(defaultFormat.BackgroundColor, selection.CharacterFormat.BackgroundColor, "Raw text have background color after a token.");

                selection.SetRange(middlePosition, middlePosition);
                await Task.Delay(10);
                selection.SetText(TextSetOptions.Unhide, "text");
                Assert.AreEqual(defaultFormat.BackgroundColor, selection.CharacterFormat.BackgroundColor, "Raw text have background color when sandwiched between 2 tokens.");

                selection.SetRange(0, 0);
                await Task.Delay(10);
                selection.SetText(TextSetOptions.Unhide, "text");
                Assert.AreEqual(defaultFormat.BackgroundColor, selection.CharacterFormat.BackgroundColor, "Raw text have background color when insert at beginning of the document.");
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        public async Task Test_RichSuggestBox_Clear()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox();
                await SetTestContentAsync(rsb);

                var document = rsb.TextDocument;
                var selection = document.Selection;
                selection.TypeText("before ");
                await AddTokenAsync(rsb, "@Token");
                selection.TypeText("after");
                document.GetText(TextGetOptions.NoHidden, out var text);

                Assert.AreEqual(1, rsb.Tokens.Count, "Unexpected tokens count before clear.");
                Assert.IsTrue(document.CanUndo(), "Document cannot undo before clear.");
                Assert.AreEqual("before \u200B@Token\u200B after", text);

                rsb.Clear();
                document.GetText(TextGetOptions.NoHidden, out text);

                Assert.AreEqual(0, rsb.Tokens.Count, "Unexpected tokens count after clear.");
                Assert.IsFalse(document.CanUndo(), "Document can undo after clear.");
                Assert.AreEqual(string.Empty, text);
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        public async Task Test_RichSuggestBox_ClearUndoRedoHistory()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox();
                await SetTestContentAsync(rsb);

                var document = rsb.TextDocument;
                var selection = document.Selection;
                selection.TypeText("before ");
                await AddTokenAsync(rsb, "@Token");
                selection.TypeText("after");
                document.GetText(TextGetOptions.NoHidden, out var text);

                Assert.AreEqual(1, rsb.Tokens.Count, "Unexpected tokens count before clear.");
                Assert.IsTrue(document.CanUndo(), "Document cannot undo before clear.");
                Assert.AreEqual("before \u200B@Token\u200B after", text);

                rsb.ClearUndoRedoSuggestionHistory();
                document.GetText(TextGetOptions.NoHidden, out text);

                Assert.AreEqual(1, rsb.Tokens.Count, "Unexpected tokens count after clear.");
                Assert.IsFalse(document.CanUndo(), "Document can undo after clear.");
                Assert.AreEqual("before \u200B@Token\u200B after", text);
            });
        }

        [TestCategory(nameof(RichSuggestBox))]
        [TestMethod]
        public async Task Test_RichSuggestBox_Load()
        {
            const string rtf = @"{\rtf1\fbidis\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}{\f1\fnil Segoe UI;}}
{\colortbl ;\red255\green255\blue255;\red0\green0\blue255;\red41\green150\blue204;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\tx720\cf1\f0\fs21\lang4105 Hello {{\field{\*\fldinst{HYPERLINK ""c3b58ee9-df54-4686-b295-f203a5d8809a""}}{\fldrslt{\ul\cf2\u8203?\cf3\highlight1 @Michael Hawker\cf1\highlight0\u8203?}}}}\f1\fs21  \f0 from {{\field{\*\fldinst{HYPERLINK ""1c6a71c3-f81f-4a27-8f17-50d64acd5b61""}}{\fldrslt{\ul\cf2\u8203?\cf3\highlight1 @Tung Huynh\cf1\highlight0\u8203?}}}}\f1\fs21\par
}
";
            var token1 = new RichSuggestToken(Guid.Parse("c3b58ee9-df54-4686-b295-f203a5d8809a"), "@Michael Hawker");
            var token2 = new RichSuggestToken(Guid.Parse("1c6a71c3-f81f-4a27-8f17-50d64acd5b61"), "@Tung Huynh");

            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rsb = new RichSuggestBox();
                await SetTestContentAsync(rsb);

                var document = rsb.TextDocument;
                var selection = document.Selection;
                selection.TypeText("before ");
                await AddTokenAsync(rsb, "@Token");
                selection.TypeText("after");

                rsb.Load(rtf, new[] { token1, token2 });
                await Task.Delay(10);
                document.GetText(TextGetOptions.NoHidden, out var text);

                Assert.AreEqual(2, rsb.Tokens.Count, "Unexpected tokens count after load.");
                Assert.AreEqual("Hello \u200b@Michael Hawker\u200b from \u200b@Tung Huynh\u200b\r", text, "Unexpected document text.");
                AssertToken(rsb, token1);
                AssertToken(rsb, token2);
            });
        }

        private static void AssertToken(RichSuggestBox rsb, RichSuggestToken token)
        {
            var document = rsb.TextDocument;
            var tokenRange = document.GetRange(token.RangeStart, token.RangeEnd);
            Assert.AreEqual(token.ToString(), tokenRange.Text);
            Assert.AreEqual($"\"{token.Id}\"", tokenRange.Link, "Unexpected link value.");
            Assert.AreEqual(LinkType.FriendlyLinkAddress, tokenRange.CharacterFormat.LinkType, "Unexpected link type.");
        }

        private static async Task TestAddTokenAsync(RichSuggestBox rsb, string tokenText)
        {
            bool suggestionsRequestedCalled = false;
            bool suggestionChosenCalled = false;

            void SuggestionsRequestedHandler(RichSuggestBox sender, SuggestionRequestedEventArgs args)
            {
                suggestionsRequestedCalled = true;
                Assert.AreEqual(tokenText[0].ToString(), args.Prefix, $"Unexpected prefix in {nameof(RichSuggestBox.SuggestionRequested)}.");
                Assert.AreEqual(tokenText.Substring(1), args.QueryText, $"Unexpected query in {nameof(RichSuggestBox.SuggestionRequested)}.");
            }

            void SuggestionChosenHandler(RichSuggestBox sender, SuggestionChosenEventArgs args)
            {
                suggestionChosenCalled = true;
                Assert.AreEqual(tokenText[0].ToString(), args.Prefix, $"Unexpected prefix in {nameof(RichSuggestBox.SuggestionChosen)}.");
                Assert.AreEqual(tokenText.Substring(1), args.QueryText, $"Unexpected query in {nameof(RichSuggestBox.SuggestionChosen)}.");
                Assert.AreEqual(args.QueryText, args.DisplayText, $"Unexpected display text in {nameof(RichSuggestBox.SuggestionChosen)}.");
                Assert.AreSame(tokenText, args.SelectedItem, $"Selected item has unknown object {args.SelectedItem} in {nameof(RichSuggestBox.SuggestionChosen)}.");
            }

            rsb.SuggestionRequested += SuggestionsRequestedHandler;
            rsb.SuggestionChosen += SuggestionChosenHandler;

            await AddTokenAsync(rsb, tokenText);

            rsb.SuggestionRequested -= SuggestionsRequestedHandler;
            rsb.SuggestionChosen -= SuggestionChosenHandler;

            Assert.IsTrue(suggestionsRequestedCalled, $"{nameof(RichSuggestBox.SuggestionRequested)} was not invoked.");
            Assert.IsTrue(suggestionChosenCalled, $"{nameof(RichSuggestBox.SuggestionChosen)} was not invoked.");
        }

        private static async Task AddTokenAsync(RichSuggestBox rsb, string tokenText)
        {
            var selection = rsb.TextDocument.Selection;
            selection.TypeText(tokenText);
            await Task.Delay(10);   // Wait for SelectionChanged to be invoked
            await rsb.CommitSuggestionAsync(tokenText);
            await Task.Delay(10);   // Wait for TextChanged to be invoked
        }
    }
}
