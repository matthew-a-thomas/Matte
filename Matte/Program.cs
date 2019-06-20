using Autofac;

namespace Matte
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main()
        {
            var containerBuilder = new ContainerBuilder();
            Module.Register(containerBuilder);
            using (var container = containerBuilder.Build())
            {
                var application = container.Resolve<Application>();
                application.Run();
            }
        }
    }
}