using System;
using System.Collections.Generic;
using Communication;
using Leap;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Softviz.Controllers;
using Softviz.Graph;
using Softviz.MetaNodes.Magnets;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using XRInteraction;

// todo(hrumy): Think about moving shared parts to controller class.

public class RestrictionObject : MonoBehaviour 
{
    public int id;

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
        if (confirmedPosition)
        {
            if (Time.time >= counter + 5f)
            {
                counter = (int)Time.time;
                RestrictionUpdateAPI();
            }
        }

        if (type == (int)Type.Box) 
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    public void RestrictionInit(int restrictionType, MetaNodesManager metaNodesManager) 
    {
        Instantiate(shapes[restrictionType], transform.position, transform.rotation, transform);

        manager = metaNodesManager;
        id      = manager.NewRestriction();

        type = restrictionType;
        mode = 1;

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
        
        RestrictionCreateAPI();
        confirmedPosition = true;
    }

    public void UIChangeMode(int newMode)
    {
        mode = newMode;
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