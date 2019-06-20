namespace Matte.Encoding.Fountain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bits;
    using Math.Linear.Solving;

    /// <summary>
    /// Uses <see cref="Slice"/>s to decode the original data that went into making those <see cref="Slice"/>s.
    /// </summary>
    /// <remarks>
    /// This class is thread-safe.
    /// </remarks>
    public sealed class SliceSolver
    {
        /// <summary>
        /// The list of coefficients that is used by Gaussian Elimination to solve things.
        /// </summary>
        readonly IList<long[]> _coefficientsList = new List<long[]>();

        /// <summary>
        /// The number of coefficients per <see cref="Slice"/>.
        /// </summary>
        readonly int _numCoefficients;

        /// <summary>
        /// The number of bytes of data in each <see cref="Slice"/>.
        /// </summary>
        readonly int _sliceSize;

        /// <summary>
        /// The list of equation solutions that is used by Gaussian Elimination to solve things.
        /// </summary>
        readonly IList<long[]> _solutionsList = new List<long[]>();

        /// <summary>
        /// The length of the original data that was encoded into <see cref="Slice"/>s.
        /// </summary>
        readonly int _totalLength;

        /// <summary>
        /// Creates a new <see cref="SliceSolver"/>, which uses <see cref="Slice"/>s to decode the original data that
        /// went into making those <see cref="Slice"/>s.
        /// </summary>
        /// <param name="sliceSize">The number of bytes of data in each slice.</param>
        /// <param name="totalLength">The total length of the original data.</param>
        public SliceSolver(int sliceSize, int totalLength)
        {
            _numCoefficients = (totalLength - 1) / sliceSize + 1;
            _sliceSize = sliceSize;
            _totalLength = totalLength;
        }

        /// <summary>
        /// Asynchronously records the given <see cref="Slice"/> so that it can be used for solving later on.
        /// </summary>
        public void Remember(Slice slice)
        {
            slice = slice.Clone();
            _coefficientsList.Add(slice.PackedCoefficients);
            _solutionsList.Add(slice.PackedData);
        }

        /// <summary>
        /// Swaps the two rows of the given <paramref name="solutions"/>.
        /// </summary>
        static void Swap(
            int from,
            int to,
            IList<long[]> solutions)
        {
            if (from >= solutions.Count || to >= solutions.Count)
                return;
            var temp = solutions[to];
            solutions[to] = solutions[from];
            solutions[from] = temp;
        }

        /// <summary>
        /// Tries to decode the original data from all <see cref="Slice"/>s previously given to
        /// <see cref="Remember"/>.
        /// </summary>
        /// <remarks>
        /// Decoding the original data is akin to solving a set of linear equations. Each <see cref="Slice"/>
        /// represents one more equation. So enough <see cref="Slice"/>s have to be given to <see cref="Remember"/>
        /// in order to be able to solve them.
        /// </remarks>
        public bool TrySolve(out byte[] solution)
        {
            var solved = false;
            foreach (var step in GaussianEliminationHelpers.Solve(
                _coefficientsList,
                _numCoefficients))
            {
                switch (step.Operation)
                {
                    case Operation.Complete:
                        solved = true;
                        break;
                    case Operation.Swap:
                        Swap(
                            step.From,
                            step.To,
                            _solutionsList);
                        break;
                    case Operation.Xor:
                        Xor(
                            step.From,
                            step.To,
                            _solutionsList);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            solution =
                solved
                    ? _solutionsList
                        .SelectMany(x => x.GetBytes(_sliceSize))
                        .Take(_totalLength)
                        .ToArray()
                    : null;
            return solved;
        }

        /// <summary>
        /// XORs the array at <paramref name="from"/> into the array at <paramref name="to"/>.
        /// </summary>
        static void Xor(
            int from,
            int to,
            IList<long[]> solutions)
        {
            if (from >= solutions.Count || to >= solutions.Count)
                return;
            var toPacked = solutions[to];
            var fromPacked = solutions[from];
            toPacked.Xor(fromPacked);
        }
    }
}