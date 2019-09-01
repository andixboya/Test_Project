

namespace Sis.MvcFramework.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StringLengthSisAttribute : ValidationSisAttribute
    {

        private readonly int minLength;
        private readonly int maxLength;

        public StringLengthSisAttribute(int minLength, int maxLength, string message = "Length must be above 3 symbols long.")
            : base(message)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;

        }

        public override bool IsValid(object value)
        {
            string objectAsString = (string)Convert.ChangeType(value, typeof(string));

            return objectAsString.Length >= this.minLength && objectAsString.Length <= this.maxLength;
        }
    }
}
