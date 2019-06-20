namespace Matte.Tests.Encoding
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Matte.Encoding;
    using Matte.Entropy.Adapters;
    using Xunit;

    public class SliceGeneratorClass
    {
        public class WhenSystematic
        {
            [SuppressMessage("ReSharper",
                "RedundantArgumentDefaultValue")]
            public class GenerateMethodShould
            {
                [Fact]
                public void ProduceSystematicSlices()
                {
                    var data = new byte[]
                    {
                        0x00,
                        0x01,
                        0x02,
                        0x03
                    };
                    var generator = new SliceGenerator(
                        isSystematic: true,
                        random: new NotRandom(0));
                    var sequence = generator.Generate(data, 1);
                    foreach (var tuple in sequence.Select((x, i) => (Slice: x, Index: i)).Take(data.Length))
                    {
                        var index = tuple.Index;
                        var slice = tuple.Slice;

                        // Verify coefficients
                        var expectedCoefficients =
                            Enumerable.Repeat(false, index)
                                .Append(true)
                                .Concat(Enumerable.Repeat(false, data.Length - index - 1))
                                .ToList();
                        Assert.True(slice.GetCoefficients().SequenceEqual(expectedCoefficients));

                        // Verify data
                        var sliceData = slice.GetData().ToList();
                        Assert.Single(sliceData);
                        Assert.Equal(sliceData[0], data[index]);
                    }
                }

                // TODO: This test feels like it's testing too much and making too many assumptions.
                [Fact]
                public void ProduceSolvableSequenceAfterSystematicSection()
                {
                    var data = new byte[]
                    {
                        0x00,
                        0x01,
                        0x02,
                        0x03,
                        0x04,
                        0x05,
                        0x06,
                        0x07
                    };
                    var generator = new SliceGenerator(
                        isSystematic: true,
                        random: new RandomAdapter(new Random(0)));
                    var mixedSection = generator
                        .Generate(data, 2)
                        .Skip(data.Length)
                        .Take(10)
                        .ToList();
                    var solver = new SliceSolver(2, data.Length);
                    foreach (var slice in mixedSection)
                        solver.Remember(slice);
                    var solved = solver.TrySolve(out var solution);
                    Assert.True(solved);
                    Assert.NotNull(solution);
                    Assert.True(solution.SequenceEqual(data));
                }
            }
        }
    }
}