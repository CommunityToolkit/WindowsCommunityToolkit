//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeElement.h"

using namespace std;
using namespace Platform;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::UI;
using namespace Windows::UI::ViewManagement;
using namespace Windows::UI::Xaml::Automation::Peers;
using namespace Windows::UI::Xaml::Hosting;

BEGIN_NAMESPACE_GAZE_INPUT

DependencyProperty^ const GazeElement::s_hasAttentionProperty = DependencyProperty::Register("HasAttention", bool::typeid, GazeElement::typeid, ref new PropertyMetadata(false));
DependencyProperty^ const GazeElement::s_invokeProgressProperty = DependencyProperty::Register("InvokeProgress", double::typeid, GazeElement::typeid, ref new PropertyMetadata(0.0));

END_NAMESPACE_GAZE_INPUT