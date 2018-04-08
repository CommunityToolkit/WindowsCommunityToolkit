using System;
using System.Security.Claims;
using Windows.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InkStrokeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InkStroke);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            var drawingAttributes = jo["DrawingAttributes"].ToObject<InkDrawingAttributes>();

            InkDrawable.StrokeBuilder.SetDefaultDrawingAttributes(drawingAttributes);


            string value = (string)jo["BoundingRect"].ToObject<Rect>();
            string valueType = (string)jo["ValueType"];
            string issuer = (string)jo["Issuer"];
            string originalIssuer = (string)jo["OriginalIssuer"];
            return new Claim(type, value, valueType, issuer, originalIssuer);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
