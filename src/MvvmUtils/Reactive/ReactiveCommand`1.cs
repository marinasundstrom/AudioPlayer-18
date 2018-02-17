using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmUtils.Reactive
{
	public class ReactiveCommand<T> : ISubject<T>, ICommand
	{
		private Subject<T> _subject = new Subject<T>();

		private readonly Func<Task<T>> _execute;
		private readonly IObservable<bool> _canExecute;
		private bool _canExecuteValue;
		private ISubject<Exception> _thrownExceptions;
		private EventHandler canExecuteChanged;

		public ReactiveCommand(Func<Task<T>> execute, IObservable<bool> canExecute = null, bool initialCondition = false)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
			_canExecuteValue = initialCondition;

			_thrownExceptions = new Subject<Exception>();

			if(_canExecute == null)
			{
				_canExecuteValue = true;
			}

			_canExecute?.Subscribe(value =>
			{
				if (_canExecuteValue != value)
				{
					canExecuteChanged.Invoke(this, EventArgs.Empty);
				}
				_canExecuteValue = value;
			});
		}

		public async Task ExecuteAsync()
		{
			try
			{
				var result = await _execute();
				OnNext(result);
			}
			catch (Exception exception)
			{
				_thrownExceptions.OnNext(exception);
			}
		}

		public IObservable<Exception> ThrownExceptions => _thrownExceptions;

		#region Explicit INotifyPropertyChanged members

		bool ICommand.CanExecute(object parameter) => _canExecuteValue;

		async void ICommand.Execute(object parameter) => await ExecuteAsync();

		public event EventHandler CanExecuteChanged
		{
			add
			{
				canExecuteChanged += value;
			}

			remove
			{
				canExecuteChanged -= value;
			}
		}

		#endregion

		#region ISubject<T>

		public void OnCompleted()
		{
			_subject.OnCompleted();
		}

		public void OnError(Exception error)
		{
			_subject.OnError(error);
		}

		public void OnNext(T value)
		{
			_subject.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return _subject.Subscribe(observer);
		}

		#endregion
	}
}
