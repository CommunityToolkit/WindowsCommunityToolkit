#include "pch.h"
#include "UniversalWindowsAppPackageFontLoader\FontCollectionLoader.h"
#include "DirectWriteResourceManager.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

std::unique_ptr<DirectWriteResourceManager> DirectWriteResourceManager::s_instance;

DirectWriteResourceManager::DirectWriteResourceManager()
{
}

DirectWriteResourceManager::~DirectWriteResourceManager()
{
}

IDWriteFactory* DirectWriteResourceManager::GetDirectWriteFactoryNoRef()
{
    winrt::check_hresult(InitializeDeviceResources());
    return m_dwriteFactory.get();
}

ID3D11Device* DirectWriteResourceManager::GetD3dDeviceNoRef()
{
    winrt::check_hresult(InitializeDeviceResources());
    return m_d3dDevice.get();
}

ID2D1DeviceContext* DirectWriteResourceManager::GetD2dDCNoRef()
{
    winrt::check_hresult(InitializeDeviceResources());
    return m_d2dContext.get();
}

IDXGIDevice* DirectWriteResourceManager::GetDXGIDeviceNoRef()
{
    winrt::check_hresult(InitializeDeviceResources());
    return m_dxgiDevice.get();
}

HRESULT DirectWriteResourceManager::InitializeDeviceResources()
{
    if (!m_deviceResourcesInitialized)
    {
        UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT;
        D3D_FEATURE_LEVEL supportedFeatureLevel;

        D3D_FEATURE_LEVEL featureLevels[] =
        {
            D3D_FEATURE_LEVEL_11_1,
            D3D_FEATURE_LEVEL_11_0,
            D3D_FEATURE_LEVEL_10_1,
            D3D_FEATURE_LEVEL_10_0,
            D3D_FEATURE_LEVEL_9_3,
            D3D_FEATURE_LEVEL_9_2,
            D3D_FEATURE_LEVEL_9_1
        };

        HRESULT hr = D3D11CreateDevice(
            nullptr,                    // Specify nullptr to use the default adapter.
            D3D_DRIVER_TYPE_HARDWARE,   // Create a device using the hardware graphics driver.
            0,                          // Should be 0 unless the driver is D3D_DRIVER_TYPE_SOFTWARE.
            creationFlags,              // Set debug and Direct2D compatibility flags.
            featureLevels,              // List of feature levels this app can support.
            ARRAYSIZE(featureLevels),
            D3D11_SDK_VERSION,          // Always set this to D3D11_SDK_VERSION for Windows Store apps.
            m_d3dDevice.put(),             // Returns the Direct3D device created.
            &supportedFeatureLevel,
            nullptr
        );

        // log failures on the initial creation
        if (FAILED(hr))
        {
            // If the initialization fails, fall back to the WARP device.
            winrt::check_hresult(D3D11CreateDevice(
                nullptr,
                D3D_DRIVER_TYPE_WARP, // Create a WARP device instead of a hardware device.
                0,
                creationFlags,
                featureLevels,
                ARRAYSIZE(featureLevels),
                D3D11_SDK_VERSION,
                m_d3dDevice.put(),
                &supportedFeatureLevel,
                nullptr
            ));
        }

        // Get the Direct3D 11.1 API device.
        m_d3dDevice.as(m_dxgiDevice);
        winrt::check_hresult(D2D1CreateDevice(m_dxgiDevice.get(), nullptr, m_d2dDevice.put()));

        winrt::check_hresult(m_d2dDevice->CreateDeviceContext(
            D2D1_DEVICE_CONTEXT_OPTIONS_NONE,
            m_d2dContext.put()
        ));

        winrt::check_hresult(DWriteCreateFactory(
            DWRITE_FACTORY_TYPE_SHARED,
            __uuidof(IDWriteFactory),
            reinterpret_cast<IUnknown**>(m_dwriteFactory.put())
        ));

        auto customLoader = UniversalWindowsAppPackageFontLoader::FontCollectionLoader::GetInstance();
        m_dwriteFactory->RegisterFontCollectionLoader(customLoader.get());

        m_deviceResourcesInitialized = true;
    }

    return S_OK;
}

DirectWriteResourceManager* DirectWriteResourceManager::GetInstance()
{
    if (s_instance == nullptr)
    {
        s_instance.reset(new DirectWriteResourceManager());
    }

    return s_instance.get();
}

END_NAMESPACE_CONTROLS_WINRT