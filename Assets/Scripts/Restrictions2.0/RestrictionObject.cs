using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Communication;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Softviz.Controllers;
using Softviz.Graph;
using Softviz.MetaNodes;
using Softviz.MetaNodes.Magnets;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR;
using XRInteraction;
using static MetaNodesManager;

// todo(hrumy): Think about moving shared parts to controller class.

public class RestrictionObject : MonoBehaviour 
{
    public int id;
    public Color color;
    // mode: 1 - outside, 2 - on edge, 3 - inside
    public int mode;
    public Transform[] shapes = new Transform[2];
    
    public BoundsControl     boundsControl;
    public ObjectManipulator objectManipulator;
    public Transform         nearMenu;
    public Transform         detailsMenu;
    public BoxCollider       boxCollider;

    public XRHandMenu menu;

    // UI
    public TMP_Text          restrictionIdText;

    enum Type {Sphere, Box}
    private int type;

    private MetaNodesManager manager;

    // debug(hrumy)
    public List<int> selectedNodes;

    private int counter = 0;
    private bool confirmedPosition;

    void Update()
    {
        // if (confirmedPosition)
        // {
        //     if (Time.time >= counter + 5f)
        //     {
        //         counter = (int)Time.time;
        //         RestrictionUpdateAPI();
        //     }
        // }
        

        if (type == (int)Type.Box) 
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    public void RestrictionInit(int restrictionType, MetaNodesManager metaNodesManager) 
    {
        Transform shape = Instantiate(shapes[restrictionType], transform.position, transform.rotation, transform);

        manager = metaNodesManager;
        MetaNodeData data = manager.NewRestriction();
        id    = data.id;
        color = data.color;

        type = restrictionType;
        mode = 1;

        shape.GetComponent<Light>().color = color;

        restrictionIdText.text = "Restriction ID: " + id;

        transform.localScale = new Vector3(
            transform.localScale.x / transform.parent.localScale.x,
            transform.localScale.y / transform.parent.localScale.y,
            transform.localScale.z / transform.parent.localScale.z
        );

        // note(hrumy): Avoiding overlaping.
        detailsMenu.localPosition += new Vector3(0, 0.5f * id, 0); 
        // detailsMenu.LookAt(Camera.main.transform);
        detailsMenu.rotation = Quaternion.LookRotation(detailsMenu.position - Camera.main.transform.position);

        // note(hrumy): Making the parent of near menus a static object to disable moving with parent.
        nearMenu.parent = detailsMenu.parent = metaNodesManager.transform;
    }

    #region UI_CALLBACKS
    public void UIAdjust()
    {
        var graph = (XRGraphController)GraphController.Instance;

        foreach (var node in selectedNodes)
        {
            var xrnode = (NodeXR)graph.graph.Nodes[node];
            // xrnode.gameObject.AddComponent<ParentConstraint>();

            ConstraintSource source = new ConstraintSource
            {
                sourceTransform = transform,
                weight = 1
            };

            xrnode.gameObject.GetComponent<ParentConstraint>().AddSource(source);
            xrnode.gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0, xrnode.transform.position - transform.position);

            xrnode.gameObject.GetComponent<ParentConstraint>().constraintActive = true;
            xrnode.gameObject.GetComponent<ParentConstraint>().locked = true;
        }

        graph.FreezeNodes(selectedNodes.ToArray());

        API_out.GetNodeIsFixedColumn();
        // var graph = (XRGraphController)GraphController.Instance;
        // // graph.TogglePauseStage(true);
        // adjusting = true;

        // ///////////////////////

        // pos = new Vector3[selectedNodes.Count];

        // int i = 0;
        // foreach (var node in selectedNodes)
        // {
        //     var xrnode = (NodeXR)graph.graph.Nodes[node];

        //     pos[i] = transform.InverseTransformPoint(xrnode.transform.position);

        //     i += 1;
        // }
    }

    public void UIDone()
    {
        var graph = (XRGraphController)GraphController.Instance;

        graph.UnfreezeNodes(selectedNodes.ToArray());
        API_out.GetNodeIsFixedColumn();
        
        
        foreach (var node in selectedNodes)
        {
            var xrnode = (NodeXR)graph.graph.Nodes[node];
            xrnode.gameObject.GetComponent<ParentConstraint>().RemoveSource(0);
            xrnode.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
            xrnode.gameObject.GetComponent<ParentConstraint>().locked = false;

            // note(hrumy): This will work, once the function is set in layouter. 
            // API_out.SetNodePosition(node, xrnode.transform.localPosition);
        }


        // var graph = (XRGraphController)GraphController.Instance;
        // // graph.TogglePauseStage(false);
        // adjusting = false;

        // foreach (var nodeID in selectedNodes)
        // {
        //     var xrnode = (NodeXR)graph.graph.Nodes[nodeID];
        //     API_out.SetNodePosition(nodeID, xrnode.transform.position);
        // }

        // // var graph = (XRGraphController)GraphController.Instance;

        // // foreach (var node in selectedNodes)
        // // {
        // //     var xrnode = (NodeXR)graph.graph.Nodes[node];
        // //     xrnode.transform.parent = transform.parent;
        // // }
    }

    // note(hrumy): This doesnt work on server. 
    public void UIAddNodes()
    {
        // selectedNodes.AddRange(XRGraphController.Instance.selectedNodes);
        // XRGraphController.Instance.selectedNodes.Clear();
    }

    public void UIConfirmedPostion() 
    {
        var graph = (XRGraphController) GraphController.Instance;

        selectedNodes.AddRange(graph.selectedNodes);

        graph.DeselectNodes();

        foreach (int node in selectedNodes) 
        {
            var xrnode = (NodeXR)graph.graph.Nodes[node];
            xrnode.OutlineWithoutSelection(color);
        }
        
        RestrictionCreateAPI();
        confirmedPosition = true;
    }

    public void UIChangeMode(int newMode)
    {
        mode = newMode;
        RestrictionUpdateAPI();
    }

    public void UIDeattachNodes()
    {
        // note(hrumy): Probably delete this, and just do RestrictionDestroy().
        // selectedNodes = null;
    }

    public void UIRestrictionDestroy()
    {
        RestrictionRemoveAPI();
        manager.DeleteRestriction();
        Destroy(gameObject);
        Destroy(nearMenu.gameObject);
        Destroy(detailsMenu.gameObject);

        var graph = (XRGraphController)GraphController.Instance;
        foreach (int node in selectedNodes)
        {
            var xrnode = (NodeXR)graph.graph.Nodes[node];
            xrnode.OutlineWithoutSelectionOff();
        }
    }

    #endregion

    #region API_CALLS

    public void RestrictionCreateAPI()
    {
        switch (type)
        {
            case (int)Type.Sphere:
                API_out.CreateSphereRestriction(
                    // graphID
                    1,
                    // restrictionId
                    id,
                    // nodeIds
                    selectedNodes.ToArray(),
                    // restriction position 
                    transform.localPosition,
                    // radiusMin
                    shapes[(int)Type.Sphere].localScale.x / 2,
                    // radiusMax
                    shapes[(int)Type.Sphere].localScale.x / 2 * transform.localScale.x,
                    // mode
                    mode
                );
                break;

            case (int)Type.Box:
                API_out.CreateBoxRestriction(
                    // graphID
                    1,
                    // restrictionId
                    id,
                    // nodeIds
                    selectedNodes.ToArray(),
                    // restriciton position 
                    transform.localPosition,
                    // box size
                    new Vector3(
                        shapes[(int)Type.Box].localScale.x * transform.localScale.x,
                        shapes[(int)Type.Box].localScale.y * transform.localScale.y,
                        shapes[(int)Type.Box].localScale.z * transform.localScale.z
                    ),
                    // mode
                    mode
                );
                break;

            default:
                break;
        }
    }

    public void RestrictionUpdateAPI()
    {
        switch (type)
        {
            case (int)Type.Sphere:
                API_out.UpdateSphereRestriction(
                    // graphID
                    1,
                    // restrictionId
                    id,
                    // nodeIds
                    selectedNodes.ToArray(),
                    // restriction position 
                    transform.localPosition,
                    // radiusMin
                    shapes[(int)Type.Sphere].localScale.x / 2,
                    // radiusMax
                    shapes[(int)Type.Sphere].localScale.x / 2 * transform.localScale.x,
                    // mode
                    mode 
                );
                break;

            case (int)Type.Box:
                API_out.UpdateBoxRestriction(
                    // graphID
                    1,
                    // restrictionId
                    id,
                    // nodeIds
                    selectedNodes.ToArray(),
                    // restriciton position 
                    transform.localPosition,
                    // boxsize
                    new Vector3(
                        shapes[(int)Type.Box].localScale.x * transform.localScale.x,
                        shapes[(int)Type.Box].localScale.y * transform.localScale.y,
                        shapes[(int)Type.Box].localScale.z * transform.localScale.z
                    ),
                    // mode
                    mode 
                );
                break;

            default:
                break;
        }
    }

    public void RestrictionRemoveAPI()
    {
        API_out.RemoveNodesFromRestriction(
            // graphID
            1,
            // restrictionId
            id,
            // nodeIds
            selectedNodes.ToArray()
        );
    }

#endregion

}