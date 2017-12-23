﻿// ReSharper disable UnusedMember.Global
namespace Matt.Interfaces
{
    /// <summary>
    /// Something which can create an exact copy of itself.
    /// </summary>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates an exact copy.
        /// </summary>
        T Clone();
    }
}