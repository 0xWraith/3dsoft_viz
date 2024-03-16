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
    public override void UpdatePosition(Vector3 position)
    {
        bool update = GameObject.FindGameObjectWithTag("CommunicationWrapper").GetComponent<NodePositionChange>().changePositions;

        if (!update) {
            return;
        }
        
        var comp = GetComponent<NodePosition>();
        if (comp == null)
        {
            comp = gameObject.AddComponent(typeof(NodePosition)) as NodePosition;
        }

        comp.SetPosition(position);
    }

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

        halo.range = 0.15f * model.transform.localScale.x;
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

        halo.range = 0.15f * model.transform.localScale.x;
        halo.enabled = selected;
    }
}
