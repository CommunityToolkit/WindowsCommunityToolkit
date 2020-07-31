// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

// The ByReference<T> type included in this project is kept internal, so
// that users referencing the Microsoft.Toolkit.HighPerformance package will
// need to go through the (overhead free) abstraction of the Ref<T> types
// present in that assembly. This makes codebases using this package more
// resilient to changes in the runtime, and the code itself more portable.
// We need to provide the full public key here, as this assembly is signed.
// See the .csproj file for more info on why this is needed.
[assembly: InternalsVisibleTo("Microsoft.Toolkit.HighPerformance, PublicKey=002400000480000094000000060200000024000052534131000400000100010041753af735ae6140c9508567666c51c6ab929806adb0d210694b30ab142a060237bc741f9682e7d8d4310364b4bba4ee89cc9d3d5ce7e5583587e8ea44dca09977996582875e71fb54fa7b170798d853d5d8010b07219633bdb761d01ac924da44576d6180cdceae537973982bb461c541541d58417a3794e34f45e6f2d129e2")]
