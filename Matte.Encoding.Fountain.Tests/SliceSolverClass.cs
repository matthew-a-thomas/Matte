namespace Matte.Encoding.Fountain.Tests
{
    using System.Linq;
    using Xunit;

    public class SliceSolverClass
    {
        [Fact]
        public void CanSolveWithACombinedSlice()
        {
            const byte
                value1 = 0xFF,
                value2 = 0x55;
            Slice
                slice1 = SliceHelpers.CreateSlice(
                    coefficients: new [] { true, false },
                    data: new [] { value1 }),
                slice2 = SliceHelpers.CreateSlice(
                    coefficients: new [] { true, true },
                    data: new [] { (byte)(value2 ^ value1) });
            var solver = new SliceSolver(1, 2);
            solver.Remember(slice1);
            solver.Remember(slice2);
            var solved = solver.TrySolve(out var solution);
            Assert.True(solved);
            Assert.NotNull(solution);
            Assert.Equal(2, solution.Length);
            Assert.Equal(solution[0], value1);
            Assert.Equal(solution[1], value2);
        }
        
        [Fact]
        public void DoesNotSolvePrematurely()
        {
            var solver = new SliceSolver(1, 1);
            var solved = solver.TrySolve(out var solution);
            Assert.False(solved);
            Assert.Null(solution);
        }
        
        [Fact]
        public void PutsOneByteBackTogether()
        {
            const byte value = 0xF5;
            var slice = SliceHelpers.CreateSlice(
                coefficients: new [] { true },
                data: new [] { value }
            );
            var solver = new SliceSolver(1, 1);
            solver.Remember(slice);
            var solved = solver.TrySolve(out var solution);
            Assert.True(solved);
            Assert.NotNull(solution);
            Assert.Single(solution);
            Assert.Equal(solution[0], value);
        }
        
        [Fact]
        public void PutsTwoSequentialSlicesBackTogether()
        {
            const byte
                value1 = 0xFF,
                value2 = 0x55;
            Slice
                slice1 = SliceHelpers.CreateSlice(
                    coefficients: new [] { true, false },
                    data: new [] { value1 }),
                slice2 = SliceHelpers.CreateSlice(
                    coefficients: new [] { false, true },
                    data: new [] { value2 });
            var solver = new SliceSolver(1, 2);
            solver.Remember(slice1);
            solver.Remember(slice2);
            var solved = solver.TrySolve(out var solution);
            Assert.True(solved);
            Assert.NotNull(solution);
            Assert.Equal(2, solution.Length);
            Assert.Equal(solution[0], value1);
            Assert.Equal(solution[1], value2);
        }
        
        [Fact]
        public void WorksEvenWhenSliceSizeAndNumberOfSlicesDoNotEvenlyDivideData()
        {
            var originalData = new []
            {
                (byte)'C',
                (byte)'a',
                (byte)'t'
            };
            Slice
                slice1 = SliceHelpers.CreateSlice(
                    coefficients: new [] { true, false },
                    data: new [] { originalData[0], originalData[1] }
                ),
                slice2 = SliceHelpers.CreateSlice(
                    coefficients: new [] { true, true },
                    data: new [] { (byte)(originalData[0] ^ originalData[2]), originalData[1] }
                );
            var solver = new SliceSolver(2, 3);
            solver.Remember(slice2);
            solver.Remember(slice1);
            var solved = solver.TrySolve(out var solution);
            Assert.True(solved);
            Assert.NotNull(solution);
            Assert.Equal(3, solution.Length);
            Assert.True(solution.SequenceEqual(originalData));
        }
    }
}