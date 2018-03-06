using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MvvmUtils;

namespace Axis.AudioPlayer
{
	public class WizardDetails : ValidatableObject, ISubmittable
	{
		private string username;
		private string password;
		private string ipAddress;
		private string alias;

		private bool pristine = true;

		[Required(ErrorMessage = "Name cannot be empty!")]
		public string Alias
		{
			get => alias;
			set => SetAndValidateProperty(ref alias, value);
		}

		[Required(ErrorMessage = "Username is required")]
		public string Username
		{
			get => username;
			set => SetAndValidateProperty(ref username, value);
		}

		public string Password
		{
			get => password;
			set => SetProperty(ref password, value);
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
