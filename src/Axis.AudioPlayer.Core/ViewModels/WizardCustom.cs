using System.ComponentModel.DataAnnotations;
using MvvmUtils;
using MvvmUtils.DataAnnotations;

namespace Axis.AudioPlayer.ViewModels
{
    public class WizardCustom : ValidatableObject
    {
        private string username;
        private string password;
        private string ipAddress;

        [Required(ErrorMessage = "Username is required")]
        public string Username
        {
            get => username;
            set => SetAndValidateProperty(ref username, value);

        }

        [Required(ErrorMessage = "Required")]
        [HostName("Invalid IP address or hostname")]
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

        public bool Validate()
        {
            this.ValidateAll();
            return !HasErrors;
        }
    }
}
