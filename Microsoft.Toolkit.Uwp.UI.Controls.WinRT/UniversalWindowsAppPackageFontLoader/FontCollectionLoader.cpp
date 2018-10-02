#include "pch.h"
#include <string>
#include <sstream>
#include <iostream>
#include "FontFileEnumerator.h"
#include "FontCollectionLoader.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

namespace UniversalWindowsAppPackageFontLoader
{
    winrt::com_ptr<IDWriteFontCollectionLoader> FontCollectionLoader::s_comInstance;

    FontCollectionLoader::FontCollectionLoader()
    {
    }

    FontCollectionLoader::~FontCollectionLoader()
    {
    }

    bool FontCollectionLoader::HasCustomFontFamily(Platform::String^ xamlFontFamily)
    {
        // is there a .ttf in the path?
        std::wstring wstringPath{ xamlFontFamily->Data() };
        std::transform(wstringPath.begin(), wstringPath.end(), wstringPath.begin(), towlower);
        auto foundCustomFontFile = wstringPath.find(L".ttf#", 0);
        return foundCustomFontFile != std::wstring::npos;
    }

    void FontCollectionLoader::ParseXamlFontFamily(_In_ Platform::String^ xamlFontFamily, _Out_ UniversalPackageFontData& parsedFont)
    {
        parsedFont = {};
        std::wstring wstringPath{ xamlFontFamily->Data() };
        auto delimLocation = wstringPath.find(L'#');
        if (delimLocation != std::wstring::npos)
        {
            auto path = wstringPath.substr(0, delimLocation);
            std::replace(path.begin(), path.end(), L'/', L'\\');
            parsedFont.packageFontFilePath = path;
            parsedFont.customFontName = wstringPath.substr(delimLocation + 1);
        }
    }

    bool FontCollectionLoader::FindCachedEnumerator(Platform::String^ xamlFontFamily, winrt::com_ptr<IDWriteFontFileEnumerator>& enumerator)
    {
        for (auto& entry : m_fontEnumerators)
        {
            if (entry.customFont->Equals(xamlFontFamily))
            {
                enumerator = entry.enumerator;
                return true;
            }
        }

        return false;
    }

    IFACEMETHODIMP FontCollectionLoader::CreateEnumeratorFromKey(_In_ IDWriteFactory* factory, void const* collectionKey, unsigned int collectionKeySize, _Outptr_ IDWriteFontFileEnumerator** fontFileEnumerator)
    try
    {
        *fontFileEnumerator = nullptr;
        auto xamlFontFamily = ref new Platform::String(reinterpret_cast<const wchar_t*>(collectionKey), collectionKeySize);

        if (HasCustomFontFamily(xamlFontFamily))
        {
            winrt::com_ptr<IDWriteFontFileEnumerator> cachedEnumerator;
            if (!FindCachedEnumerator(xamlFontFamily, cachedEnumerator))
            {
                auto enumerator{ winrt::make_self<FontFileEnumerator>() };
                UniversalPackageFontData parseData = {};
                ParseXamlFontFamily(xamlFontFamily, parseData);
                winrt::check_hresult(enumerator->Initialize(factory, parseData));

                FontEnumeratorEntry entry = {};
                entry.customFont = xamlFontFamily;
                entry.enumerator = enumerator;
                m_fontEnumerators.push_back(std::move(entry));
                cachedEnumerator = enumerator;
            }

            winrt::check_hresult(cachedEnumerator->QueryInterface(IID_PPV_ARGS(fontFileEnumerator)));
        }
        else
        {
            winrt::throw_hresult(E_INVALIDARG);
        }

        return S_OK;
    }
    catch (...)
    {
        return winrt::to_hresult();
    }

    winrt::com_ptr<IDWriteFontCollectionLoader>& FontCollectionLoader::GetInstance()
    {
        if (!s_comInstance)
        {
            s_comInstance = winrt::make<FontCollectionLoader>();
        }

        return s_comInstance;
    }
}

END_NAMESPACE_CONTROLS_WINRT