//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include <unknwn.h>
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.Devices.HumanInterfaceDevice.h>
#include <winrt/Windows.Devices.Input.Preview.h>
#include <winrt/Microsoft.UI.Composition.h>
#include <winrt/Microsoft.UI.Xaml.h>
#include <winrt/Microsoft.UI.Xaml.Controls.h>
#include <winrt/Microsoft.UI.Xaml.Controls.Primitives.h>
#include <winrt/Microsoft.UI.Xaml.Data.h>
#include <winrt/Microsoft.UI.Xaml.Markup.h>
#include <winrt/Microsoft.UI.Xaml.Navigation.h>
#include <winrt/Microsoft.UI.Xaml.Shapes.h>
#include <winrt/Microsoft.UI.Xaml.Interop.h>

//#include <collection.h>
//#include <ppltasks.h>

//#define _USE_MATH_DEFINES
//#include <math.h>
//#include <assert.h>

using namespace winrt;
using namespace winrt::Windows::Foundation;
//using namespace winrt::Windows::Devices::Enumeration;
//using namespace winrt::Windows::Devices::HumanInterfaceDevice;
using namespace winrt::Windows::UI::Core;
using namespace winrt::Microsoft::UI::Xaml;
using namespace winrt::Microsoft::UI::Xaml::Media;
using namespace winrt::Microsoft::UI::Xaml::Shapes;

#include <strsafe.h>
class Debug
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
        va_end(args);
    }
};

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

namespace winrt::impl
{
    template <typename T>
    struct xaml_typename_name
    {
        static constexpr std::wstring_view value() noexcept
        {
            return name_of<T>();
        }
    };
    template <>
    struct xaml_typename_name<Windows::Foundation::Point>
    {
        static constexpr std::wstring_view value() noexcept
        {
            return L"Point"sv;
        }
    };
    template <>
    struct xaml_typename_name<Windows::Foundation::Size>
    {
        static constexpr std::wstring_view value() noexcept
        {
            return L"Size"sv;
        }
    };
    template <>
    struct xaml_typename_name<Windows::Foundation::Rect>
    {
        static constexpr std::wstring_view value() noexcept
        {
            return L"Rect"sv;
        }
    };
    template <>
    struct xaml_typename_name<Windows::Foundation::DateTime>
    {
        static constexpr std::wstring_view value() noexcept
        {
            return L"DateTime"sv;
        }
    };
    template <>
    struct xaml_typename_name<Windows::Foundation::TimeSpan>
    {
        static constexpr std::wstring_view value() noexcept
        {
            return L"TimeSpan"sv;
        }
    };

    template <typename T>
    struct xaml_typename_kind
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Metadata;
    };
    template<>
    struct xaml_typename_kind<bool>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<char16_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<uint8_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<int8_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<uint16_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<int16_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<uint32_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<int32_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<uint64_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<int64_t>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<float>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<double>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<hstring>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
    template<>
    struct xaml_typename_kind<guid>
    {
        static constexpr Windows::UI::Xaml::Interop::TypeKind value = Windows::UI::Xaml::Interop::TypeKind::Primitive;
    };
}

WINRT_EXPORT namespace winrt
{
    template <typename T>
    inline Windows::UI::Xaml::Interop::TypeName xaml_typename()
    {
        static const Windows::UI::Xaml::Interop::TypeName name{ hstring{ impl::xaml_typename_name<T>::value() }, impl::xaml_typename_kind<T>::value };
        return name;
    }
}

#define BEGIN_NAMESPACE_GAZE_INPUT namespace winrt { namespace Microsoft { namespace Toolkit { namespace Uwp { namespace Input { namespace GazeInteraction {
#define END_NAMESPACE_GAZE_INPUT } } } } } }
