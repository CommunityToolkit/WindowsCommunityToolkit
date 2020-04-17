//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include <winrt/Microsoft.UI.Xaml.Controls.Primitives.h>

using namespace std;
using namespace winrt::Microsoft::UI::Xaml::Controls::Primitives;

BEGIN_NAMESPACE_GAZE_INPUT

class GazeFeedbackPopupFactory
{
private:

    std::vector<Popup> s_cache;

public:

    Popup Get();

    void Return(Popup const& popup);
};

END_NAMESPACE_GAZE_INPUT
