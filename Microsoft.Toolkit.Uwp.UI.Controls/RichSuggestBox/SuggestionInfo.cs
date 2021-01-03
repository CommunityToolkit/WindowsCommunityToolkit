using System;
using System.ComponentModel;
using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SuggestionInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RichSuggestBox Owner { get; }

        public Guid Id { get; }

        public string DisplayText { get; }

        public object Item { get; set; }

        public int RangeStart { get; private set; }

        public int RangeEnd { get; private set; }

        public int Position => _range?.GetIndex(TextRangeUnit.Character) - 1 ?? 0;

        internal bool Active { get; set; }

        private ITextRange _range;

        public SuggestionInfo(Guid id, string displayText, RichSuggestBox owner)
        {
            Id = id;
            DisplayText = displayText;
            Owner = owner;
        }

        internal void UpdateTextRange(ITextRange range)
        {
            if (_range == null)
            {
                _range = range;
                RangeStart = _range.StartPosition;
                RangeEnd = _range.EndPosition;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            }
            else if (RangeStart != range.StartPosition || RangeEnd != range.EndPosition)
            {
                _range.SetRange(range.StartPosition, range.EndPosition);
                RangeStart = _range.StartPosition;
                RangeEnd = _range.EndPosition;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        public override string ToString()
        {
            return $"HYPERLINK \"{Id}\"{DisplayText}";
        }
    }
}
