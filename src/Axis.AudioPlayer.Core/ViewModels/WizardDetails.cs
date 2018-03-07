using System.ComponentModel.DataAnnotations;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
    public class WizardDetails : ValidatableObject
    {
        private string username;
        private string password;
        private string alias;

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

        public bool Validate()
        {
            this.ValidateAll();
            return !HasErrors;
        }
    }
}
