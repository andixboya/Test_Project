

using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.Attributes.Security
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    public class AuthorizeAttribute : Attribute
    {
        private readonly string authority;

        public AuthorizeAttribute(string authority = "authorized")
        {
            this.authority = authority;

        }

        private bool IsLoggedIn(Principal principal)
        {
            //principal is given when you are logged in
            return principal != null;
        }

        //the target here is the entity itself, given from reflection!?
        //не можеш this, защото трябва да подадеш тогава и обекта на AuthorizeAttribute?
        public bool IsInAuthority(Principal principal)
        {
            if (!this.IsLoggedIn(principal))
            {
                //if he is not logged in , he must be anonymous
                // but by default he is authorized, but if he has no principal (not signed in, he  must be annonymous)
                // which is why they  return true as == anonymous 
                //if auth == by default to author, to be auth. , he needs to be signed in
                //!!if he is not he will have authorize authority, but needs == to anonymous, which wil give him status OutOfAuthority (unauthorized)
                return this.authority == "anonymous";
            }

            // here if he is signed in, he will receive a change "==authorize" and the power of the default authorized will persist.
            //if it is not in the default, it will check if he has in additional roles authorized.
            return this.authority == "authorized"
                   || principal.Roles.Contains(this.authority.ToLower());
        }

    }
}
