//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include <collection.h>
#include <ppltasks.h>

//#define _USE_MATH_DEFINES
//#include <math.h>
#include <assert.h>

//#include <Windows.Input.Gaze.h>

using namespace Windows::Foundation;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Shapes;

#include <strsafe.h>
private class Debug
{
public:
    static void WriteLine(wchar_t *format, ...)
    {
        wchar_t message[1024];
        va_list args;
        va_start(args, format);
        StringCchVPrintf(message, 1024, format, args);
        OutputDebugString(message);
        OutputDebugString(L"\n");
    }
};

inline static TimeSpan operator + (const TimeSpan& lhs, const TimeSpan& rhs) { return TimeSpan{ lhs.Duration + rhs.Duration }; }
inline static TimeSpan operator - (const TimeSpan& lhs, const TimeSpan& rhs) { return TimeSpan{ lhs.Duration - rhs.Duration }; }
inline static TimeSpan operator * (int lhs, const TimeSpan& rhs) { return TimeSpan{ lhs*rhs.Duration }; }
inline static bool operator < (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.Duration < rhs.Duration; }
inline static bool operator <= (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.Duration <= rhs.Duration; }
inline static bool operator > (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.Duration > rhs.Duration; }
inline static bool operator >= (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.Duration >= rhs.Duration; }
inline static bool operator == (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.Duration == rhs.Duration; }
inline static bool operator != (const TimeSpan& lhs, const TimeSpan& rhs) { return lhs.Duration != rhs.Duration; }

static TimeSpan TimeSpanZero{ 0 };
inline static TimeSpan TimeSpanFromMicroseconds(uint64 milliseconds) { return TimeSpan{ (int64)(10 * milliseconds) }; }
inline static TimeSpan TimeSpanFromMicroseconds(int64 milliseconds) { return TimeSpan{ 10 * milliseconds }; }
inline static TimeSpan TimeSpanFromMicroseconds(int milliseconds) { return TimeSpan{ 10 * milliseconds }; }

#define BEGIN_NAMESPACE_GAZE_INPUT namespace Microsoft { namespace Toolkit { namespace Uwp { namespace Input { namespace GazeInteraction {
#define END_NAMESPACE_GAZE_INPUT } } } } }
