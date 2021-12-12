global using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IoC
{
    public class Container
    {
        private Dictionary<Type, Type> registration;

        public Container()
        {
            registration = new Dictionary<Type, Type>();
        }
        public void AddType(Type value)
        {
            var interfaceValue = GetInterfaceExport(value);

            AddType(interfaceValue ?? value, value);
        }

        public void AddType<T>()
        {
            AddType(typeof(T));
        }

        public void AddType(Type interfaceValue, Type classValue)
        {
            if (registration.ContainsKey(interfaceValue))
            {
                throw new IoCException("Type already registered");
            }

            registration.Add(interfaceValue, classValue);
        }

        public void AddType<I, C>()
            where C : I
        {
            AddType(typeof(I), typeof(C));
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object CreateInstance(Type value)
        {
            if (!registration.ContainsKey(value))
            {
                throw new IoCException($"{value.Name} Type not registered");
            }

            var type = registration[value];

            if (IsImportConstructor(type))
            {
                var constructors = type.GetConstructors();

                if (constructors.Count() != 1)
                {
                    throw new IoCException("Type contains more than one constructor");
                }

                List<object> parametrs = new List<object>();

                foreach (var item in constructors.First().GetParameters())
                {
                    parametrs.Add(this.CreateInstance(item.ParameterType));
                }

                if (parametrs.Count == 0)
                {
                    return Activator.CreateInstance(type);
                }

                return Activator.CreateInstance(type, parametrs.ToArray());
            }

            var properties = type.GetProperties();
            var result = Activator.CreateInstance(type);

            foreach (var property in properties)
            {
                if (IsAttributeImport(property))
                {
                    property.SetValue(result, this.CreateInstance(property.PropertyType));
                }
            }

            return result;
        }

        public void AddAssembly(Assembly value)
        {
            foreach (var item in value.DefinedTypes)
            {
                AddType(item);
            }

            foreach (var item in value.GetReferencedAssemblies())
            {
                var assembly = Assembly.Load(item);

                foreach (var type in assembly.DefinedTypes)
                {
                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        if (!registration.ContainsKey(interfaceType))
                        {
                            AddType(interfaceType, type);
                        }
                    }
                }
            }
        }

        private bool IsAttributeImport(PropertyInfo property)
        {
            return property.GetCustomAttributesData().Any(attribute => attribute.AttributeType == (typeof(Import)));
        }

        private bool IsImportConstructor(Type type)
        {
            return type.GetCustomAttributesData()
                .Any(attribute => attribute.AttributeType == (typeof(ImportConstructor)));
        }

        private Type GetInterfaceExport(Type value)
        {
            var interfaceValue = value.GetCustomAttribute<Export>();

            return interfaceValue?.Type;
        }
    }
}