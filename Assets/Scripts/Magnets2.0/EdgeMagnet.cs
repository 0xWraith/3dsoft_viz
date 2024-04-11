using System.Collections.Generic;
using System.Linq;
using Communication;
using Softviz.Controllers;
using UnityEngine;
using static MetaNodesManager;

public class EdgeMagnet : MonoBehaviour {

    public int          id;
    public List<int>    selectedNodes = new List<int>();
    public LineRenderer edges;
    public Transform    nearMenu;

    private bool              confirmedPosition = false;
    private XRGraphController graph;
    private int               selectedCountLastFrame;
    private int               edgeId = 0;
    private int               firstNodeId = 0;

    public void EdgeMagnetInit(MetaNodesManager manager)
    {
        transform.localScale = new Vector3(
            transform.localScale.x / transform.parent.localScale.x,
            transform.localScale.y / transform.parent.localScale.y,
            transform.localScale.z / transform.parent.localScale.z
        );
        
        graph = (XRGraphController)GraphController.Instance;

        MetaNodeData data = manager.NewMagnet();
        id = data.id;

        API_out.CreateMetaNode(
            1,
            id,
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z,
            // type
            0,
            // strength
            0,
            // min dist
            5f,
            // max dist
            10f
        );

        edges.positionCount = graph.selectedNodes.Count * 2;
        edges.startWidth    = 0.05f;
        edges.endWidth      = 0.05f;

        foreach (var nodeId in graph.selectedNodes)
        {
            NodeXR node = (NodeXR)graph.graph.Nodes[nodeId];

            API_out.CreateMetaEdge(
                1,
                edgeId,
                nodeId,
                // from type
                2,
                id,
                // to type
                1
            );

            edges.SetPosition(2 * edgeId    , transform.position);
            edges.SetPosition(2 * edgeId + 1, node.transform.position);

            edgeId += 1;
        }

        firstNodeId = graph.selectedNodes[0];

        nearMenu.parent = manager.transform;
    }

    public void UIConfirmedPostion()
    {
        API_out.UpdateMetaNode(
            1,
            id,
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z,
            // type
            0,
            // strength
            100f,
            // min dist
            5f,
            // max dist
            10f
        );

        confirmedPosition = true;

        selectedNodes.AddRange(graph.selectedNodes);
        graph.DeselectNodes();
    }

    void Update()
    {
        if (!confirmedPosition)
        {
            if (selectedCountLastFrame != graph.selectedNodes.Count)
            {
                int last_index = edgeId;
                for (int i = last_index; i < graph.selectedNodes.Count; i++)
                {
                    int nodeId  = graph.selectedNodes[i];
                    NodeXR node = (NodeXR)graph.graph.Nodes[nodeId];

                    API_out.CreateMetaEdge(
                        1,
                        edgeId,
                        nodeId,
                        // from type
                        2,
                        id,
                        // to type
                        1
                    );

                    edges.positionCount += 2;
                    edges.SetPosition(2 * edgeId, transform.position);
                    edges.SetPosition(2 * edgeId + 1, node.transform.position);

                    edgeId += 1;
                }

                firstNodeId = firstNodeId == 0 ? graph.selectedNodes[0] : firstNodeId;
            }

            selectedCountLastFrame = graph.selectedNodes.Count;
        }

        if (confirmedPosition && transform.hasChanged)
        {
            API_out.UpdateMetaNode(
                    1,
                    id,
                    transform.localPosition.x,
                    transform.localPosition.y,
                    transform.localPosition.z,
                    // type
                    0,
                    // strength
                    100f,
                    // min dist
                    5f,
                    // max dist
                    10f
                );
        }

        // todo(hrumy): Add checker.
        if (graph.graph.Nodes[firstNodeId].transform.hasChanged 
            ||
            transform.hasChanged)
        {
            int edgeId = 0;
            foreach (var nodeId in confirmedPosition ? selectedNodes : graph.selectedNodes)
            {
                NodeXR node = (NodeXR)graph.graph.Nodes[nodeId];

                edges.SetPosition(2 * edgeId, transform.position);
                edges.SetPosition(2 * edgeId + 1, node.transform.position);

                edgeId += 1;
            }
        }

    }
}