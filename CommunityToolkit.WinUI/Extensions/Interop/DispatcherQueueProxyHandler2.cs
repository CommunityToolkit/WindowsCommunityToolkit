// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;
using static CommunityToolkit.WinUI.Interop.Windows;

#nullable enable

#pragma warning disable SA1023

namespace CommunityToolkit.WinUI.Interop
{
    /// <summary>
    /// A custom <c>IDispatcherQueueHandler</c> object, that internally stores a captured <see cref="DispatcherQueueHandler{T1,T2}"/> instance
    /// and the input captured state. This allows consumers to enqueue a state and a cached stateless delegate without any managed allocations.
    /// </summary>
    internal unsafe struct DispatcherQueueProxyHandler2 : IDispatcherQueueHandler
    {
        /// <summary>
        /// The shared vtable pointer for <see cref="DispatcherQueueProxyHandler2"/> instances.
        /// </summary>
        private static readonly void** Vtbl = InitVtbl();

        /// <summary>
        /// Setups the vtable pointer for <see cref="DispatcherQueueProxyHandler2"/>.
        /// </summary>
        /// <returns>The initialized vtable pointer for <see cref="DispatcherQueueProxyHandler2"/>.</returns>
        /// <remarks>
        /// The vtable itself is allocated with <see cref="RuntimeHelpers.AllocateTypeAssociatedMemory(Type, int)"/>,
        /// which allocates memory in the high frequency heap associated with the input runtime type. This will be
        /// automatically cleaned up when the type is unloaded, so there is no need to ever manually free this memory.
        /// </remarks>
        private static void** InitVtbl()
        {
            void** lpVtbl = (void**)RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(DispatcherQueueProxyHandler2), sizeof(void*) * 4);

            lpVtbl[0] = (delegate* unmanaged<DispatcherQueueProxyHandler2*, Guid*, void**, int>)&Impl.QueryInterface;
            lpVtbl[1] = (delegate* unmanaged<DispatcherQueueProxyHandler2*, uint>)&Impl.AddRef;
            lpVtbl[2] = (delegate* unmanaged<DispatcherQueueProxyHandler2*, uint>)&Impl.Release;
            lpVtbl[3] = (delegate* unmanaged<DispatcherQueueProxyHandler2*, int>)&Impl.Invoke;

            return lpVtbl;
        }

        /// <summary>
        /// The vtable pointer for the current instance.
        /// </summary>
        private void** lpVtbl;

        /// <summary>
        /// The <see cref="GCHandle"/> to the captured <see cref="DispatcherQueueHandler{T1,T2}"/> (for some unknown <c>TState</c> type).
        /// </summary>
        private GCHandle callbackHandle;

        /// <summary>
        /// The <see cref="GCHandle"/> to the first captured state (with an unknown <c>T1</c> type).
        /// </summary>
        private GCHandle state1Handle;

        /// <summary>
        /// The <see cref="GCHandle"/> to the second captured state (with an unknown <c>T2</c> type).
        /// </summary>
        private GCHandle state2Handle;

        /// <summary>
        /// The generic stub to invoke the current callback with the right generic context.
        /// </summary>
        private delegate*<DispatcherQueueProxyHandler2*, int> stub;

        /// <summary>
        /// The current reference count for the object (from <c>IUnknown</c>).
        /// </summary>
        private volatile uint referenceCount;

        /// <summary>
        /// Creates a new <see cref="DispatcherQueueProxyHandler2"/> instance for the input callback and state.
        /// </summary>
        /// <typeparam name="T1">The type of the first state to capture.</typeparam>
        /// <typeparam name="T2">The type of the second state to capture.</typeparam>
        /// <param name="handler">The input <see cref="DispatcherQueueHandler{T1,T2}"/> callback to enqueue.</param>
        /// <param name="state1">The first input state to capture and pass to the callback.</param>
        /// <param name="state2">The second input state to capture and pass to the callback.</param>
        /// <returns>A pointer to the newly initialized <see cref="DispatcherQueueProxyHandler2"/> instance.</returns>
        public static DispatcherQueueProxyHandler2* Create<T1, T2>(DispatcherQueueHandler<T1, T2> handler, T1 state1, T2 state2)
            where T1 : class
            where T2 : class
        {
            DispatcherQueueProxyHandler2* @this = (DispatcherQueueProxyHandler2*)Marshal.AllocHGlobal(sizeof(DispatcherQueueProxyHandler2));

            @this->lpVtbl = Vtbl;
            @this->callbackHandle = GCHandle.Alloc(handler);
            @this->state1Handle = GCHandle.Alloc(state1);
            @this->state2Handle = GCHandle.Alloc(state2);
            @this->stub = &Impl.Invoke<T1, T2>;
            @this->referenceCount = 1;

            return @this;
        }

        /// <summary>
        /// Devirtualized API for <c>IUnknown.AddRef()</c>.
        /// </summary>
        /// <returns>The updated reference count for the current instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint AddRef()
        {
            return Interlocked.Increment(ref referenceCount);
        }

        /// <summary>
        /// Devirtualized API for <c>IUnknown.Release()</c>.
        /// </summary>
        /// <returns>The updated reference count for the current instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Release()
        {
            uint referenceCount = Interlocked.Decrement(ref this.referenceCount);

            if (referenceCount == 0)
            {
                callbackHandle.Free();
                state1Handle.Free();
                state2Handle.Free();

                Marshal.FreeHGlobal((IntPtr)Unsafe.AsPointer(ref this));
            }

            return referenceCount;
        }

        /// <summary>
        /// A private type with the implementation of the unmanaged methods for <see cref="DispatcherQueueProxyHandler2"/>.
        /// These methods will be set into the shared vtable and invoked by WinRT from the object passed to it as an interface.
        /// </summary>
        private static class Impl
        {
            /// <summary>
            /// Implements <c>IUnknown.QueryInterface(REFIID, void**)</c>.
            /// </summary>
            [UnmanagedCallersOnly]
            public static int QueryInterface(DispatcherQueueProxyHandler2* @this, Guid* riid, void** ppvObject)
            {
                if (riid->Equals(GuidOfIUnknown) ||
                    riid->Equals(GuidOfIAgileObject) ||
                    riid->Equals(GuidOfIDispatcherQueueHandler))
                {
                    @this->AddRef();

                    *ppvObject = @this;

                    return S_OK;
                }

                return E_NOINTERFACE;
            }

            /// <summary>
            /// Implements <c>IUnknown.AddRef()</c>.
            /// </summary>
            [UnmanagedCallersOnly]
            public static uint AddRef(DispatcherQueueProxyHandler2* @this)
            {
                return Interlocked.Increment(ref @this->referenceCount);
            }

            /// <summary>
            /// Implements <c>IUnknown.Release()</c>.
            /// </summary>
            [UnmanagedCallersOnly]
            public static uint Release(DispatcherQueueProxyHandler2* @this)
            {
                uint referenceCount = Interlocked.Decrement(ref @this->referenceCount);

                if (referenceCount == 0)
                {
                    @this->callbackHandle.Free();
                    @this->state1Handle.Free();
                    @this->state2Handle.Free();

                    Marshal.FreeHGlobal((IntPtr)@this);
                }

                return referenceCount;
            }

            /// <summary>
            /// Implements <c>IDispatcherQueueHandler.Invoke()</c>.
            /// </summary>
            [UnmanagedCallersOnly]
            public static int Invoke(DispatcherQueueProxyHandler2* @this)
            {
                return @this->stub(@this);
            }

            /// <summary>
            /// Implements <c>IDispatcherQueueHandler.Invoke()</c> from within a generic context.
            /// </summary>
            public static int Invoke<T1, T2>(DispatcherQueueProxyHandler2* @this)
                where T1 : class
                where T2 : class
            {
                object callback = @this->callbackHandle.Target!;
                object state1 = @this->state1Handle.Target!;
                object state2 = @this->state2Handle.Target!;

                try
                {
                    Unsafe.As<DispatcherQueueHandler<object, object>>(callback)(
                        Unsafe.As<T1>(state1),
                        Unsafe.As<T2>(state2));
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);

                    return ExceptionHelpers.GetHRForException(e);
                }

                return S_OK;
            }
        }
    }
}