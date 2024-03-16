using UnityEngine;
using System.Collections.Generic;
using Softviz.Graph;

namespace Communication
{
    public static class API_out
    {
        #region ApiCalls

        #region Misc
        public static async void LoadGraph(string iAlgorithm)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"algorithm", iAlgorithm},
                {"path", "tests/data/say"},
                {"graphExtractor", "ModuleGraph"}
            };
            await NetHandler.Instance.Call("loadGraph", param);
        }

        public static async void ChangeLayoutAlgorithm(GraphConfiguration iConfig, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"newAlgorithm", iConfig.actualLayoutAlgorithm.ToString()},
                {"buildingLayouter", iConfig.actualBuildingLayoutAlgorithm.ToString()},
                {"functionMode", iConfig.actualFunctionType.ToString()},
                {"variableMode", iConfig.actualVariableType.ToString()},
                {"evolutionMetricsMode", iConfig.actualEvolutionMetricsMode.ToString()},
                {"evolutionFunctionMode", 4},
                {"directoryMode", iConfig.actualDirectoryMode.ToString()},
            };
            await NetHandler.Instance.Call("changeConfiguration", param);

        }

        public static void TerminateConnection(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            { 
                {"graphId", iGraphId}
            };
            _ = NetHandler.Instance.Call("terminateConnection", param);
            NetHandler.Instance.Terminate();
        }
        #endregion Misc

        #region NodeColumns
        public static async void GetNodeColorColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeColorColumn", param);
        }
        public static async void GetNodeFilteredColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeFilteredColumn", param);
        }
        public static async void GetNodePositionColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodePositionColumn", param);
        }
        public static async void GetNodeSizeColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeSizeColumn", param);
        }
        public static async void GetNodeShapeColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeShapeColumn", param);
        }
        public static async void GetNodeNameColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeNameColumn", param);
        }
        public static async void GetNodeVisibilityColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeVisibilityColumn", param);
        }

        public static async void GetNodeIsFixedColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeIsFixedColumn", param);
        }
        public static async void GetNodeIdColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeIdColumn", param);
        }
        public static async void GetNodeLabelColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeLabelColumn", param);
        }
        public static async void GetNodeMetricsColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeMetricsColumn", param);
        }
        public static async void GetNodeInfoflowMetricsColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeInfoflowMetricsColumn", param);
        }
        public static async void GetNodeTypeColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeTypeColumn", param);
        }
        public static async void GetNodeBodyColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getNodeBodyColumn", param);
        }
        #endregion NodeColumns

        #region EdgeColums
        public static async void GetEdgeColorColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeColorColumn", param);
        }
        public static async void GetEdgeDestinationIdColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeDestinationIdColumn", param);
        }
        public static async void GetEdgeFilteredColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeFilteredColumn", param);
        }
        public static async void GetEdgeIsOrthogonalColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeIsOrthogonalColumn", param);
        }
        public static async void GetEdgeIsParentColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeIsParentColumn", param);
        }
        public static async void GetEdgeShowLabelColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeShowLabelColumn", param);
        }
        public static async void GetEdgeSourceIdColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeSourceIdColumn", param);
        }
        public static async void GetEdgeVisibleColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeVisibleColumn", param);
        }
        public static async void GetEdgeWidthColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeWidthColumn", param);
        }
        public static async void GetEdgeLabelColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeLabelColumn", param);
        }
        public static async void GetEdgeOrientationColumn(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId}
            };
            await NetHandler.Instance.Call("getEdgeOrientationColumn", param);
        }
        #endregion EdgeColumns

        #region Diagrams
        public static async void GetSequenceDiagram(int iNodeId, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"nodeId", iNodeId} 
            };
            await NetHandler.Instance.Call("getSequenceDiagram", param);
        }
        public static async void GetClassDiagram(int iNodeId, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"nodeId", iNodeId}
            };
            await NetHandler.Instance.Call("getClassDiagram", param);
        }
        #endregion Diagrams

        #region SetAtributes
        public static async void SetNodeShape(int iNodeId, string iShape, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"nodeId", iNodeId},
                {"newShape", iShape}
            };
            await NetHandler.Instance.Call("setNodeShape", param);
        }

        public static async void setNodeColor(int iNodeId, Color iColor, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"nodeId", iNodeId},
                {"R", iColor.r},
                {"G", iColor.g},
                {"B", iColor.b},
                {"A", iColor.a},
            };
            await NetHandler.Instance.Call("setNodeColor", param);
        }
        public static async void SetEdgeColor(int iEdgeId, Color iColor, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"edgeId", iEdgeId},
                {"R", iColor.r},
                {"G", iColor.g},
                {"B", iColor.b},
                {"A", iColor.a},
            };
            await NetHandler.Instance.Call("setEdgeColor", param);
        }

        public static async void SetNodePosition(int iNodeId, Vector3 iPosition, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"nodeId", iNodeId},
                {"x", iPosition.x},
                {"y", iPosition.y},
                {"z", iPosition.z},
            };
            await NetHandler.Instance.Call("setNodePosition", param);
        }

        #endregion SetAtributes

        #region Layouting
        public static async void RunLayouting(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(){{"graphId", iGraphId}};
            await NetHandler.Instance.Call("runLayouting", param);
        }
        public static async void TerminateAlgorithm(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(){{"graphId", iGraphId}};
            await NetHandler.Instance.Call("terminateAlgorithm", param);
        }
        public static async void Initialize(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(){{"graphId", iGraphId}};
            await NetHandler.Instance.Call("initialize", param);
        }
        public static async void PauseAlgorithm(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(){{"graphId", iGraphId}};
            await NetHandler.Instance.Call("pauseAlgorithm", param);
        }
        public static async void ResumeAlgorithm(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(){{"graphId", iGraphId}};
            await NetHandler.Instance.Call("resumeAlgorithm", param);
        }
        public static async void UpdateNodes(int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(){{"graphId", iGraphId}};
            await NetHandler.Instance.Call("updateNodes", param);
        }
        #endregion Layouting

        #region Magnets
        public static async void CreateMetaNode(int iGraphId, int iMetaNodeId, float iX, float iY, float iZ, int iType, float iStrength, float iMinDist, float iMaxDist)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"metaNodeId", iMetaNodeId},
                {"x", iX},
                {"y", iY},
                {"z", iZ},
                {"type", iType},
                {"strength", iStrength},
                {"minDist", iMinDist},
                {"maxDist", iMaxDist},
            };
            await NetHandler.Instance.Call("createMetaNode", param);
        }
        public static async void UpdateMetaNode(int iGraphId, int iMetaNodeId, float iX, float iY, float iZ, int iType, float iStrength, float iMinDist, float iMaxDist)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"metaNodeId", iMetaNodeId},
                {"x", iX},
                {"y", iY},
                {"z", iZ},
                {"type", iType},
                {"strength", iStrength},
                {"minDist", iMinDist},
                {"maxDist", iMaxDist},
            };
            await NetHandler.Instance.Call("updateMetaNode", param);
        }
        public static async void DeleteMetaNode(int iGraphId, int iMetaNodeId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"metaNodeId", iMetaNodeId},
            };
            await NetHandler.Instance.Call("deleteMetaNode", param);
        }
        public static async void CreateMetaEdge(int iGraphId, int iMetaEdgeId, int from, int fromType, int to, int toType)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"metaEdgeId", iMetaEdgeId},
                {"from", from},
                {"fromType", fromType},
                {"to", to},
                {"toType", toType}
            };
            await NetHandler.Instance.Call("createMetaEdge", param);
        }

        public static async void RemoveNodesFromRestriction(int iGraphId, int restrictionId, int[] nodeIds)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"restrictionId", restrictionId},
                {"nodeIds", nodeIds}
            };
            await NetHandler.Instance.Call("removeNodesFromRestriction", param);
        }

        public static async void CreateSphereRestriction(int iGraphId, int restrictionId, int[] nodeIds, Vector3 center, float radiusMin, float radiusMax, int mode)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"restrictionId", restrictionId},
                {"nodeIds", nodeIds},
                {"x", center.x},
                {"y", center.y},
                {"z", center.z},
                {"radiusMin", radiusMin},
                {"radiusMax", radiusMax},
                {"mode", mode}
            };
            await NetHandler.Instance.Call("createSphereRestriction", param);
        }

        public static async void UpdateSphereRestriction(int iGraphId, int restrictionId, int[] nodeIds, Vector3 center, float radiusMin, float radiusMax, int mode)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"restrictionId", restrictionId},
                {"nodeIds", nodeIds},
                {"x", center.x},
                {"y", center.y},
                {"z", center.z},
                {"radiusMin", radiusMin},
                {"radiusMax", radiusMax},
                {"mode", mode}
            };
            await NetHandler.Instance.Call("updateSphereRestriction", param);
        }

        public static async void CreateBoxRestriction(int iGraphId, int restrictionId, int[] nodeIds, Vector3 center, Vector3 boxSize, int mode)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"restrictionId", restrictionId},
                {"nodeIds", nodeIds},

                {"x", center.x},
                {"y", center.y},
                {"z", center.z},

                {"boxSizeX", boxSize.x},
                {"boxSizeY", boxSize.y},
                {"boxSizeZ", boxSize.z},
                
                {"mode", mode}
            };
            await NetHandler.Instance.Call("createBoxRestriction", param);
        }

        public static async void UpdateBoxRestriction(int iGraphId, int restrictionId, int[] nodeIds, Vector3 center, Vector3 boxSize, int mode)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"restrictionId", restrictionId},
                {"nodeIds", nodeIds},

                {"x", center.x},
                {"y", center.y},
                {"z", center.z},

                {"boxSizeX", boxSize.x},
                {"boxSizeY", boxSize.y},
                {"boxSizeZ", boxSize.z},

                {"mode", mode}
            };
            await NetHandler.Instance.Call("updateBoxRestriction", param);
        }

        public static async void DeleteMetaEdge(int iGraphId, int iMetaEdgeId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"metaEdgeId", iMetaEdgeId},
            };
            await NetHandler.Instance.Call("deleteMetaEdge", param);
        }
        public static async void SetAttractiveForce(float iValue, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"value", iValue},
            };
            await NetHandler.Instance.Call("setAttractiveForceFactor", param);
        }
        public static async void SetRepulsiveForce(float iValue, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"value", iValue},
            };
            await NetHandler.Instance.Call("setRepulsiveForceFactor", param);
        }
        public static async void SetMinNodeDistance(float iValue, int iGraphId = 1)
        {
            Dictionary<string, object> param = new Dictionary<string, object>()
            {
                {"graphId", iGraphId},
                {"value", iValue},
            };
            await NetHandler.Instance.Call("setMinNodeMargin", param);
        }
        #endregion MetaNode

        #endregion ApiCalls
    }
}