namespace Matte.Client.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Nodes;
    using Common.Manifests;

    /// <summary>
    /// Coordinates <see cref="INode"/>s and <see cref="Manifest"/>s.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Asynchronously gets a collection of nodes which have slices for the
        /// given manifest ID.
        /// </summary>
        Task<IReadOnlyCollection<INode>> GetNodesAsync(Guid manifestId);
    }
}