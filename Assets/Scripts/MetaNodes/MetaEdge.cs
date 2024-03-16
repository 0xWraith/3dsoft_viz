using Softviz.Graph;

namespace Softviz.MetaNodes
{
    /// <summary>
    /// Connector edge for EdgeMetaNode
    /// </summary>
    public class MetaEdge
    {
        /// <summary>
        /// Attracting metaNode
        /// </summary>
        public MetaNode metaNode;

        /// <summary>
        /// Attracted node
        /// </summary>
        public Node node;
    }
}
