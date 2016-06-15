using System;

namespace Microsoft.Windows.Toolkit.SampleApp.Mvvm
{
    // http://codepaste.net/jgxazh
    using System.Diagnostics;

    public class Command : System.Windows.Input.ICommand
    {
        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;
        public event EventHandler CanExecuteChanged;

        public Command(Action execute)
            : this(execute, () => true)
        { /* empty */ }

        public Command(Action execute, Func<bool> canexecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            m_Execute = execute;
            m_CanExecute = canexecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            try
            {
                return m_CanExecute == null ? true : m_CanExecute();
            }
            catch
            {
                Debugger.Break();
                return false;
            }
        }

        public void Execute(object p)
        {
            if (CanExecute(p))
                try
                {
                    m_Execute();
                }
                catch { Debugger.Break(); }
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    public class Command<T> : System.Windows.Input.ICommand
    {
        private readonly Action<T> m_Execute;
        private readonly Func<T, bool> m_CanExecute;
        public event EventHandler CanExecuteChanged;

        public Command(Action<T> execute)
            : this(execute, (x) => true)
        { /* empty */ }

        public Command(Action<T> execute, Func<T, bool> canexecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            m_Execute = execute;
            m_CanExecute = canexecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            try
            {
                var _Value = (T)Convert.ChangeType(p, typeof(T));
                return m_CanExecute == null ? true : m_CanExecute(_Value);
            }
            catch
            {
                Debugger.Break();
                return false;
            }
        }

        public void Execute(object p)
        {
            if (CanExecute(p))
                try
                {
                    var _Value = (T)Convert.ChangeType(p, typeof(T));
                    m_Execute(_Value);
                }
                catch { Debugger.Break(); }
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
