using System;
using System.Windows.Input;

namespace MvvmUtils
{
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;

		public RelayCommand(Action<T> execute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		}

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
			: this(execute)
		{
			_canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => _canExecute?.Invoke((T)parameter) ?? true;

		public void Execute(object parameter) => _execute?.Invoke((T)parameter);

		public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
