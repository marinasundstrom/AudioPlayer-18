using System;
using System.Windows.Input;

namespace MvvmUtils
{
	public class RelayCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public RelayCommand(Action execute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		}

		public RelayCommand(Action execute, Func<bool> canExecute)
			: this(execute)
		{
			_canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

		public void Execute(object parameter) => _execute?.Invoke();

		public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public static RelayCommand Create(Action execute) => new RelayCommand(execute);

		public static RelayCommand Create(Action execute, Func<bool> canExecute) => new RelayCommand(execute, canExecute);

		public static RelayCommand<T> Create<T>(Action<T> execute) => new RelayCommand<T>(execute);

		public static RelayCommand<T> Create<T>(Action<T> execute, Func<T, bool> canExecute) => new RelayCommand<T>(execute, canExecute);
    }
}
