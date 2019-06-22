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
            Console.SetIn(new StreamReader(Console.OpenStandardInput(1 >> 14)));
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                switch (ConsoleHelpers.Choose(
                    null,
                    new[]
                    {
                        "Record slices",
                        "Decode slices"
                    }))
                {
                    case 0:
                        RecordSlices();
                        break;
                    case 1:
                        DecodeSlices();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        void DecodeSlices()
        {
            var label = ConsoleHelpers.Prompt("Label? ");
            var directory = Path.Combine(
                GetCurrentDirectory(),
                label);

            var metaFileName = Path.Combine(
                directory,
                "meta.json");
            if (!File.Exists(metaFileName))
            {
                Console.WriteLine("Cannot find meta.json there");
                return;
            }

            var meta = JsonConvert.DeserializeObject<Meta>(File.ReadAllText(metaFileName));
            if (meta.Label != label)
            {
                Console.WriteLine($"Warning: label \"{label}\" in meta.json doesn't match");
            }

            var sha256 = meta.Sha256;
            if (sha256 == null)
            {
                Console.WriteLine("Warning: no SHA256 value specified in meta.json");
            }

            var sliceSize = meta.SliceSize;
            if (sliceSize <= 0)
            {
                Console.WriteLine($"Invalid slice size specified in meta.json ({sliceSize})");
                return;
            }

            var totalBytes = meta.TotalBytes;
            if (totalBytes <= 0)
            {
                Console.WriteLine($"Invalid number of bytes specified ({totalBytes})");
                return;
            }

            var sliceSolver = new SliceSolver(
                sliceSize,
                totalBytes);
            var numCoefficients = SliceHelpers.CalculateNumberOfSlices(
                totalBytes,
                sliceSize);
            foreach (var file in Directory.EnumerateFiles(directory))
            {
                var contents = File.ReadAllText(file);
                var serializedSlice = JsonConvert.DeserializeObject<SerializedSlice>(contents);
                
                if (serializedSlice.Coef == null)
                {
                    Console.WriteLine($"Skipping {file} because no coefficients given");
                    continue;
                }
                var coefficients = serializedSlice
                    .Coef
                    .Select(c => c == '1')
                    .ToList();
                if (numCoefficients != coefficients.Count)
                {
                    Console.WriteLine($"Skipping {file} because it has {coefficients.Count} coefficients instead of the expected {numCoefficients}");
                    continue;
                }

                var data = serializedSlice.Data;
                if (data == null)
                {
                    Console.WriteLine($"Skipping {file} because no data given");
                    continue;
                }
                if (sliceSize != data.Length)
                {
                    Console.WriteLine($"Skipping {file} because it has {data.Length} bytes instead of the expected {sliceSize}");
                    continue;
                }

                var slice = SliceHelpers.CreateSlice(
                    coefficients,
                    data);
                sliceSolver.Remember(slice);
                Console.WriteLine($"Used {file}");

                if (!sliceSolver.TrySolve(out var solution))
                    continue;
                
                var message = System.Text.Encoding.UTF8.GetString(solution);
                Console.WriteLine(message);
                return;
            }
        }

        void RecordSlices()
        {
            var message = ConsoleHelpers.PromptParagraph(
                "Enter a message. Type DONE to finish: ",
                "DONE");
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
                JsonConvert.SerializeObject(
                    new Meta {Label = label, Sha256 = hash, SliceSize = sliceSize, TotalBytes = messageBytes.Length},
                    Formatting.Indented));

            var slices = _sliceGenerator.Generate(
                messageBytes,
                sliceSize);
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
                var data = slice.GetData()
                    .ToArray();
                var dataAsHexString = "0x" + BitConverter
                                          .ToString(data)
                                          .Replace(
                                              "-",
                                              "")
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
                var fileOutput = JsonConvert.SerializeObject(
                    new SerializedSlice {Coef = coefficients, Data = data, Hint = new string(dataAsAscii)},
                    Formatting.Indented);
                File.WriteAllText(
                    fileName,
                    fileOutput);
            }
        }
    }
}