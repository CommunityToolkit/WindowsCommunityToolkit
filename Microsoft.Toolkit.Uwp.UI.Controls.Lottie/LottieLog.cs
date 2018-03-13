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
using System.Diagnostics;
using System.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Provides Log related properties
    /// </summary>
    public static class LottieLog
    {
        internal const string Tag = "LOTTIE";

        private const int MaxDepth = 100;
        private static bool _traceEnabled;
        private static bool _shouldResetTrace;
        private static string[] _sections;
        private static long[] _startTimeNs;
        private static int _traceDepth;
        private static int _depthPastMaxDepth;

        internal static void Warn(string msg)
        {
            Debug.WriteLine(msg, Tag);
        }

        private static readonly Queue<string> Msgs = new Queue<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the tracing is enabled or not. Do not use in production. Only for debug!
        /// </summary>
        public static bool TraceEnabled
        {
            get => _traceEnabled;
            set
            {
                if (_traceEnabled == value)
                {
                    return;
                }

                if (value)
                {
                    _shouldResetTrace = true;
                    TryResetTrace();
                }
                else
                {
                    _traceEnabled = false;
                    _shouldResetTrace = false;
                }
            }
        }

        internal static void BeginSection(string section)
        {
            TryResetTrace();

            if (_traceDepth == MaxDepth)
            {
                _depthPastMaxDepth++;
                return;
            }

            _traceDepth++;

            if (!_traceEnabled)
            {
                return;
            }

            _sections[_traceDepth - 1] = section;
            _startTimeNs[_traceDepth - 1] = CurrentUnixTime();
            BatchedDebugWriteLine($"Begin Section: {section}");
        }

        internal static float EndSection(string section)
        {
            if (_depthPastMaxDepth > 0)
            {
                _depthPastMaxDepth--;
                return 0;
            }

            _traceDepth--;

            if (!_traceEnabled)
            {
                return 0;
            }

            if (_traceDepth == -1)
            {
                throw new System.InvalidOperationException("Can't end trace section. There are none.");
            }

            if (!section.Equals(_sections[_traceDepth]))
            {
                throw new System.InvalidOperationException("Unbalanced trace call " + section + ". Expected " + _sections[_traceDepth] + ".");
            }

            var duration = (CurrentUnixTime() - _startTimeNs[_traceDepth]) / 1000000f;
            BatchedDebugWriteLine($"End Section - {duration}ms");
            return duration;
        }

        private static void TryResetTrace()
        {
            if (_shouldResetTrace && _traceDepth == 0)
            {
                _traceEnabled = true;
                _shouldResetTrace = false;

                _sections = new string[MaxDepth];
                _startTimeNs = new long[MaxDepth];

                _depthPastMaxDepth = 0;
            }
        }

        private static void BatchedDebugWriteLine(string message)
        {
            Msgs.Enqueue($"{new string(' ', _traceDepth)}{message}");
            if (_traceDepth == 0 && Msgs.Count >= MaxDepth)
            {
                Sb.Clear();
                while (Msgs.Count > 0)
                {
                    Sb.AppendLine(Msgs.Dequeue());
                }

                Debug.WriteLine(Sb.ToString());
            }
        }

        private static readonly System.DateTime Epoc = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        private static readonly StringBuilder Sb = new StringBuilder();

        private static long CurrentUnixTime()
        {
            return (long)(System.DateTime.UtcNow - Epoc).TotalMilliseconds;
        }
    }
}
