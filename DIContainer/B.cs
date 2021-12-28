using System;

namespace DIContainer
{
    public interface IB
    {
        void WriteB();
    }

    public class B : IB
    {
        public B(IA a) { }
        public void WriteB()
        {
            Console.WriteLine("Class B");
        }
    }
}
