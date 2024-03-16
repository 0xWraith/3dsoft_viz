namespace Softviz.MetaNodes
{
    public enum MetaNodeTypeEnum
    {
        /// <summary>
        /// Only nodes which are connected with meta edge to the meta node are attracted
        /// </summary>
        AttractWithEdges = 0,

        /// <summary>
        /// All nodes within specified radius are attracted with specified strength
        /// </summary>
        AttractInRange = 1,

        /// <summary>
        /// Nodes which comply to specified filter function are attracted with specified strength
        /// </summary>
        AttractWithFunction = 2
    }
}
