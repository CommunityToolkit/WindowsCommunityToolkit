//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

namespace Shapes = Windows::UI::Xaml::Shapes;

BEGIN_NAMESPACE_GAZE_INPUT

ref struct GazeHistoryItem
{
    property UIElement^ HitTarget;
    property int64 Timestamp;
    property int64 Duration
    {
        int64 get() { assert(_duration1 == _duration2); return _duration1; }
        void set(int64 value)
        {
            assert(_duration1 == _duration2);
            Debug::WriteLine(L" h %ld -> %ld", _duration1, value);
            _duration1 = value;
            _duration2 = value;
        }
    }

private:
    int64 _duration1;
    int64 _duration2;
};

END_NAMESPACE_GAZE_INPUT