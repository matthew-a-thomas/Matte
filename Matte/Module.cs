using System.Security.Cryptography;
using Autofac;
using Matte.Encoding.Fountain;
using Matte.Random;
using Matte.Random.Adapters;

namespace Matte
{
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