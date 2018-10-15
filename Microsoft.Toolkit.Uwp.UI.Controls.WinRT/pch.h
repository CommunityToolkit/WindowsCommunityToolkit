// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once

// these are c++/winrt default includes : https://docs.microsoft.com/en-us/windows/uwp/cpp-and-winrt-apis/author-coclasses
#include <unknwn.h>
#include <winrt/Windows.Foundation.h>

#include <vector>
#include <memory>
#include <algorithm>
#include <string>
#include <d3d11.h>
#include <d2d1_1.h>
#include <dwrite.h>

#define BEGIN_NAMESPACE_CONTROLS_WINRT namespace Microsoft { namespace Toolkit { namespace Uwp { namespace UI { namespace Controls { namespace WinRT {
#define END_NAMESPACE_CONTROLS_WINRT } } } } } }

#include "DirectWriteTextBlock\DirectWriteTextBlock.h"

// it would be nice to run Code Analysis on this project, but the Code Analyzer crashes due to the generated XAML files and apparently can't find pch.h