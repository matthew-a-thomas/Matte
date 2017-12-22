namespace Matt.Math.Linear.SolvingTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Solving;

    [TestClass]
    public class GaussianEliminationHelpersClass
    {
        [TestClass]
        public class SolveMethod
        {
            [TestMethod]
            public void CanBeCalledWithoutEquations()
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                GaussianEliminationHelpers.Solve(new List<BitArray>()).ToList();
            }

            [TestMethod]
            public void DoesNotSolveUnsolvableSystem()
            {
                var coefficients = new List<BitArray>
                {
                    new BitArray(new[] { true, false, false })
                };
                var solution = GaussianEliminationHelpers.Solve(coefficients).ToList();

                if (solution.Count > 0)
                    Assert.IsTrue(solution[solution.Count].Operation != Operation.Complete);
            }

            [TestMethod]
            public void ProducesStepsWhichResultInIdenticalChangesAndASolution()
            {
                var coefficients = new List<BitArray>
                {
                    new BitArray(new [] { true, true, true }),
                    new BitArray(new [] { false, true, true }),
                    new BitArray(new [] { false, true, false })
                };
                var copy = coefficients.Select(x => x.Clone() as BitArray).ToList();
                
                var steps = GaussianEliminationHelpers.Solve(coefficients).ToList();
                
                // Assert that it has been solved
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                Assert.IsTrue(coefficients[0][0]); Assert.IsFalse(coefficients[0][1]); Assert.IsFalse(coefficients[0][2]);
                Assert.IsFalse(coefficients[1][0]); Assert.IsTrue(coefficients[1][1]); Assert.IsFalse(coefficients[1][2]);
                Assert.IsFalse(coefficients[2][0]); Assert.IsFalse(coefficients[2][1]); Assert.IsTrue(coefficients[2][2]);
                
                // Perform the steps on the copy
                var mappedOperations = new Dictionary<Operation, Action<int, int, IList<BitArray>>>
                {
                    {
                        Operation.Swap,
                        (from, to, list) =>
                        {
                            var temp = list[to];
                            list[to] = list[from];
                            list[from] = temp;
                        }
                    },
                    {
                        Operation.Xor,
                        (from, to, list) => list[to].Xor(list[from])
                    }
                };
                foreach (var step in steps)
                {
                    if (!mappedOperations.TryGetValue(step.Operation, out var operation))
                        continue;
                    operation.Invoke(step.From, step.To, copy);
                }
                
                // Assert that the copy has been solved
                Assert.IsTrue(copy[0][0]); Assert.IsFalse(copy[0][1]); Assert.IsFalse(copy[0][2]);
                Assert.IsFalse(copy[1][0]); Assert.IsTrue(copy[1][1]); Assert.IsFalse(copy[1][2]);
                Assert.IsFalse(copy[2][0]); Assert.IsFalse(copy[2][1]); Assert.IsTrue(copy[2][2]);
            }

            [TestMethod]
            public void SolvesAlreadySolvedSystem()
            {
                var coefficients = new List<BitArray>
                {
                    new BitArray(new[] { true, false, false }),
                    new BitArray(new[] { false, true, false }),
                    new BitArray(new[] { false, false, true })
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                Assert.IsTrue(coefficients[0][0]); Assert.IsFalse(coefficients[0][1]); Assert.IsFalse(coefficients[0][2]);
                Assert.IsFalse(coefficients[1][0]); Assert.IsTrue(coefficients[1][1]); Assert.IsFalse(coefficients[1][2]);
                Assert.IsFalse(coefficients[2][0]); Assert.IsFalse(coefficients[2][1]); Assert.IsTrue(coefficients[2][2]);
            }

            [TestMethod]
            public void SolvesEasilySolvableSystem()
            {
                var coefficients = new List<BitArray>
                {
                    new BitArray(new[] { false, false, true }),
                    new BitArray(new[] { false, true, false }),
                    new BitArray(new[] { true, false, false })
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                Assert.IsTrue(coefficients[0][0]); Assert.IsFalse(coefficients[0][1]); Assert.IsFalse(coefficients[0][2]);
                Assert.IsFalse(coefficients[1][0]); Assert.IsTrue(coefficients[1][1]); Assert.IsFalse(coefficients[1][2]);
                Assert.IsFalse(coefficients[2][0]); Assert.IsFalse(coefficients[2][1]); Assert.IsTrue(coefficients[2][2]);
            }

            [TestMethod]
            public void SolvesComplicatedSystem()
            {
                var coefficients = new List<BitArray>
                {
                    new BitArray(new[] {true, false, true}),
                    new BitArray(new[] {true, true, true}),
                    new BitArray(new[] {false, false, true})
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                Assert.IsTrue(coefficients[0][0]); Assert.IsFalse(coefficients[0][1]); Assert.IsFalse(coefficients[0][2]);
                Assert.IsFalse(coefficients[1][0]); Assert.IsTrue(coefficients[1][1]); Assert.IsFalse(coefficients[1][2]);
                Assert.IsFalse(coefficients[2][0]); Assert.IsFalse(coefficients[2][1]); Assert.IsTrue(coefficients[2][2]);
            }

            [TestMethod]
            public void SolvesOverlySolvedSystem()
            {
                var coefficients = new List<BitArray>
                {
                    new BitArray(new[] { false, false, true }),
                    new BitArray(new[] { false, true, false }),
                    new BitArray(new[] { true, false, false }),
                    new BitArray(new[] { false, false, true }),
                    new BitArray(new[] { false, true, false }),
                    new BitArray(new[] { true, false, false })
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                Assert.IsTrue(coefficients[0][0]); Assert.IsFalse(coefficients[0][1]); Assert.IsFalse(coefficients[0][2]);
                Assert.IsFalse(coefficients[1][0]); Assert.IsTrue(coefficients[1][1]); Assert.IsFalse(coefficients[1][2]);
                Assert.IsFalse(coefficients[2][0]); Assert.IsFalse(coefficients[2][1]); Assert.IsTrue(coefficients[2][2]);
            }
        }
    }
}
