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
                var type = Uri.CheckHostName(value as string);
                return type == UriHostNameType.Dns || type == UriHostNameType.IPv4 || type == UriHostNameType.IPv6;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
