using System.Numerics;

namespace Softviz.MetaNodes
{
    /// <summary>
    /// Base class for all meta nodes
    /// </summary>
    public abstract class MetaNode
    {
        /// <summary>
        /// Position of the meta node
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Force multiplier
        /// </summary>
        public double Strength { get; set; }

    }
}
