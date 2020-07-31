// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE_RUNTIME

using System.Runtime.CompilerServices;

// This is needed so that the runtime will actually let us access the internal
// ByReference<T> type exposed through the fake System.Private.CoreLib assembly
// that is referenced by this project. The reason for this is that while the proxy
// type we declared belongs to a project we have internals access to, the real
// ByReference<T> from the actual CoreLib assembly does not, so the runtime would
// normally fail to load the type if this attribute wasn't present.
// Note that while this attribute is internally recognized by the runtime, we still
// need the fake CoreLib assembly to be able to use the type when building from source,
// as Roslyn itself does not actually recognize and/or respect it.
[assembly: IgnoresAccessChecksTo("System.Private.CoreLib")]

#endif
