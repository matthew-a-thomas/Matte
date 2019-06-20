using System.Security.Cryptography;
using Autofac;

namespace Matte
{
    using Matte.Encoding;
    using Matte.Entropy;
    using Matte.Entropy.Adapters;

    static class Module
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<Application>();
            builder
                .Register(c => new RandomNumberGeneratorAdapter(RandomNumberGenerator.Create()))
                .As<IRandom>();
            builder.RegisterType<SliceGeneratorFactory>();
            builder
                .Register(c => c.Resolve<SliceGeneratorFactory>().CreateSystematic())
                .As<SliceGenerator>();
        }
    }
}