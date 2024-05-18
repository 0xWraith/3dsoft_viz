using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.Controllers;
using Softviz.Graph;
using UnityEngine;

// Used by UI prefab (TypeItemEntry)
public class SortItemEntry : MonoBehaviour
{
    // public Transform toggleOn;
    // public Transform toggleOff;

    // private void Start()
    // {
    //     toggleOff.gameObject.SetActive(true);
    //     toggleOn.gameObject.SetActive(false);
    // }

    public bool       selected = false;
    public Transform  icon;

    public void AddToTypeFilter()
    {
        var graph   = (XRGraphController)GraphController.Instance;
        string type = GetComponent<ButtonConfigHelper>().MainLabelText;
        
        selected = !selected;
        if (selected)
        {
            graph.typeFilter.Add(type);
            icon.gameObject.SetActive(true);
        }
        else 
        {
            graph.typeFilter.Remove(type);
            icon.gameObject.SetActive(false);
        }
    }

    // public void RemoveFromTypeFilter()
    // {
    //     var graph = (XRGraphController)GraphController.Instance;
    //     string type = GetComponent<ButtonConfigHelper>().MainLabelText;

    //     graph.typeFilter.Remove(type);
    // }
}
