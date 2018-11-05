// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

// If defined, an issue will be reported for each field that is discovered
// but not parsed. This is used to help test that parsing is complete.
#define CheckForUnparsedFields
using PathGeometry = Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Sequence<Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.BezierSegment>;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

#if CheckForUnparsedFields
using JObject = Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization.CheckedJsonObject;
using JArray = Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization.CheckedJsonArray;
#endif

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization
{
    // See: https://github.com/airbnb/lottie-web/tree/master/docs/json for the (usually out-of-date) schema.
    // See: https://helpx.adobe.com/pdf/after_effects_reference.pdf for the After Effects semantics.
#if PUBLIC
    public
#endif
    sealed class LottieCompositionReader
    {
        static readonly AnimatableFloatParser s_animatableFloatParser = new AnimatableFloatParser();
        static readonly AnimatableVector2Parser s_animatableVector2Parser = new AnimatableVector2Parser();
        static readonly AnimatableVector3Parser s_animatableVector3Parser = new AnimatableVector3Parser();
        static readonly AnimatableColorParser s_animatableColorParser = new AnimatableColorParser();
        static readonly AnimatableGeometryParser s_animatableGeometryParser = new AnimatableGeometryParser();
        static readonly JsonLoadSettings s_jsonLoadSettings = new JsonLoadSettings
        {
            // Ignore commands and line info. Not needed and makes the parser a bit faster.
            CommentHandling = CommentHandling.Ignore,
            LineInfoHandling = LineInfoHandling.Ignore
        };

        readonly ParsingIssues _issues = new ParsingIssues();

        Options _options;

        /// <summary>
        /// Specifies optional behavior for the reader.
        /// </summary>
        public enum Options
        {
            None = 0,
            /// <summary>
            /// Do not read the Name values.
            /// </summary>
            IgnoreNames,
            /// <summary>
            /// Do not read the Match Name values.
            /// </summary>
            IgnoreMatchNames,
        }

        /// <summary>
        /// Parses a Lottie file to create a <see cref="LottieData.LottieComposition"/>.
        /// </summary>
        public static LottieComposition ReadLottieCompositionFromJsonStream(Stream stream, Options options, out (string Code, string Description)[] issues)
        {
            JsonReader jsonReader;
            try
            {
                var streamReader = new StreamReader(stream);
                jsonReader = new JsonTextReader(streamReader);
            }
            catch (Exception e)
            {
                var issueCollector = new ParsingIssues();
                issueCollector.FailedToParseJson(e.Message);
                issues = issueCollector.GetIssues();
                return null;
            }

            return ReadLottieCompositionFromJson(jsonReader, options, out issues);
        }


        LottieCompositionReader(Options options) { _options = options; }

        static LottieComposition ReadLottieCompositionFromJson(JsonReader jsonReader, Options options, out (string Code, string Description)[] issues)
        {
            var reader = new LottieCompositionReader(options);
            LottieComposition result = null;
            try
            {
                result = reader.ParseLottieComposition(jsonReader);
            }
            catch (JsonReaderException e)
            {
                reader._issues.FatalError(e.Message);
            }
            catch (LottieCompositionReaderException e)
            {
                reader._issues.FatalError(e.Message);
            }
            issues = reader._issues.GetIssues();
            return result;
        }

        LottieComposition ParseLottieComposition(JsonReader reader)
        {
            string version = null;
            double? framesPerSecond = null;
            double? inPoint = null;
            double? outPoint = null;
            double? width = null;
            double? height = null;
            string name = null;
            bool? is3d = null;
            var assets = new Asset[0];
            var chars = new Char[0];
            var fonts = new Font[0];
            var layers = new Layer[0];
            var markers = new Marker[0];

            ConsumeToken(reader);

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                    case JsonToken.StartArray:
                    case JsonToken.StartConstructor:
                    case JsonToken.Raw:
                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.String:
                    case JsonToken.Boolean:
                    case JsonToken.Null:
                    case JsonToken.Undefined:
                    case JsonToken.EndArray:
                    case JsonToken.EndConstructor:
                    case JsonToken.Date:
                    case JsonToken.Bytes:
                        // Here means the JSON was invalid or our parser got confused.
                        throw UnexpectedTokenException(reader);

                    case JsonToken.Comment:
                        // Ignore comments.
                        ConsumeToken(reader);
                        break;

                    case JsonToken.PropertyName:
                        var currentProperty = (string)reader.Value;

                        ConsumeToken(reader);

                        switch (currentProperty)
                        {
                            case "assets":
                                assets = ParseArrayOf(reader, ParseAsset).ToArray();
                                break;
                            case "chars":
                                chars = ParseArrayOf(reader, ParseChar).ToArray();
                                break;
                            case "comps":
                                _issues.IgnoredField("comps");
                                ConsumeArray(reader);
                                break;
                            case "ddd":
                                is3d = ParseBool(reader);
                                break;
                            case "fr":
                                framesPerSecond = ParseDouble(reader);
                                break;
                            case "fonts":
                                fonts = ParseFonts(reader).ToArray();
                                break;
                            case "layers":
                                layers = ParseLayers(reader);
                                break;
                            case "h":
                                height = ParseDouble(reader);
                                break;
                            case "ip":
                                inPoint = ParseDouble(reader);
                                break;
                            case "op":
                                outPoint = ParseDouble(reader);
                                break;
                            case "markers":
                                markers = ParseArrayOf(reader, ParseMarker).ToArray();
                                break;
                            case "nm":
                                name = (string)reader.Value;
                                break;
                            case "v":
                                version = (string)reader.Value;
                                break;
                            case "w":
                                width = ParseDouble(reader);
                                break;

                            default:
                                throw UnexpectedTokenException(reader);
                        }
                        break;

                    case JsonToken.EndObject:
                        {
                            if (version == null)
                            {
                                throw new LottieCompositionReaderException("Version parameter not found.");
                            }

                            if (!width.HasValue)
                            {
                                throw new LottieCompositionReaderException("Width parameter not found.");
                            }

                            if (!height.HasValue)
                            {
                                throw new LottieCompositionReaderException("Height parameter not found.");
                            }

                            if (!inPoint.HasValue)
                            {
                                throw new LottieCompositionReaderException("Start frame parameter not found.");
                            }

                            if (!outPoint.HasValue)
                            {
                                throw new LottieCompositionReaderException("End frame parameter not found.");
                            }

                            int[] versions = new[] { 0, 0, 0 };
                            try
                            {
                                versions = version.Split('.').Select(int.Parse).ToArray();
                            }
                            catch (FormatException)
                            {
                                // Ignore
                            }
                            catch (OverflowException)
                            {
                                // Ignore
                            }

                            if (layers == null)
                            {
                                throw new LottieCompositionReaderException("No layers found.");
                            }

                            var result = new LottieComposition(
                                                name: name ?? "",
                                                width: width ?? 0.0,
                                                height: height ?? 0.0,
                                                inPoint: inPoint ?? 0.0,
                                                outPoint: outPoint ?? 0.0,
                                                framesPerSecond: framesPerSecond ?? 0.0,
                                                is3d: false,
                                                version: new Version(versions[0], versions[1], versions[2]),
                                                assets: new AssetCollection(assets),
                                                chars: chars,
                                                fonts: fonts,
                                                layers: new LayerCollection(layers),
                                                markers: markers);
                            return result;
                        }

                    default:
                        throw new InvalidOperationException();
                }
            }
            throw EofException;
        }

        Marker ParseMarker(JsonReader reader)
        {
            ExpectToken(reader, JsonToken.StartObject);

            string name = null;
            double durationSeconds = 0;
            double progress = 0;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var currentProperty = (string)reader.Value;

                        ConsumeToken(reader);

                        switch (currentProperty)
                        {
                            case "cm":
                                name = (string)reader.Value;
                                break;
                            case "dr":
                                durationSeconds = ParseDouble(reader);
                                break;
                            case "tm":
                                progress = ParseDouble(reader);
                                break;
                            default:
                                throw UnexpectedFieldException(reader, currentProperty);
                        }
                        break;
                    case JsonToken.EndObject:
                        return new Marker(progress, name, durationSeconds);
                    default:
                        throw UnexpectedTokenException(reader);
                }
            }
            throw EofException;
        }

        Asset ParseAsset(JsonReader reader)
        {
            ExpectToken(reader, JsonToken.StartObject);

            int e = 0;
            string id = null;
            double width = 0.0;
            double height = 0.0;
            string imagePath = null;
            string fileName = null;
            string name = null;
            Layer[] layers = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        {
                            var currentProperty = (string)reader.Value;
                            ConsumeToken(reader);

                            switch (currentProperty)
                            {
                                case "e":
                                    // TODO: unknown what this is. It shows up in image assets.
                                    e = ParseInt(reader);
                                    break;
                                case "h":
                                    height = ParseDouble(reader);
                                    break;
                                case "id":
                                    // Older lotties use a string. New lotties use an int. Handle either as strings.
                                    switch (reader.TokenType)
                                    {
                                        case JsonToken.String:
                                            id = (string)reader.Value;
                                            break;
                                        case JsonToken.Integer:
                                            id = ParseInt(reader).ToString();
                                            break;
                                        default:
                                            throw UnexpectedTokenException(reader);
                                    }
                                    break;
                                case "layers":
                                    layers = ParseLayers(reader);
                                    break;
                                case "nm":
                                    // TODO - not sure why, but shows up in one Layers asset in the corpus.
                                    name = (string)reader.Value;
                                    break;
                                case "p":
                                    fileName = (string)reader.Value;
                                    break;
                                case "u":
                                    imagePath = (string)reader.Value;
                                    break;
                                case "w":
                                    width = ParseDouble(reader);
                                    break;
                                case "xt":
                                    // TODO - unknown - seen once in Layers asset in the corpus.
                                    var xt = ParseInt(reader);
                                    break;
                                default:
                                    throw UnexpectedFieldException(reader, currentProperty);
                            }
                        }
                        break;
                    case JsonToken.EndObject:
                        {
                            if (id == null)
                            {
                                throw Exception("Asset with no id", reader);
                            }

                            if (layers != null)
                            {
                                return new LayerCollectionAsset(id, new LayerCollection(layers));
                            }
                            else if (imagePath != null && fileName != null)
                            {
                                return new ImageAsset(id, width, height, imagePath, fileName);
                            }
                            else
                            {
                                _issues.AssetType("NaN");
                                return null;
                            }
                        }
                    default: throw UnexpectedTokenException(reader);
                }
            }

            throw EofException;
        }

        Char ParseChar(JsonReader reader)
        {
            ExpectToken(reader, JsonToken.StartObject);

            string ch = null;
            string fFamily = null;
            double? size = null;
            string style = null;
            double? width = null;
            List<ShapeLayerContent> shapes = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        {
                            var currentProperty = (string)reader.Value;
                            ConsumeToken(reader);

                            switch (currentProperty)
                            {
                                case "ch":
                                    ch = (string)reader.Value;
                                    break;
                                case "data":
                                    shapes = ReadShapes(JObject.Load(reader, s_jsonLoadSettings));
                                    break;
                                case "fFamily":
                                    fFamily = (string)reader.Value;
                                    break;
                                case "size":
                                    size = ParseDouble(reader);
                                    break;
                                case "style":
                                    style = (string)reader.Value;
                                    break;
                                case "w":
                                    width = ParseDouble(reader);
                                    break;
                                default:
                                    throw UnexpectedFieldException(reader, currentProperty);
                            }
                        }
                        break;
                    case JsonToken.EndObject:
                        {
                            return new Char(ch, fFamily, style, size ?? 0, width ?? 0, shapes);
                        }

                    default: throw UnexpectedTokenException(reader);

                }
            }
            throw EofException;
        }

        IEnumerable<Font> ParseFonts(JsonReader reader)
        {
            var fontsObject = JObject.Load(reader, s_jsonLoadSettings);
            foreach (JObject item in fontsObject.GetNamedArray("list"))
            {
                var fName = item.GetNamedString("fName");
                var fFamily = item.GetNamedString("fFamily");
                var fStyle = item.GetNamedString("fStyle");
                var ascent = ReadFloat(item.GetNamedValue("ascent"));
                AssertAllFieldsRead(item);
                yield return new Font(fName, fFamily, fStyle, ascent);
            }
            AssertAllFieldsRead(fontsObject);
        }

        Layer[] ParseLayers(JsonReader reader)
        {
            return LoadArrayOfJObjects(reader).Select(a => ReadLayer(a)).Where(a => a != null).ToArray();
        }

        // May return null if there was a problem reading the layer.
        Layer ReadLayer(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "bounds");
            IgnoreFieldThatIsNotYetSupported(obj, "sy");
            IgnoreFieldThatIsNotYetSupported(obj, "td");

            var name = obj.GetNamedString("nm", "");
            var layerIndex = ReadInt(obj, "ind").Value;
            var parentIndex = ReadInt(obj, "parent");
            var is3d = ReadBool(obj, "ddd") == true;
            var autoOrient = ReadBool(obj, "ao") == true;
            var blendMode = BmToBlendMode(obj.GetNamedNumber("bm", 0));
            var isHidden = ReadBool(obj, "hd") == true;
            var render = ReadBool(obj, "render") != false;

            if (!render)
            {
                _issues.LayerWithRenderFalse();
                return null;
            }

            // Warnings
            if (name.EndsWith(".ai") || obj.GetNamedString("cl", "") == "ai")
            {
                _issues.IllustratorLayers();
            }

            if (obj.ContainsKey("ef"))
            {
                _issues.LayerEffects();
            }

            if (obj.ContainsKey("tt"))
            {
                _issues.Mattes();
            }

            // ----------------------
            // Layer Transform
            // ----------------------

            var transform = ReadTransform(obj.GetNamedObject("ks"));

            // ------------------------------
            // Layer Animation
            // ------------------------------
            var timeStretch = obj.GetNamedNumber("sr", 1.0);
            // Time when the layer starts
            var startFrame = obj.GetNamedNumber("st");

            // Time when the layer becomes visible.
            var inFrame = obj.GetNamedNumber("ip");
            var outFrame = obj.GetNamedNumber("op");

            // Field 'hasMask' is deprecated and thus we are intentionally ignoring it
            IgnoreFieldIntentionally(obj, "hasMask");

            // NOTE: The spec specifies this as 'maskProperties' but the BodyMovin tool exports
            // 'masksProperties' with the plural 'masks'.
            var maskProperties = obj.GetNamedArray("masksProperties", null);
            var masks = maskProperties != null ? ReadMaskProperties(maskProperties) : null;

            switch (TyToLayerType(obj.GetNamedNumber("ty", double.NaN)))
            {
                case Layer.LayerType.PreComp:
                    {
                        var refId = obj.GetNamedString("refId", "");
                        var width = obj.GetNamedNumber("w");
                        var height = obj.GetNamedNumber("h");
                        var tm = obj.GetNamedObject("tm", null);
                        if (tm != null)
                        {
                            _issues.TimeRemappingOfPreComps();
                        }

                        AssertAllFieldsRead(obj);
                        return new PreCompLayer(
                            name,
                            layerIndex,
                            parentIndex,
                            isHidden,
                            transform,
                            timeStretch,
                            startFrame,
                            inFrame,
                            outFrame,
                            blendMode,
                            is3d,
                            autoOrient,
                            refId,
                            width,
                            height,
                            masks);
                    }
                case Layer.LayerType.Solid:
                    {
                        var solidWidth = ReadInt(obj, "sw").Value;
                        var solidHeight = ReadInt(obj, "sh").Value;
                        var solidColor = GetSolidColorFromString(obj.GetNamedString("sc"));
                        AssertAllFieldsRead(obj);
                        return new SolidLayer(
                            name,
                            layerIndex,
                            parentIndex,
                            isHidden,
                            transform,
                            solidWidth,
                            solidHeight,
                            solidColor,
                            timeStretch,
                            startFrame,
                            inFrame,
                            outFrame,
                            blendMode,
                            is3d,
                            autoOrient,
                            masks);
                    }
                case Layer.LayerType.Image:
                    {
                        var refId = obj.GetNamedString("refId", "");

                        AssertAllFieldsRead(obj);
                        return new ImageLayer(
                            name,
                            layerIndex,
                            parentIndex,
                            isHidden,
                            transform,
                            timeStretch,
                            startFrame,
                            inFrame,
                            outFrame,
                            blendMode,
                            is3d,
                            autoOrient,
                            refId,
                            masks);
                    }
                case Layer.LayerType.Null:
                    {
                        AssertAllFieldsRead(obj);

                        return new NullLayer(
                            name,
                            layerIndex,
                            parentIndex,
                            isHidden,
                            transform,
                            timeStretch,
                            startFrame,
                            inFrame,
                            outFrame,
                            blendMode,
                            is3d,
                            autoOrient,
                            masks);
                    }
                case Layer.LayerType.Shape:
                    {
                        var shapes = ReadShapes(obj);

                        AssertAllFieldsRead(obj);
                        return new ShapeLayer(
                            name,
                            shapes,
                            layerIndex,
                            parentIndex,
                            isHidden,
                            transform,
                            timeStretch,
                            startFrame,
                            inFrame,
                            outFrame,
                            blendMode,
                            is3d,
                            autoOrient,
                            masks);
                    }
                case Layer.LayerType.Text:
                    {
                        // Text layer references an asset.
                        var refId = obj.GetNamedString("refId", "");

                        // Text data.
                        ReadTextData(obj.GetNamedObject("t"));

                        AssertAllFieldsRead(obj);
                        return new TextLayer(
                            name,
                            layerIndex,
                            parentIndex,
                            isHidden,
                            transform,
                            timeStretch,
                            startFrame,
                            inFrame,
                            outFrame,
                            blendMode,
                            is3d,
                            autoOrient,
                            refId,
                            masks);
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        void ReadTextData(JObject obj)
        {
            // TODO - read text data

            // Animatable text value
            // "t":text
            // "f":fontName
            // "s":size
            // "j":(int)justification
            // "tr":(int)tracking
            // "lh":lineHeight
            // "ls":baselineShift
            // "fc":fillColor
            // "sc":strokeColor
            // "sw":strokeWidth
            // "of":(bool)strokeOverFill
            IgnoreFieldThatIsNotYetSupported(obj, "d");

            IgnoreFieldThatIsNotYetSupported(obj, "p");
            IgnoreFieldThatIsNotYetSupported(obj, "m");

            // Array of animatable text properties (fc:fill color, sc:stroke color, sw:stroke width, t:tracking (float))
            IgnoreFieldThatIsNotYetSupported(obj, "a");
            AssertAllFieldsRead(obj);
        }

        List<ShapeLayerContent> ReadShapes(JObject obj)
        {
            return ReadShapesList(obj.GetNamedArray("shapes", null));
        }

        List<ShapeLayerContent> ReadShapesList(JArray shapesJson)
        {
            var shapes = new List<ShapeLayerContent>();
            if (shapesJson != null)
            {
                var shapesJsonCount = shapesJson.Count;
                shapes.Capacity = shapesJsonCount;
                for (var i = 0; i < shapesJsonCount; i++)
                {
                    var item = ReadShapeContent(shapesJson[i].AsObject());
                    if (item != null)
                    {
                        shapes.Add(item);
                    }
                }
            }
            return shapes;
        }


        IEnumerable<Mask> ReadMaskProperties(JArray array)
        {
            foreach (var elem in array)
            {
                var obj = elem.AsObject();

                // Ignoring field 'x' because it is not in the official spec
                // The x property refers to the mask expansion. In AE you can 
                // expand or shrink a mask getting a reduced or expanded version of the same shape.
                IgnoreFieldThatIsNotYetSupported(obj, "x");

                var inverted = obj.GetNamedBoolean("inv");
                var name = obj.GetNamedString("nm");
                var animatedGeometry = ReadAnimatableGeometry(obj.GetNamedObject("pt"));
                var opacity = ReadAnimatableFloat(obj.GetNamedObject("o"));
                var mode = Mask.MaskMode.None;
                var maskMode = obj.GetNamedString("mode");
                switch (maskMode)
                {
                    case "a":
                        mode = Mask.MaskMode.Additive;
                        break;
                    case "d":
                        mode = Mask.MaskMode.Darken;
                        break;
                    case "f":
                        mode = Mask.MaskMode.Difference;
                        break;
                    case "i":
                        mode = Mask.MaskMode.Intersect;
                        break;
                    case "l":
                        mode = Mask.MaskMode.Lighten;
                        break;
                    case "n":
                        mode = Mask.MaskMode.None;
                        break;
                    case "s":
                        mode = Mask.MaskMode.Subtract;
                        break;
                    default:
                        throw new LottieCompositionReaderException($"Unexpected mask mode: {maskMode}");
                }

                AssertAllFieldsRead(obj);
                yield return new Mask(
                    inverted,
                    name,
                    animatedGeometry,
                    opacity,
                    mode
                );
            }
        }

        static Color GetSolidColorFromString(string hex)
        {
            var index = 1; // Skip '#'
                           // '#AARRGGBB'
            byte a = 255;
            if (hex.Length == 9)
            {
                a = Convert.ToByte(hex.Substring(index, 2), 16);
                index += 2;
            }
            var r = Convert.ToByte(hex.Substring(index, 2), 16);
            index += 2;
            var g = Convert.ToByte(hex.Substring(index, 2), 16);
            index += 2;
            var b = Convert.ToByte(hex.Substring(index, 2), 16);
            return Color.FromArgb(a / 255.0, r / 255.0, g / 255.0, b / 255.0);
        }

        ShapeLayerContent ReadShapeContent(JObject obj)
        {
            var type = obj.GetNamedString("ty");

            switch (type)
            {
                case "gr":
                    return ReadShapeGroup(obj);
                case "st":
                    return ReadSolidColorStroke(obj);
                case "gs":
                    return ReadGradientStroke(obj);
                case "fl":
                    return ReadSolidColorFill(obj);
                case "gf":
                    return ReadGradientFill(obj);
                case "tr":
                    return ReadTransform(obj);
                case "el":
                    return ReadEllipse(obj);
                case "sr":
                    return ReadPolystar(obj);
                case "rc":
                    return ReadRectangle(obj);
                case "sh":
                    return ReadShape(obj);
                case "tm":
                    return ReadTrimPath(obj);
                case "mm":
                    return ReadMergePaths(obj);
                case "rd":
                    return ReadRoundedCorner(obj);
                case "rp":
                    return ReadRepeater(obj);
                default:
                    break;
            }
            _issues.UnexpectedShapeContentType(type);
            return null;
        }

        ShapeGroup ReadShapeGroup(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "cix");
            IgnoreFieldThatIsNotYetSupported(obj, "cl");
            IgnoreFieldThatIsNotYetSupported(obj, "ix");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var numberOfProperties = ReadInt(obj, "np");
            var items = ReadShapesList(obj.GetNamedArray("it", null));
            AssertAllFieldsRead(obj);
            return new ShapeGroup(name.Name, name.MatchName, items);
        }

        // "st"
        SolidColorStroke ReadSolidColorStroke(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "fillEnabled");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var color = ReadColor(obj);
            var opacityPercent = ReadOpacityPercent(obj);
            var strokeWidth = ReadAnimatableFloat(obj.GetNamedObject("w"));
            var capType = LcToLineCapType(obj.GetNamedNumber("lc"));
            var joinType = LjToLineJoinType(obj.GetNamedNumber("lj"));
            var miterLimit = obj.GetNamedNumber("ml", 4); // Default miter limit in AfterEffects is 4

            // Get dash pattern to be set as StrokeDashArray
            Animatable<double> offset = null;
            var dashPattern = new List<double>();
            var dashesJson = obj.GetNamedArray("d", null);
            if (dashesJson != null)
            {
                for (int i = 0; i < dashesJson.Count; i++)
                {
                    var dashObj = dashesJson[i].AsObject();

                    switch (dashObj.GetNamedString("n"))
                    {
                        case "o":
                            offset = ReadAnimatableFloat(dashObj.GetNamedObject("v"));
                            break;
                        case "d":
                        case "g":
                            dashPattern.Add(ReadAnimatableFloat(dashObj.GetNamedObject("v")).InitialValue);
                            break;
                    }
                }
            }

            AssertAllFieldsRead(obj);
            return new SolidColorStroke(
                name.Name,
                name.MatchName,
                offset ?? new Animatable<double>(0, null),
                dashPattern,
                color,
                opacityPercent,
                strokeWidth,
                capType,
                joinType,
                miterLimit);
        }

        // gs
        ShapeLayerContent ReadGradientStroke(JObject obj)
        {
            switch (TToGradientType(obj.GetNamedNumber("t")))
            {
                case GradientType.Linear:
                    return ReadLinearGradientStroke(obj);
                case GradientType.Radial:
                    return ReadRadialGradientStroke(obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        LinearGradientStroke ReadLinearGradientStroke(JObject obj)
        {
            _issues.GradientStrokes();

            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "hd");
            IgnoreFieldThatIsNotYetSupported(obj, "g");
            IgnoreFieldThatIsNotYetSupported(obj, "t");
            // highlightLength - ReadAnimatableFloat(obj.GetNamedObject("h")) - but is optional
            IgnoreFieldThatIsNotYetSupported(obj, "h");
            // highlightAngle - ReadAnimatableFloat(obj.GetNamedObject("a")) - but is optional
            IgnoreFieldThatIsNotYetSupported(obj, "1");

            var name = ReadName(obj);
            var opacityPercent = ReadOpacityPercent(obj);
            var strokeWidth = ReadAnimatableFloat(obj.GetNamedObject("w"));
            var capType = LcToLineCapType(obj.GetNamedNumber("lc"));
            var joinType = LjToLineJoinType(obj.GetNamedNumber("lj"));
            var miterLimit = obj.GetNamedNumber("ml", 4); // Default miter limit in AfterEffects is 4
            var startPoint = ReadAnimatableVector3(obj.GetNamedObject("s"));
            var endPoint = ReadAnimatableVector3(obj.GetNamedObject("e"));

            AssertAllFieldsRead(obj);
            return new LinearGradientStroke(
                name.Name,
                name.MatchName,
                opacityPercent,
                strokeWidth,
                capType,
                joinType,
                miterLimit);
        }

        RadialGradientStroke ReadRadialGradientStroke(JObject obj)
        {
            _issues.GradientStrokes();

            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "t");
            // highlightLength - ReadAnimatableFloat(obj.GetNamedObject("h")) - but is optional
            IgnoreFieldThatIsNotYetSupported(obj, "h");
            // highlightAngle - ReadAnimatableFloat(obj.GetNamedObject("a")) - but is optional
            IgnoreFieldThatIsNotYetSupported(obj, "1");

            var name = ReadName(obj);
            var opacityPercent = ReadOpacityPercent(obj);
            var strokeWidth = ReadAnimatableFloat(obj.GetNamedObject("w"));
            var capType = LcToLineCapType(obj.GetNamedNumber("lc"));
            var joinType = LjToLineJoinType(obj.GetNamedNumber("lj"));
            var miterLimit = obj.GetNamedNumber("ml", 4); // Default miter limit in AfterEffects is 4
            var startPoint = ReadAnimatableVector3(obj.GetNamedObject("s"));
            var endPoint = ReadAnimatableVector3(obj.GetNamedObject("e"));

            AssertAllFieldsRead(obj);
            return new RadialGradientStroke(
                name.Name,
                name.MatchName,
                opacityPercent,
                strokeWidth,
                capType,
                joinType,
                miterLimit);
        }

        // "fl"
        SolidColorFill ReadSolidColorFill(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "fillEnabled");
            IgnoreFieldThatIsNotYetSupported(obj, "cl");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var color = ReadColor(obj);
            var opacityPercent = ReadOpacityPercent(obj);
            var isWindingFill = ReadBool(obj, "r") == true;
            var fillType = isWindingFill ? SolidColorFill.PathFillType.Winding : SolidColorFill.PathFillType.EvenOdd;
            AssertAllFieldsRead(obj);
            return new SolidColorFill(name.Name, name.MatchName, fillType, color, opacityPercent);
        }

        // gf
        ShapeLayerContent ReadGradientFill(JObject obj)
        {
            switch (TToGradientType(obj.GetNamedNumber("t")))
            {
                case GradientType.Linear:
                    return ReadLinearGradientFill(obj);
                case GradientType.Radial:
                    return ReadRadialGradientFill(obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        RadialGradientFill ReadRadialGradientFill(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "hd");
            IgnoreFieldThatIsNotYetSupported(obj, "r");
            IgnoreFieldThatIsNotYetSupported(obj, "1");

            var name = ReadName(obj);
            var opacityPercent = ReadOpacityPercent(obj);
            var startPoint = ReadAnimatableVector3(obj.GetNamedObject("s"));
            var endPoint = ReadAnimatableVector3(obj.GetNamedObject("e"));
            var gradientStops = ReadAnimatableGradientStops(obj.GetNamedObject("g"));

            Animatable<double> highlightLength = null;
            var highlightLengthObject = obj.GetNamedObject("h");
            if (highlightLengthObject != null)
            {
                highlightLength = ReadAnimatableFloat(highlightLengthObject);
            }

            Animatable<double> highlightDegrees = null;
            var highlightAngleObject = obj.GetNamedObject("a");
            if (highlightAngleObject != null)
            {
                highlightDegrees = ReadAnimatableFloat(highlightAngleObject);
            }

            AssertAllFieldsRead(obj);
            return new RadialGradientFill(
                name.Name,
                name.MatchName,
                opacityPercent: opacityPercent,
                startPoint: startPoint,
                endPoint: endPoint,
                gradientStops: gradientStops,
                highlightLength: null,
                highlightDegrees: null);
        }

        LinearGradientFill ReadLinearGradientFill(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "r");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var opacityPercent = ReadOpacityPercent(obj);
            var startPoint = ReadAnimatableVector2(obj.GetNamedObject("s"));
            var endPoint = ReadAnimatableVector2(obj.GetNamedObject("e"));
            var gradientStops = ReadAnimatableGradientStops(obj.GetNamedObject("g"));
            AssertAllFieldsRead(obj);
            return new LinearGradientFill(
                name.Name,
                name.MatchName,
                opacityPercent: opacityPercent,
                startPoint: startPoint,
                endPoint: endPoint,
                gradientStops: gradientStops);
        }

        Ellipse ReadEllipse(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "closed");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var position = ReadAnimatableVector3(obj.GetNamedObject("p"));
            var diameter = ReadAnimatableVector3(obj.GetNamedObject("s"));
            var direction = ReadBool(obj, "d") == true;
            AssertAllFieldsRead(obj);
            return new Ellipse(name.Name, name.MatchName, direction, position, diameter);
        }

        Polystar ReadPolystar(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "ix");

            var name = ReadName(obj);
            var direction = ReadBool(obj, "d") == true;

            var type = SyToPolystarType(obj.GetNamedNumber("sy", double.NaN));

            var points = ReadAnimatableFloat(obj.GetNamedObject("pt"));
            if (points.IsAnimated)
            {
                _issues.PolystarAnimation("points");
            }

            var position = ReadAnimatableVector3(obj.GetNamedObject("p"));
            if (position.IsAnimated)
            {
                _issues.PolystarAnimation("position");
            }

            var rotation = ReadAnimatableFloat(obj.GetNamedObject("r"));
            if (rotation.IsAnimated)
            {
                _issues.PolystarAnimation("rotation");
            }

            var outerRadius = ReadAnimatableFloat(obj.GetNamedObject("or"));
            if (outerRadius.IsAnimated)
            {
                _issues.PolystarAnimation("outer radius");
            }

            var outerRoundedness = ReadAnimatableFloat(obj.GetNamedObject("os"));
            if (outerRoundedness.IsAnimated)
            {
                _issues.PolystarAnimation("outer roundedness");
            }

            Animatable<double> innerRadius;
            Animatable<double> innerRoundedness;

            if (type == Polystar.PolyStarType.Star)
            {
                innerRadius = ReadAnimatableFloat(obj.GetNamedObject("ir"));
                if (innerRadius.IsAnimated)
                {
                    _issues.PolystarAnimation("inner radius");
                }

                innerRoundedness = ReadAnimatableFloat(obj.GetNamedObject("is"));
                if (innerRoundedness.IsAnimated)
                {
                    _issues.PolystarAnimation("inner roundedness");
                }
            }
            else
            {
                innerRadius = null;
                innerRoundedness = null;
            }

            AssertAllFieldsRead(obj);
            return new Polystar(
                name.Name,
                name.MatchName,
                direction,
                type,
                points,
                position,
                rotation,
                innerRadius,
                outerRadius,
                innerRoundedness,
                outerRoundedness);
        }

        Rectangle ReadRectangle(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var direction = ReadBool(obj, "d") == true;
            var position = ReadAnimatableVector3(obj.GetNamedObject("p"));
            var size = ReadAnimatableVector3(obj.GetNamedObject("s"));
            var cornerRadius = ReadAnimatableFloat(obj.GetNamedObject("r"));

            AssertAllFieldsRead(obj);
            return new Rectangle(name.Name, name.MatchName, direction, position, size, cornerRadius);
        }


        Shape ReadShape(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "ind");
            IgnoreFieldThatIsNotYetSupported(obj, "ix");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");
            IgnoreFieldThatIsNotYetSupported(obj, "cl");
            IgnoreFieldThatIsNotYetSupported(obj, "closed");

            var name = ReadName(obj);
            var geometry = ReadAnimatableGeometry(obj.GetNamedObject("ks"));
            var direction = ReadBool(obj, "d") == true;
            AssertAllFieldsRead(obj);
            return new Shape(name.Name, name.MatchName, direction, geometry);
        }

        TrimPath ReadTrimPath(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "ix");
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var startPercent = ReadAnimatableFloat(obj.GetNamedObject("s"));
            var endPercent = ReadAnimatableFloat(obj.GetNamedObject("e"));
            var offsetDegrees = ReadAnimatableFloat(obj.GetNamedObject("o"));
            var trimType = MToTrimType(obj.GetNamedNumber("m", 1));
            AssertAllFieldsRead(obj);
            return new TrimPath(
                name.Name,
                name.MatchName,
                trimType,
                startPercent,
                endPercent,
                offsetDegrees);
        }

        Repeater ReadRepeater(JObject obj)
        {
            var name = ReadName(obj);
            var count = ReadAnimatableFloat(obj.GetNamedObject("c"));
            var offset = ReadAnimatableFloat(obj.GetNamedObject("o"));
            var transform = ReadRepeaterTransform(obj.GetNamedObject("tr"));
            return new Repeater(count, offset, transform, name.Name, name.MatchName);
        }

        MergePaths ReadMergePaths(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "hd");

            var name = ReadName(obj);
            var mergeMode = MmToMergeMode(obj.GetNamedNumber("mm"));
            AssertAllFieldsRead(obj);
            return new MergePaths(
                name.Name,
                name.MatchName,
                mergeMode);
        }

        RoundedCorner ReadRoundedCorner(JObject obj)
        {
            // Not clear whether we need to read these fields.
            IgnoreFieldThatIsNotYetSupported(obj, "hd");
            IgnoreFieldThatIsNotYetSupported(obj, "ix");

            var name = ReadName(obj);
            var radius = ReadAnimatableFloat(obj.GetNamedObject("r"));
            AssertAllFieldsRead(obj);
            return new RoundedCorner(
                name.Name,
                name.MatchName,
                radius);
        }

        Animatable<double> ReadOpacityPercent(JObject obj)
        {
            var jsonOpacity = obj.GetNamedObject("o", null);
            return ReadOpacityPercentFromObject(jsonOpacity);
        }

        Animatable<double> ReadOpacityPercentFromObject(JObject obj)
        {
            var result = obj != null
                ? ReadAnimatableFloat(obj)
                : new Animatable<double>(100, null);
            return result;
        }

        Animatable<Color> ReadColor(JObject obj) =>
            ReadAnimatableColor(obj.GetNamedObject("c", null));

        Animatable<Color> ReadAnimatableColor(JObject obj)
        {
            if (obj == null)
            {
                return new Animatable<Color>(Color.Black, null);
            }

            s_animatableColorParser.ParseJson(this, obj, out IEnumerable<KeyFrame<Color>> keyFrames, out Color initialValue);

            var propertyIndex = ReadInt(obj, "ix");

            return new Animatable<Color>(initialValue, keyFrames, propertyIndex);
        }

        // Reads the transform for a repeater. Repeater transforms are the same as regular transforms
        // except they have an extra couple properties.
        RepeaterTransform ReadRepeaterTransform(JObject obj)
        {
            var startOpacityPercent = ReadOpacityPercentFromObject(obj.GetNamedObject("so", null));
            var endOpacityPercent = ReadOpacityPercentFromObject(obj.GetNamedObject("eo", null));
            var transform = ReadTransform(obj);
            return new RepeaterTransform(
                transform.Name,
                transform.Anchor,
                transform.Position,
                transform.ScalePercent,
                transform.RotationDegrees,
                transform.OpacityPercent,
                startOpacityPercent,
                endOpacityPercent);
        }

        Transform ReadTransform(JObject obj)
        {
            IAnimatableVector3 anchor = null;
            IAnimatableVector3 position = null;
            IAnimatableVector3 scalePercent = null;
            Animatable<double> rotation = null;

            var anchorJson = obj.GetNamedObject("a", null);
            if (anchorJson != null)
            {
                anchor = ReadAnimatableVector3(anchorJson);
            }
            else
            {
                anchor = new AnimatableVector3(new Vector3(), null);
            }

            var positionJson = obj.GetNamedObject("p", null);
            if (positionJson != null)
            {
                position = ReadAnimatableVector3(positionJson);
            }
            else
            {
                throw new LottieCompositionReaderException("Missing transform for position");
            }

            var scaleJson = obj.GetNamedObject("s", null);
            if (scaleJson != null)
            {
                scalePercent = ReadAnimatableVector3(scaleJson);
            }

            var rotationJson = obj.GetNamedObject("r", null);
            if (rotationJson == null)
            {
                rotationJson = obj.GetNamedObject("rz", null);
            }
            if (rotationJson != null)
            {
                rotation = ReadAnimatableFloat(rotationJson);
            }
            else
            {
                throw new LottieCompositionReaderException("Missing transform for rotation");
            }

            var opacityPercent = ReadOpacityPercent(obj);
            var name = ReadName(obj);

            return new Transform(name.Name, anchor, position, scalePercent, rotation, opacityPercent);
        }

        static bool? ReadBool(JObject obj, string name)
        {
            if (!obj.ContainsKey(name))
            {
                return null;
            }

            var value = obj.GetNamedValue(name);

            switch (value.Type)
            {
                case JTokenType.Boolean:
                    return obj.GetNamedBoolean(name);
                case JTokenType.Integer:
                case JTokenType.Float:
                    return ReadInt(obj, name)?.Equals(1);
                case JTokenType.Null:
                case JTokenType.String:
                case JTokenType.Array:
                case JTokenType.Object:
                default:
                    throw new InvalidOperationException();
            }
        }

        static int? ReadInt(JObject obj, string name)
        {
            var value = obj.GetNamedNumber(name, double.NaN);
            if (double.IsNaN(value))
            {
                return null;
            }
            // Newtonsoft has its own casting logic so to bypass this, we first cast to a double and then round
            // because the desired behavior is to round doubles to the nearest value.
            var intValue = unchecked((int)Math.Round((double)value));
            if (value != intValue)
            {
                return null;
            }
            return intValue;
        }

        Animatable<Vector2> ReadAnimatableVector2(JObject obj)
        {
            IgnoreFieldThatIsNotYetSupported(obj, "s");
            // Expressions not supported.
            IgnoreFieldThatIsNotYetSupported(obj, "x");

            var propertyIndex = ReadInt(obj, "ix");
            if (obj.ContainsKey("k"))
            {
                s_animatableVector2Parser.ParseJson(this, obj, out IEnumerable<KeyFrame<Vector2>> keyFrames, out Vector2 initialValue);
                AssertAllFieldsRead(obj);
                return new Animatable<Vector2>(initialValue, keyFrames, propertyIndex);
            }

            throw new LottieCompositionReaderException("Animatable Vector2 could not be read.");
        }

        IAnimatableVector3 ReadAnimatableVector3(JObject obj)
        {
            IgnoreFieldThatIsNotYetSupported(obj, "s");
            // Expressions not supported.
            IgnoreFieldThatIsNotYetSupported(obj, "x");

            var propertyIndex = ReadInt(obj, "ix");
            if (obj.ContainsKey("k"))
            {
                s_animatableVector3Parser.ParseJson(this, obj, out IEnumerable<KeyFrame<Vector3>> keyFrames, out Vector3 initialValue);
                AssertAllFieldsRead(obj);
                return new AnimatableVector3(initialValue, keyFrames, propertyIndex);
            }
            else
            {
                // Split X and Y dimensions 
                var x = ReadAnimatableFloat(obj.GetNamedObject("x"));
                var y = ReadAnimatableFloat(obj.GetNamedObject("y"));
                AssertAllFieldsRead(obj);

                return new AnimatableXYZ(x, y, new Animatable<double>(0, propertyIndex));
            }
        }

        Animatable<PathGeometry> ReadAnimatableGeometry(JObject obj)
        {
            s_animatableGeometryParser.ParseJson(this, obj, out IEnumerable<KeyFrame<PathGeometry>> keyFrames, out PathGeometry initialValue);
            var propertyIndex = ReadInt(obj, "ix");
            return new Animatable<PathGeometry>(initialValue, keyFrames, propertyIndex);
        }

        Animatable<Sequence<GradientStop>> ReadAnimatableGradientStops(JObject obj)
        {
            var numberOfColorStops = ReadInt(obj, "p");

            var animatableGradientStopsParser = new AnimatableGradientStopsParser(numberOfColorStops);
            animatableGradientStopsParser.ParseJson(
                this,
                obj.GetNamedObject("k"), out IEnumerable<KeyFrame<Sequence<GradientStop>>> keyFrames,
                out Sequence<GradientStop> initialValue);

            var propertyIndex = ReadInt(obj, "ix");
            return new Animatable<Sequence<GradientStop>>(initialValue, keyFrames, propertyIndex);
        }

        Animatable<double> ReadAnimatableFloat(JObject obj)
        {
            s_animatableFloatParser.ParseJson(this, obj, out IEnumerable<KeyFrame<double>> keyFrames, out double initialValue);
            var propertyIndex = ReadInt(obj, "ix");
            return new Animatable<double>(initialValue, keyFrames, propertyIndex);
        }

        static Vector3 ReadVector3FromJsonArray(JArray array)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            int i = 0;
            var count = array.Count;
            for (; i < count; i++)
            {
                // NOTE: indexing JsonArray is faster than enumerating it.
                var number = (double)array[i];
                switch (i)
                {
                    case 0:
                        x = number;
                        break;
                    case 1:
                        y = number;
                        break;
                    case 2:
                        z = number;
                        break;
                    default:
                        throw new LottieCompositionReaderException("Too many values for Vector3.");
                }
            }

            // Allow either 2 or 3 values to be specified. If 2 values, assume z==0.
            if (i < 2)
            {
                throw new LottieCompositionReaderException("Not enough values for Vector3.");
            }

            return new Vector3(x, y, z);
        }

        static Vector2 ReadVector2FromJsonArray(JArray array)
        {
            double x = 0;
            double y = 0;
            int i = 0;
            var count = array.Count;
            for (; i < count; i++)
            {
                // NOTE: indexing JsonArray is faster than enumerating it.
                var number = (double)array[i];
                switch (i)
                {
                    case 0:
                        x = number;
                        break;
                    case 1:
                        y = number;
                        break;
                }
            }

            if (i < 2)
            {
                throw new LottieCompositionReaderException("Not enough values for Vector2.");
            }

            return new Vector2(x, y);
        }


        struct AfterEffectsName
        {
            internal string Name;
            internal string MatchName;
        }

        AfterEffectsName ReadName(JObject obj)
        {
            var result = new AfterEffectsName();
            if (_options.HasFlag(Options.IgnoreNames))
            {
                IgnoreFieldIntentionally(obj, "nm");
            }
            else
            {
                result.Name = obj.GetNamedString("nm", "");
            }
            if (_options.HasFlag(Options.IgnoreMatchNames))
            {
                IgnoreFieldIntentionally(obj, "mn");
            }
            else
            {
                result.MatchName = obj.GetNamedString("mn", "");
            }

            return result;
        }

        sealed class AnimatableVector2Parser : AnimatableParser<Vector2>
        {
            protected override Vector2 ReadValue(JToken obj) => ReadVector2FromJsonArray(obj.AsArray());
        }

        sealed class AnimatableVector3Parser : AnimatableParser<Vector3>
        {
            protected override Vector3 ReadValue(JToken obj) => ReadVector3FromJsonArray(obj.AsArray());
        }

        sealed class AnimatableColorParser : AnimatableParser<Color>
        {
            protected override Color ReadValue(JToken obj)
            {
                var colorArray = obj.AsArray();
                double a = 0;
                double r = 0;
                double g = 0;
                double b = 0;
                int i = 0;
                var count = colorArray.Count;
                for (; i < count; i++)
                {
                    // Note: indexing a JsonArray is faster than enumerating.
                    var number = (double)colorArray[i];
                    switch (i)
                    {
                        case 0:
                            r = number;
                            break;
                        case 1:
                            g = number;
                            break;
                        case 2:
                            b = number;
                            break;
                        case 3:
                            a = number;
                            break;
                        default:
                            throw new LottieCompositionReaderException("Too many values for Color.");
                    }
                }

                if (i != 4)
                {
                    throw new LottieCompositionReaderException("Not enough values for Color.");
                }

                // Some versions of Lottie use floats, some use bytes. Assume bytes if any values are > 1.
                if (r > 1 || g > 1 || b > 1 || a > 1)
                {
                    // Convert byte to float.
                    a /= 255;
                    r /= 255;
                    g /= 255;
                    b /= 255;
                }
                return Color.FromArgb(a, r, g, b);
            }
        }

        sealed class AnimatableGeometryParser : AnimatableParser<PathGeometry>
        {
            protected override PathGeometry ReadValue(JToken value)
            {
                JObject pointsData = null;
                if (value.Type == JTokenType.Array)
                {
                    var firstItem = value.AsArray().First();
                    var firstItemAsObject = firstItem.AsObject();
                    if (firstItem.Type == JTokenType.Object && firstItemAsObject.ContainsKey("v"))
                    {
                        pointsData = firstItemAsObject;
                    }
                }
                else if (value.Type == JTokenType.Object && value.AsObject().ContainsKey("v"))
                {
                    pointsData = value.AsObject();
                }

                if (pointsData == null)
                {
                    return null;
                }

                var vertices = pointsData.GetNamedArray("v", null);
                var inTangents = pointsData.GetNamedArray("i", null);
                var outTangents = pointsData.GetNamedArray("o", null);
                var isClosed = pointsData.GetNamedBoolean("c", false);

                if (vertices == null || inTangents == null || outTangents == null)
                {
                    throw new LottieCompositionReaderException($"Unable to process points array or tangents. {pointsData}");
                }

                var beziers = new BezierSegment[isClosed ? vertices.Count : Math.Max(vertices.Count - 1, 0)];

                if (beziers.Length > 0)
                {
                    // The vertices for the figure.
                    var verticesAsVector2 = ReadVector2Array(vertices);

                    // The control points that define the cubic beziers between the vertices.
                    var inTangentsAsVector2 = ReadVector2Array(inTangents);
                    var outTangentsAsVector2 = ReadVector2Array(outTangents);

                    if (verticesAsVector2.Length != inTangentsAsVector2.Length ||
                        verticesAsVector2.Length != outTangentsAsVector2.Length)
                    {
                        throw new LottieCompositionReaderException($"Invalid path data. {pointsData}");
                    }

                    var cp3 = verticesAsVector2[0];

                    for (var i = 0; i < beziers.Length; i++)
                    {
                        // cp0 is the start point of the segment.
                        var cp0 = cp3;

                        // cp1 is relative to cp0
                        var cp1 = cp0 + outTangentsAsVector2[i];

                        // cp3 is the endpoint of the segment.
                        cp3 = verticesAsVector2[(i + 1) % verticesAsVector2.Length];

                        // cp2 is relative to cp3
                        var cp2 = cp3 + inTangentsAsVector2[(i + 1) % inTangentsAsVector2.Length];

                        beziers[i] = new BezierSegment(
                            cp0: cp0,
                            cp1: cp1,
                            cp2: cp2,
                            cp3: cp3);
                    }
                }

                return new PathGeometry(beziers);
            }

            static Vector2[] ReadVector2Array(JArray array)
            {
                IEnumerable<Vector2> ToVector2Enumerable()
                {
                    var count = array.Count;
                    for (int i = 0; i < count; i++)
                    {
                        yield return ReadVector2FromJsonArray(array[i].AsArray());
                    }
                }

                return ToVector2Enumerable().ToArray();
            }
        }

        sealed class AnimatableGradientStopsParser : AnimatableParser<Sequence<GradientStop>>
        {
            // The number of color stops. The opacity stops follow this number
            // of color stops. If not specified, all of the values are color stops.
            readonly int? _colorStopCount;

            internal AnimatableGradientStopsParser(int? colorStopCount) { _colorStopCount = colorStopCount; }

            protected override Sequence<GradientStop> ReadValue(JToken obj)
            {
                var gradientStopsData = obj.AsArray().Select(v => (double)v).ToArray();

                // Get the number of color stops. If _colorStopCount wasn't specified, all of
                // the data in the array is for color stops.
                var colorStopsDataLength = _colorStopCount.HasValue
                    ? _colorStopCount.Value * 4
                    : gradientStopsData.Length;

                if (gradientStopsData.Length < colorStopsDataLength)
                {
                    throw new LottieCompositionReaderException("Fewer gradient stop values than expected");
                }

                var gradientStops = new List<GradientStop>(colorStopsDataLength / 4);

                var offset = 0.0;
                var r = 0.0;
                var g = 0.0;
                int i;
                for (i = 0; i < colorStopsDataLength; i++)
                {
                    var value = gradientStopsData[i];
                    switch (i % 4)
                    {
                        case 0:
                            offset = value;
                            break;
                        case 1:
                            r = value;
                            break;
                        case 2:
                            g = value;
                            break;
                        case 3:
                            var b = value;
                            // Some versions of Lottie use floats, some use bytes. Assume bytes if any values are > 1.
                            if (r > 1 || g > 1 || b > 1)
                            {
                                // Convert byte to float.
                                r /= 255;
                                g /= 255;
                                b /= 255;
                            }
                            gradientStops.Add(GradientStop.FromColor(offset, Color.FromArgb(1, r, g, b)));
                            break;
                    }
                }

                // The rest of the array contains the opacity stops.
                for (; i < gradientStopsData.Length; i++)
                {
                    var value = gradientStopsData[i];
                    switch (i % 2)
                    {
                        case 0:
                            offset = value;
                            break;
                        case 1:
                            double opacity = value;
                            // Some versions of Lottie use floats, some use bytes. Assume bytes if any values are > 1.
                            if (opacity > 1)
                            {
                                // Convert byte to float.
                                opacity /= 255;
                            }
                            gradientStops.Add(GradientStop.FromOpacity(offset, opacity));
                            break;
                    }
                }

                // Merge the stops that have the same offset, and order by offset.
                var merged =
                    from stop in gradientStops
                    group stop by stop.Offset into grouped
                    // Order by offset.
                    orderby grouped.Key
                    // Note that if there are multiple color stops with the same offset or
                    // multiple opacity stops with the same offset, one will be chosen at
                    // random.
                    select grouped.Aggregate((g1, g2) => new GradientStop(g1.Offset, g1.Color ?? g2.Color, g1.Opacity ?? g2.Opacity));

                return new Sequence<GradientStop>(merged);
            }
        }

        sealed class AnimatableFloatParser : AnimatableParser<double>
        {
            protected override double ReadValue(JToken obj) => ReadFloat(obj);
        }

        abstract class AnimatableParser<T> where T : IEquatable<T>
        {
            static readonly KeyFrame<T>[] s_emptyKeyFrames = new KeyFrame<T>[0];

            protected private AnimatableParser() { }

            protected abstract T ReadValue(JToken obj);

            internal void ParseJson(LottieCompositionReader reader, JObject obj, out IEnumerable<KeyFrame<T>> keyFrames, out T initialValue)
            {
                var isAnimated = ReadBool(obj, "a") == true;

                keyFrames = s_emptyKeyFrames;
                initialValue = default(T);

                foreach (var field in obj)
                {
                    switch (field.Key)
                    {
                        case "k":
                            {
                                var k = field.Value;
                                if (k.Type == JTokenType.Array)
                                {
                                    var kArray = k.AsArray();
                                    if (HasKeyframes(kArray))
                                    {
                                        keyFrames = ReadKeyFrames(reader, kArray).ToArray();
                                        initialValue = keyFrames.First().Value;
                                    }
                                }

                                if (keyFrames == s_emptyKeyFrames)
                                {
                                    initialValue = ReadValue(k);
                                }
                            }
                            break;

                        // Defines if property is animated. 0 or 1. 
                        // Currently ignored because we derive this from the existence of keyframes.
                        case "a":
                            break;

                        // Property index. Used for expressions. Currently ignored because we don't support expressions.
                        case "ix":
                            // Do not report it as an issue - existence of "ix" doesn't mean that an expression is actually used.
                            break;

                        // Extremely rare fields seen in 1 Lottie file. Ignore.
                        case "nm":  // Name
                        case "mn":  // ??
                        case "hd":  // IsHidden
                            break;

                        // Property expression. Currently ignored because we don't support expressions.
                        case "x":
                            reader._issues.Expressions();
                            break;
                        default:
                            reader._issues.UnexpectedField(field.Key);
                            break;
                    }
                }

                if (isAnimated && keyFrames == s_emptyKeyFrames)
                {
                    throw new LottieCompositionReaderException("Expected keyframes.");
                }
            }

            static bool HasKeyframes(JArray array)
            {
                var firstItem = array[0];
                return firstItem.Type == JTokenType.Object && firstItem.AsObject().ContainsKey("t");
            }

            IEnumerable<KeyFrame<T>> ReadKeyFrames(LottieCompositionReader reader, JArray jsonArray)
            {
                int count = jsonArray.Count;

                if (count == 0)
                {
                    yield break;
                }

                //
                // Keyframes are encoded in Lottie as an array consisting of a sequence
                // of start and end value with start frame and easing function. The final
                // entry in the array is the frame at which the last interpolation ends.
                // [
                //   { startValue_1, endValue_1, startFrame_1 },  # interpolates from startValue_1 to endValue_1 from startFrame_1 to startFrame_2
                //   { startValue_2, endValue_2, startFrame_2 },  # interpolates from startValue_2 to endValue_2 from startFrame_2 to startFrame_3
                //   { startValue_3, endValue_3, startFrame_3 },  # interpolates from startValue_3 to endValue_3 from startFrame_3 to startFrame_4
                //   { startFrame_4 }
                // ]
                // We convert these to keyframes that match the Windows.UI.Composition notion of a keyframe,
                // which is a triple: {endValue, endTime, easingFunction}.
                // An initial keyframe is created to describe the initial value. It has no easing function.
                //

                T endValue = default(T);
                // The initial keyframe has the same value as the initial value. Easing therefore doesn't
                // matter, but might as well use hold as it's the simplest (it does not interpolation)
                Easing easing = HoldEasing.Instance;
                // Start by holding from the initial value.
                bool isHolding = true;
                // SpatialBeziers.
                var ti = default(Vector3);
                var to = default(Vector3);

                // NOTE: indexing an array with GetObjectAt is faster than enumerating.
                for (int i = 0; i < count; i++)
                {
                    var lottieKeyFrame = jsonArray[i].AsObject();

                    // "n" is a name on the keyframe. Never seems to be useful.
                    reader.IgnoreFieldIntentionally(lottieKeyFrame, "n");

                    // Read the start frame.
                    var startFrame = lottieKeyFrame.GetNamedNumber("t", 0);

                    if (i == count - 1)
                    {
                        // This is the final frame. Final frames optionally don't have a value.
                        if (!lottieKeyFrame.ContainsKey("s"))
                        {
                            // It has no value associated with it.
                            yield return new KeyFrame<T>(startFrame, endValue, to, ti, easing);
                            break;
                        }
                    }

                    // Read the start value.
                    var startValue = ReadValue(lottieKeyFrame.GetNamedValue("s"));

                    // The start of the next entry must be the same as the end of the previous entry
                    // unless in a hold.
                    if (!isHolding && !endValue.Equals(startValue))
                    {
                        throw new InvalidOperationException();
                    }

                    yield return new KeyFrame<T>(startFrame, startValue, to, ti, easing);

                    // Spatial control points.
                    if (lottieKeyFrame.ContainsKey("ti"))
                    {
                        ti = ReadVector3FromJsonArray(lottieKeyFrame.GetNamedArray("ti"));
                        to = ReadVector3FromJsonArray(lottieKeyFrame.GetNamedArray("to"));
                    }

                    // Get the easing to the end value, and get the end value.
                    if (ReadBool(lottieKeyFrame, "h") == true)
                    {
                        // Hold the current value. The next value comes from the start
                        // of the next entry.
                        isHolding = true;
                        easing = HoldEasing.Instance;
                        // Synthesize an endValue. This is only used if this is the final frame.
                        endValue = startValue;
                    }
                    else
                    {
                        // Read the easing function parameters. If there are any parameters, it's a CubicBezierEasing.
                        var cp1Json = lottieKeyFrame.GetNamedObject("o", null);
                        var cp2Json = lottieKeyFrame.GetNamedObject("i", null);
                        if (cp1Json != null && cp2Json != null)
                        {
                            var cp1 = new Vector3(ReadFloat(cp1Json.GetNamedValue("x")), ReadFloat(cp1Json.GetNamedValue("y")), 0);
                            var cp2 = new Vector3(ReadFloat(cp2Json.GetNamedValue("x")), ReadFloat(cp2Json.GetNamedValue("y")), 0);
                            easing = new CubicBezierEasing(cp1, cp2);
                        }
                        else
                        {
                            easing = LinearEasing.Instance;
                        }

                        // Read the end value. The end frame number isn't known until 
                        // the next pair is read.
                        endValue = ReadValue(lottieKeyFrame.GetNamedValue("e"));
                    }

                    reader.AssertAllFieldsRead(lottieKeyFrame);
                }
            }
        }

        static double ReadFloat(JToken jsonValue)
        {
            switch (jsonValue.Type)
            {
                case JTokenType.Float:
                case JTokenType.Integer:
                    return (double)jsonValue;
                case JTokenType.Array:
                    {
                        var array = jsonValue.AsArray();
                        switch (array.Count)
                        {
                            case 0:
                                throw new LottieCompositionReaderException("Expecting float but found empty array.");
                            case 1:
                                return (double)array[0];
                            default:
                                // Some Lottie files have multiple values in arrays that should only have one. Just
                                // take the first value.
                                return (double)array[0];
                        }
                    }
                case JTokenType.Null:
                case JTokenType.Boolean:
                case JTokenType.String:
                case JTokenType.Object:
                default:
                    throw new LottieCompositionReaderException($"Expected float but found {jsonValue.Type}.");
            }
        }

        static BlendMode BmToBlendMode(double bm)
        {
            if (bm == (int)bm)
            {
                switch ((int)bm)
                {
                    case 0: return BlendMode.Normal;
                    case 1: return BlendMode.Multiply;
                    case 2: return BlendMode.Screen;
                    case 3: return BlendMode.Overlay;
                    case 4: return BlendMode.Darken;
                    case 5: return BlendMode.Lighten;
                    case 6: return BlendMode.ColorDodge;
                    case 7: return BlendMode.ColorBurn;
                    case 8: return BlendMode.HardLight;
                    case 9: return BlendMode.SoftLight;
                    case 10: return BlendMode.Difference;
                    case 11: return BlendMode.Exclusion;
                    case 12: return BlendMode.Hue;
                    case 13: return BlendMode.Saturation;
                    case 14: return BlendMode.Color;
                    case 15: return BlendMode.Luminosity;
                    default:
                        throw new LottieCompositionReaderException($"Unexpected blend mode: {bm}.");
                }
            }
            throw new LottieCompositionReaderException($"Unexpected blend mode: {bm}.");
        }

        static Layer.LayerType TyToLayerType(double ty)
        {
            if (ty == (int)ty)
            {
                switch ((int)ty)
                {
                    case 0: return Layer.LayerType.PreComp;
                    case 1: return Layer.LayerType.Solid;
                    case 2: return Layer.LayerType.Image;
                    case 3: return Layer.LayerType.Null;
                    case 4: return Layer.LayerType.Shape;
                    case 5: return Layer.LayerType.Text;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected layer type: {ty}.");
        }

        static Polystar.PolyStarType SyToPolystarType(double sy)
        {
            if (sy == (int)sy)
            {
                switch ((int)sy)
                {
                    case 1: return Polystar.PolyStarType.Star;
                    case 2: return Polystar.PolyStarType.Polygon;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected polystar type: {sy}.");
        }

        static SolidColorStroke.LineCapType LcToLineCapType(double lc)
        {
            if (lc == (int)lc)
            {
                switch ((int)lc)
                {
                    case 1: return SolidColorStroke.LineCapType.Butt;
                    case 2: return SolidColorStroke.LineCapType.Round;
                    case 3: return SolidColorStroke.LineCapType.Projected;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected linecap type: {lc}.");
        }

        static SolidColorStroke.LineJoinType LjToLineJoinType(double lj)
        {
            if (lj == (int)lj)
            {
                switch ((int)lj)
                {
                    case 1: return SolidColorStroke.LineJoinType.Miter;
                    case 2: return SolidColorStroke.LineJoinType.Round;
                    case 3: return SolidColorStroke.LineJoinType.Bevel;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected linejoin type: {lj}.");
        }

        static TrimPath.TrimType MToTrimType(double m)
        {
            if (m == (int)m)
            {
                switch ((int)m)
                {
                    case 1: return TrimPath.TrimType.Simultaneously;
                    case 2: return TrimPath.TrimType.Individually;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected trim type: {m}.");
        }

        static MergePaths.MergeMode MmToMergeMode(double mm)
        {
            if (mm == (int)mm)
            {
                switch ((int)mm)
                {
                    case 1: return MergePaths.MergeMode.Merge;
                    case 2: return MergePaths.MergeMode.Add;
                    case 3: return MergePaths.MergeMode.Subtract;
                    case 4: return MergePaths.MergeMode.Intersect;
                    case 5: return MergePaths.MergeMode.ExcludeIntersections;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected merge mode: {mm}.");
        }

        static GradientType TToGradientType(double t)
        {
            if (t == (int)t)
            {
                switch ((int)t)
                {
                    case 1: return GradientType.Linear;
                    case 2: return GradientType.Radial;
                }
            }
            throw new LottieCompositionReaderException($"Unexpected gradient type: {t}");
        }

        enum GradientType
        {
            Linear,
            Radial,
        }

        // Indicates that the given field will not be read because we don't yet support it.
        [Conditional("CheckForUnparsedFields")]
        void IgnoreFieldThatIsNotYetSupported(JObject obj, string fieldName)
        {
#if CheckForUnparsedFields
            obj._readFields.Add(fieldName);
#endif
        }

        // Indicates that the given field is not read because we don't need to read it.
        [Conditional("CheckForUnparsedFields")]
        void IgnoreFieldIntentionally(JObject obj, string fieldName)
        {
#if CheckForUnparsedFields
            obj._readFields.Add(fieldName);
#endif
        }

        // Reports an issue if the given JsonObject has fields that were not read.
        [Conditional("CheckForUnparsedFields")]
        void AssertAllFieldsRead(JObject obj, [CallerMemberName]string memberName = "")
        {
#if CheckForUnparsedFields
            var read = obj._readFields;
            var unread = new List<string>();
            foreach (var pair in obj)
            {
                if (!read.Contains(pair.Key))
                {
                    unread.Add(pair.Key);
                }
            }

            unread.Sort();
            foreach (var unreadField in unread)
            {
                _issues.IgnoredField($"{memberName}.{unreadField}");
            }
#endif
        }

        static void ExpectToken(JsonReader reader, JsonToken token)
        {
            if (reader.TokenType != token)
            {
                throw UnexpectedTokenException(reader);
            }
        }

        static bool ParseBool(JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    return (long)reader.Value != 0;
                case JsonToken.Float:
                    return (double)reader.Value != 0;
                case JsonToken.Boolean:
                    return (bool)reader.Value;
                default:
                    throw Exception($"Expected a bool, but got {reader.TokenType}", reader);
            }
        }

        static double ParseDouble(JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    return (double)(long)reader.Value;
                case JsonToken.Float:
                    return (double)reader.Value;
                default:
                    throw Exception($"Expected a number, but got {reader.TokenType}", reader);
            }
        }

        static int ParseInt(JsonReader reader, bool strict = false)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    return checked((int)(long)reader.Value);
                case JsonToken.Float:
                    if (strict)
                    {
                        throw Exception("Expected an integer, but got a float", reader);
                    }
                    return checked((int)(long)Math.Round((double)reader.Value));
                default:
                    throw Exception($"Expected a number, but got {reader.TokenType}", reader);
            }
        }

        // Loads the JObjects in an array.
        static IEnumerable<JObject> LoadArrayOfJObjects(JsonReader reader)
        {
            ExpectToken(reader, JsonToken.StartArray);

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        yield return JObject.Load(reader, s_jsonLoadSettings);
                        break;
                    case JsonToken.EndArray:
                        yield break;
                    default:
                        throw UnexpectedTokenException(reader);
                }
            }
            throw EofException;
        }

        IEnumerable<T> ParseArrayOf<T>(JsonReader reader, Func<JsonReader, T> parser)
        {
            ExpectToken(reader, JsonToken.StartArray);

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        var result = parser(reader);
                        if (result != null)
                        {
                            yield return result;
                        }
                        break;

                    case JsonToken.EndArray:
                        yield break;

                    default:
                        throw UnexpectedTokenException(reader);
                }
            }
        }

        // Consumes a token from the stream.
        static void ConsumeToken(JsonReader reader)
        {
            if (!reader.Read())
            {
                throw EofException;
            }
        }

        // Consumes an array from the stream.
        void ConsumeArray(JsonReader reader)
        {
            ExpectToken(reader, JsonToken.StartArray);

            var startArrayCount = 1;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        startArrayCount++;
                        break;
                    case JsonToken.EndArray:
                        startArrayCount--;
                        if (startArrayCount == 0)
                        {
                            return;
                        }
                        break;
                }
            }
            throw EofException;
        }

        // Consumes an object from the stream.
        void ConsumeObject(JsonReader reader)
        {
            ExpectToken(reader, JsonToken.StartObject);

            var objectStartCount = 1;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        objectStartCount++;
                        break;
                    case JsonToken.EndObject:
                        objectStartCount--;
                        if (objectStartCount == 0)
                        {
                            return;
                        }
                        break;
                }
            }

            throw EofException;
        }

        static LottieCompositionReaderException EofException => new LottieCompositionReaderException("EOF");

        static LottieCompositionReaderException UnexpectedFieldException(JsonReader reader, string field) => Exception($"Unexpected field: {field}", reader);

        static LottieCompositionReaderException UnexpectedTokenException(JsonReader reader) => Exception($"Unexpected token: {reader.TokenType}", reader);

        static LottieCompositionReaderException Exception(string message, JsonReader reader) => new LottieCompositionReaderException($"{message} @ {reader.Path}");
    }

#if CheckForUnparsedFields
    sealed class CheckedJsonObject : IEnumerable<KeyValuePair<string, JToken>>
    {
        internal readonly Newtonsoft.Json.Linq.JObject _wrapped;
        internal readonly HashSet<string> _readFields = new HashSet<string>();

        internal CheckedJsonObject(Newtonsoft.Json.Linq.JObject wrapped)
        {
            _wrapped = wrapped;
        }

        internal static CheckedJsonObject Parse(string input, JsonLoadSettings loadSettings) => new CheckedJsonObject(Newtonsoft.Json.Linq.JObject.Parse(input, loadSettings));

        internal bool ContainsKey(string key)
        {
            _readFields.Add(key);
            return _wrapped.ContainsKey(key);
        }

        internal bool TryGetValue(string propertyName, out JToken value)
        {
            _readFields.Add(propertyName);
            return _wrapped.TryGetValue(propertyName, out value);
        }

        internal static CheckedJsonObject Load(JsonReader reader, JsonLoadSettings settings)
        {
            return new CheckedJsonObject(Newtonsoft.Json.Linq.JObject.Load(reader, settings));
        }

        public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
        {
            return _wrapped.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _wrapped.GetEnumerator();
        }

        public static implicit operator CheckedJsonObject(Newtonsoft.Json.Linq.JObject value)
        {
            return value == null ? null : new CheckedJsonObject(value);
        }
    }

    sealed class CheckedJsonArray : IList<JToken>
    {
        internal readonly Newtonsoft.Json.Linq.JArray _wrapped;
        internal CheckedJsonArray(Newtonsoft.Json.Linq.JArray wrapped)
        {
            _wrapped = wrapped;
        }

        internal static CheckedJsonArray Load(JsonReader reader, JsonLoadSettings settings)
        {
            return new CheckedJsonArray(Newtonsoft.Json.Linq.JArray.Load(reader, settings));
        }

        public JToken this[int index] { get => _wrapped[index]; set => throw new NotImplementedException(); }

        public int Count => _wrapped.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(JToken item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(JToken item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(JToken[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<JToken> GetEnumerator()
        {
            foreach (var value in _wrapped)
            {
                yield return value;
            }
        }

        public int IndexOf(JToken item) => throw new NotImplementedException();
        public void Insert(int index, JToken item) => throw new NotImplementedException();
        public bool Remove(JToken item) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator CheckedJsonArray(Newtonsoft.Json.Linq.JArray value)
        {
            return value == null ? null : new CheckedJsonArray(value);
        }
    }

    static class JObjectExtensions
    {
        internal static JToken GetNamedValue(this JObject jObject, string name, JToken defaultValue = null)
        {
            return jObject.TryGetValue(name, out JToken value) ? value : defaultValue;
        }

        internal static string GetNamedString(this JObject jObject, string name, string defaultValue = "")
        {
            return jObject.TryGetValue(name, out JToken value) ? (string)value : defaultValue;
        }

        internal static double GetNamedNumber(this JObject jObject, string name, double defaultValue = double.NaN)
        {
            return jObject.TryGetValue(name, out JToken value) ? (double)value : defaultValue;
        }

        internal static JArray GetNamedArray(this JObject jObject, string name, JArray defaultValue = null)
        {
            return jObject.TryGetValue(name, out JToken value) ? value.AsArray() : defaultValue;
        }

        internal static JObject GetNamedObject(this JObject jObject, string name, JObject defaultValue = null)
        {
            return jObject.TryGetValue(name, out JToken value) ? value.AsObject() : defaultValue;
        }

        internal static bool GetNamedBoolean(this JObject jObject, string name, bool defaultValue = false)
        {
            return jObject.TryGetValue(name, out JToken value) ? (bool)value : defaultValue;
        }
    }

    static class JTokenExtensions
    {
        internal static JObject AsObject(this JToken token)
        {
            try
            {
                return (JObject)token;
            }
            catch (InvalidCastException ex)
            {
                var exceptionString = ex.Message;
                if (!string.IsNullOrWhiteSpace(token.Path))
                {
                    exceptionString += $" Failed to cast to correct type for token in path: {token.Path}.";
                }
                throw new LottieCompositionReaderException(exceptionString, ex);
            }
        }

        internal static JArray AsArray(this JToken token)
        {
            try
            {
                return (JArray)token;
            }
            catch (InvalidCastException ex)
            {
                var exceptionString = ex.Message;
                if (!string.IsNullOrWhiteSpace(token.Path))
                {
                    exceptionString += $" Failed to cast to correct type for token in path: {token.Path}.";
                }
                throw new LottieCompositionReaderException(exceptionString, ex);
            }
        }
    }
#endif
}
