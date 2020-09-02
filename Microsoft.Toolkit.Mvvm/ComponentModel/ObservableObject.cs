// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1512

// This file is inspired from the MvvmLight library (lbugnion/MvvmLight),
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
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Performs the required configuration when a property is changing, and then
        /// raises the <see cref="PropertyChanged"/> event to notify listeners of the update.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <remarks>The base implementation only raises the <see cref="PropertyChanging"/> event.</remarks>
        protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
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
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            // We duplicate the code here instead of calling the overload because we can't
            // guarantee that the invoked SetProperty<T> will be inlined, and we need the JIT
            // to be able to see the full EqualityComparer<T>.Default.Equals call, so that
            // it'll use the intrinsics version of it and just replace the whole invocation
            // with a direct comparison when possible (eg. for primitive numeric types).
            // This is the fastest SetProperty<T> overload so we particularly care about
            // the codegen quality here, and the code is small and simple enough so that
            // duplicating it still doesn't make the whole class harder to maintain.
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
        /// See additional notes about this overload in <see cref="SetProperty{T}(ref T,T,string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected bool SetProperty<T>(ref T field, T newValue, IEqualityComparer<T> comparer, [CallerMemberName] string? propertyName = null)
        {
            if (comparer.Equals(field, newValue))
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
        /// This overload is much less efficient than <see cref="SetProperty{T}(ref T,T,string)"/> and it
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
        protected bool SetProperty<T>(T oldValue, T newValue, Action<T> callback, [CallerMemberName] string? propertyName = null)
        {
            return SetProperty(oldValue, newValue, EqualityComparer<T>.Default, callback, propertyName);
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// See additional notes about this overload in <see cref="SetProperty{T}(T,T,Action{T},string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The current property value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected bool SetProperty<T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, [CallerMemberName] string? propertyName = null)
        {
            if (comparer.Equals(oldValue, newValue))
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
        /// <see cref="PropertyChanged"/> event. The behavior mirrors that of <see cref="SetProperty{T}(ref T,T,string)"/>,
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
        /// notifications), injecting the notification to the properties of that model, like so:
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
        /// <see cref="SetProperty{T}(ref T,T,string)"/>, which offers better performance and less memory usage. Only use this
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
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyExpression"/> is not valid.</exception>
        protected bool SetProperty<T>(Expression<Func<T>> propertyExpression, T newValue, [CallerMemberName] string? propertyName = null)
        {
            return SetProperty(propertyExpression, newValue, EqualityComparer<T>.Default, out _, propertyName);
        }

        /// <summary>
        /// Compares the current and new values for a given nested property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property and then raises the
        /// <see cref="PropertyChanged"/> event. The behavior mirrors that of <see cref="SetProperty{T}(ref T,T,string)"/>,
        /// with the difference being that this method is used to relay properties from a wrapped model in the
        /// current instance. See additional notes about this overload in <see cref="SetProperty{T}(Expression{Func{T}},T,string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of property to set.</typeparam>
        /// <param name="propertyExpression">An <see cref="Expression{TDelegate}"/> returning the property to update.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyExpression"/> is not valid.</exception>
        protected bool SetProperty<T>(Expression<Func<T>> propertyExpression, T newValue, IEqualityComparer<T> comparer, [CallerMemberName] string? propertyName = null)
        {
            return SetProperty(propertyExpression, newValue, comparer, out _, propertyName);
        }

        /// <summary>
        /// Implements the shared logic for <see cref="SetProperty{T}(Expression{Func{T}},T,IEqualityComparer{T},string)"/>
        /// </summary>
        /// <typeparam name="T">The type of property to set.</typeparam>
        /// <param name="propertyExpression">An <see cref="Expression{TDelegate}"/> returning the property to update.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
        /// <param name="oldValue">The resulting initial value for the target property.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyExpression"/> is not valid.</exception>
        private protected bool SetProperty<T>(Expression<Func<T>> propertyExpression, T newValue, IEqualityComparer<T> comparer, out T oldValue, [CallerMemberName] string? propertyName = null)
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
                oldValue = default!;

                return false;
            }

            object parent = parentPropertyInfo is null
                ? parentFieldInfo!.GetValue(instance)
                : parentPropertyInfo.GetValue(instance);
            oldValue = (T)targetPropertyInfo.GetValue(parent);

            if (comparer.Equals(oldValue, newValue))
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
        /// The behavior mirrors that of <see cref="SetProperty{T}(ref T,T,string)"/>, with the difference being that
        /// this method will also monitor the new value of the property (a generic <see cref="Task"/>) and will also
        /// raise the <see cref="PropertyChanged"/> again for the target property when it completes.
        /// This can be used to update bindings observing that <see cref="Task"/> or any of its properties.
        /// This method and its overload specifically rely on the <see cref="TaskAccessor{TTask}"/> type, which needs
        /// to be used in the backing field for the target <see cref="Task"/> property. The field doesn't need to be
        /// initialized, as this method will take care of doing that automatically. The <see cref="TaskAccessor{TTask}"/>
        /// type also includes an implicit operator, so it can be assigned to any <see cref="Task"/> instance directly.
        /// Here is a sample property declaration using this method:
        /// <code>
        /// private TaskAccessor&lt;Task&gt; myTask;
        ///
        /// public Task MyTask
        /// {
        ///     get => myTask;
        ///     private set => SetAndNotifyOnCompletion(ref myTask, value);
        /// }
        /// </code>
        /// </summary>
        /// <typeparam name="TTask">The type of <see cref="Task"/> to set and monitor.</typeparam>
        /// <param name="taskAccessor">The field accessor to modify.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised if the current
        /// and new value for the target property are the same. The return value being <see langword="true"/> only
        /// indicates that the new value being assigned to <paramref name="taskAccessor"/> is different than the previous one,
        /// and it does not mean the new <typeparamref name="TTask"/> instance passed as argument is in any particular state.
        /// </remarks>
        protected bool SetPropertyAndNotifyOnCompletion<TTask>(ref TaskAccessor<TTask>? taskAccessor, TTask? newValue, [CallerMemberName] string? propertyName = null)
            where TTask : Task
        {
            // We invoke the overload with a callback here to avoid code duplication, and simply pass an empty callback.
            // The lambda expression here is transformed by the C# compiler into an empty closure class with a
            // static singleton field containing a closure instance, and another caching the instantiated Action<TTask>
            // instance. This will result in no further allocations after the first time this method is called for a given
            // generic type. We only pay the cost of the virtual call to the delegate, but this is not performance critical
            // code and that overhead would still be much lower than the rest of the method anyway, so that's fine.
            return SetPropertyAndNotifyOnCompletion(ref taskAccessor, newValue, _ => { }, propertyName);
        }

        /// <summary>
        /// Compares the current and new values for a given field (which should be the backing
        /// field for a property). If the value has changed, raises the <see cref="PropertyChanging"/>
        /// event, updates the field and then raises the <see cref="PropertyChanged"/> event.
        /// This method is just like <see cref="SetPropertyAndNotifyOnCompletion{TTask}(ref TaskAccessor{TTask},TTask,string)"/>,
        /// with the difference being an extra <see cref="Action{T}"/> parameter with a callback being invoked
        /// either immediately, if the new task has already completed or is <see langword="null"/>, or upon completion.
        /// </summary>
        /// <typeparam name="TTask">The type of <see cref="Task"/> to set and monitor.</typeparam>
        /// <param name="taskAccessor">The field accessor to modify.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="callback">A callback to invoke to update the property value.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool SetPropertyAndNotifyOnCompletion<TTask>(ref TaskAccessor<TTask>? taskAccessor, TTask? newValue, Action<TTask?> callback, [CallerMemberName] string? propertyName = null)
            where TTask : Task
        {
            // Initialize the task accessor, if none is present
            var localAccessor = taskAccessor ??= new TaskAccessor<TTask>();

            if (ReferenceEquals(taskAccessor.Value, newValue))
            {
                return false;
            }

            // Check the status of the new task before assigning it to the
            // target field. This is so that in case the task is either
            // null or already completed, we can avoid the overhead of
            // scheduling the method to monitor its completion.
            bool isAlreadyCompletedOrNull = newValue?.IsCompleted ?? true;

            OnPropertyChanging(propertyName);

            localAccessor.Value = newValue;

            OnPropertyChanged(propertyName);

            // If the input task is either null or already completed, we don't need to
            // execute the additional logic to monitor its completion, so we can just bypass
            // the rest of the method and return that the field changed here. The return value
            // does not indicate that the task itself has completed, but just that the property
            // value itself has changed (ie. the referenced task instance has changed).
            // This mirrors the return value of all the other synchronous Set methods as well.
            if (isAlreadyCompletedOrNull)
            {
                callback(newValue);

                return true;
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

                // Only notify if the property hasn't changed
                if (ReferenceEquals(newValue, localAccessor.Value))
                {
                    OnPropertyChanged(propertyName);
                }

                callback(newValue);
            }

            MonitorTask();

            return true;
        }

        /// <summary>
        /// A wrapping class that can hold a <see cref="Task"/> value.
        /// </summary>
        /// <typeparam name="TTask">The type of value to store.</typeparam>
        protected sealed class TaskAccessor<TTask>
            where TTask : Task
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TaskAccessor{TTask}"/> class.
            /// </summary>
            internal TaskAccessor()
            {
            }

#pragma warning disable SA1401 // Fields should be private
            /// <summary>
            /// Gets or sets the wrapped <see cref="Task"/> value.
            /// </summary>
            internal TTask? Value;
#pragma warning restore SA1401

            /// <summary>
            /// Unwraps the <typeparamref name="TTask"/> value stored in the current instance.
            /// </summary>
            /// <param name="accessor">The input <see cref="TaskAccessor{TTask}"/> instance.</param>
            public static implicit operator TTask?(TaskAccessor<TTask>? accessor)
            {
                return accessor?.Value;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a given <see cref="Expression{TDelegate}"/> is invalid for a property.
        /// </summary>
        private static void ThrowArgumentExceptionForInvalidPropertyExpression()
        {
            throw new ArgumentException("The given expression must be in the form () => MyModel.MyProperty");
        }
    }
}