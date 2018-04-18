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
using Windows.Storage;
using Windows.UI.Popups;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// InfinteCanvas sample page.
    /// </summary>
    public sealed partial class InfiniteCanvasPage : Page, IXamlRenderListener
    {
        private InfiniteCanvas _infiniteCanvas;

        public InfiniteCanvasPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _infiniteCanvas = control.FindChildByName("canvas") as InfiniteCanvas;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Export & Save", async (sender, args) =>
            {
                if (_infiniteCanvas != null)
                {
                    var savePicker = new Windows.Storage.Pickers.FileSavePicker
                    {
                        SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
                    };
                    savePicker.FileTypeChoices.Add("application/json", new List<string> { ".json" });
                    savePicker.SuggestedFileName = "Infinite Canvas Export";

                    StorageFile file = await savePicker.PickSaveFileAsync();
                    if (file != null)
                    {
                        var json = _infiniteCanvas.ExportAsJson();
                        CachedFileManager.DeferUpdates(file);
                        await FileIO.WriteTextAsync(file, json);
                    }
                }
            });

            Shell.Current.RegisterNewCommand("Import and Load", async (sender, args) =>
            {
                if (_infiniteCanvas != null)
                {
                    var picker = new Windows.Storage.Pickers.FileOpenPicker
                    {
                        ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                        SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
                    };
                    picker.FileTypeFilter.Add(".json");
                    var file = await picker.PickSingleFileAsync();

                    if (file != null)
                    {
                        try
                        {
                            var json = await FileIO.ReadTextAsync(file);
                            _infiniteCanvas.ImportFromJson(json);
                        }
                        catch
                        {
                            var dialog = new MessageDialog("Invalid File");
                            await dialog.ShowAsync();
                        }
                    }
                }
            });
        }
    }
}
