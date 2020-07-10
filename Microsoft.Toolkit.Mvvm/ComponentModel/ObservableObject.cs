// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1512

// This file is inspired from the MvvmLight libray (lbugnion/mvvmlight),
// more info in ThirdPartyNotices.txt in the root of the project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Mvvm.ComponentModel
{
    /// <summary>
    /// A base class for objects of which the properties must be observable.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc cref="INotifyPropertyChanging.PropertyChanging"/>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// Performs the required configuration when a property has changed, and then
        /// raises the <see cref="PropertyChanged"/> event to notify listeners of the update.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <remarks>The base implementation only raises the <see cref="PropertyChanged"/> event.</remarks>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Performs the required configuration when a property is changing, and then
        /// raises the <see cref="PropertyChanged"/> event to notify listeners of the update.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <remarks>The base implementation only raises the <see cref="PropertyChanging"/> event.</remarks>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null!)
        {
            RaisePropertyChanging(propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void RaisePropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// This overload is much less efficient than <see cref="Set{T}(ref T,T,string)"/> and it
        /// should only be used when the former is not viable (eg. when the target property being
        /// updated does not directly expose a backing field that can be passed by reference).
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool Set<T>(T oldValue, T newValue, Action<T> callback, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given nested property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property and then raises the
        /// <see cref="PropertyChanged"/> event. The behavior mirrors that of <see cref="Set{T}(ref T,T,string)"/>,
        /// with the difference being that this method is used to relay properties from a wrapped model in the
        /// current instance. This type is useful when creating wrapping, bindable objects that operate over
        /// models that lack support for notification (eg. for CRUD operations).
        /// Suppose we have this model (eg. for a database row in a table):
        /// <code>
        /// public class Person
        /// {
        ///     public string Name { get; set; }
        /// }
        /// </code>
        /// We can then use a property to wrap instances of this type into our observable model (which supports
        /// notifications), injecting the notification to theproperties of that model, like so:
        /// <code>
        /// public class BindablePerson : ObservableObject
        /// {
        ///     public Model { get; }
        ///
        ///     public BindablePerson(Person model)
        ///     {
        ///         Model = model;
        ///     }
        ///
        ///     public string Name
        ///     {
        ///         get => Model.Name;
        ///         set => Set(() => Model.Name, value);
        ///     }
        /// }
        /// </code>
        /// This way we can then use the wrapping object in our application, and all those "proxy" properties will
        /// also raise notifications when changed. Note that this method is not meant to be a replacement for
        /// <see cref="Set{T}(ref T,T,string)"/>, which offers better performance and less memory usage. Only use this
        /// overload when relaying properties to a model that doesn't support notifications, and only if you can't
        /// implement notifications to that model directly (eg. by having it inherit from <see cref="ObservableObject"/>).
        /// </summary>
        /// <typeparam name="T">The type of property to set.</typeparam>
        /// <param name="propertyExpression">An <see cref="Expression{TDelegate}"/> returning the property to update.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same. Additionally, <paramref name="propertyExpression"/>
        /// must return a property from a model that is stored as another property in the current instance.
        /// This method only supports one level of indirection: <paramref name="propertyExpression"/> can only
        /// be used to access properties of a model that is directly stored as a property of the current instance.
        /// Additionally, this method can only be used if the wrapped item is a reference type.
        /// </remarks>
        protected bool Set<T>(Expression<Func<T>> propertyExpression, T newValue, [CallerMemberName] string propertyName = null!)
        {
            PropertyInfo? parentPropertyInfo;
            FieldInfo? parentFieldInfo = null;

            // Get the target property info
            if (!(propertyExpression.Body is MemberExpression targetExpression &&
                  targetExpression.Member is PropertyInfo targetPropertyInfo &&
                  targetExpression.Expression is MemberExpression parentExpression &&
                  (!((parentPropertyInfo = parentExpression.Member as PropertyInfo) is null) ||
                   !((parentFieldInfo = parentExpression.Member as FieldInfo) is null)) &&
                  parentExpression.Expression is ConstantExpression instanceExpression &&
                  instanceExpression.Value is object instance))
            {
                ThrowArgumentExceptionForInvalidPropertyExpression();

                // This is never executed, as the method above always throws
                return false;
            }

            object parent = parentPropertyInfo is null
                ? parentFieldInfo!.GetValue(instance)
                : parentPropertyInfo.GetValue(instance);
            T oldValue = (T)targetPropertyInfo.GetValue(parent);

            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            targetPropertyInfo.SetValue(parent, newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given field (which should be the backing
        /// field for a property). If the value has changed, raises the <see cref="PropertyChanging"/>
        /// event, updates the field and then raises the <see cref="PropertyChanged"/> event.
        /// The behavior mirrors that of <see cref="Set{T}(ref T,T,string)"/>, with the difference being that this method
        /// will also monitor the new value of the property (a generic <see cref="Task"/>) and will also
        /// raise the <see cref="PropertyChanged"/> again for the target property when it completes.
        /// This can be used to update bindings observing that <see cref="Task"/> or any of its properties.
        /// Here is a sample property declaration using this method:
        /// <code>
        /// private Task myTask;
        ///
        /// public Task MyTask
        /// {
        ///     get => myTask;
        ///     private set => SetAndNotifyOnCompletion(ref myTask, () => myTask, value);
        /// }
        /// </code>
        /// </summary>
        /// <typeparam name="TTask">The type of <see cref="Task"/> to set and monitor.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="fieldExpression">
        /// An <see cref="Expression{TDelegate}"/> returning the field to update. This is needed to be
        /// able to raise the <see cref="PropertyChanged"/> to notify the completion of the input task.
        /// </param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool SetAndNotifyOnCompletion<TTask>(ref TTask? field, Expression<Func<TTask?>> fieldExpression, TTask? newValue, [CallerMemberName] string propertyName = null!)
            where TTask : Task
        {
            if (ReferenceEquals(field, newValue))
            {
                return false;
            }

            // Check the status of the new task before assigning it to the
            // target field. This is so that in case the task is either
            // null or already completed, we can avoid the overhead of
            // scheduling the method to monitor its completion.
            bool isAlreadyCompletedOrNull = newValue?.IsCompleted ?? true;

            OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            // If the input task is either null or already completed, we don't need to
            // execute the additional logic to monitor its completion, so we can just bypass
            // the rest of the method and return that the field changed here. The return value
            // does not indicate that the task itself has completed, but just that the property
            // value itself has changed (ie. the referenced task instance has changed).
            // This mirrors the return value of all the other synchronous Set methods as well.
            if (isAlreadyCompletedOrNull)
            {
                return true;
            }

            // Get the target field to set. This is needed because we can't
            // capture the ref field in a closure (for the async method).
            if (!((fieldExpression.Body as MemberExpression)?.Member is FieldInfo fieldInfo))
            {
                ThrowArgumentExceptionForInvalidFieldExpression();

                // This is never executed, as the method above always throws
                return false;
            }

            // We use a local async function here so that the main method can
            // remain synchronous and return a value that can be immediately
            // used by the caller. This mirrors Set<T>(ref T, T, string).
            // We use an async void function instead of a Task-returning function
            // so that if a binding update caused by the property change notification
            // causes a crash, it is immediately reported in the application instead of
            // the exception being ignored (as the returned task wouldn't be awaited),
            // which would result in a confusing behavior for users.
            async void MonitorTask()
            {
                try
                {
                    // Await the task and ignore any exceptions
                    await newValue!;
                }
                catch
                {
                }

                TTask? currentTask = (TTask?)fieldInfo.GetValue(this);

                // Only notify if the property hasn't changed
                if (ReferenceEquals(newValue, currentTask))
                {
                    OnPropertyChanged(propertyName);
                }
            }

            MonitorTask();

            return true;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a given <see cref="Expression{TDelegate}"/> is invalid for a property.
        /// </summary>
        private static void ThrowArgumentExceptionForInvalidPropertyExpression()
        {
            throw new ArgumentException("The given expression must be in the form () => MyModel.MyProperty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a given <see cref="Expression{TDelegate}"/> is invalid for a property field.
        /// </summary>
        private static void ThrowArgumentExceptionForInvalidFieldExpression()
        {
            throw new ArgumentException("The given expression must be in the form () => field");
        }
    }
}