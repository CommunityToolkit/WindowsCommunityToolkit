//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include <unknwn.h>
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.UI.Composition.h>
#include <winrt/Windows.Devices.HumanInterfaceDevice.h>
#include <winrt/Windows.Devices.Input.Preview.h>
#include <winrt/Microsoft.UI.Xaml.h>
#include <winrt/Microsoft.UI.Xaml.Controls.h>
#include <winrt/Microsoft.UI.Xaml.Controls.Primitives.h>
#include <winrt/Microsoft.UI.Xaml.Data.h>
#include <winrt/Microsoft.UI.Xaml.Markup.h>
#include <winrt/Microsoft.UI.Xaml.Navigation.h>
#include <winrt/Microsoft.UI.Xaml.Shapes.h>

//#include <collection.h>
//#include <ppltasks.h>

//#define _USE_MATH_DEFINES
//#include <math.h>
//#include <assert.h>

//#include <Windows.Input.Gaze.h>

//using namespace winrt;
using namespace winrt::Windows::Foundation;
//using namespace winrt::Windows::Devices::Enumeration;
//using namespace winrt::Windows::Devices::HumanInterfaceDevice;
//using namespace winrt::Windows::UI::Core;
//using namespace winrt::Microsoft::UI::Xaml;
//using namespace winrt::Microsoft::UI::Xaml::Media;
//using namespace winrt::Microsoft::UI::Xaml::Shapes;

//#include <strsafe.h>
//private class Debug
//{
//public:
//    static void WriteLine(wchar_t *format, ...)
//    {
//        wchar_t message[1024];
//        va_list args;
//        va_start(args, format);
//        StringCchVPrintf(message, 1024, format, args);
//        OutputDebugString(message);
//        OutputDebugString(L"\n");
//    }
//};

inline static TimeSpan operator + (const TimeSpan& lhs, const TimeSpan& rhs) { return TimeSpan{ lhs.count() + rhs.count() }; }
inline static TimeSpan operator - (const TimeSpan& lhs, const TimeSpan& rhs) { return TimeSpan{ lhs.count() - rhs.count() }; }
inline static TimeSpan operator * (int lhs, const TimeSpan& rhs) { return TimeSpan{ lhs*rhs.count() }; }
inline static bool operator < (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.count() < rhs.count(); }
inline static bool operator <= (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.count() <= rhs.count(); }
inline static bool operator > (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.count() > rhs.count(); }
inline static bool operator >= (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.count() >= rhs.count(); }
inline static bool operator == (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.count() == rhs.count(); }
inline static bool operator != (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.count() != rhs.count(); }

static TimeSpan TimeSpanZero{ 0 };
inline static TimeSpan TimeSpanFromMicroseconds(unsigned __int64 milliseconds) { return TimeSpan{ (__int64)(10 * milliseconds) }; }
inline static TimeSpan TimeSpanFromMicroseconds(__int64 milliseconds) { return TimeSpan{ 10 * milliseconds }; }
inline static TimeSpan TimeSpanFromMicroseconds(int milliseconds) { return TimeSpan{ 10 * milliseconds }; }

#define BEGIN_NAMESPACE_GAZE_INPUT namespace Microsoft { namespace Toolkit { namespace Uwp { namespace Input { namespace GazeInteraction {
#define END_NAMESPACE_GAZE_INPUT } } } } }
