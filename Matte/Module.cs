using System.Security.Cryptography;
using Autofac;

namespace Matte
{
    using Matte.Encoding;
    using Matte.Entropy;

    static class Module
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<Application>();
            builder
                .Register(c => RandomNumberGenerator.Create().AsRandom(16))
                .As<IRandom>();
            builder.RegisterType<SliceGeneratorFactory>();
            builder
                .Register(c => c.Resolve<SliceGeneratorFactory>().CreateSystematic())
                .As<SliceGenerator>();
        }
    }
}