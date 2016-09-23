using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="ListViewBase"/> content element that allows
    /// it to invoke a <see cref="ICommand"/> when clicked
    /// </summary>
    public static class ListViewBaseExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding an <see cref="ICommand"/> instance to a <see cref="ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(ListViewBaseExtensions),
            new PropertyMetadata(null, OnCommandPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a command parameter to a <see cref="ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(ListViewBaseExtensions),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="ICommand"/> instance assocaited with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> from which to get the associated <see cref="ICommand"/> instance</param>
        /// <returns>The <see cref="ICommand"/> instance associated with the the <see cref="DependencyObject"/> or null</returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the <see cref="ICommand"/> instance assocaited with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> to associated the <see cref="ICommand"/> instance to</param>
        /// <param name="value">The <see cref="ICommand"/> instance to bind to the <see cref="DependencyObject"/></param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="CommandProperty"/> instance assocaited with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> from which to get the associated <see cref="CommandProperty"/> value</param>
        /// <returns>The <see cref="CommandProperty"/> value associated with the the <see cref="DependencyObject"/> or null</returns>
        public static object GetCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Sets the <see cref="CommandProperty"/> assocaited with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> to associated the <see cref="CommandProperty"/> instance to</param>
        /// <param name="value">The <see cref="object"/> to set the <see cref="CommandProperty"/> to</param>
        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ListViewBase listViewBase = sender as ListViewBase;

            if (listViewBase != null)
            {
                listViewBase.ItemClick -= OnItemClicked;

                ICommand command = args.NewValue as ICommand;

                if (command != null)
                {
                    listViewBase.ItemClick += OnItemClicked;
                }
            }
        }

        private static void OnItemClicked(object sender, ItemClickEventArgs args)
        {
            ListViewBase listViewBase = sender as ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            ICommand command = GetCommand(listViewBase);
            object parameter = GetCommandParameter(listViewBase);

            if (command != null)
            {
                command.Execute(parameter);
            }
        }
    }
}
