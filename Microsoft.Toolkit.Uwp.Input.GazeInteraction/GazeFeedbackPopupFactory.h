#pragma once

using namespace Windows::UI::Xaml::Controls::Primitives;

BEGIN_NAMESPACE_GAZE_INPUT

private ref class GazeFeedbackPopupFactory
{
public:

    static Popup^ Get();

    static void Return(Popup^ popup);
};

END_NAMESPACE_GAZE_INPUT
