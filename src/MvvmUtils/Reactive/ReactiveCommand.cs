using System;
using System.Threading.Tasks;

namespace MvvmUtils.Reactive
{
	public static class ReactiveCommand
	{
		public static ReactiveCommand<T> Create<T>(Func<Task<T>> execute, IObservable<bool> canExecute = null, bool initialCondition = false) =>
			new ReactiveCommand<T>(execute, canExecute, initialCondition);

		public static ReactiveCommand<T, TResult> Create<T, TResult>(Func<T, Task<TResult>> execute, IObservable<bool> canExecute = null, bool initialCondition = false) =>
			new ReactiveCommand<T, TResult>(execute, canExecute, initialCondition);
	}
}
