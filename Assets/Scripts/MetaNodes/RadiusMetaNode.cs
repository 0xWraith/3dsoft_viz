namespace Softviz.MetaNodes
{
    /// <summary>
    /// MetaNode which attracts nodes Which are between minimum radius and maximum radius
    /// </summary>
    public class RadiusMetaNode : MetaNode
    {
        /// <summary>
        /// Minimum radius
        /// </summary>
        public double MinRadius { get; set; }

        /// <summary>
        /// Maximum radius
        /// </summary>
        public double MaxRadius { get; set; }
    }
}
