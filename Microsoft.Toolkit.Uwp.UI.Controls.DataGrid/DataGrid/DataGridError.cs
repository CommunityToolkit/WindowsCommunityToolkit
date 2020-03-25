// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.DataGridInternals
{
    internal static class DataGridError
    {
        public static class DataGrid
        {
            public static InvalidOperationException CannotChangeItemsWhenLoadingRows()
            {
                return new InvalidOperationException("Items cannot be added, removed or reset while rows are loading or unloading.");
            }

            public static InvalidOperationException CannotChangeColumnCollectionWhileAdjustingDisplayIndexes()
            {
                return new InvalidOperationException("Column collection cannot be changed while adjusting display indexes.");
            }

            public static InvalidOperationException ColumnCannotBeCollapsed()
            {
                return new InvalidOperationException("Column cannot be collapsed.");
            }

            public static InvalidOperationException ColumnCannotBeReassignedToDifferentDataGrid()
            {
                return new InvalidOperationException("Column already belongs to a DataGrid instance and cannot be reassigned.");
            }

            public static ArgumentException ColumnNotInThisDataGrid()
            {
                return new ArgumentException("Provided column does not belong to this DataGrid.");
            }

            public static ArgumentException ItemIsNotContainedInTheItemsSource(string paramName)
            {
                return new ArgumentException("The item is not contained in the ItemsSource.", paramName);
            }

            public static InvalidOperationException NoCurrentRow()
            {
                return new InvalidOperationException("There is no current row. Operation cannot be completed.");
            }

            public static InvalidOperationException NoOwningGrid(Type type)
            {
                return new InvalidOperationException(Format("There is no instance of DataGrid assigned to this {0}. Operation cannot be completed.", type.FullName));
            }

            public static InvalidOperationException UnderlyingPropertyIsReadOnly(string paramName)
            {
                return new InvalidOperationException(Format("{0} cannot be set because the underlying property is read only.", paramName));
            }

            public static ArgumentException ValueCannotBeSetToInfinity(string paramName)
            {
                return new ArgumentException(Format("{0} cannot be set to infinity.", paramName));
            }

            public static ArgumentException ValueCannotBeSetToNAN(string paramName)
            {
                return new ArgumentException(Format("{0} cannot be set to double.NAN.", paramName));
            }

            public static ArgumentNullException ValueCannotBeSetToNull(string paramName, string valueName)
            {
                return new ArgumentNullException(paramName, Format("{0} cannot be set to a null value.", valueName));
            }

            public static ArgumentException ValueIsNotAnInstanceOf(string paramName, Type type)
            {
                return new ArgumentException(paramName, Format("The value is not an instance of {0}.", type.FullName));
            }

            public static ArgumentException ValueIsNotAnInstanceOfEitherOr(string paramName, Type type1, Type type2)
            {
                return new ArgumentException(paramName, Format("The value is not an instance of {0} or {1}.", type1.FullName, type2.FullName));
            }

            public static ArgumentOutOfRangeException ValueMustBeBetween(string paramName, string valueName, object lowValue, bool lowInclusive, object highValue, bool highInclusive)
            {
                string message;

                if (lowInclusive && highInclusive)
                {
                    message = "{0} must be greater than or equal to {1} and less than or equal to {2}.";
                }
                else if (lowInclusive && !highInclusive)
                {
                    message = "{0} must be greater than or equal to {1} and less than {2}.";
                }
                else if (!lowInclusive && highInclusive)
                {
                    message = "{0} must be greater than {1} and less than or equal to {2}.";
                }
                else
                {
                    message = "{0} must be greater than {1} and less than {2}.";
                }

                return new ArgumentOutOfRangeException(paramName, Format(message, valueName, lowValue, highValue));
            }

            public static ArgumentOutOfRangeException ValueMustBeGreaterThanOrEqualTo(string paramName, string valueName, object value)
            {
                return new ArgumentOutOfRangeException(paramName, Format("{0} must be greater than or equal to {1}.", valueName, value));
            }

            public static ArgumentOutOfRangeException ValueMustBeLessThanOrEqualTo(string paramName, string valueName, object value)
            {
                return new ArgumentOutOfRangeException(paramName, Format("{0} must be less than or equal to {1}.", valueName, value));
            }

            public static ArgumentOutOfRangeException ValueMustBeLessThan(string paramName, string valueName, object value)
            {
                return new ArgumentOutOfRangeException(paramName, Format("{0} must be less than {1}.", valueName, value));
            }
        }

        public static class DataGridAutomationPeer
        {
            public static InvalidOperationException OperationCannotBePerformed()
            {
                return new InvalidOperationException("Cannot perform the operation.");
            }
        }

        public static class DataGridColumnHeader
        {
            public static NotSupportedException ContentDoesNotSupportUIElements()
            {
                return new NotSupportedException("Content does not support UIElement; use ContentTemplate instead.");
            }
        }

        public static class DataGridLength
        {
            public static ArgumentException InvalidUnitType(string paramName)
            {
                return new ArgumentException(Format("{0} is not a valid DataGridLengthUnitType.", paramName), paramName);
            }
        }

        public static class DataGridLengthConverter
        {
            public static NotSupportedException CannotConvertFrom(string paramName)
            {
                return new NotSupportedException(Format("DataGridLengthConverter cannot convert from {0}.", paramName));
            }

            public static NotSupportedException CannotConvertTo(string paramName)
            {
                return new NotSupportedException(Format("Cannot convert from DataGridLength to {0}.", paramName));
            }

            public static NotSupportedException InvalidDataGridLength(string paramName)
            {
                return new NotSupportedException(Format("Invalid DataGridLength.", paramName));
            }
        }

        public static class DataGridRow
        {
            public static InvalidOperationException InvalidRowIndexCannotCompleteOperation()
            {
                return new InvalidOperationException("Invalid row index. Operation cannot be completed.");
            }
        }

        public static class DataGridSelectedItemsCollection
        {
            public static InvalidOperationException CannotChangeSelectedItemsCollectionInSingleMode()
            {
                return new InvalidOperationException("Can only change SelectedItems collection in Extended selection mode. Use SelectedItem property in Single selection mode.");
            }
        }

        public static class DataGridComboBoxColumn
        {
            public static ArgumentException UnsetBinding(string header)
            {
                return new ArgumentException(Format("Binding for column {0} is null. Ensure that the binding path has been set correctly.", header));
            }

            public static ArgumentException UnsetBinding(Type type)
            {
                return new ArgumentException(Format("Binding for column of type {0} is null. Ensure that the binding path has been set correctly.", type.FullName));
            }

            public static ArgumentException UnknownBindingPath(Binding binding, Type type)
            {
                return new ArgumentException(Format("Binding path {0} could not be found in type {1}. Ensure that the binding path has been set correctly.", binding.Path.Path, type.FullName));
            }

            public static ArgumentException UnknownDisplayMemberPath(string displayMemberPath, Type type)
            {
                return new ArgumentException(Format("DisplayMemberPath {0} could not be found in type {1}. Ensure that the value has been set correctly and note that for built-in types DisplayMemberPath should not be used.", displayMemberPath, type.FullName));
            }

            public static ArgumentException UnknownItemsSourcePath(Binding binding)
            {
                return new ArgumentException(Format("The ItemsSource elements do not contain a property {0}. Ensure that the binding path has been set correctly.", binding.Path.Path));
            }
        }

        public static class DataGridTemplateColumn
        {
            public static TypeInitializationException MissingTemplateForType(Type type)
            {
                return new TypeInitializationException(Format("Missing template. Cannot initialize {0}.", type.FullName), null);
            }
        }

        private static string Format(string formatString, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, formatString, args);
        }
    }
}
