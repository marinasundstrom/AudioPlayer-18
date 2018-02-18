using System;
using System.ComponentModel.DataAnnotations;

namespace MvvmUtils.DataAnnotations
{
    public class HostNameAttribute : ValidationAttribute
    {
        public HostNameAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            try
            {
                return Uri.CheckHostName(value as string) != UriHostNameType.Unknown;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
