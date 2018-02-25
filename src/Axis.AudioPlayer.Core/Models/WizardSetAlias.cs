using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MvvmUtils;

namespace Axis.AudioPlayer
{
	public class WizardSetAlias : ValidatableObject, ISubmittable
	{
		private string alias;

		private bool pristine = true;

		[Required(ErrorMessage = "Name cannot be empty!")]
		public string Alias
		{
			get => alias;
			set => SetAndValidateProperty(ref alias, value);
		}

		protected override void ValidateProperty<T>(T value, [CallerMemberName] string propertyName = null)
		{
			base.ValidateProperty(value, propertyName);

			OnPropertyChanged(nameof(IsSubmitEnabled));
		}

		public bool IsSubmitEnabled
		{
			get
			{
				if (pristine) return false;
				return !HasErrors;
			}
		}
	}
}
