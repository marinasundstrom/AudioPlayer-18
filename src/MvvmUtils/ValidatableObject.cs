using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MvvmUtils
{
	public class ValidatableObject : ObservableObject, INotifyDataErrorInfo
	{
		private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

		public ValidatableObject()
		{
			ErrorsChanged += ValidationBase_ErrorsChanged;
		}

		private void ValidationBase_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			OnPropertyChanged("HasErrors");
			OnPropertyChanged("ErrorsList");
		}

		#region INotifyDataErrorInfo Members

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public IEnumerable GetErrors(string propertyName)
		{
			if (!string.IsNullOrEmpty(propertyName))
			{
				if (_errors.ContainsKey(propertyName) && (_errors[propertyName].Count > 0))
				{
					return _errors[propertyName].ToList();
				}
				else
				{
					return new List<string>();
				}
			}
			else
			{
				return _errors.SelectMany(err => err.Value.ToList()).ToList();
			}
		}

		public bool HasErrors
		{
			get
			{
				return _errors.Any(propErrors => propErrors.Value.Count > 0);
			}
		}

		#endregion

		protected virtual void ValidateProperty<T>(T value, [CallerMemberName] string propertyName = null)
		{
			var validationContext = new ValidationContext(this, null)
			{
				MemberName = propertyName
			};

			var validationResults = new List<ValidationResult>();
			Validator.TryValidateProperty(value, validationContext, validationResults);

			RemoveErrorsByPropertyName(propertyName);

			HandleValidationResults(validationResults);
		}

		protected virtual bool SetAndValidateProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) 
		{
			if (SetProperty(ref field, value, propertyName))
			{
				ValidateProperty(value, propertyName);
				return true;
			}
			return false;
		}

		protected virtual void ValidateAll()
		{
			var validationContext = new ValidationContext(this, null);

			var validationResults = new List<ValidationResult>();
			Validator.TryValidateObject(this, validationContext, validationResults);

			RemoveAllErrors();

			HandleValidationResults(validationResults);
		}

        protected void ClearErrors() => RemoveAllErrors();

        private void RemoveErrorsByPropertyName(string propertyName)
		{
			if (_errors.ContainsKey(propertyName))
			{
				_errors.Remove(propertyName);
			}

			RaiseErrorsChanged(propertyName);
		}

		private void RemoveAllErrors()
		{
			_errors.Clear();

			RaiseErrorsChanged(null);
		}

		private void HandleValidationResults(List<ValidationResult> validationResults)
		{
			var resultsByPropertyName = from results in validationResults
										from memberNames in results.MemberNames
										group results by memberNames into groups
										select groups;

			foreach (var property in resultsByPropertyName)
			{
				_errors.Add(property.Key, property.Select(r => r.ErrorMessage).ToList());
				RaiseErrorsChanged(property.Key);
			}
		}

		private void RaiseErrorsChanged(string propertyName)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		public IList<string> ErrorsList
		{
			get
			{
				return GetErrors(string.Empty).Cast<string>().ToList();
			}
		}
	}
}
