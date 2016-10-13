using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Helper class used to simplify document printing.
    /// Based on <see cref="https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/Printing/cs/PrintHelper.cs" />
    /// </summary>
    public class PrintHelper
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
        /// Event which is called after print preview pages are generated.
        /// </summary>
        public event Action<List<UIElement>> PreviewPagesCreated;

        /// <summary>
        /// Gets or sets the percent of app's margin width, content is set at 85% (0.85) of the area's width
        /// </summary>
        public double ApplicationContentMarginLeft { get; set; } = 0.075;

        /// <summary>
        /// Gets or sets the percent of app's margin height, content is set at 94% (0.94) of tha area's height
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
        private List<UIElement> _printPreviewPages;

        /// <summary>
        ///  A hidden canvas used to hold pages we wish to print
        /// </summary>
        private Canvas _printCanvas;

        private string _printTaskName;

        /// <summary>
        /// Gets the list of Framework element to print
        /// </summary>
        public List<FrameworkElement> ElementsToPrint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintHelper"/> class.
        /// </summary>
        public PrintHelper()
        {
            _printPreviewPages = new List<UIElement>();
            _printCanvas = new Canvas();

            ElementsToPrint = new List<FrameworkElement>();

            RegisterForPrinting();
        }

        /// <summary>
        /// This function registers the app for printing with Windows and sets up the necessary event handlers for the print process.
        /// </summary>
        public void RegisterForPrinting()
        {
            _printDocument = new PrintDocument();
            _printDocumentSource = _printDocument.DocumentSource;
            _printDocument.Paginate += CreatePrintPreviewPages;
            _printDocument.GetPreviewPage += GetPrintPreviewPage;
            _printDocument.AddPages += AddPrintPages;

            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;
        }

        /// <summary>
        /// This function unregisters the app for printing with Windows.
        /// </summary>
        public void UnregisterForPrinting()
        {
            if (_printDocument == null)
            {
                return;
            }

            _printDocument.Paginate -= CreatePrintPreviewPages;
            _printDocument.GetPreviewPage -= GetPrintPreviewPage;
            _printDocument.AddPages -= AddPrintPages;

            // Remove the handler for printing initialization.
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;

            _printCanvas.Children.Clear();
        }

        /// <summary>
        /// Start the print task.
        /// </summary>
        /// <param name="printTaskName">Name of the print task to use</param>
        public async void ShowPrintUIAsync(string printTaskName)
        {
            _printTaskName = printTaskName;
            await PrintManager.ShowPrintUIAsync();
        }

        /// <summary>
        /// Method that will generate print content
        /// </summary>
        /// <param name="page">The page to print</param>
        public void PreparePrintContent(Page page)
        {
            // Add the (newly created) page to the print canvas which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            _printCanvas.Children.Add(page);
            _printCanvas.InvalidateMeasure();
            _printCanvas.UpdateLayout();
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
                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    // Notify the user when the print operation fails.
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            OnPrintFailed?.Invoke();
                        });
                    }
                };

                sourceRequested.SetSource(_printDocumentSource);
            });
        }

        /// <summary>
        /// This is the event handler for PrintDocument.Paginate. It creates print preview pages for the app.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Paginate Event Arguments</param>
        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Clear the cache of preview pages
            _printPreviewPages.Clear();

            // Clear the print canvas of preview pages
            _printCanvas.Children.Clear();

            // Get the PrintTaskOptions
            PrintTaskOptions printingOptions = e.PrintTaskOptions;

            // Get the page description to determine how big the page is
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            foreach (var element in ElementsToPrint)
            {
                AddOnePrintPreviewPage(element, pageDescription);
            }

            if (PreviewPagesCreated != null)
            {
                PreviewPagesCreated?.Invoke(_printPreviewPages);
            }

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
        /// <param name="page">XAML element that is used to represent the "printing page"</param>
        /// <param name="printPageDescription">Printer's page description</param>
        private void AddOnePrintPreviewPage(FrameworkElement page, PrintPageDescription printPageDescription)
        {
            // Set "paper" width
            page.Width = printPageDescription.PageSize.Width;
            page.Height = printPageDescription.PageSize.Height;

            Grid printableArea = (Grid)page.FindName("PrintableArea");

            // Get the margins size
            // If the ImageableRect is smaller than the app provided margins use the ImageableRect
            double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * ApplicationContentMarginLeft * 2);
            double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * ApplicationContentMarginTop * 2);

            // Set-up "printable area" on the "paper"
            printableArea.Width = page.Width - marginWidth;
            printableArea.Height = page.Height - marginHeight;

            // Add the (newley created) page to the print canvas which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            _printCanvas.Children.Add(page);
            _printCanvas.InvalidateMeasure();
            _printCanvas.UpdateLayout();

            // Add the page to the page preview collection
            _printPreviewPages.Add(page);
        }
    }
}