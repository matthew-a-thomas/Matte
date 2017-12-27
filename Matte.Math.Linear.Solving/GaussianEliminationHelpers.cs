namespace Matte.Math.Linear.Solving
{
    using System;
    using System.Collections.Generic;
    using Bits;

    /// <summary>
    /// Solves a system of equations
    /// </summary>
    public static class GaussianEliminationHelpers
    {
        /// <summary>
        /// Puts the given system of equations into reduced row echelon form as
        /// much as possible. Returns the steps required to solve the system of
        /// equations. The steps will end with a
        /// <see cref="Operation.Complete"/> step if the system of equations is
        /// solvable.
        /// </summary>
        /// <remarks>
        /// The given <paramref name="packedCoefficients"/> are modified because
        /// we actually put them into reduced row echelon form in order to find
        /// the steps necessary to do so.
        /// 
        /// So make sure that any data associated with the given
        /// <paramref name="packedCoefficients"/> is mutated accordingly.
        /// </remarks>
        public static IEnumerable<Step> Solve(IList<long[]> packedCoefficients, int numCoefficients)
        {
            if (packedCoefficients == null || numCoefficients <= 0)
                yield break;
            var numRows = packedCoefficients.Count;
            if (numRows == 0)
                yield break;
            var kMax = Math.Min(numRows, numCoefficients);

            // Put matrix into row echelon form
            for (var k = 0; k < kMax; k++) // O(n^2)
            {
                // Find the pivot point
                var iMax = 0;
                for (var i = k; i < numRows; i++) // O(n)
                {
                    if (!packedCoefficients[i].GetBit(numCoefficients, k))
                        continue;
                    iMax = i;
                    break;
                }
                if (!packedCoefficients[iMax].GetBit(numCoefficients, k))
                    yield break;
                // Swap rows k and i_max
                if (iMax != k)
                {
                    yield return SwapRows(packedCoefficients, iMax, k);
                }
                // XOR pivot with all rows below the pivot
                for (var i = k + 1; i < numRows; i++) // O(n)
                {
                    if (!packedCoefficients[i].GetBit(numCoefficients, k))
                        continue;
                    yield return XorRows(packedCoefficients, k, i); // We can just XOR since we're dealing with Galois Fields
                }
            }

            // Put the matrix into reduced row echelon form using back substitution
            for (var k = kMax - 1; k > 0; k--) // O(n^2)
            {
                if (!packedCoefficients[k].GetBit(numCoefficients, k))
                    yield break;
                // See which other rows need to be XOR'd with this one
                for (var i = k - 1; i >= 0; i--) // O(n)
                {
                    if (!packedCoefficients[i].GetBit(numCoefficients, k))
                        continue;
                    yield return XorRows(packedCoefficients, k, i);
                }
            }

            // Make sure the top part of the coefficients matrix is the identity matrix and that the bottom part is only zeros
            for (var row = 0; row < numRows; row++) // O(n^2)
            {
                for (var column = 0; column < numCoefficients; column++) // O(n)
                {
                    if ((row == column) ^ packedCoefficients[row].GetBit(numCoefficients, column)) // There should be a coefficient and there's not, or there shouldn't be and there is
                        yield break;
                }
            }

            // Make sure there are at least as many rows as there are columns
            if (numRows < numCoefficients)
                yield break;
            
            // If we made it this far, then things have been solved
            yield return Complete();
        }
        
        private static Step Complete() =>
            new Step(
                @from: 0,
                operation: Operation.Complete,
                to: 0);
        
        /// <summary>
        /// Swaps the coefficients and solutions between the two given rows
        /// </summary>
        private static Step SwapRows(
            IList<long[]> list,
            int fromRow,
            int toRow)
        {
            var temp = list[fromRow];
            list[fromRow] = list[toRow];
            list[toRow] = temp;
            return new Step(
                @from: fromRow,
                operation: Operation.Swap,
                to: toRow);
        }

        /// <summary>
        /// XOR's the row at <paramref name="fromRow"/> into the row at <paramref name="toRow"/>, for both the coefficients and the solutions
        /// </summary>
        private static Step XorRows(
            IList<long[]> list,
            int fromRow,
            int toRow)
        {
            list[toRow].Xor(list[fromRow]);
            return new Step(
                @from: fromRow,
                operation: Operation.Xor,
                to: toRow);
        }
    }
}
