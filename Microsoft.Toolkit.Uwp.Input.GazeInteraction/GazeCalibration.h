#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

using namespace Windows::Devices::Input::Preview;
using namespace Windows::Foundation;

public ref class GazeCalibration sealed
{
private:

    GazeDeviceWatcherPreview^ _watcher;
    EventRegistrationToken _addedToken;
    EventRegistrationToken _completedToken;
    bool _done;

    GazeCalibration()
    {
        _watcher = GazeInputSourcePreview::CreateWatcher();
        _addedToken = _watcher->Added += ref new TypedEventHandler<GazeDeviceWatcherPreview^, GazeDeviceWatcherAddedPreviewEventArgs^>(this, &GazeCalibration::OnDeviceAdded);
        _completedToken = _watcher->EnumerationCompleted += ref new TypedEventHandler<GazeDeviceWatcherPreview^, Object^>(this, &GazeCalibration::OnEnumerationCompleted);
        _watcher->Start();
    }

    void OnDeviceAdded(GazeDeviceWatcherPreview^ sender, GazeDeviceWatcherAddedPreviewEventArgs^ args)
    {
        if (!_done)
        {
            _done = true;

            args->Device->RequestCalibrationAsync();
        }
    }

    void OnEnumerationCompleted(GazeDeviceWatcherPreview^ sender, Object^ args)
    {
        _watcher->Stop();
        _watcher->Added -= _addedToken;
        _watcher->EnumerationCompleted -= _completedToken;
        _watcher = nullptr;
    }

public:

    static Object^ Run()
    {
        auto calibration = ref new GazeCalibration();
        return calibration;
    }
};

END_NAMESPACE_GAZE_INPUT