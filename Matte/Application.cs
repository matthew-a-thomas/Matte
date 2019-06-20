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
            var sliceSize = (int)System.Math.Max(1, System.Math.Ceiling(messageBytes.Length / 10.0));
            var slices = _sliceGenerator.Generate(messageBytes, sliceSize);
            foreach (var slice in slices)
            {
                var coefficients = string.Join(
                    "",
                    slice
                        .GetCoefficients()
                        .Select(x => x ? "1" : "0"));
                var data = slice.GetData().ToArray();
                var dataAsHexString = "0x" + BitConverter
                    .ToString(data)
                    .Replace("-", "")
                    .ToLower();
                var dataAsAscii = new char[data.Length];
                for (var i = 0; i < data.Length; ++i)
                {
                    dataAsAscii[i] = (char) data[i];
                    if (char.IsControl(dataAsAscii[i]))
                        dataAsAscii[i] = '?';
                }
                Console.Write(coefficients);
                Console.Write(" -> ");
                Console.Write(dataAsHexString);
                Console.Write(" \"");
                Console.Write(dataAsAscii);
                Console.Write("\"");
                Console.WriteLine();
                Thread.Sleep(1000);
            }
        }
    }
}