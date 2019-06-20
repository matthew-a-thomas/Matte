using System;
using System.Linq;

namespace Matte
{
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using Matte.Encoding;
    using Newtonsoft.Json;

    class Application
    {
        readonly SliceGenerator _sliceGenerator;

        public Application(SliceGenerator sliceGenerator)
        {
            _sliceGenerator = sliceGenerator;
        }

        string GetCurrentDirectory() => Path.GetDirectoryName(
            Assembly.GetExecutingAssembly()
                .Location);

        public void Run()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var message = ConsoleHelpers.PromptParagraph("Enter a message. Type DONE to finish: ", "DONE");
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            
            Console.Write($"That produced {messageBytes.Length} bytes. ");
            int sliceSize;
            while (!ConsoleHelpers.TryPrompt(
                       "How large should each slice be? ",
                       out sliceSize) || sliceSize < 1)
            {
                //
            }
            
            int numSlices;
            while (!ConsoleHelpers.TryPrompt(
                       "How many slices would you like to produce? ",
                       out numSlices) || numSlices < 0)
            {
                //
            }
            
            var label = ConsoleHelpers.Prompt("Enter a label: ");
            var directory = Path.Combine(
                GetCurrentDirectory(),
                label);
            Directory.CreateDirectory(directory);

            byte[] hash;
            using (var sha = SHA256.Create())
                hash = sha.ComputeHash(messageBytes);
            File.WriteAllText(
                Path.Combine(
                    directory,
                    "meta.json"),
                JsonConvert.SerializeObject(new
                {
                    Label = label,
                    Sha256 = hash,
                    TotalBytes = messageBytes.Length
                }, Formatting.Indented));
            
            var slices = _sliceGenerator.Generate(messageBytes, sliceSize);
            var numGenerated = 0;
            foreach (var slice in slices)
            {
                ++numGenerated;
                if (numGenerated > numSlices)
                    break;
                
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
                    var b = data[i];
                    var c = (char) b;
                    dataAsAscii[i] = char.IsControl(c) || char.IsSurrogate(c)
                        ? '□'
                        : c;
                }

                var line = $"{coefficients} -> {dataAsHexString} \"{new string(dataAsAscii)}\"";
                Console.WriteLine(line);

                var fileName = Path.Join(
                    directory,
                    $"{numGenerated}.json");
                var fileOutput = JsonConvert.SerializeObject(new
                {
                    Coef = coefficients,
                    Data = data,
                    Hint = new string(dataAsAscii)
                }, Formatting.Indented);
                File.WriteAllText(fileName, fileOutput);
            }
        }
    }
}