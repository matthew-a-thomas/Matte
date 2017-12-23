namespace Matte.Encoding.Fountain.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SliceSolverClass
    {
        [TestMethod]
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
            Assert.IsTrue(solved, "Didn't find a solution");
            Assert.IsNotNull(solution, "Didn't find a solution");
            Assert.AreEqual(solution.Length, 2, $"Found a solution of {solution.Length} bytes, instead of 2 bytes");
            Assert.AreEqual(solution[0], value1, $"Found the wrong first byte: {solution[0]} instead of {value1}");
            Assert.AreEqual(solution[1], value2, $"Found the wrong second byte: {solution[1]} instead of {value2}");
        }
        
        [TestMethod]
        public void DoesNotSolvePrematurely()
        {
            var solver = new SliceSolver(1, 1);
            var solved = solver.TrySolve(out var solution);
            Assert.IsFalse(solved, "It created a solution from nothing");
            Assert.IsNull(solution, "It created a solution from nothing");
        }
        
        [TestMethod]
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
            Assert.IsTrue(solved, "Didn't find a solution");
            Assert.IsNotNull(solution, "Didn't find a solution");
            Assert.AreEqual(solution.Length, 1, $"Found a solution of {solution.Length} bytes, instead of 1 byte");
            Assert.AreEqual(solution[0], value, $"Found the wrong solution: {solution[0]} instead of {value}");
        }
        
        [TestMethod]
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
            Assert.IsTrue(solved, "Didn't find a solution");
            Assert.IsNotNull(solution, "Didn't find a solution");
            Assert.AreEqual(solution.Length, 2, $"Found a solution of {solution.Length} bytes, instead of 2 bytes");
            Assert.AreEqual(solution[0], value1, $"Found the wrong first byte: {solution[0]} instead of {value1}");
            Assert.AreEqual(solution[1], value2, $"Found the wrong second byte: {solution[1]} instead of {value2}");
        }
        
        [TestMethod]
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
            Assert.IsTrue(solved, "Didn't find a solution");
            Assert.IsNotNull(solution, "Didn't find a solution");
            Assert.AreEqual(solution.Length, 3, $"Found a solution of {solution.Length} bytes, instead of 3 bytes");
            Assert.IsTrue(solution.SequenceEqual(originalData), "Found different solution than original data");
        }
    }
}