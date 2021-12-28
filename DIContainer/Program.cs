namespace DIContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new DIContainer();
            container.AddTransient<IA, A>();
            container.AddTransient<IB, B>();
            container.Display();
            container.Get<IB>();
        }
    }
}