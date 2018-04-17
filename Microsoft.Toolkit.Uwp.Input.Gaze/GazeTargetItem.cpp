//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeTargetItem.h"
#include "GazeFeedbackPopupFactory.h"

using namespace std;
using namespace Platform;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::UI;
using namespace Windows::UI::ViewManagement;
using namespace Windows::UI::Xaml::Automation::Peers;
using namespace Windows::UI::Xaml::Hosting;
using namespace Windows::UI::Xaml::Media;

static Brush^ ProgressingBrush = ref new SolidColorBrush(Colors::Green);
static Brush^ CompleteBrush = ref new SolidColorBrush(Colors::Red);

BEGIN_NAMESPACE_GAZE_INPUT

void GazeTargetItem::RaiseProgressEvent(GazeProgressState state)
{
    switch (state)
    {
    case GazeProgressState::Idle:
        if (_notifiedProgressState != state)
        {
            Debug::WriteLine(L"Now in Idle state");
        }
        break;
    case GazeProgressState::Progressing:
        Debug::WriteLine(L"Now progressing %f", ((double)(ElapsedTime - _prevStateTime)) / (_nextStateTime - _prevStateTime));
        break;
    case GazeProgressState::Complete:
        if (_notifiedProgressState != state)
        {
            Debug::WriteLine(L"Now complete");
        }
    }

    if (_notifiedProgressState != state || state == GazeProgressState::Progressing)
    {
        if (state != GazeProgressState::Idle)
        {
            if (_feedbackPopup == nullptr)
            {
                _feedbackPopup = GazeFeedbackPopupFactory::Get();
            }

            auto control = safe_cast<Control^>(TargetElement);

            auto transform = control->TransformToVisual(_feedbackPopup);
            auto bounds = transform->TransformBounds(*ref new Rect(*ref new Point(0, 0), *ref new Size(control->ActualWidth, control->ActualHeight)));
            auto rectangle = safe_cast<Rectangle^>(_feedbackPopup->Child);

            if (state == GazeProgressState::Progressing)
            {
                auto progress = ((double)(ElapsedTime - _prevStateTime)) / (_nextStateTime - _prevStateTime);

                rectangle->Stroke = ProgressingBrush;
                rectangle->Width = (1 - progress) * bounds.Width;
                rectangle->Height = (1 - progress) * bounds.Height;

                _feedbackPopup->HorizontalOffset = bounds.Left + progress * bounds.Width / 2; ;
                _feedbackPopup->VerticalOffset = bounds.Top + progress * bounds.Height / 2; ;
            }
            else
            {
                rectangle->Stroke = CompleteBrush;
                rectangle->Width = bounds.Width;
                rectangle->Height = bounds.Height;

                _feedbackPopup->HorizontalOffset = bounds.Left;
                _feedbackPopup->VerticalOffset = bounds.Top;
            }

            _feedbackPopup->IsOpen = true;
        }
        else
        {
            if (_feedbackPopup != nullptr)
            {
                GazeFeedbackPopupFactory::Return(_feedbackPopup);
                _feedbackPopup = nullptr;
            }
        }
    }

    _notifiedProgressState = state;
}

END_NAMESPACE_GAZE_INPUT