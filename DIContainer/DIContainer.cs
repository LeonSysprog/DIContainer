using System;
using System.Collections.Generic;
using System.Linq;

namespace DIContainer
{
    public class Record
    {
        public Type ServiceType { get; }

        public Type ImplementationType { get; }

        public object Implementation { get; set; }

        public Record(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }
    }

    public class DIContainer
    {
        private Dictionary<Record, string> regs;

        public DIContainer()
        {
            regs = new Dictionary<Record, string>();
        }

        public void AddTransient<TService, TImplementation>()
        {
            regs.Add(new Record(typeof(TService), typeof(TImplementation)), "Transient");
        }

        public void AddSingleton<TService, TImplementation>()
        {
            regs.Add(new Record(typeof(TService), typeof(TImplementation)), "Singleton");
        }

        public T Get<T>() => (T)Get(typeof(T));
        public object Get(Type serviceType)
        {
            var descriptor = regs.SingleOrDefault(x => x.Key.ServiceType == serviceType);
            if (descriptor.Key == null)
            {
                throw new Exception("Service not found");
            }

            if (descriptor.Key.Implementation != null)
            {
                return descriptor.Key.Implementation;
            }

            var actualType = descriptor.Key.ImplementationType;
            var constructor = actualType.GetConstructors().First();
            if (constructor.GetParameters().Any(x => IsCircle(serviceType, x.ParameterType)))
            {
                throw new Exception("Circle");
            }
            var parameters = constructor.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
            var implementation = Activator.CreateInstance(actualType, parameters);
            if (descriptor.Value == "Singleton")
            {
                descriptor.Key.Implementation = implementation;
            }

            return implementation;
        }

        public bool IsCircle(Type serviceType, Type parametrType)
        {
            var descriptor = regs.SingleOrDefault(x => x.Key.ServiceType == parametrType);
            var actualType = descriptor.Key.ImplementationType;
            var constructorType = actualType.GetConstructors().First();
            return constructorType.GetParameters().Any(x => Equals(serviceType, x.ParameterType));
        }

        public void Display()
        {
            foreach (var reg in regs)
                Console.WriteLine("{0}, {1}, {2}", reg.Key.ServiceType, reg.Key.ImplementationType, reg.Value);
        }
    }
}
