using System;

namespace DIContainer
{
    public interface IA
    {
        void WriteA();
    }

    public class A : IA
    {
        public A(IB b) { }
        public void WriteA()
        {
            Console.WriteLine("Class A");
        }
    }
}
