namespace Matt.Math.Linear.Solving
{
    /// <summary>
    /// Kinds of row operations
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// Things should now be far enough along that you get a solution.
        /// </summary>
        Complete,
        
        /// <summary>
        /// Swap the two rows
        /// </summary>
        Swap,
            
        /// <summary>
        /// Perform XOR from the first row into the second
        /// </summary>
        Xor
    }
}