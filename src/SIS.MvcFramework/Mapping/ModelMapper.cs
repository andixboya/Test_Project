using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIS.MvcFramework.Mapping
{
    public static class ModelMapper
    {
        public static TDestination ProjectTo<TDestination>(object origin)
        {
            //origin == source
            //1. create an instance of the destination object. (with null props.)
            var destinationInstance = (TDestination)Activator.CreateInstance(typeof(TDestination));


            //2. get the props from the source
            var propertiesOfOrirgin = origin.GetType().GetProperties();
            //origin.GetType().GetProperties();


            foreach (var originProperty in propertiesOfOrirgin)
            {
                string propertyName = originProperty.Name;

                PropertyInfo destinationProperty = destinationInstance.GetType().GetProperty(propertyName);

                if (destinationProperty != null)
                {
                    if (destinationProperty.PropertyType == typeof(string))
                    {
                        destinationProperty.SetValue(destinationInstance,
                            originProperty.GetValue(origin).ToString());
                    }
                    else
                    {
                        destinationProperty.SetValue(destinationInstance,
                            originProperty.GetValue(origin));
                    }
                }
            }

            return destinationInstance;
        }
    }
}
