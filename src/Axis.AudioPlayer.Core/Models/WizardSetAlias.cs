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

		public string Alias
		{
			get => alias;
			set => SetAndValidateProperty(ref alias, value);
		}

        public override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            pristine = false;
            base.OnPropertyChanged(propertyName);
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
