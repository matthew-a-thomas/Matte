namespace Matte.Math.Linear.Solving
{
    /// <summary>
    /// An operation between two rows.
    /// </summary>
    public struct Step
    {
        /// <summary>
        /// The row from which the <see cref="Operation"/> should be performed.
        /// </summary>
        public readonly int From;

        /// <summary>
        /// The operation to perform between the rows.
        /// </summary>
        public readonly Operation Operation;

        /// <summary>
        /// The row into which the <see cref="Operation"/> should be performed.
        /// </summary>
        public readonly int To;

        /// <summary>
        /// Creates a new <see cref="Step"/>, which is an operation between two rows.
        /// </summary>
        public Step(
            int @from,
            Operation operation,
            int to)
        {
            From = @from;
            Operation = operation;
            To = to;
        }
    }
}