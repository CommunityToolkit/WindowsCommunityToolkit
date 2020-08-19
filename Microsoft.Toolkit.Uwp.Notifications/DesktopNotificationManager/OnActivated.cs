namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Event triggered when a notification is clicked.
    /// </summary>
    /// <param name="e">Arguments that specify what action was taken and the user inputs.</param>
    public delegate void OnActivated(DesktopNotificationActivatedEventArgs e);
}