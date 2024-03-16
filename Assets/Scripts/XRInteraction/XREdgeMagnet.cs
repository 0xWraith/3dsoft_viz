using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Softviz.Controllers;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

// Overloading class for XR adjustments

public class XREdgeMagnet : MonoBehaviour
{
    [SerializeField]
    GameObject selectionController;
    
    public void Start() {
    }
    
    bool state = false;

    public void SelectedFromXR()
    {
        if (this.state) {
            gameObject.GetComponent<BoundsControl>().enabled = true;
            selectionController.GetComponent<SelectionController>().Select(gameObject);
            Debug.Log("SELECTED");
        } else {
            Debug.Log("UNSELECTED");
            selectionController.GetComponent<SelectionController>().Unselect(gameObject);
            gameObject.GetComponent<BoundsControl>().enabled = false;
        }
        this.state = !this.state;
    }

}
