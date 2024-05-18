using System;
using System.Collections.Generic;
using System.Linq;
using Communication;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.Controllers;
using UnityEngine;
using static MetaNodesManager;

public class EdgeMagnet : MonoBehaviour {

    public int              id;
    public List<int>        selectedNodes = new List<int>();
    public LineRenderer     edges;
    public Transform        nearMenu;
    public Transform        detailsMenu;
    public TMPro.TMP_Text   magnetIdText;
    public TMPro.TMP_Text   strengthText;
    public InteractionTimer logger;

    private bool              confirmedPosition = false;
    private XRGraphController graph;
    private int               selectedCountLastFrame;
    private int               edgeId = 0;
    private int               firstNodeId = -1;
    private MetaNodesManager  manager;
    private float             strength;

    public void EdgeMagnetInit(MetaNodesManager metaNodesManager, InteractionTimer logs)
    {
        transform.localScale = new Vector3(
            transform.localScale.x / transform.parent.localScale.x,
            transform.localScale.y / transform.parent.localScale.y,
            transform.localScale.z / transform.parent.localScale.z
        );
        
        graph = (XRGraphController)GraphController.Instance;

        manager = metaNodesManager;
        logger  = logs;

        MetaNodeData data = manager.NewMagnet();
        id = data.id;

        magnetIdText.text = "Magnet ID: " + id;

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

        if (graph.selectedNodes.Count != 0)
        {
            firstNodeId = graph.selectedNodes[0];
        }

        detailsMenu.localPosition += new Vector3(0, 0.5f * id, 0);
        detailsMenu.SetParent(manager.transform);
    }

    public void UIStrengthChange(SliderEventData data)
    {
        strength = data.NewValue;// * 10f;
        strengthText.text = String.Format("{0:0.00}", strength);

        API_out.UpdateMetaNode(
            1,
            id,
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z,
            // type
            0,
            // strength
            strength,
            // min dist
            5f,
            // max dist
            10f
        );
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
            strength,
            // min dist
            5f,
            // max dist
            10f
        );

        confirmedPosition = true;

        selectedNodes.AddRange(graph.selectedNodes);
        graph.DeselectNodes();
    }

    public void UIDestroy()
    {
        API_out.DeleteMetaNode(1, id);
        manager.DeleteMagnet();
        Destroy(gameObject);
        // Destroy(detailsMenu.gameObject);
    }

    public void UIManipulationStart()
    {
        logger.StartMagnetTimer(id);
    }

    public void UIManipulationEnd()
    {
        logger.EndMagnetTimer(id);
    }

    public void UpdateMagnet()
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
                strength,
                // min dist
                5f,
                // max dist
                10f
            );
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

                firstNodeId = firstNodeId == -1 ? graph.selectedNodes[0] : firstNodeId;
            }

            selectedCountLastFrame = graph.selectedNodes.Count;
        }

        // if (confirmedPosition && transform.hasChanged)
        // {
        //     API_out.UpdateMetaNode(
        //             1,
        //             id,
        //             transform.localPosition.x,
        //             transform.localPosition.y,
        //             transform.localPosition.z,
        //             // type
        //             0,
        //             // strength
        //             strength,
        //             // min dist
        //             5f,
        //             // max dist
        //             10f
        //         );
        // }

        if (firstNodeId != -1 
            &&
            graph.graph.Nodes[firstNodeId].transform.hasChanged 
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

        if (nearMenu.gameObject.activeSelf)
        {
            nearMenu.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            nearMenu.position = transform.position + 0.15f * transform.localScale.x * -nearMenu.transform.forward;
        }

    }
}