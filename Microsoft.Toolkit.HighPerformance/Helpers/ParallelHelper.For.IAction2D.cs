// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.HighPerformance.Helpers
{
    /// <summary>
    /// Helpers to work with parallel code in a highly optimized manner.
    /// </summary>
    public static partial class ParallelHelper
    {
        /// <summary>
        /// Executes a specified action in an optimized parallel loop.
        /// </summary>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IAction2D"/>) to invoke for each pair of iteration indices.</typeparam>
        /// <param name="i">The outer iteration range.</param>
        /// <param name="j">The inner iteration range.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008", Justification = "ValueTuple<T1,T2> type")]
        public static void For2D<TAction>((int Start, int End) i, (int Start, int End) j)
            where TAction : struct, IAction2D
        {
            For2D(i, j, default(TAction), 1);
        }

        /// <summary>
        /// Executes a specified action in an optimized parallel loop.
        /// </summary>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IAction2D"/>) to invoke for each pair of iteration indices.</typeparam>
        /// <param name="i">The outer iteration range.</param>
        /// <param name="j">The inner iteration range.</param>
        /// <param name="minimumActionsPerThread">
        /// The minimum number of actions to run per individual thread. Set to 1 if all invocations
        /// should be parallelized, or to a greater number if each individual invocation is fast
        /// enough that it is more efficient to set a lower bound per each running thread.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008", Justification = "ValueTuple<T1,T2> type")]
        public static void For2D<TAction>((int Start, int End) i, (int Start, int End) j, int minimumActionsPerThread)
            where TAction : struct, IAction2D
        {
            For2D(i, j, default(TAction), minimumActionsPerThread);
        }

        /// <summary>
        /// Executes a specified action in an optimized parallel loop.
        /// </summary>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IAction2D"/>) to invoke for each pair of iteration indices.</typeparam>
        /// <param name="i">The outer iteration range.</param>
        /// <param name="j">The inner iteration range.</param>
        /// <param name="action">The <typeparamref name="TAction"/> instance representing the action to invoke.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008", Justification = "ValueTuple<T1,T2> type")]
        public static void For2D<TAction>((int Start, int End) i, (int Start, int End) j, in TAction action)
            where TAction : struct, IAction2D
        {
            For2D(i, j, action, 1);
        }

        /// <summary>
        /// Executes a specified action in an optimized parallel loop.
        /// </summary>
        /// <typeparam name="TAction">The type of action (implementing <see cref="IAction2D"/>) to invoke for each pair of iteration indices.</typeparam>
        /// <param name="i">The outer iteration range.</param>
        /// <param name="j">The inner iteration range.</param>
        /// <param name="action">The <typeparamref name="TAction"/> instance representing the action to invoke.</param>
        /// <param name="minimumActionsPerThread">
        /// The minimum number of actions to run per individual thread. Set to 1 if all invocations
        /// should be parallelized, or to a greater number if each individual invocation is fast
        /// enough that it is more efficient to set a lower bound per each running thread.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008", Justification = "ValueTuple<T1,T2> type")]
        public static void For2D<TAction>((int Start, int End) i, (int Start, int End) j, in TAction action, int minimumActionsPerThread)
            where TAction : struct, IAction2D
        {
            if (minimumActionsPerThread <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumActionsPerThread),
                    "Each thread needs to perform at least one action");
            }

            if (i.Start > i.End)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Start i index must be less than end");
            }

            if (j.Start > j.End)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Start j index must be less than end");
            }

            if (i.Start == i.End && j.Start == j.End)
            {
                return;
            }

            int
                height = Math.Abs(i.Start - i.End),
                width = Math.Abs(j.Start - j.End),
                count = height * width,
                maxBatches = 1 + ((count - 1) / minimumActionsPerThread),
                cores = Environment.ProcessorCount,
                numBatches = Math.Min(maxBatches, cores);

            // Skip the parallel invocation when a single batch is needed
            if (numBatches == 1)
            {
                for (int y = i.Start; y < i.End; y++)
                {
                    for (int x = j.Start; x < j.End; x++)
                    {
                        Unsafe.AsRef(action).Invoke(y, x);
                    }
                }

                return;
            }

            int batchHeight = 1 + ((height - 1) / numBatches);

            var actionInvoker = new Action2DInvoker<TAction>(i.Start, i.End, j.Start, j.End, batchHeight, action);

            // Run the batched operations in parallel
            Parallel.For(
                0,
                numBatches,
                new ParallelOptions { MaxDegreeOfParallelism = numBatches },
                actionInvoker.Invoke);
        }

        // Wrapping struct acting as explicit closure to execute the processing batches
        private readonly struct Action2DInvoker<TAction>
            where TAction : struct, IAction2D
        {
            private readonly int startY;
            private readonly int endY;
            private readonly int startX;
            private readonly int endX;
            private readonly int batchHeight;
            private readonly TAction action;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Action2DInvoker(
                int startY,
                int endY,
                int startX,
                int endX,
                int batchHeight,
                in TAction action)
            {
                this.startY = startY;
                this.endY = endY;
                this.startX = startX;
                this.endX = endX;
                this.batchHeight = batchHeight;
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
                    heightOffset = i * batchHeight,
                    lowY = startY + heightOffset,
                    highY = lowY + heightOffset,
                    stopY = Math.Min(highY, endY);

                for (int y = lowY; y < stopY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        Unsafe.AsRef(action).Invoke(y, x);
                    }
                }
            }
        }
    }

    /// <summary>
    /// A contract for actions being executed with two input indices.
    /// </summary>
    /// <remarks>If the <see cref="Invoke"/> method is small enough, it is highly recommended to mark it with <see cref="MethodImplOptions.AggressiveInlining"/>.</remarks>
    public interface IAction2D
    {
        /// <summary>
        /// Executes the action associated with two specified indices.
        /// </summary>
        /// <param name="i">The first index for the action to execute.</param>
        /// <param name="j">The second index for the action to execute.</param>
        void Invoke(int i, int j);
    }
}
