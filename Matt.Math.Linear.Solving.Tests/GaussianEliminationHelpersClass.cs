namespace Matt.Math.Linear.Solving.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bits;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Solving;

    [TestClass]
    public class GaussianEliminationHelpersClass
    {
        [TestClass]
        public class SolveMethod
        {
            private static void AssertIsSolved(
                IReadOnlyList<Packed> list,
                int width)
            {
                for (var i = 0; i < width; ++i)
                {
                    for (var j = 0; j < width; ++j)
                    {
                        Assert.AreEqual(i == j, list[i].GetBit(j));
                    }
                }
            }

            [TestMethod]
            public void CanBeCalledWithoutEquations()
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                GaussianEliminationHelpers.Solve(new List<Packed>(), 1).ToList();
            }

            [TestMethod]
            public void DoesNotSolveUnsolvableSystem()
            {
                var coefficients = new List<Packed>
                {
                    Packed.Create(new[] { true, false, false })
                };
                var solution = GaussianEliminationHelpers.Solve(coefficients, 3).ToList();

                if (solution.Count > 0)
                    Assert.IsTrue(solution[solution.Count].Operation != Operation.Complete);
            }

            [TestMethod]
            public void ProducesStepsWhichResultInIdenticalChangesAndASolution()
            {
                var coefficients = new List<Packed>
                {
                    Packed.Create(new [] { true, true, true }),
                    Packed.Create(new [] { false, true, true }),
                    Packed.Create(new [] { false, true, false })
                };
                var copy = coefficients.Select(x => x.Clone()).ToList();
                
                var steps = GaussianEliminationHelpers.Solve(coefficients, 3).ToList();
                
                // Assert that it has been solved
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                AssertIsSolved(coefficients, 3);
                
                // Perform the steps on the copy
                var mappedOperations = new Dictionary<Operation, Action<int, int, IList<Packed>>>
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
                AssertIsSolved(copy, 3);
            }

            [TestMethod]
            public void SolvesAlreadySolvedSystem()
            {
                var coefficients = new List<Packed>
                {
                    Packed.Create(new[] { true, false, false }),
                    Packed.Create(new[] { false, true, false }),
                    Packed.Create(new[] { false, false, true })
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients, 3).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                AssertIsSolved(coefficients, 3);
            }

            [TestMethod]
            public void SolvesEasilySolvableSystem()
            {
                var coefficients = new List<Packed>
                {
                    Packed.Create(new[] { false, false, true }),
                    Packed.Create(new[] { false, true, false }),
                    Packed.Create(new[] { true, false, false })
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients, 3).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                AssertIsSolved(coefficients, 3);
            }

            [TestMethod]
            public void SolvesComplicatedSystem()
            {
                var coefficients = new List<Packed>
                {
                    Packed.Create(new[] {true, false, true}),
                    Packed.Create(new[] {true, true, true}),
                    Packed.Create(new[] {false, false, true})
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients, 3).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                AssertIsSolved(coefficients, 3);
            }

            [TestMethod]
            public void SolvesOverlySolvedSystem()
            {
                var coefficients = new List<Packed>
                {
                    Packed.Create(new[] { false, false, true }),
                    Packed.Create(new[] { false, true, false }),
                    Packed.Create(new[] { true, false, false }),
                    Packed.Create(new[] { false, false, true }),
                    Packed.Create(new[] { false, true, false }),
                    Packed.Create(new[] { true, false, false })
                };
                var steps = GaussianEliminationHelpers.Solve(coefficients, 3).ToList();
                Assert.IsTrue(steps.Count > 0);
                Assert.AreEqual(Operation.Complete, steps[steps.Count - 1].Operation);
                AssertIsSolved(coefficients, 3);
            }
        }
    }
}
