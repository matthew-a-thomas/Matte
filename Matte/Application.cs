using System;
using System.Linq;
using System.Threading;
using Matte.Encoding.Fountain;

namespace Matte
{
    class Application
    {
        readonly SliceGenerator _sliceGenerator;

        public Application(SliceGenerator sliceGenerator)
        {
            _sliceGenerator = sliceGenerator;
        }

        public void Run()
        {
            Console.Write("Enter a message: ");
            var message = Console.ReadLine();
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            var sliceSize = System.Math.Max(1, messageBytes.Length / 5);
            var slices = _sliceGenerator.Generate(messageBytes, sliceSize);
            foreach (var slice in slices)
            {
                var coefficients = string.Join(
                    "",
                    slice
                        .GetCoefficients()
                        .Select(x => x ? "1" : "0"));
                var data = "0x" + BitConverter
                    .ToString(slice.GetData().ToArray())
                    .Replace("-", "")
                    .ToLower();
                Console.Write(coefficients);
                Console.Write(" -> ");
                Console.WriteLine(data);
                Thread.Sleep(1000);
            }
        }
    }
}