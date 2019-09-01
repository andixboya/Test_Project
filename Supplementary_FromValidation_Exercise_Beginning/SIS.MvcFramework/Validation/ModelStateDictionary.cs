

namespace SIS.MvcFramework.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    public class ModelStateDictionary
    {
        private readonly Dictionary<string, List<string>> erroMessages;

        public ModelStateDictionary()
        {
            //starts with is Valid, because it will only get false, there probably won`t be a case in which it goes true
            //unless it is re-instantiated.
            this.IsValid = true;
            this.erroMessages = new Dictionary<string, List<string>>();
        }

        public bool IsValid { get; set; }

        public void AddMessage(string propertyName, string message)
        {
            if (!this.erroMessages.ContainsKey(propertyName))
            {
                this.erroMessages.Add(propertyName, new List<string>());
            }
            this.erroMessages[propertyName].Add(message);
        }


        public IReadOnlyDictionary<string, List<string>> ErrorMessages => this.erroMessages.ToImmutableDictionary();


    }
}
