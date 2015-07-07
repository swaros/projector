using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projector
{
    class ObjectCloner
    {
        public object CloneObject(object source)
        {
            Type sourceType = source.GetType();

            // First make an instance of the type of the source object.
            object target = CreateInstanceOfType(sourceType);

            // Get a list of all the fields of the type.
            PropertyInfo[] fields = sourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Iterate through all the fields getting the value from the source, and setting them on the target.
            foreach (PropertyInfo fi in fields)
            {
                if (fi.CanWrite)
                {
                    // If the type of the field is a value type, an enumeration, or a string,
                    // we can suffice by just setting the value.
                    if (fi.PropertyType.IsValueType || fi.PropertyType.IsEnum || fi.PropertyType.Equals(typeof(System.String)))
                        fi.SetValue(target, fi.GetValue(source, null), null);

                    // If the type of the field is a complex type
                    // we need to call clone object to clone it as well.
                    else
                    {
                        object value = fi.GetValue(source, null);
                        if (value == null)
                            fi.SetValue(target, null, null);
                        else
                            fi.SetValue(target, CloneObject(value), null);
                    }
                }
            }

            return target;
        }

        public object CreateInstanceOfType(Type type)
        {
            // First make an instance of the type.
            object instance = null;

            // If there is an empty constructor, call that.
            if (type.GetConstructor(Type.EmptyTypes) != null)
                instance = Activator.CreateInstance(type);
            else
            {
                // Otherwise get the first available constructor and fill in some default values.
                // (we are trying to set all properties anyway so it shouldn't be much of a problem).
                ConstructorInfo ci = type.GetConstructors()[0];
                ParameterInfo[] parameters = ci.GetParameters();

                object[] ciParams = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].DefaultValue != null)
                    {
                        ciParams[i] = parameters[i].DefaultValue;
                        continue;
                    }

                    Type paramType = parameters[i].ParameterType;
                    ciParams[i] = CreateInstanceOfType(paramType);
                }

                instance = ci.Invoke(ciParams);
            }

            return instance;
        }
    }
}
