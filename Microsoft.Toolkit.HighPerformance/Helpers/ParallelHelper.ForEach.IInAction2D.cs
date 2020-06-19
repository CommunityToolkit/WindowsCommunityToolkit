// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Toolkit.HighPerformance.Memory;

namespace Microsoft.Toolkit.HighPerformance.Helpers
{
    /// <summary>
    /// Helpers to work with parallel code in a highly optimized manner.
    /// </summary>
    public static partial class ParallelHelper
    {
        /// <summary>
        /// Executes a specified action in an optimized parallel loop over the input data.
        /// </summary>
        /// <typeparam name="TItem">The type of items to iterate over.</typeparam>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IInAction{T}"/> of <typeparamref name="TItem"/>) to invoke over each item.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory2D{T}"/> representing the data to process.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<TItem, TAction>(ReadOnlyMemory2D<TItem> memory)
            where TAction : struct, IInAction<TItem>
        {
            ForEach(memory, default(TAction), 1);
        }

        /// <summary>
        /// Executes a specified action in an optimized parallel loop over the input data.
        /// </summary>
        /// <typeparam name="TItem">The type of items to iterate over.</typeparam>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IInAction{T}"/> of <typeparamref name="TItem"/>) to invoke over each item.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory2D{T}"/> representing the data to process.</param>
        /// <param name="minimumActionsPerThread">
        /// The minimum number of actions to run per individual thread. Set to 1 if all invocations
        /// should be parallelized, or to a greater number if each individual invocation is fast
        /// enough that it is more efficient to set a lower bound per each running thread.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<TItem, TAction>(ReadOnlyMemory2D<TItem> memory, int minimumActionsPerThread)
            where TAction : struct, IInAction<TItem>
        {
            ForEach(memory, default(TAction), minimumActionsPerThread);
        }

        /// <summary>
        /// Executes a specified action in an optimized parallel loop over the input data.
        /// </summary>
        /// <typeparam name="TItem">The type of items to iterate over.</typeparam>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IInAction{T}"/> of <typeparamref name="TItem"/>) to invoke over each item.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory2D{T}"/> representing the data to process.</param>
        /// <param name="action">The <typeparamref name="TAction"/> instance representing the action to invoke.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<TItem, TAction>(ReadOnlyMemory2D<TItem> memory, in TAction action)
            where TAction : struct, IInAction<TItem>
        {
            ForEach(memory, action, 1);
        }

        /// <summary>
        /// Executes a specified action in an optimized parallel loop over the input data.
        /// </summary>
        /// <typeparam name="TItem">The type of items to iterate over.</typeparam>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IInAction{T}"/> of <typeparamref name="TItem"/>) to invoke over each item.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory2D{T}"/> representing the data to process.</param>
        /// <param name="action">The <typeparamref name="TAction"/> instance representing the action to invoke.</param>
        /// <param name="minimumActionsPerThread">
        /// The minimum number of actions to run per individual thread. Set to 1 if all invocations
        /// should be parallelized, or to a greater number if each individual invocation is fast
        /// enough that it is more efficient to set a lower bound per each running thread.
        /// </param>
        public static void ForEach<TItem, TAction>(ReadOnlyMemory2D<TItem> memory, in TAction action, int minimumActionsPerThread)
            where TAction : struct, IInAction<TItem>
        {
            if (minimumActionsPerThread <= 0)
            {
                ThrowArgumentOutOfRangeExceptionForInvalidMinimumActionsPerThread();
            }

            if (memory.IsEmpty)
            {
                return;
            }

            int
                maxBatches = 1 + ((memory.Size - 1) / minimumActionsPerThread),
                clipBatches = Math.Min(maxBatches, memory.Height),
                cores = Environment.ProcessorCount,
                numBatches = Math.Min(clipBatches, cores),
                batchHeight = 1 + ((memory.Height - 1) / numBatches);

            var actionInvoker = new InActionInvokerWithReaadOnlyMemory2D<TItem, TAction>(batchHeight, memory, action);

            // Skip the parallel invocation when possible
            if (numBatches == 1)
            {
                actionInvoker.Invoke(0);

                return;
            }

            // Run the batched operations in parallel
            Parallel.For(
                0,
                numBatches,
                new ParallelOptions { MaxDegreeOfParallelism = numBatches },
                actionInvoker.Invoke);
        }

        // Wrapping struct acting as explicit closure to execute the processing batches
        private readonly struct InActionInvokerWithReaadOnlyMemory2D<TItem, TAction>
            where TAction : struct, IInAction<TItem>
        {
            private readonly int batchHeight;
            private readonly ReadOnlyMemory2D<TItem> memory;
            private readonly TAction action;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public InActionInvokerWithReaadOnlyMemory2D(
                int batchHeight,
                ReadOnlyMemory2D<TItem> memory,
                in TAction action)
            {
                this.batchHeight = batchHeight;
                this.memory = memory;
                this.action = action;
            }

            /// <summary>
            /// Processes the batch of actions at a specified index
            /// </summary>
            /// <param name="i">The index of the batch to process</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Invoke(int i)
            {
                int
                    lowY = i * this.batchHeight,
                    highY = lowY + this.batchHeight,
                    stopY = Math.Min(highY, this.memory.Height),
                    width = this.memory.Width;

                ReadOnlySpan2D<TItem> span = this.memory.Span;

                for (int y = lowY; y < stopY; y++)
                {
                    ref TItem r0 = ref span.DangerousGetReferenceAt(y, 0);

                    for (int x = 0; x < width; x++)
                    {
                        Unsafe.AsRef(this.action).Invoke(Unsafe.Add(ref r0, x));
                    }
                }
            }
        }
    }
}
