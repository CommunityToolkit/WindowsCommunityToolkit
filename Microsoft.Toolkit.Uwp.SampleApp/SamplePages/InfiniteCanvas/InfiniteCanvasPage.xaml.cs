// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _infiniteCanvas = control.FindChildByName("canvas") as InfiniteCanvas;
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Export & Save", async (sender, args) =>
            {
                if (_infiniteCanvas != null)
                {
                    var savePicker = new FileSavePicker
                    {
                        SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                    };
                    savePicker.FileTypeChoices.Add("application/json", new List<string> { ".json" });
                    savePicker.SuggestedFileName = "Infinite Canvas Export";

                    var file = await savePicker.PickSaveFileAsync();
                    if (file != null)
                    {
                        var json = _infiniteCanvas.ExportAsJson();
                        CachedFileManager.DeferUpdates(file);
                        await FileIO.WriteTextAsync(file, json);
                    }
                }
            });

            SampleController.Current.RegisterNewCommand("Import and Load", async (sender, args) =>
            {
                if (_infiniteCanvas != null)
                {
                    var picker = new FileOpenPicker
                    {
                        ViewMode = PickerViewMode.Thumbnail,
                        SuggestedStartLocation = PickerLocationId.DocumentsLibrary
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

            SampleController.Current.RegisterNewCommand("Export max view as image", async (sender, args) =>
            {
                if (_infiniteCanvas != null)
                {
                    var savePicker = new FileSavePicker
                    {
                        SuggestedStartLocation = PickerLocationId.Desktop
                    };
                    savePicker.FileTypeChoices.Add("image/png", new List<string> { ".png" });
                    savePicker.SuggestedFileName = "Infinite Canvas Max View";

                    var file = await savePicker.PickSaveFileAsync();

                    if (file != null)
                    {
                        CachedFileManager.DeferUpdates(file);
                        using (var stream = new InMemoryRandomAccessStream())
                        using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            var offScreen = _infiniteCanvas.ExportMaxOffScreenDrawings();
                            await offScreen.SaveAsync(stream, CanvasBitmapFileFormat.Png);
                            await RandomAccessStream.CopyAndCloseAsync(stream.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                        }
                    }
                }
            });

            SampleController.Current.RegisterNewCommand("Export current view as image", async (sender, args) =>
            {
                if (_infiniteCanvas != null)
                {
                    var savePicker = new FileSavePicker
                    {
                        SuggestedStartLocation = PickerLocationId.Desktop
                    };
                    savePicker.FileTypeChoices.Add("image/png", new List<string> { ".png" });
                    savePicker.SuggestedFileName = "Infinite Canvas Current View";

                    var file = await savePicker.PickSaveFileAsync();

                    if (file != null)
                    {
                        CachedFileManager.DeferUpdates(file);
                        using (var stream = new InMemoryRandomAccessStream())
                        using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            var offScreen = _infiniteCanvas.ExportCurrentViewOffScreenDrawings();
                            await offScreen.SaveAsync(stream, CanvasBitmapFileFormat.Png);
                            await RandomAccessStream.CopyAndCloseAsync(stream.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                        }
                    }
                }
            });
        }
    }
}
