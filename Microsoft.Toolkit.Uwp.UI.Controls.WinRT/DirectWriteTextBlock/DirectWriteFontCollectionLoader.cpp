// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "pch.h"
#include <string>
#include <sstream>
#include <iostream>
#include "DirectWriteFontFileEnumerator.h"
#include "DirectWriteFontCollectionLoader.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

winrt::com_ptr<IDWriteFontCollectionLoader> DirectWriteFontCollectionLoader::s_comInstance;

DirectWriteFontCollectionLoader::DirectWriteFontCollectionLoader()
{
}

DirectWriteFontCollectionLoader::~DirectWriteFontCollectionLoader()
{
}

bool DirectWriteFontCollectionLoader::HasCustomFontFamily(Platform::String^ xamlFontFamily)
{
    // is there a .ttf in the path?
    std::wstring wstringPath{ xamlFontFamily->Data() };
    std::transform(wstringPath.begin(), wstringPath.end(), wstringPath.begin(), towlower);
    auto foundCustomFontFile = wstringPath.find(L".ttf#", 0);
    return foundCustomFontFile != std::wstring::npos;
}

void DirectWriteFontCollectionLoader::ParseXamlFontFamily(_In_ Platform::String^ xamlFontFamily, _Out_ DirectWriteUniversalPackageFontData& parsedFont)
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

bool DirectWriteFontCollectionLoader::FindCachedEnumerator(Platform::String^ xamlFontFamily, winrt::com_ptr<IDWriteFontFileEnumerator>& enumerator)
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

IFACEMETHODIMP DirectWriteFontCollectionLoader::CreateEnumeratorFromKey(_In_ IDWriteFactory* factory, void const* collectionKey, unsigned int collectionKeySize, _Outptr_ IDWriteFontFileEnumerator** fontFileEnumerator)
try
{
    *fontFileEnumerator = nullptr;
    auto xamlFontFamily = ref new Platform::String(reinterpret_cast<const wchar_t*>(collectionKey), collectionKeySize);

    if (HasCustomFontFamily(xamlFontFamily))
    {
        winrt::com_ptr<IDWriteFontFileEnumerator> cachedEnumerator;
        if (!FindCachedEnumerator(xamlFontFamily, cachedEnumerator))
        {
            auto enumerator{ winrt::make_self<DirectWriteFontFileEnumerator>() };
            DirectWriteUniversalPackageFontData parseData = {};
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

winrt::com_ptr<IDWriteFontCollectionLoader>& DirectWriteFontCollectionLoader::GetInstance()
{
    if (!s_comInstance)
    {
        s_comInstance = winrt::make<DirectWriteFontCollectionLoader>();
    }

    return s_comInstance;
}

END_NAMESPACE_CONTROLS_WINRT