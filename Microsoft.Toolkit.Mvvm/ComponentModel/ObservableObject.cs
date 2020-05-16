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
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

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
        /// Raises the <see cref="PropertyChanged"/> event if needed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event if needed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Broadcasts a <see cref="PropertyChangedMessage{T}"/> with the specified
        /// parameters, without using any particular token (so using the default channel).
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The value of the property before it changed.</param>
        /// <param name="newValue">The value of the property after it changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <remarks>
        /// You should override this method if you wish to customize the channel being
        /// used to send the message (eg. if you need to use a specific token for the channel).
        /// </remarks>
        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            var message = new PropertyChangedMessage<T>(this, propertyName, oldValue, newValue);

            Messenger.Default.Send(message);
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

            this.OnPropertyChanging(propertyName);

            field = newValue;

            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with
        /// the new value, then raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="broadcast">If <see langword="true"/>, <see cref="Broadcast{T}"/> will also be invoked.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This method is just like <see cref="Set{T}(ref T, T, string)"/>, just with the addition
        /// of the <paramref name="broadcast"/> parameter. As such, following the behavior of the base method,
        /// the <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events
        /// are not raised if the current and new value for the target property are the same.
        /// </remarks>
        protected bool Set<T>(ref T field, T newValue, bool broadcast, [CallerMemberName] string propertyName = null!)
        {
            if (!broadcast)
            {
                return Set(ref field, newValue, propertyName);
            }

            T oldValue = field;

            if (Set(ref field, newValue, propertyName))
            {
                Broadcast(oldValue, newValue, propertyName);

                return true;
            }

            return false;
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
            // Get the target property info
            if (!(propertyExpression.Body is MemberExpression targetExpression &&
                  targetExpression.Member is PropertyInfo targetPropertyInfo &&
                  targetExpression.Expression is MemberExpression parentExpression &&
                  parentExpression.Member is PropertyInfo parentPropertyInfo &&
                  parentExpression.Expression is ConstantExpression instanceExpression &&
                  instanceExpression.Value is object instance))
            {
                ThrowArgumentExceptionForInvalidPropertyExpression();

                // This is never executed, as the method above always throws
                return false;
            }

            object parent = parentPropertyInfo.GetValue(instance);
            T oldValue = (T)targetPropertyInfo.GetValue(parent);

            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            this.OnPropertyChanging(propertyName);

            targetPropertyInfo.SetValue(parent, newValue);

            this.OnPropertyChanged(propertyName);

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

            this.OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

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
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForInvalidPropertyExpression()
        {
            throw new ArgumentException("The given expression must be in the form () => MyModel.MyProperty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a given <see cref="Expression{TDelegate}"/> is invalid for a property field.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForInvalidFieldExpression()
        {
            throw new ArgumentException("The given expression must be in the form () => field");
        }
    }
}