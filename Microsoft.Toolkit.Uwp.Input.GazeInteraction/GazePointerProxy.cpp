#include "pch.h"
#include "GazePointerProxy.h"
#include "GazePointer.h"

BEGIN_NAMESPACE_GAZE_INPUT

DependencyProperty^ GazePointerProxy::GazePointerProxyProperty::get()
{
    static auto value = DependencyProperty::RegisterAttached("_GazePointerProxy", GazePointerProxy::typeid, GazePointerProxy::typeid,
        ref new PropertyMetadata(nullptr));
    return value;
}

GazePointerProxy::GazePointerProxy(FrameworkElement^ element)
    : _element(element)
{
    // element.Loaded has already happened if it is in the visual tree...
    auto parent = VisualTreeHelper::GetParent(element);
    if (parent != nullptr)
    {
        _isLoaded = true;
    }
    // ...or...
    else
    {
        // ...if the element is a dynamically created Popup that has been opened.
        auto popup = dynamic_cast<Popup^>(element);
        _isLoaded = popup != nullptr && popup->IsOpen;
    }

    element->Loaded += ref new RoutedEventHandler(this, &GazePointerProxy::OnPageLoaded);
    element->Unloaded += ref new RoutedEventHandler(this, &GazePointerProxy::OnPageUnloaded);
}

void GazePointerProxy::SetGazeInteraction(FrameworkElement^ element, Interaction value)
{
    auto proxy = safe_cast<GazePointerProxy^>(element->GetValue(GazePointerProxyProperty));
    if (proxy == nullptr)
    {
        proxy = ref new GazePointerProxy(element);
        element->SetValue(GazePointerProxyProperty, proxy);
    }

    proxy->IsEnabled = value;
}

void GazePointerProxy::IsEnabled::set(Interaction value)
{
    if (_isEnabled != value)
    {
        _isEnabled = value;

        if (_isLoaded)
        {
            if (value == Interaction::Enabled)
            {
                GazePointer::Instance->AddRoot(_element);
            }
            else
            {
                GazePointer::Instance->RemoveRoot(_element);
            }
        }
    }
}

void GazePointerProxy::OnPageLoaded(Object^ sender, RoutedEventArgs^ args)
{
    assert(!_isLoaded);

    _isLoaded = true;

    if (_isEnabled == Interaction::Enabled)
    {
        GazePointer::Instance->AddRoot(_element);
    }
}

void GazePointerProxy::OnPageUnloaded(Object^ sender, RoutedEventArgs^ args)
{
    assert(_isLoaded);

    _isLoaded = false;

    if (_isEnabled == Interaction::Enabled)
    {
        GazePointer::Instance->RemoveRoot(_element);
    }
}

END_NAMESPACE_GAZE_INPUT