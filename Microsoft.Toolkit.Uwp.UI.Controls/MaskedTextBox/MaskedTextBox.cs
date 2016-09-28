using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class MaskedTextBox : TextBox
    {
        private MaskedTextProvider _maskedTextProvider;
        // Сontrol's password char.
        private char _passwordChar;

        private const bool Forward = true;
        private const bool Backward = false;
        // any char/str is OK here.
        private const string NullMask = "<>";

        // Bit vector to represent bool variables.
        private BitVector32 _flagState;

        // Bit mask - Determines when the Korean IME composition string is completed so converted character can be processed.
        private static int _imeEndingComposition = BitVector32.CreateMask();

        // Bit mask - Determines when the Korean IME is completing a composition, used when forcing convertion.
        private static int _imeCompleting = BitVector32.CreateMask(_imeEndingComposition);

        // Used for handling characters that have a modifier (Ctrl-A, Shift-Del...).
        private static int _handleCharacterReceive = BitVector32.CreateMask(_imeCompleting);

        // Bit mask - Used to simulate a null mask.  Needed since a MaskedTextProvider object cannot be 
        // initialized with a null mask but we need one even in this case as a backend for 
        // default properties.  This is to support creating a MaskedTextBox with the default 
        // constructor, specially at design time.
        private static int _isNullMask = BitVector32.CreateMask(_handleCharacterReceive);

        // Bit mask - Used in conjuction with get_Text to return the text that is actually set in the native
        // control.  This is required to be able to measure text correctly (GetPreferredSize) and
        // to compare against during set_Text (to bail if the same and not to raise TextChanged event).
        private static int _queryBaseText = BitVector32.CreateMask(_isNullMask);

        // If true, the input text is rejected whenever a character does not comply with the mask; a MaskInputRejected
        // event is fired for the failing character.  
        // If false, characters in the input string are processed one by one accepting the ones that comply
        // with the mask and raising the MaskInputRejected event for the rejected ones.
        private static int _rejectInputOnFirstFailure = BitVector32.CreateMask(_queryBaseText);

        // Bit masks for boolean properties.
        private static int _hidePromptOnLeave = BitVector32.CreateMask(_rejectInputOnFirstFailure);
        private static int _insertToggled = BitVector32.CreateMask(_hidePromptOnLeave);
        private static int _cutcopyincludeprompt = BitVector32.CreateMask(_insertToggled);
        private static int _cutcopyincludeliterals = BitVector32.CreateMask(_cutcopyincludeprompt);
        private static int _hideMaskOnLeave = BitVector32.CreateMask(_cutcopyincludeliterals);

        // Used for caret positioning.
        private int _caretTestPos;
        private int _selectionStart;
        private int _selectionLength;

        private bool _focused;
        private InsertKeyMode _insertMode;

        /// <summary>
        /// DependencyProperty for <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(MaskedTextBox),
            new PropertyMetadata(null, ValueChangedCallback));

        /// <summary>
        /// Gets or sets the value contents of the masked text box.
        /// </summary>
        public string Value
        {
            get { return _flagState[_isNullMask] ? base.Text : (string) GetValue(ValueProperty); }
            //get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            var maskedTextBox = target as MaskedTextBox;
            maskedTextBox?.OnValueChanged(args.NewValue as string);
        }

        /// <summary>
        /// DependencyProperty for <see cref="Mask"/> property.
        /// </summary>
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
            nameof(Mask),
            typeof(string),
            typeof(MaskedTextBox),
            new PropertyMetadata(null, MaskChangedCallback));

        /// <summary>
        /// The mask applied to this control.
        /// </summary>
        ///<remarks>https://msdn.microsoft.com/ru-ru/library/system.windows.forms.maskedtextbox.mask(v=vs.110).aspx</remarks>
        public string Mask
        {
            //get { return flagState[IS_NULL_MASK] ? string.Empty : maskedTextProvider.Mask; }
            get { return (string) GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        private static void MaskChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            var maskedTextBox = target as MaskedTextBox;
            maskedTextBox?.OnMaskChanged(args.NewValue as string);
        }

        /// <summary>
        /// Constructs the MaskedTextBox with the specified MaskedTextProvider object.
        /// </summary>
        public MaskedTextBox()
        {
            _flagState[_isNullMask] = true;
            Initialize(new MaskedTextProvider(NullMask, CultureInfo.CurrentCulture));
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Paste += OnPaste;
            SelectionChanged += OnSelectionChanged;
            TextChanged += OnTextChanged;
            var deleteButton = GetTemplateChild("DeleteButton") as Button;
            if (deleteButton != null)
            {
                deleteButton.Click += OnDeleteButtonClick;
            }
        }

        /// <summary>
        /// Raises the <see cref='UIElement.KeyDown'/> event.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (_flagState[_isNullMask])
            {
                base.OnKeyDown(e);
                // Operates as a regular text box base.
                return;
            }

            // TODO: проверить все сочетания клавиш https://ru.wikipedia.org/wiki/%D0%A1%D0%BE%D1%87%D0%B5%D1%82%D0%B0%D0%BD%D0%B8%D0%B5_%D0%BA%D0%BB%D0%B0%D0%B2%D0%B8%D1%88

            var key = e.Key;
            switch (key)
            {
                case VirtualKey.Insert:
                    // Insert is toggled when not modified with some other key (ctrl, shift...).
                    // Note that shift-Insert is same as paste.
                    if (Modifiers == VirtualKey.None && _insertMode == InsertKeyMode.Default)
                    {
                        _flagState[_insertToggled] = !_flagState[_insertToggled];
                        OnIsOverwriteModeChanged(EventArgs.Empty);
                    }
                    e.Handled = false;
                    break;

                case VirtualKey.Delete:
                case VirtualKey.Back:
                    // Deletion keys.
                    if (!IsReadOnly)
                    {
                        switch (Modifiers)
                        {
                            case VirtualKey.Shift:
                                if (key == VirtualKey.Delete)
                                {
                                    key = VirtualKey.Back;
                                }
                                break;

                            case VirtualKey.Control:
                                if (_selectionLength == 0)
                                {
                                    // In other case, the selected text should be deleted.
                                    if (key == VirtualKey.Delete)
                                    {
                                        // delete to the end of the string.
                                        _selectionLength = _maskedTextProvider.Length - _selectionStart;
                                    }
                                    else
                                    {
                                        // ( keyCode == Keys.Back ) // delete to the beginning of the string.
                                        _selectionLength = _selectionStart == _maskedTextProvider.Length
                                            /*at end of text*/
                                            ? _selectionStart
                                            : _selectionStart + 1;
                                        _selectionStart = 0;
                                    }
                                }
                                break;
                        }
                        Delete(key, _selectionStart, _selectionLength);
                    }
                    e.Handled = true;
                    break;

                case VirtualKey.Left:
                case VirtualKey.Up:
                case VirtualKey.Right:
                case VirtualKey.Down:
                case VirtualKey.End:
                case VirtualKey.Home:
                    e.Handled = false;
                    break;

                default:
                    e.Handled = true;
                    break;
            }
            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Window.Current.CoreWindow.CharacterReceived += OnCharacterReceived;
            Clipboard.ContentChanged += OnClipboardContentChanged;
            _focused = true;

            if ((HidePromptOnLeave && !MaskFull) || (HideMaskOnLeave && !_flagState[_isNullMask]))
            {
                // Prompt will show up.
                SetWindowText();
            }

            // Restore previous selection. Do this always (as opposed to within the condition above as in WmKillFocus)
            // because HidePromptOnLeave could have changed while the control did not have the focus.
            SelectInternal(_caretTestPos, _selectionLength);

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            Window.Current.CoreWindow.CharacterReceived -= OnCharacterReceived;
            Clipboard.ContentChanged -= OnClipboardContentChanged;
            _focused = false;

            if ((HidePromptOnLeave && !MaskFull) || (HideMaskOnLeave && !_flagState[_isNullMask]))
            {
                // Update text w/ no prompt.
                SetWindowText();

                // We need to update selection info in case the control is queried for it while it doesn't have the focus.
                SelectInternal(_caretTestPos, _selectionLength);
            }

            base.OnLostFocus(e);
        }

        /// <summary>
        /// Returns the length of the displayed text.
        /// </summary>
        public int TextLength
        {
            get
            {
                // In Win9x systems TextBoxBase.TextLength calls Text.Length directly and does not query the window for the actual text length.  
                // If TextMaskFormat is set to a anything different from IncludePromptAndLiterals or HidePromptOnLeave is true the return value 
                // may be incorrect because the Text property value and the display text may be different.  We need to handle this here.
                return _flagState[_isNullMask] ? Text.Length : GetFormattedDisplayString().Length;
            }
        }

        /// <summary>
        /// Specifies whether the test string required input positions, as specified by the mask, have all been assigned.
        /// </summary>
        public bool MaskCompleted
        {
            get { return _maskedTextProvider.MaskCompleted; }
        }

        /// <summary>
        /// Specifies whether all inputs (required and optional) have been provided into the mask successfully.
        /// </summary>
        public bool MaskFull
        {
            get { return _maskedTextProvider.MaskFull; }
        }

        /// <summary>
        /// Returns a copy of the control's internal MaskedTextProvider.  This is useful for user's to provide
        /// cloning semantics for the control (we don't want to do it) w/o incurring in any perf penalty since
        /// some of the properties require recreating the underlying provider when they are changed.
        /// </summary>
        public MaskedTextProvider MaskedTextProvider
        {
            get { return _flagState[_isNullMask] ? null : _maskedTextProvider.Clone(); }
        }

        /// <summary>
        /// Specifies the character to be used in the formatted string in place of editable characters,
        /// if set to any printable character, the text box becomes a password text box, to reset it use the null character.
        /// </summary>
        public char PasswordChar
        {
            get
            {
                // The password char could be the one set in the control or the system password char, 
                // in any case the maskedTextProvider has the correct one.
                return _maskedTextProvider.PasswordChar;
            }

            set
            {
                if (!MaskedTextProvider.IsValidPasswordChar(value))
                {
                    // null character accepted (resets value)
                    throw new ArgumentException("Invalid char error");
                }

                if (_passwordChar != value)
                {
                    if (value == _maskedTextProvider.PromptChar)
                    {
                        // Prompt and password chars must be different.
                        throw new InvalidOperationException("Password and prompt char error");
                    }
                    _passwordChar = value;
                    _maskedTextProvider.PasswordChar = value;
                    if (!_flagState[_isNullMask])
                    {
                        SetWindowText();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies whether the prompt character should be treated as a valid input character or not.
        /// The setter resets the underlying MaskedTextProvider object and attempts
        /// to add the existing input text (if any) using the new mask, failure is ignored.
        /// This property has no particular effect if no mask has been set.
        /// </summary>
        public bool AllowPromptAsInput
        {
            get
            {
                return _maskedTextProvider.AllowPromptAsInput;
            }

            set
            {
                if (value == _maskedTextProvider.AllowPromptAsInput)
                {
                    return;
                }

                SetMaskedTextProvider(new MaskedTextProvider(_maskedTextProvider.Mask,
                    _maskedTextProvider.Culture,
                    value,
                    _maskedTextProvider.PromptChar,
                    _maskedTextProvider.PasswordChar,
                    _maskedTextProvider.AsciiOnly));
            }
        }

        /// <summary>
        /// Specifies whether only ASCII characters are accepted as valid input.
        /// This property has no particular effect if no mask has been set.
        /// </summary>
        public bool AsciiOnly
        {
            get
            {
                return _maskedTextProvider.AsciiOnly;
            }

            set
            {
                if (value == _maskedTextProvider.AsciiOnly)
                {
                    return;
                }

                SetMaskedTextProvider(new MaskedTextProvider(_maskedTextProvider.Mask,
                    _maskedTextProvider.Culture,
                    _maskedTextProvider.AllowPromptAsInput,
                    _maskedTextProvider.PromptChar,
                    _maskedTextProvider.PasswordChar,
                    value));
            }
        }

        /// <summary>
        /// The culture that determines the value of the localizable mask language separators and placeholders.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return _maskedTextProvider.Culture;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (_maskedTextProvider.Culture.Equals(value))
                {
                    return;
                }

                SetMaskedTextProvider(new MaskedTextProvider(_maskedTextProvider.Mask,
                    value,
                    _maskedTextProvider.AllowPromptAsInput,
                    _maskedTextProvider.PromptChar,
                    _maskedTextProvider.PasswordChar,
                    _maskedTextProvider.AsciiOnly));
            }
        }

        /// <summary>
        /// Specifies the formatting options for text cut/copited to the clipboard
        /// (Whether the mask returned from the Text property includes Literals and/or prompt characters).
        /// When prompt characters are excluded, theyare returned as spaces in the string returned.
        /// </summary>
        public MaskFormat CutCopyMaskFormat
        {
            get
            {
                if (_flagState[_cutcopyincludeprompt])
                {
                    return _flagState[_cutcopyincludeliterals]
                        ? MaskFormat.IncludePromptAndLiterals
                        : MaskFormat.IncludePrompt;
                }

                return _flagState[_cutcopyincludeliterals]
                    ? MaskFormat.IncludeLiterals
                    : MaskFormat.ExcludePromptAndLiterals;
            }

            set
            {
                //valid values are 0x0 to 0x3
                if (
                    !Utils.IsEnumValid(value, (int) value, (int) MaskFormat.ExcludePromptAndLiterals,
                        (int) MaskFormat.IncludePromptAndLiterals))
                {
                    throw new ArgumentException(nameof(MaskFormat));
                }

                if (value == MaskFormat.IncludePrompt)
                {
                    _flagState[_cutcopyincludeprompt] = true;
                    _flagState[_cutcopyincludeliterals] = false;
                }
                else if (value == MaskFormat.IncludeLiterals)
                {
                    _flagState[_cutcopyincludeprompt] = false;
                    _flagState[_cutcopyincludeliterals] = true;
                }
                else
                {
                    // value == MaskFormat.IncludePromptAndLiterals || value == MaskFormat.ExcludePromptAndLiterals
                    var include = value == MaskFormat.IncludePromptAndLiterals;
                    _flagState[_cutcopyincludeprompt] = include;
                    _flagState[_cutcopyincludeliterals] = include;
                }
            }
        }

        /// <summary>
        /// Specifies the prompt character to be used in the formatted string for unsupplied characters.
        /// </summary>
        public char PromptChar
        {
            get
            {
                return _maskedTextProvider.PromptChar;
            }

            set
            {
                if (!MaskedTextProvider.IsValidInputChar(value))
                {
                    throw new ArgumentException("Invalid char error");
                }

                if (_maskedTextProvider.PromptChar == value)
                {
                    return;
                }

                if (value == _passwordChar || value == _maskedTextProvider.PasswordChar)
                {
                    throw new InvalidOperationException("Password and prompt char error");
                }

                SetMaskedTextProvider(new MaskedTextProvider(_maskedTextProvider.Mask,
                    _maskedTextProvider.Culture,
                    _maskedTextProvider.AllowPromptAsInput,
                    value,
                    _maskedTextProvider.PasswordChar,
                    _maskedTextProvider.AsciiOnly));
            }
        }

        /// <summary>
        /// Overwrite base class' property.
        /// </summary>
        public new bool IsReadOnly
        {
            get
            {
                return base.IsReadOnly;
            }

            set
            {
                if (base.IsReadOnly == value)
                {
                    return;
                }

                base.IsReadOnly = value;
                if (!_flagState[_isNullMask])
                {
                    // Prompt will be hidden.
                    SetWindowText();
                }
            }
        }

        /// <summary>
        /// Specifies whether to include the mask prompt character when formatting the text in places
        /// where an edit char has not being assigned.
        /// </summary>
        public bool RejectInputOnFirstFailure
        {
            get { return _flagState[_rejectInputOnFirstFailure]; }
            set { _flagState[_rejectInputOnFirstFailure] = value; }
        }

        /// <summary>
        /// Specifies whether to reset and skip the current position if editable, when the input character
        /// has the same value as the prompt.  This property takes precedence over AllowPromptAsInput.
        /// </summary>
        public bool ResetOnPrompt
        {
            get { return _maskedTextProvider.ResetOnPrompt; }
            set { _maskedTextProvider.ResetOnPrompt = value; }
        }

        /// <summary>
        /// Specifies whether to reset and skip the current position if editable, when the input 
        ///     is the space character.
        /// </summary>
        public bool ResetOnSpace
        {
            get { return _maskedTextProvider.ResetOnSpace; }
            set { _maskedTextProvider.ResetOnSpace = value; }
        }

        /// <summary>
        /// Specifies whether to skip the current position if non-editable and the input character has
        /// the same value as the literal at that position.
        /// </summary>
        public bool SkipLiterals
        {
            get { return _maskedTextProvider.SkipLiterals; }
            set { _maskedTextProvider.SkipLiterals = value; }
        }

        /// <summary>
        /// Specifies whether the PromptCharacter is displayed when the control loses focus.
        /// </summary>
        public bool HidePromptOnLeave
        {
            get
            {
                return _flagState[_hidePromptOnLeave];
            }

            set
            {
                if (_flagState[_hidePromptOnLeave] == value)
                {
                    return;
                }

                _flagState[_hidePromptOnLeave] = value;
                if (!_flagState[_isNullMask] && !_focused && !MaskFull && !DesignMode.DesignModeEnabled)
                {
                    SetWindowText();
                }
            }
        }

        /// <summary>
        /// Specifies whether the Mask is displayed when the control loses focus.
        /// </summary>
        public bool HideMaskOnLeave
        {
            get
            {
                return _flagState[_hideMaskOnLeave];
            }

            set
            {
                if (_flagState[_hideMaskOnLeave] == value)
                {
                    return;
                }

                _flagState[_hideMaskOnLeave] = value;
                if (!_flagState[_isNullMask])
                {
                    SetWindowText();
                }
            }
        }

        /// <summary>
        /// Specifies the text insertion mode of the text box.  This can be used to simulated the Access masked text
        /// control behavior where insertion is set to TextInsertionMode.AlwaysOverwrite
        /// This property has no particular effect if no mask has been set.
        /// </summary>
        public InsertKeyMode InsertKeyMode
        {
            get
            {
                return _insertMode;
            }

            set
            {
                if (!Utils.IsEnumValid(value, (int) value, (int) InsertKeyMode.Default, (int) InsertKeyMode.Overwrite))
                {
                    throw new ArgumentException(nameof(InsertKeyMode));
                }
                if (_insertMode == value)
                {
                    return;
                }

                var isOverwrite = IsOverwriteMode;
                _insertMode = value;
                if (isOverwrite != IsOverwriteMode)
                {
                    OnIsOverwriteModeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Specifies whether text insertion mode in 'on' or not.
        /// </summary>
        public bool IsOverwriteMode
        {
            get
            {
                if (_flagState[_isNullMask])
                {
                    return false;
                }
                switch (_insertMode)
                {
                    case InsertKeyMode.Overwrite:
                        return true;
                    case InsertKeyMode.Insert:
                        return false;
                    case InsertKeyMode.Default:
                        // Note that the insert key state should be per process and its initial state insert, this is the
                        // behavior of apps like WinWord, WordPad and VS; so we have to keep track of it and not query its
                        // system value.
                        //return Control.IsKeyLocked(Keys.Insert);
                        return _flagState[_insertToggled];
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// The Text setter validates the input char by char, raising the MaskInputRejected event for invalid chars.
        /// The Text getter returns the formatted text according to the IncludeLiterals and IncludePrompt properties.
        /// </summary>
        public new string Text
        {
            get
            {
                return _flagState[_isNullMask] || _flagState[_queryBaseText] ? base.Text : TextOutput;
            }

            set
            {
                if (_flagState[_isNullMask])
                {
                    base.Text = value;
                    return;
                }
                if (string.IsNullOrEmpty(value))
                {
                    // reset the input text.
                    Delete(VirtualKey.Delete, 0, _maskedTextProvider.Length);
                }
                else
                {
                    if (RejectInputOnFirstFailure)
                    {
                        MaskedTextResultHint hint;
                        var oldText = TextOutput;
                        if (_maskedTextProvider.Set(value, out _caretTestPos, out hint))
                        {
                            if (TextOutput != oldText)
                            {
                                SetText();
                            }
                            SelectionStart = ++_caretTestPos;
                        }
                        else
                        {
                            OnMaskInputRejected(new MaskInputRejectedEventArgs(_caretTestPos, hint));
                        }
                    }
                    else
                    {
                        Replace(value, 0, _maskedTextProvider.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the formatting options for text output (Whether the mask returned from the Text
        /// property includes Literals and/or prompt characters).
        /// When prompt characters are excluded, theyare returned as spaces in the string returned.
        /// </summary>
        public MaskFormat TextMaskFormat
        {
            get
            {
                if (IncludePrompt)
                {
                    return IncludeLiterals ? MaskFormat.IncludePromptAndLiterals : MaskFormat.IncludePrompt;
                }

                return IncludeLiterals ? MaskFormat.IncludeLiterals : MaskFormat.ExcludePromptAndLiterals;
            }

            set
            {
                if (TextMaskFormat == value)
                {
                    return;
                }

                //valid values are 0x0 to 0x3
                if (
                    !Utils.IsEnumValid(value, (int) value, (int) MaskFormat.ExcludePromptAndLiterals,
                        (int) MaskFormat.IncludePromptAndLiterals))
                {
                    throw new ArgumentException(nameof(MaskFormat));
                }

                // Changing the TextMaskFormat will likely change the 'output' text (Text getter value).  Cache old value to 
                // verify it against the new value and raise OnTextChange if needed.
                var oldText = _flagState[_isNullMask] ? null : TextOutput;

                if (value == MaskFormat.IncludePrompt)
                {
                    IncludePrompt = true;
                    IncludeLiterals = false;
                }
                else if (value == MaskFormat.IncludeLiterals)
                {
                    IncludePrompt = false;
                    IncludeLiterals = true;
                }
                else
                {
                    // value == MaskFormat.IncludePromptAndLiterals || value == MaskFormat.ExcludePromptAndLiterals
                    var include = value == MaskFormat.IncludePromptAndLiterals;
                    IncludePrompt = include;
                    IncludeLiterals = include;
                }

                if (oldText != null && oldText != TextOutput)
                {
                    //OnTextChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The formatted text, it is what the Text getter returns when a mask has been applied to the control.
        /// The text format follows the IncludeLiterals and IncludePrompt properties (See MaskedTextProvider.ToString()).
        /// </summary>
        private string TextOutput
        {
            get
            {
                //Debug.Assert(!flagState[IS_NULL_MASK], "This method must be called when a Mask is provided.");
                return _flagState[_isNullMask] ? string.Empty : _maskedTextProvider.ToString();
            }
        }

        /// <summary>
        /// Specifies whether to include mask literal characters when formatting the text.
        /// </summary>
        private bool IncludeLiterals
        {
            get { return _maskedTextProvider.IncludeLiterals; }
            set { _maskedTextProvider.IncludeLiterals = value; }
        }

        /// <summary>
        /// Specifies whether to include the mask prompt character when formatting the text in places
        /// where an edit char has not being assigned.
        /// </summary>
        private bool IncludePrompt
        {
            get { return _maskedTextProvider.IncludePrompt; }
            set { _maskedTextProvider.IncludePrompt = value; }
        }

        /// <summary>
        /// Gets the modifier flags for a <see cref='TextBox.KeyDown'/> or <see cref='TextBox.KeyUp'/> event.
        /// This indicates which modifier keys (CTRL, SHIFT, and/or ALT) were pressed.
        /// </summary>
        private VirtualKey Modifiers
        {
            get
            {
                var controlKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                var ctrl = (controlKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                if (ctrl)
                {
                    return VirtualKey.Control;
                }

                var shiftKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);
                var shift = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                if (shift)
                {
                    return VirtualKey.Shift;
                }

                var altKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu);
                var alt = (altKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                return alt ? VirtualKey.Menu : VirtualKey.None;
            }
        }

        /// <summary>
        /// MaskInputRejected event.
        /// </summary>
        public event MaskInputRejectedEventHandler MaskInputRejected;

        /// <summary>
        /// IsOverwriteModeChanged event.
        /// </summary>
        public event EventHandler IsOverwriteModeChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDeleteButtonClick(object sender, RoutedEventArgs args)
        {
            if (!_flagState[_isNullMask])
            {
                _maskedTextProvider.Clear();
                SetText();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            _selectionStart = SelectionStart;
            _selectionLength = SelectionLength;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnPaste(object sender, TextControlPasteEventArgs e)
        {
            if (_flagState[_isNullMask])
            {
                // Operates as a regular text box base.
                return;
            }
            e.Handled = true;
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var text = await dataPackageView.GetTextAsync();
                PasteInt(text);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (_flagState[_isNullMask])
            {
                // Operates as a regular text box base.
                Value = base.Text;
            }
        }

        /// <summary>
        /// Specifies the event that is fired when a new character is received by the input queue.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="args">The event data. If there is no event data, this parameter will be null.</param>
        private void OnCharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (_flagState[_isNullMask])
            {
                // Operates as a regular text box base.
                return;
            }

            // Handle event
            args.Handled = true;

            // Convert the key to Unicode character
            var symbol = Convert.ToChar(args.KeyCode);

            if (IsReadOnly)
            {
                return;
            }

            // At this point the character needs to be processed ...
            MaskedTextResultHint hint;

            string oldText = TextOutput;
            if (PlaceChar(symbol, _selectionStart, _selectionLength, IsOverwriteMode, out hint))
            {
                //if( hint == MaskedTextResultHint.Success || hint == MaskedTextResultHint.SideEffect )
                if (TextOutput != oldText)
                {
                    // Now set the text in the display.
                    SetText();
                }
                // caretTestPos is updated in PlaceChar.
                SelectionStart = ++_caretTestPos;
            }
            else
            {
                // caretTestPos is updated in PlaceChar.
                OnMaskInputRejected(new MaskInputRejectedEventArgs(_caretTestPos, hint));
            }

            if (_selectionLength > 0)
            {
                _selectionLength = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        private void OnClipboardContentChanged(object sender, object o)
        {
            if (_maskedTextProvider.IsPassword)
            {
                // cannot copy password to clipboard.
                return;
            }
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var dataPackage = new DataPackage {RequestedOperation = dataPackageView.RequestedOperation};
                var text = GetSelectedText();
                dataPackage.SetText(text);
                Clipboard.ContentChanged -= OnClipboardContentChanged;
                Clipboard.Clear();
                Clipboard.SetContent(dataPackage);
                Clipboard.ContentChanged += OnClipboardContentChanged;
                if (base.Text != Text)
                {
                    Delete(VirtualKey.Delete, _selectionStart, _selectionLength);
                }
            }
        }

        /// <summary>
        /// Callback for changes to the Value property.
        /// </summary>
        /// <param name="value">The new value of the property.</param>
        private void OnValueChanged(string value)
        {
            //if (flagState[IS_NULL_MASK] || flagState[QUERY_BASE_TEXT])
            if (_flagState[_queryBaseText])
            {
                return;
            }

            Text = value;
        }

        /// <summary>
        /// Callback for changes to the Mask property.
        /// </summary>
        /// <param name="value">The new value of the property.</param>
        private void OnMaskChanged(string value)
        {
            // We dont' do anything if:
            // 1.  IsNullOrEmpty( value )->[Reset control] && this.flagState[IS_NULL_MASK]==>Already Reset.
            // 2. !IsNullOrEmpty( value )->[Set control] && !this.flagState[IS_NULL_MASK][control is set] && [value is the same]==>No need to update.
            if (_flagState[_isNullMask] == string.IsNullOrEmpty(value) &&
                (_flagState[_isNullMask] || value == _maskedTextProvider.Mask))
            {
                return;
            }

            string text = null;
            var newMask = value;

            // We need to update the this.flagState[IS_NULL_MASK]field before raising any events (when setting the maskedTextProvider) so 
            // querying for properties from an event handler returns the right value (i.e: Text).

            // Resetting the control, the native edit control will be in charge.
            if (string.IsNullOrEmpty(value))
            {
                var unformattedText = _maskedTextProvider.ToString(false, false);
                _flagState[_isNullMask] = true;

                // Set the window text to the unformatted text before raising events. Also, TextChanged needs to be raised after MaskChanged so
                // pass false to SetWindowText 'raiseTextChanged' param.
                SetWindowText(unformattedText, false);
                newMask = NullMask;
            }
            else
            {
                // Setting control to a new value.
                if (value.Any(c => !MaskedTextProvider.IsValidMaskChar(c)))
                {
                    throw new ArgumentException("Mask invalid char");
                }

                if (_flagState[_isNullMask])
                {
                    // If this.IsNullMask, we are setting the mask to a new value; in this case we need to get the text because
                    // the underlying MTP does not have it (used as a property backend only) and pass it to SetMaskedTextProvider
                    // method below to update the provider.
                    text = base.Text;
                }
            }

            // Recreate masked text provider since this property is read-only.
            //text == null when setting to a different mask value or when resetting the mask to null.
            //text != null only when setting the mask from null to some value.
            SetMaskedTextProvider(new MaskedTextProvider(newMask,
                    _maskedTextProvider.Culture,
                    _maskedTextProvider.AllowPromptAsInput,
                    _maskedTextProvider.PromptChar,
                    _maskedTextProvider.PasswordChar,
                    _maskedTextProvider.AsciiOnly),
                text);
        }

        /// <summary>
        /// Initializes the object with the specified MaskedTextProvider object and default property values.
        /// </summary>
        /// <param name="newProvider">The new value of the masked text provider.</param>
        private void Initialize(MaskedTextProvider newProvider)
        {
            _maskedTextProvider = newProvider;

            // set the initial display text.
            if (!_flagState[_isNullMask])
            {
                SetWindowText();
            }

            // set default values.
            _passwordChar = _maskedTextProvider.PasswordChar;
            _insertMode = InsertKeyMode.Default;

            _flagState[_hidePromptOnLeave] = false;
            _flagState[_hideMaskOnLeave] = false;
            _flagState[_rejectInputOnFirstFailure] = false;

            // CutCopyMaskFormat - set same defaults as TextMaskFormat (IncludePromptAndLiterals).
            // It is a lot easier to handle this flags individually since that's the way the MaskedTextProvider does it.
            _flagState[_cutcopyincludeprompt] = _maskedTextProvider.IncludePrompt;
            _flagState[_cutcopyincludeliterals] = _maskedTextProvider.IncludeLiterals;

            // fields for internal use.
            _flagState[_handleCharacterReceive] = true;
            _caretTestPos = 0;
        }

        /// <summary>
        /// Sets the underlying MaskedTextProvider object.
        /// Overload to allow for passing the text when the mask is being changed from null,
        /// in this case the maskedTextProvider holds backend info only (not the text).
        /// </summary>
        /// <param name="newProvider">The new value of the masked text provider.</param>
        /// <param name="textOnInitializingMask">The value of the current Text property.</param>
        private void SetMaskedTextProvider(MaskedTextProvider newProvider, string textOnInitializingMask = null)
        {
            // Set R/W properties.
            newProvider.IncludePrompt = _maskedTextProvider.IncludePrompt;
            newProvider.IncludeLiterals = _maskedTextProvider.IncludeLiterals;
            newProvider.SkipLiterals = _maskedTextProvider.SkipLiterals;
            newProvider.ResetOnPrompt = _maskedTextProvider.ResetOnPrompt;
            newProvider.ResetOnSpace = _maskedTextProvider.ResetOnSpace;

            // If mask not initialized and not initializing it, the new provider is just a property backend.
            // Change won't have any effect in text.
            if (_flagState[_isNullMask] && textOnInitializingMask == null)
            {
                _maskedTextProvider = newProvider;
                return;
            }

            var testPos = 0;
            // Raise if new provider rejects old text.
            bool raiseOnMaskInputRejected;
            var hint = MaskedTextResultHint.NoEffect;
            var oldProvider = _maskedTextProvider;

            // Attempt to add previous text.
            // If the mask is the same, we need to preserve the caret and character positions if the text is added successfully.
            var preserveCharPos = oldProvider.Mask == newProvider.Mask;

            // NOTE: Whenever changing the MTP, the text is lost if any character in the old text violates the new provider's mask.

            // Changing Mask (from null), which is the only RO property that requires passing text.
            if (textOnInitializingMask != null)
            {
                raiseOnMaskInputRejected = !newProvider.Set(textOnInitializingMask, out testPos, out hint);
            }
            else
            {
                // We need to attempt to set the input characters one by one in the edit positions so they are not
                // escaped. 
                int assignedCount = oldProvider.AssignedEditPositionCount;
                int srcPos = 0;
                int dstPos = 0;

                while (assignedCount > 0)
                {
                    srcPos = oldProvider.FindAssignedEditPositionFrom(srcPos, Forward);
                    Debug.Assert(srcPos != MaskedTextProvider.InvalidIndex, "InvalidIndex unexpected at this time.");

                    if (preserveCharPos)
                    {
                        dstPos = srcPos;
                    }
                    else
                    {
                        dstPos = newProvider.FindEditPositionFrom(dstPos, Forward);

                        if (dstPos == MaskedTextProvider.InvalidIndex)
                        {
                            newProvider.Clear();

                            testPos = newProvider.Length;
                            hint = MaskedTextResultHint.UnavailableEditPosition;
                            break;
                        }
                    }

                    if (!newProvider.Replace(oldProvider[srcPos], dstPos, out testPos, out hint))
                    {
                        preserveCharPos = false;
                        newProvider.Clear();
                        break;
                    }

                    srcPos++;
                    dstPos++;
                    assignedCount--;
                }

                raiseOnMaskInputRejected = !MaskedTextProvider.GetOperationResultFromHint(hint);
            }

            // Set provider.
            _maskedTextProvider = newProvider;
            if (_flagState[_isNullMask])
            {
                _flagState[_isNullMask] = false;
            }

            // Raising events need to be done only after the new provider has been set so the MTB is in a state where properties 
            // can be queried from event handlers safely.
            if (raiseOnMaskInputRejected)
            {
                OnMaskInputRejected(new MaskInputRejectedEventArgs(testPos, hint));
            }

            SetWindowText(GetFormattedDisplayString(), preserveCharPos);

            // Корректировка позиции
            if (!preserveCharPos)
            {
                SelectionStart = ++testPos;
            }
        }

        /// <summary>
        /// The currently selected text (if any) in the control.
        /// </summary>
        public new string SelectedText
        {
            get
            {
                if (_flagState[_isNullMask])
                {
                    return base.SelectedText;
                }
                return GetSelectedText();
            }

            set
            {
                if (_flagState[_isNullMask])
                {
                    // Operates as a regular text box base.
                    base.SelectedText = value;
                    return;
                }
                PasteInt(value);
            }
        }

        /// <summary>
        /// The selected text in the control according to the CutCopyMaskFormat properties (IncludePrompt/IncludeLiterals).
        /// This is used in Cut/Copy operations (SelectedText).
        /// The prompt character is always replaced with a blank character.
        /// </summary>
        private string GetSelectedText()
        {
            Debug.Assert(!_flagState[_isNullMask], "This method must be called when a Mask is provided.");

            if (_selectionLength == 0)
            {
                return string.Empty;
            }

            bool includePrompt = (CutCopyMaskFormat & MaskFormat.IncludePrompt) != 0;
            bool includeLiterals = (CutCopyMaskFormat & MaskFormat.IncludeLiterals) != 0;

            return _maskedTextProvider.ToString( /*ignorePasswordChar*/
                true, includePrompt, includeLiterals, _selectionStart, _selectionLength);
        }

        /// <summary>
        /// Gets the string in the text box following the formatting parameters includePrompt
        /// and includeLiterals and honoring the PasswordChar property.
        /// </summary>
        /// <returns></returns>
        private string GetFormattedDisplayString()
        {
            Debug.Assert(!_flagState[_isNullMask], "This method must be called when a Mask is provided.");

            if (HideMaskOnLeave && !_focused && string.IsNullOrEmpty(Value))
            {
                return string.Empty;
            }

            bool includePrompt;
            if (IsReadOnly)
            {
                // Always hide prompt.
                includePrompt = false;
            }
            else if (DesignMode.DesignModeEnabled)
            {
                // Not RO and at design time, always show prompt.
                includePrompt = true;
            }
            else
            {
                // follow HidePromptOnLeave property.
                includePrompt = !(HidePromptOnLeave && !_focused);
            }
            return _maskedTextProvider.ToString(ignorePasswordChar: false,
                includePrompt: includePrompt,
                includeLiterals: true,
                startPosition: 0,
                length: _maskedTextProvider.Length);
        }

        /// <summary>
        /// Sets the control's text to the formatted text obtained from the underlying MaskedTextProvider.
        /// TextChanged is raised always, this assumes the display or the output text changed.
        /// The caret position is lost (unless cached somewhere else like when lossing the focus).
        /// This is the common way of changing the text in the control.
        /// </summary>
        private void SetText()
        {
            SetWindowText(GetFormattedDisplayString(), false);
        }

        /// <summary>
        /// Sets the control's text to the formatted text obtained from the underlying MaskedTextProvider.
        /// TextChanged is not raised. [PasswordChar] The caret position is preserved.
        /// </summary>
        private void SetWindowText()
        {
            SetWindowText(GetFormattedDisplayString(), true);
        }

        /// <summary>
        /// Sets the text directly in the underlying edit control to the value specified.
        /// The 'raiseTextChangedEvent' param determines whether TextChanged event is raised or not.
        /// The 'preserveCaret' param determines whether an attempt to preserve the caret position should be made
        /// or not after the call to SetWindowText (WindowText) is performed.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="preserveCaret"></param>
        private void SetWindowText(string text, bool preserveCaret)
        {
            _flagState[_queryBaseText] = true;
            try
            {
                if (preserveCaret)
                {
                    _caretTestPos = _selectionStart;
                }

                base.Text = text;
                Value = _maskedTextProvider.ToString(false, false);

                if (preserveCaret)
                {
                    SelectionStart = _caretTestPos;
                }
            }
            finally
            {
                _flagState[_queryBaseText] = false;
            }
        }

        /// <summary>
        /// Raises the MaskInputRejected event.
        /// </summary>
        /// <param name="e">Event args.</param>
        private void OnMaskInputRejected(MaskInputRejectedEventArgs e)
        {
            var handler = MaskInputRejected;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the IsOverwriteModeChanged event.
        /// </summary>
        /// <param name="e"></param>
        private void OnIsOverwriteModeChanged(EventArgs e)
        {
            var handler = IsOverwriteModeChanged;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Deletes characters from the control's text according to the key pressed (Delete/Backspace).
        /// Returns true if something gets actually deleted, false otherwise.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="startPosition"></param>
        /// <param name="selectionLen"></param>
        private void Delete(VirtualKey keyCode, int startPosition, int selectionLen)
        {
            Debug.Assert(!_flagState[_isNullMask], "This method must be called when a Mask is provided.");
            Debug.Assert(keyCode == VirtualKey.Delete || keyCode == VirtualKey.Back,
                "Delete called with keyCode == " + keyCode);
            Debug.Assert(startPosition >= 0 && (startPosition + selectionLen <= _maskedTextProvider.Length),
                "Invalid position range.");

            // On backspace, moving the start postion back by one has the same effect as delete.  If text is selected, there is no
            // need for moving the position back.

            _caretTestPos = startPosition;

            if (selectionLen == 0)
            {
                if (keyCode == VirtualKey.Back)
                {
                    // At beginning of string, backspace does nothing.
                    if (startPosition == 0)
                    {
                        return;
                    }
                    // so it can be treated as delete.
                    startPosition--;
                }
                else
                {
                    // (keyCode == Keys.Delete)
                    if (startPosition + selectionLen == _maskedTextProvider.Length)
                    {
                        // At end of string, delete does nothing.
                        return;
                    }
                }
            }

            int tempPos;
            var endPos = selectionLen > 0 ? startPosition + selectionLen - 1 : startPosition;
            MaskedTextResultHint hint;

            var oldText = TextOutput;
            if (_maskedTextProvider.RemoveAt(startPosition, endPos, out tempPos, out hint))
            {
                if (TextOutput != oldText)
                {
                    SetText();
                    _caretTestPos = startPosition;
                }
                else
                {
                    // If succeeded but nothing removed, the caret should move as follows:
                    // 1. If selectionLen > 0, or on back and hint == SideEffect: move to selectionStart.
                    // 2. If hint == NoEffect, On Delete move to next edit position, if any or not already in one. 
                    //    On back move to the next edit postion at the left if no more assigned position at the right, 
                    //    in such case find an assigned position and move one past or one position left if no assigned pos found 
                    //    (taken care by 'startPosition--' above).
                    // 3. If hint == SideEffect, on Back move like arrow key, (startPosition is already moved, startPosition-- above).

                    if (selectionLen > 0)
                    {
                        _caretTestPos = startPosition;
                    }
                    else
                    {
                        if (hint == MaskedTextResultHint.NoEffect)
                        {
                            // Case 2.
                            if (keyCode == VirtualKey.Delete)
                            {
                                _caretTestPos = _maskedTextProvider.FindEditPositionFrom(startPosition, Forward);
                            }
                            else
                            {
                                if (_maskedTextProvider.FindAssignedEditPositionFrom(startPosition, Forward) ==
                                    MaskedTextProvider.InvalidIndex)
                                {
                                    // No assigned position at the right, nothing to shift then move to the next assigned position at the
                                    // left (if any).
                                    _caretTestPos = _maskedTextProvider.FindAssignedEditPositionFrom(startPosition,
                                        Backward);
                                }
                                else
                                {
                                    // there are assigned positions at the right so move to an edit position at the left to get ready for 
                                    // removing the character on it or just shifting the characters at the right
                                    _caretTestPos = _maskedTextProvider.FindEditPositionFrom(startPosition, Backward);
                                }

                                if (_caretTestPos != MaskedTextProvider.InvalidIndex)
                                {
                                    // backspace gets ready to remove one position past the edit position.
                                    _caretTestPos++;
                                }
                            }

                            if (_caretTestPos == MaskedTextProvider.InvalidIndex)
                            {
                                _caretTestPos = startPosition;
                            }
                        }
                        else
                        {
                            // (hint == MaskedTextProvider.OperationHint.SideEffect)
                            if (keyCode == VirtualKey.Back)
                            {
                                // Case 3.
                                _caretTestPos = startPosition;
                            }
                        }
                    }
                }
            }
            else
            {
                OnMaskInputRejected(new MaskInputRejectedEventArgs(tempPos, hint));
            }

            // Reposition caret.
            SelectInternal(_caretTestPos, 0);
        }

        /// <summary>
        /// Replaces the current selection in the text box specified by the startPosition
        /// and selectionLen parameters with the contents of the supplied string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startPosition"></param>
        /// <param name="selectionLen"></param>
        private void Replace(string text, int startPosition, int selectionLen)
        {
            Debug.Assert(!_flagState[_isNullMask], "This method must be called when a Mask is provided.");
            Debug.Assert(text != null, "text is null.");

            // Clone the MaskedTextProvider so text properties are not modified until the paste operation is
            // completed.  This is needed in case one of these properties is retreived in a MaskedInputRejected
            // event handler (clipboard text is attempted to be set into the input text char by char).

            var clonedProvider = _maskedTextProvider.Clone();

            // Cache the current caret position so we restore it in case the text does not change. VSW#498875.
            int currentCaretPos = _caretTestPos;

            // First replace characters in the selection (if any and if any edit positions) until completed, or the test position falls 
            // outside the selection range, or there's no more room in the test string for editable characters.
            // Then insert any remaining characters from the input.

            var hint = MaskedTextResultHint.NoEffect;
            int endPos = startPosition + selectionLen - 1;

            if (RejectInputOnFirstFailure)
            {
                bool succeeded;

                succeeded = (startPosition > endPos)
                    ? clonedProvider.InsertAt(text, startPosition, out _caretTestPos, out hint)
                    : clonedProvider.Replace(text, startPosition, endPos, out _caretTestPos, out hint);

                if (!succeeded)
                {
                    OnMaskInputRejected(new MaskInputRejectedEventArgs(_caretTestPos, hint));
                }
            }
            else
            {
                // temp hint used to preserve the 'primary' operation hint (no side effects).
                MaskedTextResultHint tempHint = hint;
                int testPos;

                foreach (char ch in text)
                {
                    // char won't be escaped, find and edit position for it.
                    if (!_maskedTextProvider.VerifyEscapeChar(ch, startPosition))
                    {
                        // Observe that we look for a position w/o respecting the selection length, because the input text could be larger than
                        // the number of edit positions in the selection.
                        testPos = clonedProvider.FindEditPositionFrom(startPosition, Forward);

                        if (testPos == MaskedTextProvider.InvalidIndex)
                        {
                            // this will continue to execute (fail) until the end of the text so we fire the event for each remaining char.
                            //OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, MaskedTextResultHint.UnavailableEditPosition));
                            continue;
                        }

                        startPosition = testPos;
                    }

                    int length = endPos >= startPosition ? 1 : 0;

                    // if length > 0 we are (re)placing the input char in the current startPosition, otherwise we are inserting the input.
                    bool replace = length > 0;

                    if (PlaceChar(clonedProvider, ch, startPosition, length, replace, out tempHint))
                    {
                        // caretTestPos is updated in PlaceChar call.
                        startPosition = _caretTestPos + 1;

                        // place char will insert or replace a single character so the hint must be success, and that will be the final operation
                        // result hint.
                        if (tempHint == MaskedTextResultHint.Success && hint != tempHint)
                        {
                            hint = tempHint;
                        }
                    }
                    else
                    {
                        OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, tempHint));
                    }
                }

                if (selectionLen > 0)
                {
                    // At this point we have processed all characters from the input text (if any) but still need to 
                    // remove remaining characters from the selected text (if editable and valid chars).

                    if (startPosition <= endPos)
                    {
                        if (!clonedProvider.RemoveAt(startPosition, endPos, out _caretTestPos, out tempHint))
                        {
                            OnMaskInputRejected(new MaskInputRejectedEventArgs(_caretTestPos, tempHint));
                        }

                        // If 'replace' is not actually performed (maybe the input is empty which means 'remove', hint will be whatever
                        // the 'remove' operation result hint is.
                        if (hint == MaskedTextResultHint.NoEffect && hint != tempHint)
                        {
                            hint = tempHint;
                        }
                    }
                }
            }

            bool updateText = TextOutput != clonedProvider.ToString();

            // Always set the mtp, the formatted text could be the same but the assigned positions may be different.
            _maskedTextProvider = clonedProvider;

            // Update text if needed.
            if (updateText)
            {
                SetText();

                // Update caret position.
                _caretTestPos = startPosition;
                SelectInternal(_caretTestPos, 0);
            }
            else
            {
                _caretTestPos = currentCaretPos;
            }
        }

        /// <summary>
        /// Insert or replaces the specified character into the control's text and updates the caret position.
        /// If overwrite is true, it replaces the character at the selection start position.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="startPosition"></param>
        /// <param name="length"></param>
        /// <param name="overwrite"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        private bool PlaceChar(char ch, int startPosition, int length, bool overwrite, out MaskedTextResultHint hint)
        {
            return PlaceChar(_maskedTextProvider, ch, startPosition, length, overwrite, out hint);
        }

        /// <summary>
        /// Override version to be able to perform the operation on a cloned provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="ch"></param>
        /// <param name="startPosition"></param>
        /// <param name="length"></param>
        /// <param name="overwrite"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        private bool PlaceChar(MaskedTextProvider provider, char ch, int startPosition, int length, bool overwrite,
            out MaskedTextResultHint hint)
        {
            Debug.Assert(!_flagState[_isNullMask], "This method must be called when a Mask is provided.");

            _caretTestPos = startPosition;

            if (startPosition < _maskedTextProvider.Length)
            {
                if (length > 0)
                {
                    // Replacing selection with input char.
                    int endPos = startPosition + length - 1;
                    return provider.Replace(ch, startPosition, endPos, out _caretTestPos, out hint);
                }
                else
                {
                    if (overwrite)
                    {
                        // overwrite character at next edit position from startPosition (inclusive).
                        return provider.Replace(ch, startPosition, out _caretTestPos, out hint);
                    }
                    else
                    {
                        // insert.
                        return provider.InsertAt(ch, startPosition, out _caretTestPos, out hint);
                    }
                }
            }

            hint = MaskedTextResultHint.UnavailableEditPosition;
            return false;
        }

        /// <summary>
        /// Performs the actual select without doing arg checking.
        /// </summary>
        private void SelectInternal(int start, int length)
        {
            base.Select(start, length);
        }

        /// <summary>
        /// Pastes specified text over the currently selected text (if any) shifting upper characters if
        /// input is longer than selected text, and/or removing remaining characters from the selection if
        /// input contains less characters.
        /// </summary>
        /// <param name="text"></param>
        private void PasteInt(string text)
        {
            Debug.Assert(!_flagState[_isNullMask], "This method must be called when a Mask is provided.");
            if (string.IsNullOrEmpty(text))
            {
                Delete(VirtualKey.Delete, _selectionStart, _selectionLength);
            }
            else
            {
                Replace(text, _selectionStart, _selectionLength);
            }
        }
    }
}