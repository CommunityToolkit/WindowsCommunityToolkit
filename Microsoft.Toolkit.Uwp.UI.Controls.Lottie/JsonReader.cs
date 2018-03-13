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

using System.IO;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    internal class JsonReader : JsonTextReader
    {
        public JsonReader(TextReader reader)
            : base(reader)
        {
        }

        public bool HasNext()
        {
            return TokenType != JsonToken.EndArray && TokenType != JsonToken.EndObject;
        }

        public JsonToken Peek()
        {
            return TokenType;
        }

        public string NextName()
        {
            if (TokenType == JsonToken.PropertyName)
            {
                var value = (string)Value;
                Read();
                return value;
            }

            throw new JsonReaderException("Expected property name");
        }

        public int NextInt()
        {
            int value;
            if (ValueType == typeof(long))
            {
                value = (int)(long)Value;
            }
            else
            {
                value = (int)((double)Value);
            }

            Read();
            return value;
        }

        public float NextDouble()
        {
            float value;
            if (ValueType == typeof(long))
            {
                value = (long)Value;
            }
            else
            {
                value = (float)((double)Value);
            }

            Read();
            return value;
        }

        public bool NextBoolean()
        {
            bool value = false;
            if (ValueType == typeof(bool))
            {
                value = (bool)Value;
            }

            Read();
            return value;
        }

        public string NextString()
        {
            var value = (string)Value;
            Read();
            return value;
        }

        public void BeginArray()
        {
            IfNoneReadFirst();
            if (TokenType != JsonToken.StartArray)
            {
                throw new JsonReaderException("Could not start the array parsing.");
            }

            Read();
        }

        public void EndArray()
        {
            IfNoneReadFirst();
            if (TokenType != JsonToken.EndArray)
            {
                throw new JsonReaderException("Could not end the array parsing.");
            }

            Read();
        }

        public void BeginObject()
        {
            IfNoneReadFirst();
            if (TokenType != JsonToken.StartObject)
            {
                throw new JsonReaderException("Could not start the object parsing.");
            }

            Read();
        }

        public void EndObject()
        {
            IfNoneReadFirst();
            if (TokenType != JsonToken.EndObject)
            {
                throw new JsonReaderException("Could not end the object parsing.");
            }

            Read();
        }

        private void IfNoneReadFirst()
        {
            if (TokenType == JsonToken.None)
            {
                Read();
            }
        }

        public void SkipValue()
        {
            int count = 0;
            do
            {
                JsonToken token = TokenType;
                if (token == JsonToken.StartArray || token == JsonToken.StartObject)
                {
                    count++;
                }
                else if (token == JsonToken.EndArray || token == JsonToken.EndObject)
                {
                    count--;
                }

                Read();
            }
            while (count != 0);
        }
    }
}
