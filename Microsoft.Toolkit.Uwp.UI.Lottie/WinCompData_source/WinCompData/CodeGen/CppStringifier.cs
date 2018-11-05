// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Wui;
using System;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen
{
    /// <summary>
    /// Stringifiers for C++ syntax.
    /// </summary>
    sealed class CppStringifier : InstantiatorGeneratorBase.StringifierBase
    {
        public override string CanvasFigureLoop(Mgcg.CanvasFigureLoop value)
        {
            switch (value)
            {
                case Mgcg.CanvasFigureLoop.Open:
                    return "D2D1_FIGURE_END_OPEN";
                case Mgcg.CanvasFigureLoop.Closed:
                    return "D2D1_FIGURE_END_CLOSED";
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string CanvasGeometryCombine(Mgcg.CanvasGeometryCombine value)
        {
            switch (value)
            {
                case Mgcg.CanvasGeometryCombine.Union:
                    return "D2D1_COMBINE_MODE_UNION";
                case Mgcg.CanvasGeometryCombine.Exclude:
                    return "D2D1_COMBINE_MODE_EXCLUDE";
                case Mgcg.CanvasGeometryCombine.Intersect:
                    return "D2D1_COMBINE_MODE_INTERSECT";
                case Mgcg.CanvasGeometryCombine.Xor:
                    return "D2D1_COMBINE_MODE_XOR";
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string Color(Color value) => $"ColorHelper::FromArgb({Hex(value.A)}, {Hex(value.R)}, {Hex(value.G)}, {Hex(value.B)})";

        public override string Deref => "->";

        public override string FilledRegionDetermination(Mgcg.CanvasFilledRegionDetermination value)
        {
            switch (value)
            {
                case Mgcg.CanvasFilledRegionDetermination.Alternate:
                    return "D2D1_FILL_MODE_ALTERNATE";
                case Mgcg.CanvasFilledRegionDetermination.Winding:
                    return "D2D1_FILL_MODE_WINDING";
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string Int64(long value) => $"{value}L";

        public override string Int64TypeName => "int64_t";

        public override string ScopeResolve => "::";

        public override string New => "ref new";

        public override string Null => "nullptr";

        public override string Matrix3x2(Matrix3x2 value)
        {
            return $"{{{Float(value.M11)}, {Float(value.M12)}, {Float(value.M21)}, {Float(value.M22)}, {Float(value.M31)}, {Float(value.M32)}}}";
        }

        public override string Matrix4x4(Matrix4x4 value)
        {
            return $"{{{Float(value.M11)}, {Float(value.M12)}, {Float(value.M13)}, {Float(value.M14)}, {Float(value.M21)}, {Float(value.M22)}, {Float(value.M23)}, {Float(value.M24)}, {Float(value.M31)}, {Float(value.M32)}, {Float(value.M33)}, {Float(value.M34)}, {Float(value.M41)}, {Float(value.M42)}, {Float(value.M43)}, {Float(value.M44)}}}";
        }

        public override string Readonly(string value) => $"{value} const";

        public override string ReferenceTypeName(string value) =>
            value == "CanvasGeometry"
                // C++ uses a typdef for CanvasGeometry that is ComPtr<GeoSource>, thus no hat pointer
                ? "CanvasGeometry"
                : $"{value}^";

        public override string TimeSpan(TimeSpan value) => TimeSpan(Int64(value.Ticks));
        public override string TimeSpan(string ticks) => $"{{ {ticks} }}";

        public override string Var => "auto";

        public override string Vector2(Vector2 value) => $"{{ {Float(value.X)}, {Float(value.Y)} }}";

        public override string Vector3(Vector3 value) => $"{{ {Float(value.X)}, {Float(value.Y)}, {Float(value.Z)} }}";

        public override string IListAdd => "Append";

        public override string FactoryCall(string value) => $"CanvasGeometryToIGeometrySource2D({value})";

        public string FailFastWrapper(string value) => $"FFHR({value})";

        /// <summary>
        /// Returns the code for a class that wraps an ID2D1Geometry in an IGeometrySource2DInterop
        /// as required by CompositionPath.
        /// This class will be included inline in every codegen so that the generated code
        /// doesn't need to depend on another file.
        /// The implementation is very simple - just enough to satisfy CompositionPath.
        /// </summary>
        public string GeoSourceClass =>
@"class GeoSource final :
    public ABI::Windows::Graphics::IGeometrySource2D,
    public ABI::Windows::Graphics::IGeometrySource2DInterop
 {
    ULONG _cRef;
    ComPtr<ID2D1Geometry> _cpGeometry;

public:
    GeoSource(ID2D1Geometry* pGeometry)
        : _cRef(1)
        , _cpGeometry(pGeometry)
    { }

    IFACEMETHODIMP QueryInterface(REFIID iid, void ** ppvObject) override
    {
        if (iid == __uuidof(ABI::Windows::Graphics::IGeometrySource2DInterop))
        {
            AddRef();
            *ppvObject = static_cast<ABI::Windows::Graphics::IGeometrySource2DInterop*>(this);
            return S_OK;
        }
        return E_NOINTERFACE;
    }

    IFACEMETHODIMP_(ULONG) AddRef() override
    {
        return InterlockedIncrement(&_cRef);
    }

    IFACEMETHODIMP_(ULONG) Release() override
    {
        ULONG cRef = InterlockedDecrement(&_cRef);
        if (cRef == 0)
        {
            delete this;
        }
        return cRef;
    }

    IFACEMETHODIMP GetIids(ULONG*, IID**) override
    {
        return E_NOTIMPL;
    }

    IFACEMETHODIMP GetRuntimeClassName(HSTRING*) override
    {
        return E_NOTIMPL;
    }

    IFACEMETHODIMP GetTrustLevel(TrustLevel*) override
    {
        return E_NOTIMPL;
    }

    IFACEMETHODIMP GetGeometry(ID2D1Geometry** value) override
    {
        *value = _cpGeometry.Get();
        (*value)->AddRef();
        return S_OK;
    }

    IFACEMETHODIMP TryGetGeometryUsingFactory(ID2D1Factory*, ID2D1Geometry**) override
    {
        return E_NOTIMPL;
    }
};
";
    }
}
