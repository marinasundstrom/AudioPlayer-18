using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MvvmUtils
{
	public class ObservableObject : INotifyPropertyChanged
	{
		public virtual bool SetProperty<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
		{
			if (Equals(field, newValue))
			{
				return false;
			}

			var propertyName = GetPropertyName(propertyExpression);

			field = newValue;
			OnPropertyChanged(propertyName);

			return true;
		}

		public virtual bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, newValue))
			{
				return false;
			}

			field = newValue;
			OnPropertyChanged(propertyName);

			return true;
		}

		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Helpers

		private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression.Body is MemberExpression memberExpression
			   && memberExpression.Member.MemberType == MemberTypes.Property)
			{
				return memberExpression.Member.Name;
			}

			throw new InvalidOperationException("Expected a property expression.");
		}

		#endregion
	}
}
