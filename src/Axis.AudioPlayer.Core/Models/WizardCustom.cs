using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MvvmUtils;
using MvvmUtils.DataAnnotations;

namespace Axis.AudioPlayer
{
	public class WizardCustom : ValidatableObject, ISubmittable
	{
		private string username;
		private string password;
		private string ipAddress;

		private bool pristine = true;

		[Required(ErrorMessage = "Username is required")]
		public string Username
		{
			get => username;
			set => SetAndValidateProperty(ref username, value);

		}

		[Required(ErrorMessage = "IP Address is required")]
		[IPAddress("Invalid IP Address")]
		public string IPAddress
		{
			get => ipAddress;
			set => SetAndValidateProperty(ref ipAddress, value);

		}

		public string Password
		{
			get => password;
			set => SetProperty(ref password, value);
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
