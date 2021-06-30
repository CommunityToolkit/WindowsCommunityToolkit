//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
//See LICENSE in the project root for license information. 

using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Text;
using Windows.Storage;
using Windows.System;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    [Bindable]
    public sealed partial class GazeKeyboard : UserControl
    {
        private InputInjector _injector;
        private List<ButtonBase> _keyboardButtons;
        private KeyboardPage _rootPage;
        private TextPredictionGenerator _textPredictionGenerator;
        private WordsSegmenter _wordsSegmenter;
        private string _predictionLanguage;
        private Button[] _predictionTargets;

        public bool GazePlusClickMode;

        public TextBox Target;

        public string PredictionLanguage
        {
            get
            {
                return _predictionLanguage;
            }

            set
            {
                _predictionLanguage = value;
                _textPredictionGenerator = new TextPredictionGenerator(value);
                _textPredictionGenerator.InputScope = Windows.UI.Text.Core.CoreTextInputScope.Text;
                _wordsSegmenter = new WordsSegmenter(value);
            }
        }

        public Button[] PredictionTargets
        {
            get
            {
                return _predictionTargets;
            }

            set
            {
                if (_predictionTargets != null)
                {
                    foreach (var target in _predictionTargets)
                    {
                        target.Click -= OnPredictionSelected;
                    }
                }
                _predictionTargets = value;
                foreach (var target in _predictionTargets)
                {
                    target.Click += OnPredictionSelected;
                }

            }
        }

        public GazeKeyboard()
        {
            InitializeComponent();
            PredictionLanguage = "en-US";
            _injector = InputInjector.TryCreate();
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
          where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if (current.GetType().Equals(typeof(T)) || current.GetType().GetTypeInfo().IsSubclassOf(typeof(T)))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }

                FindChildren<T>(results, current);
            }
        }

        private void BuildPageHierarchy(KeyboardPage parent)
        {
            var children = GazeKeyboard.GetPageList(parent.Page);
            foreach (var childName in children)
            {
                var node = parent.Page.FindName(childName) as FrameworkElement;
                if (node != null)
                {
                    var childPage = new KeyboardPage(node, parent);
                    parent.ChildrenNames.Add(childName);
                    parent.Children.Add(childPage);
                    BuildPageHierarchy(childPage);
                }
            }

            if (parent.Children.Count > 0)
            {
                parent.CurrentChild = parent.Children[0];
            }
        }

        public async Task LoadLayout(string layoutFile)
        {
            try
            {
                var uri = new Uri($"ms-appx:///Microsoft.Toolkit.Uwp.Input.GazeControls/KeyboardLayouts/{layoutFile}");
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var xaml = await FileIO.ReadTextAsync(storageFile);
                var xamlNode = XamlReader.Load(xaml) as FrameworkElement;

                _keyboardButtons = new List<ButtonBase>();
                FindChildren<ButtonBase>(_keyboardButtons, xamlNode);

                foreach (var button in _keyboardButtons)
                {
                    button.Click += OnKeyboardButtonClick;
                }

                _rootPage = new KeyboardPage(xamlNode, null);
                BuildPageHierarchy(_rootPage);

                LayoutRoot.Children.Add(xamlNode);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private bool HandleVirtualKey(int vk)
        {
            if (vk < 0)
            {
                return false;
            }

            var key = new InjectedInputKeyboardInfo();
            key.VirtualKey = (ushort)vk;
            _injector.InjectKeyboardInput(new[] { key });

            UpdatePredictions();
            return true;
        }

        private bool HandleVirtualKeyList(List<int> vkList)
        {
            var state = new Dictionary<int, bool>();
            var keys = new List<InjectedInputKeyboardInfo>();
            foreach (var vk in vkList)
            {
                var key = new InjectedInputKeyboardInfo();
                key.VirtualKey = (ushort)vk;
                if (state.ContainsKey(vk))
                {
                    key.KeyOptions = InjectedInputKeyOptions.KeyUp;
                    state.Remove(vk);
                }
                else
                {
                    state.Add(vk, true);
                }

                keys.Add(key);
            }

            _injector.InjectKeyboardInput(keys);

            UpdatePredictions();
            return true;
        }

        private bool HandleUnicodeChar(string unicode)
        {
            var curContent = Clipboard.GetContent();
            Clipboard.Clear();
            var dp = new DataPackage();
            dp.SetText(unicode);
            Clipboard.SetContent(dp);

            var keyList = new List<int>();
            keyList.Add((int)VirtualKey.Control);
            keyList.Add((int)VirtualKey.V);
            keyList.Add((int)VirtualKey.V);
            keyList.Add((int)VirtualKey.Control);

            return HandleVirtualKeyList(keyList);
        }

        private KeyboardPage FindContainer(KeyboardPage kbdPage, string containerName)
        {
            if (kbdPage.Page.Name == containerName)
            {
                return kbdPage;
            }

            KeyboardPage containerPage = null;
            for (int i = 0; i < kbdPage.Children.Count; i++)
            {
                containerPage = FindContainer(kbdPage.Children[i], containerName);
                if (containerPage != null)
                {
                    return containerPage;
                }
            }

            return null;
        }

        private void HandlePageChange(string containerName, ButtonBase sender)
        {
            var container = FindContainer(_rootPage, containerName);

            var tempPage = GazeKeyboard.GetTemporaryPage(sender);
            var newPage = GazeKeyboard.GetNewPage(sender);
            string pageName;

            pageName = (tempPage != null) ? tempPage : newPage;

            // find the new page among its siblings
            var children = container.Children;
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Page.Name == pageName)
                {
                    // hide the current page
                    container.CurrentChild.Page.Visibility = Visibility.Collapsed;
                    container.Children[i].Page.Visibility = Visibility.Visible;
                    if (tempPage != null)
                    {
                        container.PrevChild = container.CurrentChild;
                    }

                    container.CurrentChild = container.Children[i];
                }
            }
        }

        private void RevertTempPage(ButtonBase button)
        {
            // This logic is based on the assumption that the button is placed in a grid or other container
            // which is the parent page. This is the temp page that has to be reverted back. So we navigate
            // to the grandparent of that page to identify the siblings of the parent page
            var page = button.Parent as FrameworkElement;
            if (page == null)
            {
                return;
            }

            var container = page.Parent as FrameworkElement;
            if (container == null)
            {
                return;
            }

            var containerPage = FindContainer(_rootPage, container.Name);
            if ((containerPage == null) || (containerPage.PrevChild == null))
            {
                return;
            }

            containerPage.CurrentChild.Page.Visibility = Visibility.Collapsed;
            containerPage.PrevChild.Page.Visibility = Visibility.Visible;
            containerPage.CurrentChild = containerPage.PrevChild;
            containerPage.PrevChild = null;
        }

        private async void OnKeyboardButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonBase;
            Target.Focus(FocusState.Programmatic);
            await Task.Delay(1);

            string unicode;
            string container;
            int vk;
            List<int> vkList;
            bool injected = false;

            if ((container = GazeKeyboard.GetPageContainer(button)) != null)
            {
                HandlePageChange(container, button);
            }
            else if ((vk = GazeKeyboard.GetVK(button)) != 0)
            {
                injected = HandleVirtualKey(vk);
            }
            else if (((vkList = GazeKeyboard.GetVKList(button)) != null) && (vkList.Count > 0))
            {
                injected = HandleVirtualKeyList(vkList);
            }
            else if ((unicode = GazeKeyboard.GetUnicode(button)) != null)
            {
                injected = HandleUnicodeChar(unicode);
            }
            else
            {
                var key = new InjectedInputKeyboardInfo();
                key.ScanCode = button.Content.ToString()[0];
                key.KeyOptions = InjectedInputKeyOptions.Unicode;
                _injector.InjectKeyboardInput(new[] { key });
                UpdatePredictions();
                injected = true;
            }

            if (injected)
            {
                RevertTempPage(button);
            }
        }

        private void InjectString(string str, bool addSpace)
        {
            if (addSpace)
            {
                str += " ";
            }

            var keys = new List<InjectedInputKeyboardInfo>();
            foreach (var ch in str)
            {
                var key = new InjectedInputKeyboardInfo()
                {
                    ScanCode = ch,
                    KeyOptions = InjectedInputKeyOptions.Unicode
                };
                keys.Add(key);

                key = new InjectedInputKeyboardInfo()
                {
                    ScanCode = ch,
                    KeyOptions = InjectedInputKeyOptions.Unicode | InjectedInputKeyOptions.KeyUp
                };

                keys.Add(key);
            }

            _injector.InjectKeyboardInput(keys);
        }

        private void OnPredictionSelected(object sender, RoutedEventArgs e)
        {
            WordSegment replaceSegment = null;
            if (Target.SelectionStart < Target.Text.Length)
            {
                replaceSegment = _wordsSegmenter.GetTokenAt(Target.Text, (uint)Target.SelectionStart);
            }
            else if (Target.Text[Target.Text.Length - 1] != ' ')
            {
                var tokens = _wordsSegmenter.GetTokens(Target.Text);
                if (tokens == null || tokens.Count == 0)
                {
                    return;
                }

                replaceSegment = tokens[tokens.Count - 1];
            }

            if (replaceSegment != null)
            {
                Target.Select((int)replaceSegment.SourceTextSegment.StartPosition, (int)replaceSegment.SourceTextSegment.Length);
            }

            string prediction = (sender as Button).Content.ToString();
            InjectString(prediction, true);
            UpdatePredictions();
        }

        private List<string> GetPrevWords()
        {
            var segments = _wordsSegmenter.GetTokens(Target.Text);
            if ((segments == null) || (segments.Count == 0))
            {
                return null;
            }

            int i = 0;
            var words = new List<string>(segments.Count);
            foreach (var segment in segments)
            {
                words.Add(segment.Text);
                i++;
            }

            words.Reverse();
            return words;
        }

        private async void UpdateNextWordPredictions()
        {
            var prevWords = GetPrevWords();
            var predictions = await _textPredictionGenerator.GetNextWordCandidatesAsync((uint)PredictionTargets.Length, prevWords);
            DisplayPredictions(predictions);
        }

        private async void UpdatePredictions()
        {
            // IMPORTANT: Wait for the text box to be updated with the injected keys
            await Task.Delay(1);

            if ((PredictionTargets == null) || (PredictionTargets.Length <= 0))
            {
                return;
            }

            var prevWords = GetPrevWords();

            if ((prevWords == null) || (prevWords.Count == 0))
            {
                return;
            }

            IReadOnlyList<string> predictions;
            var prevWordsExceptLast = prevWords.GetRange(1, prevWords.Count - 1);

            // It looks like we need to send in a larger number than necessary to get good quality predictions.

            uint maxCandidates = (uint)PredictionTargets.Length * 2;
            predictions = await _textPredictionGenerator.GetCandidatesAsync(
                            prevWords[0],
                            maxCandidates,
                            TextPredictionOptions.Corrections | TextPredictionOptions.Predictions,
                            prevWordsExceptLast);

            DisplayPredictions(predictions);
        }

        private void DisplayPredictions(IReadOnlyList<string> predictions)
        {
            int i;
            for (i = 0; (i < predictions.Count) && (i < PredictionTargets.Length); i++)
            {
                PredictionTargets[i].Content = predictions[i];
            }

            for (; i < PredictionTargets.Length; i++)
            {
                PredictionTargets[i].Content = string.Empty;
            }
        }
    }
}
