// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Helper class used to simplify document printing.
    /// Based on https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/Printing/cs/PrintHelper.cs />
    /// It allows you to render a framework element per page.
    /// </summary>
    public class PrintHelper : IDisposable
    {
        /// <summary>
        /// Event raised when print was successful
        /// </summary>
        public event Action OnPrintSucceeded;

        /// <summary>
        /// Event raised when print failed
        /// </summary>
        public event Action OnPrintFailed;

        /// <summary>
        /// Event raised when print is cancelled by the user
        /// </summary>
        public event Action OnPrintCanceled;

        /// <summary>
        /// Event which is called after print preview pages are generated.
        /// </summary>
        /// <remarks>
        /// You can use this event to tweak the final rendering by adding/moving controls in the page.
        /// </remarks>
        public event Action<List<FrameworkElement>> OnPreviewPagesCreated;

        /// <summary>
        /// Gets or sets the percent of app's margin width
        /// </summary>
        public double ApplicationContentMarginLeft { get; set; } = 0.03;

        /// <summary>
        /// Gets or sets the percent of app's margin height
        /// </summary>
        public double ApplicationContentMarginTop { get; set; } = 0.03;

        /// <summary>
        /// PrintDocument is used to prepare the pages for printing.
        /// Prepare the pages to print in the handlers for the Paginate, GetPreviewPage, and AddPages events.
        /// </summary>
        private PrintDocument _printDocument;

        /// <summary>
        /// Marker interface for document source
        /// </summary>
        private IPrintDocumentSource _printDocumentSource;

        /// <summary>
        /// A list of UIElements used to store the print preview pages.  This gives easy access
        /// to any desired preview page.
        /// </summary>
        private List<FrameworkElement> _printPreviewPages;

        /// <summary>
        ///  A hidden canvas used to hold pages we wish to print
        /// </summary>
        private Canvas _printCanvas;
        private Panel _canvasContainer;
        private string _printTaskName;
        private Dictionary<FrameworkElement, PrintHelperStateBag> _stateBags = new Dictionary<FrameworkElement, PrintHelperStateBag>();
        private bool _directPrint = false;

        /// <summary>
        /// Gets the list of Framework element to print
        /// </summary>
        private List<FrameworkElement> _elementsToPrint;

        /// <summary>
        /// Gets the options for the print dialog
        /// </summary>
        private PrintHelperOptions _printHelperOptions;

        /// <summary>
        /// Gets the default options for the print dialog
        /// </summary>
        private PrintHelperOptions _defaultPrintHelperOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintHelper"/> class.
        /// </summary>
        /// <param name="canvasContainer">XAML panel used to attach printing canvas. Can be hidden in your UI with Opacity = 0 for instance</param>
        /// /// <param name="defaultPrintHelperOptions">Default settings for the print tasks</param>
        public PrintHelper(Panel canvasContainer, PrintHelperOptions defaultPrintHelperOptions = null)
        {
            if (canvasContainer == null)
            {
                throw new ArgumentNullException();
            }

            _printPreviewPages = new List<FrameworkElement>();
            _printCanvas = new Canvas();
            _printCanvas.Opacity = 0;

            _canvasContainer = canvasContainer;

            _elementsToPrint = new List<FrameworkElement>();

            _defaultPrintHelperOptions = defaultPrintHelperOptions ?? new PrintHelperOptions();

            RegisterForPrinting();
        }

        /// <summary>
        /// Add an element to the list of printable elements.
        /// </summary>
        /// <param name="element">Framework element to print</param>
        /// <remarks>The element cannot have a parent. He must not be included in any visual tree.</remarks>
        public void AddFrameworkElementToPrint(FrameworkElement element)
        {
            if (element.Parent != null)
            {
                throw new ArgumentException("Printable elements cannot have a parent.");
            }

            _elementsToPrint.Add(element);
        }

        /// <summary>
        /// Remove an element from the list of printable elements
        /// </summary>
        /// <param name="element">Framework element to remove</param>
        public void RemoveFrameworkElementToPrint(FrameworkElement element)
        {
            _elementsToPrint.Remove(element);
        }

        /// <summary>
        /// Empties the list of printable elements
        /// </summary>
        public void ClearListOfPrintableFrameworkElements()
        {
            _elementsToPrint.Clear();
        }

        /// <summary>
        /// Start the print task.
        /// </summary>
        /// <param name="printTaskName">Name of the print task to use</param>
        /// <param name="directPrint">Directly print the content of the container instead of relying on list built with AddFrameworkElementToPrint method</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        public async Task ShowPrintUIAsync(string printTaskName, bool directPrint = false)
        {
            this._directPrint = directPrint;

            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;

            // Launch print process
            _printTaskName = printTaskName;
            await PrintManager.ShowPrintUIAsync();
        }

        /// <summary>
        /// Start the print task.
        /// </summary>
        /// <param name="printTaskName">Name of the print task to use</param>
        /// <param name="printHelperOptions">Settings for the print task</param>
        /// <param name="directPrint">Directly print the content of the container instead of relying on list built with AddFrameworkElementToPrint method</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        public Task ShowPrintUIAsync(string printTaskName, PrintHelperOptions printHelperOptions, bool directPrint = false)
        {
            _printHelperOptions = printHelperOptions;

            return ShowPrintUIAsync(printTaskName, directPrint);
        }

        /// <summary>
        /// Release associated resources
        /// </summary>
        public void Dispose()
        {
            if (_printDocument == null)
            {
                return;
            }

            _printCanvas = null;

            _printDocument.Paginate -= CreatePrintPreviewPages;
            _printDocument.GetPreviewPage -= GetPrintPreviewPage;
            _printDocument.AddPages -= AddPrintPages;
        }

        /// <summary>
        /// This function registers the app for printing with Windows and sets up the necessary event handlers for the print process.
        /// </summary>
        private void RegisterForPrinting()
        {
            _printDocument = new PrintDocument();
            _printDocumentSource = _printDocument.DocumentSource;
            _printDocument.Paginate += CreatePrintPreviewPages;
            _printDocument.GetPreviewPage += GetPrintPreviewPage;
            _printDocument.AddPages += AddPrintPages;
        }

        private void DetachCanvas()
        {
            if (!_directPrint)
            {
                _canvasContainer.Children.Remove(_printCanvas);
                _printCanvas.Children.Clear();
            }

            _stateBags.Clear();

            // Clear the cache of preview pages
            ClearPageCache();

            // Remove the handler for printing initialization.
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;
        }

        /// <summary>
        /// This is the event handler for PrintManager.PrintTaskRequested.
        /// </summary>
        /// <param name="sender">PrintManager</param>
        /// <param name="e">PrintTaskRequestedEventArgs </param>
        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask(_printTaskName, sourceRequested =>
            {
                ApplyPrintSettings(printTask);

                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    // Notify the user when the print operation fails.
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        foreach (var element in _stateBags.Keys)
                        {
                            _stateBags[element].Restore(element);
                        }
                        _stateBags.Clear();

                        DetachCanvas();

                        switch (args.Completion)
                        {
                            case PrintTaskCompletion.Failed:
                                {
                                    OnPrintFailed?.Invoke();
                                    break;
                                }
                            case PrintTaskCompletion.Canceled:
                                {
                                    OnPrintCanceled?.Invoke();
                                    break;
                                }
                            case PrintTaskCompletion.Submitted:
                                {
                                    OnPrintSucceeded?.Invoke();
                                    break;
                                }
                        }
                    });
                };

                sourceRequested.SetSource(_printDocumentSource);
            });
        }

        private void ApplyPrintSettings(PrintTask printTask)
        {
            _printHelperOptions = _printHelperOptions ?? _defaultPrintHelperOptions;

            IEnumerable<string> displayedOptionsToAdd = _printHelperOptions.DisplayedOptions;

            if (!_printHelperOptions.ExtendDisplayedOptions)
            {
                printTask.Options.DisplayedOptions.Clear();
            }

            foreach (var displayedOption in displayedOptionsToAdd)
            {
                if (!printTask.Options.DisplayedOptions.Contains(displayedOption))
                {
                    printTask.Options.DisplayedOptions.Add(displayedOption);
                }
            }

            if (printTask.Options.Binding != PrintBinding.NotAvailable)
            {
                printTask.Options.Binding = _printHelperOptions.Binding == PrintBinding.Default ? printTask.Options.Binding : _printHelperOptions.Binding;
            }

            if (printTask.Options.Bordering != PrintBordering.NotAvailable)
            {
                printTask.Options.Bordering = _printHelperOptions.Bordering == PrintBordering.Default ? printTask.Options.Bordering : _printHelperOptions.Bordering;
            }

            if (printTask.Options.MediaType != PrintMediaType.NotAvailable)
            {
                printTask.Options.MediaType = _printHelperOptions.MediaType == PrintMediaType.Default ? printTask.Options.MediaType : _printHelperOptions.MediaType;
            }

            if (printTask.Options.MediaSize != PrintMediaSize.NotAvailable)
            {
                printTask.Options.MediaSize = _printHelperOptions.MediaSize == PrintMediaSize.Default ? printTask.Options.MediaSize : _printHelperOptions.MediaSize;
            }

            if (printTask.Options.HolePunch != PrintHolePunch.NotAvailable)
            {
                printTask.Options.HolePunch = _printHelperOptions.HolePunch == PrintHolePunch.Default ? printTask.Options.HolePunch : _printHelperOptions.HolePunch;
            }

            if (printTask.Options.Duplex != PrintDuplex.NotAvailable)
            {
                printTask.Options.Duplex = _printHelperOptions.Duplex == PrintDuplex.Default ? printTask.Options.Duplex : _printHelperOptions.Duplex;
            }

            if (printTask.Options.ColorMode != PrintColorMode.NotAvailable)
            {
                printTask.Options.ColorMode = _printHelperOptions.ColorMode == PrintColorMode.Default ? printTask.Options.ColorMode : _printHelperOptions.ColorMode;
            }

            if (printTask.Options.Collation != PrintCollation.NotAvailable)
            {
                printTask.Options.Collation = _printHelperOptions.Collation == PrintCollation.Default ? printTask.Options.Collation : _printHelperOptions.Collation;
            }

            if (printTask.Options.PrintQuality != PrintQuality.NotAvailable)
            {
                printTask.Options.PrintQuality = _printHelperOptions.PrintQuality == PrintQuality.Default ? printTask.Options.PrintQuality : _printHelperOptions.PrintQuality;
            }

            if (printTask.Options.Staple != PrintStaple.NotAvailable)
            {
                printTask.Options.Staple = _printHelperOptions.Staple == PrintStaple.Default ? printTask.Options.Staple : _printHelperOptions.Staple;
            }

            if (printTask.Options.Orientation != PrintOrientation.NotAvailable)
            {
                printTask.Options.Orientation = _printHelperOptions.Orientation == PrintOrientation.Default ? printTask.Options.Orientation : _printHelperOptions.Orientation;
            }

            _printHelperOptions = null;
        }

        /// <summary>
        /// This is the event handler for PrintDocument.Paginate. It creates print preview pages for the app.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Paginate Event Arguments</param>
        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Get the PrintTaskOptions
            PrintTaskOptions printingOptions = e.PrintTaskOptions;

            // Get the page description to determine how big the page is
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            if (_directPrint)
            {
                foreach (FrameworkElement element in this._canvasContainer.Children)
                {
                    _printPreviewPages.Add(element);
                }
            }
            else
            {
                // Attach the canvas
                if (!_canvasContainer.Children.Contains(_printCanvas))
                {
                    _canvasContainer.Children.Add(_printCanvas);
                }

                // Clear the cache of preview pages
                ClearPageCache();

                // Clear the print canvas of preview pages
                _printCanvas.Children.Clear();

                foreach (var element in _elementsToPrint)
                {
                    AddOnePrintPreviewPage(element, pageDescription);
                }
            }

            OnPreviewPagesCreated?.Invoke(_printPreviewPages);

            PrintDocument printDoc = (PrintDocument)sender;

            // Report the number of preview pages created
            printDoc.SetPreviewPageCount(_printPreviewPages.Count, PreviewPageCountType.Intermediate);
        }

        /// <summary>
        /// This is the event handler for PrintDocument.GetPrintPreviewPage. It provides a specific print preview page,
        /// in the form of an UIElement, to an instance of PrintDocument. PrintDocument subsequently converts the UIElement
        /// into a page that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Arguments containing the preview requested page</param>
        private void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, _printPreviewPages[e.PageNumber - 1]);
        }

        /// <summary>
        /// This is the event handler for PrintDocument.AddPages. It provides all pages to be printed, in the form of
        /// UIElements, to an instance of PrintDocument. PrintDocument subsequently converts the UIElements
        /// into a pages that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Add page event arguments containing a print task options reference</param>
        private void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            // Loop over all of the preview pages and add each one to  add each page to be printied
            for (int i = 0; i < _printPreviewPages.Count; i++)
            {
                // We should have all pages ready at this point...
                _printDocument.AddPage(_printPreviewPages[i]);
            }

            PrintDocument printDoc = (PrintDocument)sender;

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        /// <summary>
        /// This function creates and adds one print preview page to the internal cache of print preview
        /// pages stored in printPreviewPages.
        /// </summary>
        /// <param name="element">FrameworkElement that is used to represent the "printing page"</param>
        /// <param name="printPageDescription">Printer's page description</param>
        private void AddOnePrintPreviewPage(FrameworkElement element, PrintPageDescription printPageDescription)
        {
            var page = new Page();

            // Save state
            if (!_stateBags.ContainsKey(element))
            {
                var stateBag = new PrintHelperStateBag();
                stateBag.Capture(element);
                _stateBags.Add(element, stateBag);
            }

            // Set "paper" width
            page.Width = printPageDescription.PageSize.Width;
            page.Height = printPageDescription.PageSize.Height;

            // Get the margins size
            double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * ApplicationContentMarginLeft * 2);
            double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * ApplicationContentMarginTop * 2);

            // Set-up "printable area" on the "paper"
            element.VerticalAlignment = VerticalAlignment.Top;
            element.HorizontalAlignment = HorizontalAlignment.Left;

            if (element.Width > element.Height)
            {
                var newWidth = page.Width - marginWidth;

                element.Height = element.Height * (newWidth / element.Width);
                element.Width = newWidth;
            }
            else
            {
                var newHeight = page.Height - marginHeight;

                element.Width = element.Width * (newHeight / element.Height);
                element.Height = newHeight;
            }

            element.Margin = new Thickness(marginWidth / 2, marginHeight / 2, marginWidth / 2, marginHeight / 2);
            page.Content = element;

            // Add the (newly created) page to the print canvas which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            _printCanvas.Children.Add(page);
            _printCanvas.InvalidateMeasure();
            _printCanvas.UpdateLayout();

            // Add the page to the page preview collection
            _printPreviewPages.Add(page);
        }

        private void ClearPageCache()
        {
            if (!_directPrint)
            {
                foreach (Page page in _printPreviewPages)
                {
                    page.Content = null;
                }
            }

            _printPreviewPages.Clear();
        }
    }
}