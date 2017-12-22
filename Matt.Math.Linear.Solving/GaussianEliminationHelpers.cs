namespace Matt.Math.Linear.Solving
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Solves a system of equations
    /// </summary>
    public sealed class GaussianEliminationHelpers
    {
        /// <summary>
        /// Puts the given system of equations into reduced row echelon form as much as possible. Returns the steps
        /// required to solve the system of equations. The steps will end with a <see cref="Operation.Complete"/> step
        /// if the system of equations is solvable.
        /// </summary>
        /// <remarks>
        /// The given <paramref name="coefficients"/> are modified because we actually put them into reduced row echelon
        /// form in order to find the steps necessary to do so.
        /// 
        /// So make sure that any data associated with the given <paramref name="coefficients"/> is mutated accordingly.
        /// </remarks>
        public static IEnumerable<Step> Solve(IList<BitArray> coefficients)
        {
            if (coefficients == null)
                yield break;
            var numRows = coefficients.Count;
            if (numRows == 0)
                yield break;
            var numColumns = coefficients[0].Length;
            var kMax = Math.Min(numRows, numColumns);

            // Put matrix into row echelon form
            for (var k = 0; k < kMax; k++) // O(n^2)
            {
                // Find the pivot point
                var iMax = 0;
                for (var i = k; i < numRows; i++) // O(n)
                {
                    if (!coefficients[i][k])
                        continue;
                    iMax = i;
                    break;
                }
                if (!coefficients[iMax][k])
                    yield break;
                // Swap rows k and i_max
                if (iMax != k)
                {
                    yield return SwapRows(coefficients, iMax, k);
                }
                // XOR pivot with all rows below the pivot
                for (var i = k + 1; i < numRows; i++) // O(n)
                {
                    if (!coefficients[i][k])
                        continue;
                    yield return XorRows(coefficients, k, i); // We can just XOR since we're dealing with Galois Fields
                }
            }

            // Put the matrix into reduced row echelon form using back substitution
            for (var k = kMax - 1; k > 0; k--) // O(n^2)
            {
                if (!coefficients[k][k])
                    yield break;
                // See which other rows need to be XOR'd with this one
                for (var i = k - 1; i >= 0; i--) // O(n)
                {
                    if (!coefficients[i][k])
                        continue;
                    yield return XorRows(coefficients, k, i);
                }
            }

            // Make sure the top part of the coefficients matrix is the identity matrix and that the bottom part is only zeros
            for (var row = 0; row < numRows; row++) // O(n^2)
            {
                for (var column = 0; column < numColumns; column++) // O(n)
                {
                    if ((row == column) ^ coefficients[row][column]) // There should be a coefficient and there's not, or there shouldn't be and there is
                        yield break;
                }
            }

            // Make sure there are at least as many rows as there are columns
            if (numRows < numColumns)
                yield break;
            
            // If we made it this far, then things have been solved
            yield return Complete();
        }
        
        private static Step Complete() =>
            new Step(
                from: 0,
                operation: Operation.Complete,
                to: 0);
        
        /// <summary>
        /// Swaps the coefficients and solutions between the two given rows
        /// </summary>
        private static Step SwapRows(
            IList<BitArray> list,
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
            IList<BitArray> list,
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
