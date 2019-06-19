namespace Matte.Client.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Slices;

    /// <summary>
    /// Holds <see cref="Slice"/>s.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Asynchronously gets a collection of coefficients from all available
        /// <see cref="Slice"/>s.
        /// </summary>
        Task<IReadOnlyCollection<bool[]>> GetCoefficientsAsync(Guid manifestId);

        /// <summary>
        /// Asynchronously gets the <see cref="Slice"/> having the given
        /// <paramref name="coefficients"/>.
        /// </summary>
        Task<Slice> GetSliceAsync(bool[] coefficients);
    }
}