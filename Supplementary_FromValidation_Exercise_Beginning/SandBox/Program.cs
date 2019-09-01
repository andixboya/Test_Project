using System;
using System.Linq;

namespace SandBox
{
    public class Program
    {
        static void Main(string[] args)
        {

            User user = new User()
            {
                UserName = "Gh"
            };


            //IsValid(user);
        }


        //public static void IsValid(object value)
        //{
        //    var objectProperties = value.GetType().GetProperties();
        //    //1st get the props
        //    string newName = "gosho";

        //    foreach (var objectProperty in objectProperties)
        //    {
        //        //for test purposes;
        //        //objectProperty.SetValue(value, newName);



        //        //2.1. in each prop, we get only the validation attributes
        //        //2.2. we cast each OBJECT ATTRIBUTE to the attribute we NEED (vlidationattribute, because we`ll use the is valid method!)
        //        var validationAttributes = objectProperty
        //            .GetCustomAttributes(typeof(ValidationSisAttribute),true)
        //            .Cast<ValidationSisAttribute>()
        //            .ToList();

        //        //от тук надолу, вече почваш да губиш връзката!
        //        //3. вземаш prop-a и му казваш да си покаже стойността с obj  (забравяш, че винаги трябва да имаш инстанция, за да видиш,
        //        //1 Prop, каква му е стойността. (то няма логика другояче, ама както и да е.
        //        foreach (var validationAttribute in validationAttributes)
        //        {
                    
        //            //
        //            if (validationAttribute.IsValid(objectProperty.GetValue(value)))
        //            {
        //                Console.WriteLine("true");
        //            }
        //            else
        //            {
        //                Console.WriteLine("false");
        //            }

        //        }



        //    }
            
        //}
    }
}
