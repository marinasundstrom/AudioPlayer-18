using System;

namespace MvvmUtils
{
	public interface IMessageBus : IDisposable
	{
		void Publish<T>(T instance);
		IObservable<T> WhenPublished<T>();
	}
}