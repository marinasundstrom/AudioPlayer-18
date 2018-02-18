using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MvvmUtils.DataAnnotations
{
    public class IPAddressAttribute : ValidationAttribute
    {
        public IPAddressAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            try 
            {
                IPAddress.Parse(value as string);
                return true;
            } 
            catch (Exception) 
            {
                return false;
            }
        }
    }
}
