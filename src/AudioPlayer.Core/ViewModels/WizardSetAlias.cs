using System.ComponentModel.DataAnnotations;
using MvvmUtils;

namespace AudioPlayer.ViewModels
{
    public class WizardSetAlias : ValidatableObject
    {
        private string alias;

        [Required]
        public string Alias
        {
            get => alias;
            set => SetAndValidateProperty(ref alias, value);
        }

        public bool Validate()
        {
            this.ValidateAll();
            return !HasErrors;
        }
    }
}
