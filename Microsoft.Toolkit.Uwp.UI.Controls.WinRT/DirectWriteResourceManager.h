#pragma once

namespace winrt::Microsoft_Toolkit_Uwp_UI_Controls_WinRT::implementation
{
    /// <summary>
    /// This manages D3D/DWrite resources and prevents reinitialization.
    /// <summary>
    class DirectWriteResourceManager
    {
    public:
        DirectWriteResourceManager();
        virtual ~DirectWriteResourceManager();

        /// <summary>
        /// This is a singleton to prevent reintializing D3D
        /// <summary>
        static DirectWriteResourceManager* GetInstance();

        /// <summary>
        /// The DWrite Factory, caller doesn't take a reference.
        /// <summary>
        IDWriteFactory* GetDirectWriteFactoryNoRef();

        /// <summary>
        /// The D3D Device, caller doesn't take a reference.
        /// <summary>
        ID3D11Device* GetD3dDeviceNoRef();

        /// <summary>
        /// The D2D Device, caller doesn't take a reference.
        /// <summary>
        ID2D1DeviceContext* GetD2dDCNoRef();

        /// <summary>
        /// The DXGI Device, caller doesn't take a reference.
        /// <summary>
        IDXGIDevice* GetDXGIDeviceNoRef();

        /// <summary>
        /// Initializes the device resources.
        /// <summary>
        HRESULT InitializeDeviceResources();

    private:
        winrt::com_ptr<IDWriteFactory> m_dwriteFactory;
        winrt::com_ptr<ID3D11Device> m_d3dDevice;
        winrt::com_ptr<ID2D1Device> m_d2dDevice;
        winrt::com_ptr<ID2D1DeviceContext> m_d2dContext;
        winrt::com_ptr<IDXGIDevice> m_dxgiDevice;

        static std::unique_ptr<DirectWriteResourceManager> s_instance;
        bool m_deviceResourcesInitialized = false;
    };
}