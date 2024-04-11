using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Softviz.Graph;
using Softviz.Controllers;
using Microsoft.MixedReality.Toolkit.UI;

// Overloading class for XR adjustments
public class NodeXR : Node
{
    public bool selected = false;

    public Light halo;
    public Transform model;

    new public void UpdateShape(AvailableShapes shape)
    {
    }
    // public override void UpdatePosition(Vector3 position)
    // {
    //     bool update = GameObject.FindGameObjectWithTag("CommunicationWrapper").GetComponent<NodePositionChange>().changePositions;

    //     if (!update) {
    //         return;
    //     }
        
    //     var comp = GetComponent<NodePosition>();
    //     if (comp == null)
    //     {
    //         comp = gameObject.AddComponent(typeof(NodePosition)) as NodePosition;
    //     }

    //     comp.SetPosition(position);
    // }

    public void SelectedToggle()
    {
        selected = !selected;

        if (selected)
        {
            XRGraphController.Instance.selectedNodes.Add(id);
        }
        else 
        {
            XRGraphController.Instance.selectedNodes.Remove(id);
        }

        halo.range = 2f * model.transform.lossyScale.x;
        halo.enabled = selected;
    }

    public void SetSelected(bool value)
    {
        selected = value;

        if (selected)
        {
            XRGraphController.Instance.selectedNodes.Add(id);
        }
        else 
        {
            XRGraphController.Instance.selectedNodes.Remove(id);
        }

        halo.range = 2f * model.transform.lossyScale.x;
        halo.enabled = selected;
    }

    public void OutlineWithoutSelection(Color color) 
    {
        halo.range = 2f * model.transform.lossyScale.x;
        halo.color = color;
        halo.enabled = true;
    }

    public void OutlineWithoutSelectionOff()
    {
        halo.enabled = false;
        halo.color = Color.red;
    }

    public void ResizeHalo() 
    {
        halo.range = 2f * model.transform.lossyScale.x;
    }
}
