// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Mvvm.ComponentModel
{
    /// <summary>
    /// A base class for objects implementing the <see cref="INotifyDataErrorInfo"/> interface. This class
    /// also inherits from <see cref="ObservableObject"/>, so it can be used for observable items too.
    /// </summary>
    public abstract class ObservableValidator : ObservableObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// The cached <see cref="PropertyChangedEventArgs"/> for <see cref="HasErrors"/>.
        /// </summary>
        private static readonly PropertyChangedEventArgs HasErrorsChangedEventArgs = new PropertyChangedEventArgs(nameof(HasErrors));

        /// <summary>
        /// The <see cref="Dictionary{TKey,TValue}"/> instance used to store previous validation results.
        /// </summary>
        private readonly Dictionary<string, List<ValidationResult>> errors = new Dictionary<string, List<ValidationResult>>();

        /// <summary>
        /// Indicates the total number of properties with errors (not total errors).
        /// This is used to allow <see cref="HasErrors"/> to operate in O(1) time, as it can just
        /// check whether this value is not 0 instead of having to traverse <see cref="errors"/>.
        /// </summary>
        private int totalErrors;

        /// <inheritdoc/>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc/>
        public bool HasErrors => this.totalErrors > 0;

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property with
        /// the new value, then raises the <see cref="ObservableObject.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="validate">If <see langword="true"/>, <paramref name="newValue"/> will also be validated.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This method is just like <see cref="ObservableObject.SetProperty{T}(ref T,T,string)"/>, just with the addition
        /// of the <paramref name="validate"/> parameter. If that is set to <see langword="true"/>, the new value will be
        /// validated and <see cref="ErrorsChanged"/> will be raised if needed. Following the behavior of the base method,
        /// the <see cref="ObservableObject.PropertyChanging"/> and <see cref="ObservableObject.PropertyChanged"/> events
        /// are not raised if the current and new value for the target property are the same.
        /// </remarks>
        protected bool SetProperty<T>(ref T field, T newValue, bool validate, [CallerMemberName] string? propertyName = null)
        {
            bool propertyChanged = SetProperty(ref field, newValue, propertyName);

            if (propertyChanged && validate)
            {
                ValidateProperty(newValue, propertyName);
            }

            return propertyChanged;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property with
        /// the new value, then raises the <see cref="ObservableObject.PropertyChanged"/> event.
        /// See additional notes about this overload in <see cref="SetProperty{T}(ref T,T,bool,string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="validate">If <see langword="true"/>, <paramref name="newValue"/> will also be validated.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected bool SetProperty<T>(ref T field, T newValue, IEqualityComparer<T> comparer, bool validate, [CallerMemberName] string? propertyName = null)
        {
            bool propertyChanged = SetProperty(ref field, newValue, comparer, propertyName);

            if (propertyChanged && validate)
            {
                ValidateProperty(newValue, propertyName);
            }

            return propertyChanged;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property with
        /// the new value, then raises the <see cref="ObservableObject.PropertyChanged"/> event. Similarly to
        /// the <see cref="ObservableObject.SetProperty{T}(T,T,Action{T},string)"/> method, this overload should only be
        /// used when <see cref="ObservableObject.SetProperty{T}(ref T,T,string)"/> can't be used directly.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="validate">If <see langword="true"/>, <paramref name="newValue"/> will also be validated.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This method is just like <see cref="ObservableObject.SetProperty{T}(T,T,Action{T},string)"/>, just with the addition
        /// of the <paramref name="validate"/> parameter. As such, following the behavior of the base method,
        /// the <see cref="ObservableObject.PropertyChanging"/> and <see cref="ObservableObject.PropertyChanged"/> events
        /// are not raised if the current and new value for the target property are the same.
        /// </remarks>
        protected bool SetProperty<T>(T oldValue, T newValue, Action<T> callback, bool validate, [CallerMemberName] string? propertyName = null)
        {
            bool propertyChanged = SetProperty(oldValue, newValue, callback, propertyName);

            if (propertyChanged && validate)
            {
                ValidateProperty(newValue, propertyName);
            }

            return propertyChanged;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property with
        /// the new value, then raises the <see cref="ObservableObject.PropertyChanged"/> event.
        /// See additional notes about this overload in <see cref="SetProperty{T}(T,T,Action{T},bool,string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="validate">If <see langword="true"/>, <paramref name="newValue"/> will also be validated.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected bool SetProperty<T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, bool validate, [CallerMemberName] string? propertyName = null)
        {
            bool propertyChanged = SetProperty(oldValue, newValue, comparer, callback, propertyName);

            if (propertyChanged && validate)
            {
                ValidateProperty(newValue, propertyName);
            }

            return propertyChanged;
        }

        /// <summary>
        /// Compares the current and new values for a given nested property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property and then raises the
        /// <see cref="ObservableObject.PropertyChanged"/> event. The behavior mirrors that of
        /// <see cref="ObservableObject.SetProperty{TModel,T}(T,T,TModel,Action{TModel,T},string)"/>, with the difference being that this
        /// method is used to relay properties from a wrapped model in the current instance. For more info, see the docs for
        /// <see cref="ObservableObject.SetProperty{TModel,T}(T,T,TModel,Action{TModel,T},string)"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
        /// <typeparam name="T">The type of property (or field) to set.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="model">The model </param>
        /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
        /// <param name="validate">If <see langword="true"/>, <paramref name="newValue"/> will also be validated.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected bool SetProperty<TModel, T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, bool validate, [CallerMemberName] string? propertyName = null)
            where TModel : class
        {
            bool propertyChanged = SetProperty(oldValue, newValue, model, callback, propertyName);

            if (propertyChanged && validate)
            {
                ValidateProperty(newValue, propertyName);
            }

            return propertyChanged;
        }

        /// <summary>
        /// Compares the current and new values for a given nested property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property and then raises the
        /// <see cref="ObservableObject.PropertyChanged"/> event. The behavior mirrors that of
        /// <see cref="ObservableObject.SetProperty{TModel,T}(T,T,IEqualityComparer{T},TModel,Action{TModel,T},string)"/>,
        /// with the difference being that this method is used to relay properties from a wrapped model in the
        /// current instance. For more info, see the docs for
        /// <see cref="ObservableObject.SetProperty{TModel,T}(T,T,IEqualityComparer{T},TModel,Action{TModel,T},string)"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
        /// <typeparam name="T">The type of property (or field) to set.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="model">The model </param>
        /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
        /// <param name="validate">If <see langword="true"/>, <paramref name="newValue"/> will also be validated.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected bool SetProperty<TModel, T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, bool validate, [CallerMemberName] string? propertyName = null)
            where TModel : class
        {
            bool propertyChanged = SetProperty(oldValue, newValue, comparer, model, callback, propertyName);

            if (propertyChanged && validate)
            {
                ValidateProperty(newValue, propertyName);
            }

            return propertyChanged;
        }

        /// <summary>
        /// Tries to validate a new value for a specified property. If the validation is successful,
        /// <see cref="ObservableObject.SetProperty{T}(ref T,T,string?)"/> is called, otherwise no state change is performed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns>Whether the validation was successful and the property value changed as well.</returns>
        protected bool TrySetProperty<T>(ref T field, T newValue, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string? propertyName = null)
        {
            return TryValidateProperty(newValue, propertyName, out errors) &&
                   SetProperty(ref field, newValue, propertyName);
        }

        /// <summary>
        /// Tries to validate a new value for a specified property. If the validation is successful,
        /// <see cref="ObservableObject.SetProperty{T}(ref T,T,IEqualityComparer{T},string?)"/> is called, otherwise no state change is performed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns>Whether the validation was successful and the property value changed as well.</returns>
        protected bool TrySetProperty<T>(ref T field, T newValue, IEqualityComparer<T> comparer, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string? propertyName = null)
        {
            return TryValidateProperty(newValue, propertyName, out errors) &&
                   SetProperty(ref field, newValue, comparer, propertyName);
        }

        /// <summary>
        /// Tries to validate a new value for a specified property. If the validation is successful,
        /// <see cref="ObservableObject.SetProperty{T}(T,T,Action{T},string?)"/> is called, otherwise no state change is performed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns>Whether the validation was successful and the property value changed as well.</returns>
        protected bool TrySetProperty<T>(T oldValue, T newValue, Action<T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string? propertyName = null)
        {
            return TryValidateProperty(newValue, propertyName, out errors) &&
                   SetProperty(oldValue, newValue, callback, propertyName);
        }

        /// <summary>
        /// Tries to validate a new value for a specified property. If the validation is successful,
        /// <see cref="ObservableObject.SetProperty{T}(T,T,IEqualityComparer{T},Action{T},string?)"/> is called, otherwise no state change is performed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns>Whether the validation was successful and the property value changed as well.</returns>
        protected bool TrySetProperty<T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string? propertyName = null)
        {
            return TryValidateProperty(newValue, propertyName, out errors) &&
                   SetProperty(oldValue, newValue, comparer, callback, propertyName);
        }

        /// <summary>
        /// Tries to validate a new value for a specified property. If the validation is successful,
        /// <see cref="ObservableObject.SetProperty{TModel,T}(T,T,TModel,Action{TModel,T},string?)"/> is called, otherwise no state change is performed.
        /// </summary>
        /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
        /// <typeparam name="T">The type of property (or field) to set.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="model">The model </param>
        /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns>Whether the validation was successful and the property value changed as well.</returns>
        protected bool TrySetProperty<TModel, T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string? propertyName = null)
            where TModel : class
        {
            return TryValidateProperty(newValue, propertyName, out errors) &&
                   SetProperty(oldValue, newValue, model, callback, propertyName);
        }

        /// <summary>
        /// Tries to validate a new value for a specified property. If the validation is successful,
        /// <see cref="ObservableObject.SetProperty{TModel,T}(T,T,IEqualityComparer{T},TModel,Action{TModel,T},string?)"/> is called, otherwise no state change is performed.
        /// </summary>
        /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
        /// <typeparam name="T">The type of property (or field) to set.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="model">The model </param>
        /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns>Whether the validation was successful and the property value changed as well.</returns>
        protected bool TrySetProperty<TModel, T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, out IReadOnlyCollection<ValidationResult> errors, [CallerMemberName] string? propertyName = null)
            where TModel : class
        {
            return TryValidateProperty(newValue, propertyName, out errors) &&
                   SetProperty(oldValue, newValue, comparer, model, callback, propertyName);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable GetErrors(string? propertyName)
        {
            // Entity-level errors when the target property is null or empty
            if (string.IsNullOrEmpty(propertyName))
            {
                return this.GetAllErrors();
            }

            // Property-level errors, if any
            if (this.errors.TryGetValue(propertyName!, out List<ValidationResult> errors))
            {
                return errors;
            }

            // The INotifyDataErrorInfo.GetErrors method doesn't specify exactly what to
            // return when the input property name is invalid, but given that the return
            // type is marked as a non-nullable reference type, here we're returning an
            // empty array to respect the contract. This also matches the behavior of
            // this method whenever errors for a valid properties are retrieved.
            return Array.Empty<ValidationResult>();
        }

        /// <summary>
        /// Implements the logic for entity-level errors gathering for <see cref="GetErrors"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable"/> instance with all the errors in <see cref="errors"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private IEnumerable GetAllErrors()
        {
            return this.errors.Values.SelectMany(errors => errors);
        }

        /// <summary>
        /// Validates a property with a specified name and a given input value.
        /// </summary>
        /// <param name="value">The value to test for the specified property.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyName"/> is <see langword="null"/>.</exception>
        private void ValidateProperty(object? value, string? propertyName)
        {
            if (propertyName is null)
            {
                ThrowArgumentNullExceptionForNullPropertyName();
            }

            // Check if the property had already been previously validated, and if so retrieve
            // the reusable list of validation errors from the errors dictionary. This list is
            // used to add new validation errors below, if any are produced by the validator.
            // If the property isn't present in the dictionary, add it now to avoid allocations.
            if (!this.errors.TryGetValue(propertyName!, out List<ValidationResult>? propertyErrors))
            {
                propertyErrors = new List<ValidationResult>();

                this.errors.Add(propertyName!, propertyErrors);
            }

            bool errorsChanged = false;

            // Clear the errors for the specified property, if any
            if (propertyErrors.Count > 0)
            {
                propertyErrors.Clear();

                errorsChanged = true;
            }

            // Validate the property, by adding new errors to the existing list
            bool isValid = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null) { MemberName = propertyName },
                propertyErrors);

            // Update the shared counter for the number of errors, and raise the
            // property changed event if necessary. We decrement the number of total
            // errors if the current property is valid but it wasn't so before this
            // validation, and we increment it if the validation failed after being
            // correct before. The property changed event is raised whenever the
            // number of total errors is either decremented to 0, or incremented to 1.
            if (isValid)
            {
                if (errorsChanged)
                {
                    this.totalErrors--;

                    if (this.totalErrors == 0)
                    {
                        OnPropertyChanged(HasErrorsChangedEventArgs);
                    }
                }
            }
            else if (!errorsChanged)
            {
                this.totalErrors++;

                if (this.totalErrors == 1)
                {
                    OnPropertyChanged(HasErrorsChangedEventArgs);
                }
            }

            // Only raise the event once if needed. This happens either when the target property
            // had existing errors and is now valid, or if the validation has failed and there are
            // new errors to broadcast, regardless of the previous validation state for the property.
            if (errorsChanged || !isValid)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Tries to validate a property with a specified name and a given input value, and returns
        /// the computed errors, if any. If the property is valid, it is assumed that its value is
        /// about to be set in the current object. Otherwise, no observable local state is modified.
        /// </summary>
        /// <param name="value">The value to test for the specified property.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <param name="errors">The resulting validation errors, if any.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyName"/> is <see langword="null"/>.</exception>
        private bool TryValidateProperty(object? value, string? propertyName, out IReadOnlyCollection<ValidationResult> errors)
        {
            if (propertyName is null)
            {
                ThrowArgumentNullExceptionForNullPropertyName();
            }

            // Add the cached errors list for later use.
            if (!this.errors.TryGetValue(propertyName!, out List<ValidationResult>? propertyErrors))
            {
                propertyErrors = new List<ValidationResult>();

                this.errors.Add(propertyName!, propertyErrors);
            }

            bool hasErrors = propertyErrors.Count > 0;

            List<ValidationResult> localErrors = new List<ValidationResult>();

            // Validate the property, by adding new errors to the local list
            bool isValid = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null) { MemberName = propertyName },
                localErrors);

            // We only modify the state if the property is valid and it wasn't so before. In this case, we
            // clear the cached list of errors (which is visible to consumers) and raise the necessary events.
            if (isValid && hasErrors)
            {
                propertyErrors.Clear();

                this.totalErrors--;

                if (this.totalErrors == 0)
                {
                    OnPropertyChanged(HasErrorsChangedEventArgs);
                }

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }

            errors = localErrors;

            return isValid;
        }

#pragma warning disable SA1204
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when a property name given as input is <see langword="null"/>.
        /// </summary>
        private static void ThrowArgumentNullExceptionForNullPropertyName()
        {
            throw new ArgumentNullException("propertyName", "The input property name cannot be null when validating a property");
        }
#pragma warning restore SA1204
    }
}