

namespace Sis.MvcFramework.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationSisAttribute : Attribute
    {
        
        public ValidationSisAttribute(string message = "Error in the attribute.")
        {
            this.ErrorMessage = message;
        }

        public  string ErrorMessage { get; }

        public abstract bool IsValid(object value);

    }
}
