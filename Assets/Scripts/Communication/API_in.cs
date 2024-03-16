using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softviz.Controllers;
using Utils;

namespace Communication
{
    public static class API_in
    {
        private static readonly Dictionary<string, Action<string>> methods = new Dictionary<string, Action<string>>()
            {
                {"loadGraph", LoadGraph},
                {"changeConfiguration", ChangeLayoutAlgorithm},
                {"layoutedGraph", GetNodePositionColumn},

                {"getNodeColorColumn", GetNodeColorColumn},
                {"getNodeFilteredColumn", GetNodeFilteredColumn},
                {"getNodePositionColumn", GetNodePositionColumn},
                {"getNodeSizeColumn", GetNodeSizeColumn},
                {"getNodeShapeColumn", GetNodeShapeColumn},
                {"getNodeVisibilityColumn", GetNodeVisibilityColumn},
                {"getNodeIdColumn", GetNodeIdColumn},
                {"getNodeLabelColumn", GetNodeLabelColumn},
                {"getNodeMetricsColumn", GetNodeMetricsColumn},
                {"getNodeInfoflowMetricsColumn", GetNodeInfoflowMetricsColumn},
                {"getNodeTypeColumn", GetNodeTypeColumn},
                {"getNodeBodyColumn", GetNodeBodyColumn},

                {"getEdgeColorColumn", GetEdgeColorColumn},
                {"getEdgeDestinationIdColumn", GetEdgeDestinationIdColumn},
                {"getEdgeFilteredColumn", GetEdgeFilteredColumn},
                {"getEdgeSourceIdColumn", GetEdgeSourceIdColumn},
                {"getEdgeLabelColumn", GetEdgeLabelColumn},
                {"getEdgeIsParentColumn", GetEdgeIsParentColumn},


                {"CreateSphereRestriction", CreateSphereRestriction},
                {"UpdateSphereRestriction", UpdateSphereRestriction},
                {"RemoveNodesFromRestriction", RemoveNodesFromRestriction},
                      
                {"runLayouting", RunLayouting},
                {"updateNodes", UpdateNodes},
            };

        public static async void CallLocal(string functionName, string result)
        {
            bool keyExists = methods.TryGetValue(functionName, out Action<string> function);
            if (!keyExists)
            {
                // Debug.LogWarning("Method \"" + functionName + "\" not implemented yet, Result: " + result);
                return;
            }
            await Task.Run(() => function(result));
        }

        private static void CheckAck(string functionName, string result)
        {
            if (!result.Equals("ack"))
            {
                Debug.LogError("Result of \"" + functionName + "\" is not \"ack\". Result: " + result);
            }
        }

        #region ApiCalls

        #region Misc
        private static void LoadGraph(string result)
        {
            CheckAck("LoadGraph", result);
        }
        private static void ChangeLayoutAlgorithm(string result)
        {
            CheckAck("ChangeLayoutAlgorithm", result);
        }
        #endregion Misc

        #region NodeColumns
        private static void GetNodeColorColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesColor(result));
        }
        private static void GetNodeFilteredColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesFiltered(result));
        }
        private static void GetNodePositionColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesPosition(result));
        }
        private static void GetNodeSizeColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesSize(result));
        }
        private static void GetNodeShapeColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesShapes(result));
        }
        private static void GetNodeVisibilityColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesVisibility(result));
        }
        private static void GetNodeIdColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.CreateNodes(result));
        }
        private static void GetNodeLabelColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesLabel(result));
        }
        private static void GetNodeMetricsColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesMetrics(result));
        }
        private static void GetNodeInfoflowMetricsColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesInfoflowMetrics(result));
            /// TODO prerobit (metoda v ClientGraphe)
        }
        private static void GetNodeTypeColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesType(result));
        }
        private static void GetNodeBodyColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateNodesBody(result));
            /// TODO prerobit (metoda v ClientGraphe)
        }
        #endregion NodeColumns

        #region EdgeColums

        private static void RemoveNodesFromRestriction(string result)
        {
            SynchEventProcessor.Enqueue(() => { Debug.Log("DELETED RESTRICITON"); });
        }

        private static void CreateSphereRestriction(string result)
        {
            SynchEventProcessor.Enqueue(() => { Debug.Log("CREATED RESTRICITON"); });
        }

        private static void UpdateSphereRestriction(string result)
        {
            SynchEventProcessor.Enqueue(() => { Debug.Log("UPDATE RESTRICITON"); });
        }

        private static void GetEdgeColorColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateEdgesColor(result));
        }
        private static void GetEdgeDestinationIdColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.CreateEdgesDst(result));
        }
        private static void GetEdgeFilteredColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateEdgesFiltered(result));
        }
        private static void GetEdgeIsParentColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateEdgesParent(result));
        }
        private static void GetEdgeSourceIdColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.CreateEdgesSrc(result));
        }
        private static void GetEdgeLabelColumn(string result)
        {
            SynchEventProcessor.Enqueue(() => GraphController.Instance.UpdateEdgesLabel(result));
        }
        #endregion EdgeColumns


        #region Layouting

        private static void RunLayouting(string result)
        {
            CheckAck("RunLayouting", result);
        }

        private static void UpdateNodes(string result)
        {
            CheckAck("UpdateNodes", result);
        }

        #endregion Layouting

        #endregion ApiCalls
    }
}