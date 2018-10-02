#include "pch.h"
#include "FontFileEnumerator.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

namespace UniversalWindowsAppPackageFontLoader
{
    using namespace Windows::Storage;

    FontFileEnumerator::FontFileEnumerator()
    {
    }

    FontFileEnumerator::~FontFileEnumerator()
    {
    }

    HRESULT FontFileEnumerator::Initialize(_In_ IDWriteFactory* factory, const UniversalPackageFontData& packageFont)
    try
    {
        if ((factory == nullptr) || packageFont.packageFontFilePath.empty() || packageFont.customFontName.empty())
        {
            winrt::throw_hresult(E_INVALIDARG);
        }

        m_factory.attach(factory);
        m_packageFontArgs = packageFont;
        return S_OK;
    }
    catch (...)
    {
        return winrt::to_hresult();
    }

    IFACEMETHODIMP FontFileEnumerator::MoveNext(_Out_ BOOL* hasCurrentFile)
    try
    {
        *hasCurrentFile = FALSE;
        if (!m_enumerated)
        {
            m_currentFontFile = nullptr;
            auto uwappStorage = Windows::ApplicationModel::Package::Current->InstalledLocation;
            std::wstring filePath{ uwappStorage->Path->Data() };
            filePath.append(m_packageFontArgs.packageFontFilePath);
            winrt::check_hresult(m_factory->CreateFontFileReference(filePath.c_str(), nullptr, m_currentFontFile.put()));

            *hasCurrentFile = TRUE;
            m_enumerated = true;
        }

        return S_OK;
    }
    catch (...)
    {
        return winrt::to_hresult();
    }

    IFACEMETHODIMP FontFileEnumerator::GetCurrentFontFile(_Outptr_ IDWriteFontFile** fontFile)
    try
    {
        m_currentFontFile.copy_to(fontFile);
        return S_OK;
    }
    catch (...)
    {
        return winrt::to_hresult();
    }
}

END_NAMESPACE_CONTROLS_WINRT