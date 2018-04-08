// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class SerializableStroke
    {
        [JsonIgnore]
        public List<InkPoint> FinalPointList { get; set; }

        public List<SerializablePoint> SerializableFinalPointList { get; set; }

        public InkDrawingAttributes DrawingAttributes { get; set; }

        public Matrix3x2 PointTransform { get; set; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            SerializableFinalPointList = new List<SerializablePoint>(FinalPointList.Count);
            foreach (var point in FinalPointList)
            {
                var serializablePoint = new SerializablePoint();
                serializablePoint.Position = point.Position;
                serializablePoint.Pressure = point.Pressure;
                serializablePoint.TiltX = point.TiltX;
                serializablePoint.TiltY = point.TiltY;
                serializablePoint.Pressure = point.Pressure;
                SerializableFinalPointList.Add(serializablePoint);
            }
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var finalPointList = new List<InkPoint>(SerializableFinalPointList.Count);

            foreach (var point in SerializableFinalPointList)
            {
                finalPointList.Add(new InkPoint(point.Position, point.Pressure, point.TiltX, point.TiltY, point.Timestamp));
            }

            FinalPointList = finalPointList;
            SerializableFinalPointList = null;
        }

        [OnSerialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            SerializableFinalPointList = null;
        }
    }
}