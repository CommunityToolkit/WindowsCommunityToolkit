#include "pch.h"
#include "GazeFeedbackPopupFactory.h"

BEGIN_NAMESPACE_GAZE_INPUT

Popup^ GazeFeedbackPopupFactory::Get()
{
    Popup^ popup;

    if (s_cache->Size != 0)
    {
        popup = s_cache->GetAt(0);
        s_cache->RemoveAt(0);
    }
    else
    {
        popup = ref new Popup();

        auto rectangle = ref new Rectangle();
        rectangle->StrokeThickness = 2;

        popup->Child = rectangle;
    }

    return popup;
}

void GazeFeedbackPopupFactory::Return(Popup^ popup)
{
    popup->IsOpen = false;
    s_cache->Append(popup);
}

END_NAMESPACE_GAZE_INPUT