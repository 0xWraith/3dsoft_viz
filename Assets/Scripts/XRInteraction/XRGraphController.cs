using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Softviz.Controllers;
using Microsoft.MixedReality.Toolkit.UI;
using System.Linq;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Softviz.Graph;
using Softviz.Graph.VisualMapping;
using System;
using Microsoft.MixedReality.Toolkit.Utilities;
using Communication;

public class XRGraphController : GraphController
{
    protected bool layoutingCalled = false;
    float layoutingRunningTime = 0;
    protected bool groundPositionSet = false;

    public MultipleNodeSelection selector;

    public Transform typeFilterCollection;
    public Transform typeFilterEntryPrefab;

    public List<string> typeFilter;

    public MetaNodesManager metaNodes;

    private bool sortListLoaded = false;

    // public List<string> types;

    new public void Start()
    {
        base.Start();
        graph.GetComponent<BoundsControl>().enabled = false;
        graph.GetComponent<ObjectManipulator>().enabled = false;
    }

    new public void Update() {
        base.Update();

        if (!sortListLoaded)
        {
            Dictionary<int, Node> nodes = graph.Nodes;

            if (nodes == null)
            {
                return;
            }

            List<string> types = new List<string>();

            foreach (var node in nodes)
            {
                NodeXR xrNode = (NodeXR)node.Value;
                string type = xrNode.GetComponent<NodeType>().type;

                if (!types.Contains(type))
                {
                    types.Add(type);
                }
            }

            foreach (var type in types)
            {
                var spawn = Instantiate(typeFilterEntryPrefab, typeFilterCollection);
                spawn.GetComponent<ButtonConfigHelper>().MainLabelText = type;
            }
            typeFilterCollection.GetComponent<GridObjectCollection>().UpdateCollection();

            sortListLoaded = true;
        }
        // // As MRTK doesnt provide getter for active state of teleport system,
        // // when we dont enable teleportation, we dont place ground object
        // GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        // if (ground != null)
        // {
        //     if (layoutingCalled) {
        //         layoutingRunningTime += Time.deltaTime;
        //     }

        //     // We have to set proper ground height so we can navigate.
        //     // To determine position where the ground should be, we wait approximatelly 12s, in that time graph is usually fully layouted.
        //     // Then we get a position of the lowest node and set position based on that.

        //     if (layoutingCalled && !groundPositionSet && layoutingRunningTime > 12f) {
        //         GameObject[] nodes = GameObject.FindGameObjectsWithTag("XRNode");

        //         List<Transform> transforms = new List<Transform>();

        //         foreach(GameObject node in nodes) {
        //             transforms.Add(node.transform);
        //         }

        //         var lowestNode = transforms.OrderBy(t => t.position.y).First();
        //         ground.transform.position = new Vector3(0, lowestNode.position.y, 0);
        //         groundPositionSet = true;
        //     }
        // }


        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     SortNodesByType();
        // }

        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     FreezeSelectedNodes();
        // }

    }

    public void SortNodesByType()
    {
        Dictionary<int, Node> nodes = graph.Nodes;

        foreach (var node in nodes)
        {
            NodeXR xrNode = (NodeXR)node.Value;
            string type   = xrNode.nodeType.type;

            if (typeFilter.Contains(type))
            {
                xrNode.SetSelected(true);
            }
        }
    }

    public void SelectAllNodes()
    {
        Dictionary<int, Node> nodes = graph.Nodes;

        foreach (var node in nodes)
        {
            NodeXR xrNode = (NodeXR)node.Value;
            xrNode.SetSelected(true);
        }
    }

    public void DeselectAllNodes()
    {
        Dictionary<int, Node> nodes = graph.Nodes;

        foreach (var node in nodes)
        {
            NodeXR xrNode = (NodeXR)node.Value;
            xrNode.SetSelected(false);
        }

        selector.ResetSelector();
    }

    public void DeselectNodes()
    {
        foreach (int id in selectedNodes.ToArray())
        {
            NodeXR node = (NodeXR)graph.Nodes[id];
            node.SetSelected(false);
        }

        selector.ResetSelector();
    }

    public void ResizeSelectedVisuals()
    {
        foreach (int id in selectedNodes.ToArray())
        {
            NodeXR node = (NodeXR)graph.Nodes[id];
            node.ResizeHalo();
        }

        if (metaNodes.restrictionsCounter > 0)
        {
            foreach (int id in metaNodes.restrictionNodesIds)
            {
                NodeXR node = (NodeXR)graph.Nodes[id];
                node.ResizeHalo();
            }

            foreach (var rest in metaNodes.restrictions)
            {
                rest.halo.range = rest.transform.lossyScale.x; 
            }
        }
    }

    public void ShowNodeLabels(bool state) 
    {
        foreach (var node in graph.Nodes.Values)
        {
            node.GetComponent<NodeLabel>().label.SetActive(state);
        }
    }

    public void ShowEdgeLabes(bool state)
    {
        foreach (var edge in graph.Edges.Values)
        {
            edge.GetComponent<EdgeLabel>().label.SetActive(state);
        }
    }

    public void FreezeSelectedNodes() 
    {
        foreach (var node_id in selectedNodes)
        {
            API_out.SetNodeFixed(node_id, true);
        }
    }

    public void FreezeNodes(int[] nodes)
    {
        foreach (var node_id in nodes)
        {
            API_out.SetNodeFixed(node_id, true);
        }
    }

    public void UnfreezeNodes(int[] nodes)
    {
        foreach (var node_id in nodes)
        {
            API_out.SetNodeFixed(node_id, false);
        }
    }

    public void RunLayoutingFromXR()
    {
        RunLayouting();
        layoutingCalled = true;

    }

    // Updating graph holder scale - we have to update holder, not graph itself since the nodes-edges size ratio would break.
    public void UpdateSizeUsingSlider(SliderEventData eventData)
    {
        GameObject nodesHolder = GameObject.FindGameObjectWithTag("nodesHolder");
        nodesHolder.transform.localScale = new Vector3(eventData.NewValue, eventData.NewValue, eventData.NewValue);
    }

    public void BoundsControlStateChanged(bool state) 
    {
    }

    public void ManipulatorStateChanged(bool state)
    {
        graph.GetComponent<BoundsControl>().enabled = state;
        graph.GetComponent<ObjectManipulator>().enabled = state;
        graph.GetComponent<BoxCollider>().enabled = state;
    }

    public void TogglePauseStage(bool state) {
        if (state) {
            base.PauseAlgorithm();
        } else {
            // Magnets and restrictions do not know about our local graph properties such as scale and position
            // so if we want the algorithm to reflect correct data, we have to trash local graph changes
            // graph.transform.localScale = new Vector3(0, 0, 0);
            // graph.transform.position = new Vector3(0, 0, 0);
            base.ResumeAlgorithm();
        }
    }

}
