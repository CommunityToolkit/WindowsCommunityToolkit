using System;
using System.Threading;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Microsoft.Windows.Toolkit.UI.Services
{
    /// <summary>
    /// Defines a service for handling the use of the <see cref="MessageDialog"/> popup.
    /// </summary>
    public class DialogService : IDisposable
    {
        private static DialogService instance;

        /// <summary>
        /// Gets a static instance of the <see cref="DialogService"/>.
        /// </summary>
        public static DialogService Instance => instance ?? (instance = new DialogService());

        private SemaphoreSlim _dialogSemaphore = new SemaphoreSlim(1);

        /// <summary>
        /// Shows the message dialog with a given message.
        /// </summary>
        /// <param name="message">
        /// The message to show.
        /// </param>
        public void Show(string message)
        {
            this.ShowAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows the message dialog with a given title and message.
        /// </summary>
        /// <param name="title">
        /// The title of the dialog.
        /// </param>
        /// <param name="message">
        /// The message to show.
        /// </param>
        public void Show(string title, string message)
        {
            this.ShowAsync(title, message).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows the message dialog with a given message and buttons.
        /// </summary>
        /// <param name="message">
        /// The message to show.
        /// </param>
        /// <param name="commands">
        /// The buttons to show with corresponding commands.
        /// </param>
        public void Show(string message, params IUICommand[] commands)
        {
            this.ShowAsync(message, commands).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows the message dialog with a given title, message and buttons.
        /// </summary>
        /// <param name="title">
        /// The title of the dialog.
        /// </param>
        /// <param name="message">
        /// The message to show.
        /// </param>
        /// <param name="commands">
        /// The buttons to show with corresponding commands.
        /// </param>
        public void Show(string title, string message, params IUICommand[] commands)
        {
            this.ShowAsync(title, message, commands).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows the message dialog with a given message asynchronously.
        /// </summary>
        /// <param name="message">
        /// The message to show.
        /// </param>
        /// <returns>
        /// Returns an awaitable task.
        /// </returns>
        public async Task ShowAsync(string message)
        {
            await this.ShowAsync(null, message, null);
        }

        /// <summary>
        /// Shows the message dialog with a given title and message asynchronously.
        /// </summary>
        /// <param name="title">
        /// The title of the dialog.
        /// </param>
        /// <param name="message">
        /// The message to show.
        /// </param>
        /// <returns>
        /// Returns an awaitable task.
        /// </returns>
        public async Task ShowAsync(string title, string message)
        {
            await this.ShowAsync(title, message, null);
        }

        /// <summary>
        /// Shows the message dialog with a given message and buttons asynchronously.
        /// </summary>
        /// <param name="message">
        /// The message to show.
        /// </param>
        /// <param name="commands">
        /// The buttons to show with corresponding commands.
        /// </param>
        /// <returns>
        /// Returns an awaitable task.
        /// </returns>
        public async Task ShowAsync(string message, params IUICommand[] commands)
        {
            await this.ShowAsync(null, message, commands);
        }

        /// <summary>
        /// Shows the message dialog with a given title, message and buttons asynchronously.
        /// </summary>
        /// <param name="title">
        /// The title of the dialog.
        /// </param>
        /// <param name="message">
        /// The message to show.
        /// </param>
        /// <param name="commands">
        /// The buttons to show with corresponding commands.
        /// </param>
        /// <returns>
        /// Returns an awaitable task.
        /// </returns>
        public async Task ShowAsync(string title, string message, params IUICommand[] commands)
        {
            var tcs = new TaskCompletionSource<bool>();

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                    {
                        await this._dialogSemaphore.WaitAsync();

                        var dialog = new MessageDialog(message)
                                         {
                                             Title =
                                                 string.IsNullOrWhiteSpace(title)
                                                     ? "Alert!"
                                                     : title
                                         };

                        if (commands != null)
                        {
                            foreach (var command in commands)
                            {
                                dialog.Commands.Add(command);
                            }
                        }

                        await dialog.ShowAsync();
                        this._dialogSemaphore.Release();
                        tcs.SetResult(true);
                    });

            await tcs.Task;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">
        /// A boolean value indicating whether the control is disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this._dialogSemaphore == null)
            {
                return;
            }

            this._dialogSemaphore.Dispose();
            this._dialogSemaphore = null;
        }
    }
}