using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes;
using XRInteraction;

/// <summary>
/// Rodičovská trieda pre obmedzovače.
/// </summary>
public class Restriction : MonoBehaviour
{
    // Ukladáme doň renderer gameObjectu, ktorého vlastnosti chceme meniť
    [SerializeField] private Renderer TargetRenderer;

    // Prefab objektu obmedzovača
    [SerializeField] GameObject restrictionObject;

    // Prefab vnútorného obmedzovača.
    [SerializeField] GameObject innerRestriction;

    BoundsControl boundsControl;
    Microsoft.MixedReality.Toolkit.UI.ObjectManipulator objectManipulator;


    [SerializeField] GameObject menuVisuals;

    protected int restrictionId;

    // Start is called before the first frame update
    public void Start()
    {
        objectManipulator = gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
        boundsControl = gameObject.GetComponent<BoundsControl>();

        BoundsControlStateChanged(false);
        ObjectManipulatorStateChanged(false);

        MetaNodesManager metaNodeManager = GameObject.FindGameObjectWithTag("MetaNodesManager").GetComponent<MetaNodesManager>();
        restrictionId = metaNodeManager.GetMetaNodeCounter();

        metaNodeManager.IncrementCounter();

        GameObject MRTKContent = GameObject.FindGameObjectWithTag("MRTKSceneContent");
        MRTKContent.transform.position = new Vector3(MRTKContent.transform.position.x, MRTKContent.transform.position.y, 20);

        GameObject graph = GameObject.FindGameObjectWithTag("Graph");
        graph.transform.localScale = new Vector3(1, 1, 1);
        graph.transform.localPosition = new Vector3(0, 0, 0);
        graph.transform.rotation = Quaternion.identity;

        GameObject nodesHolder = GameObject.FindGameObjectWithTag("nodesHolder");
        nodesHolder.transform.localScale = new Vector3(0.4708009f, 0.4708009f, 0.4708009f);
        nodesHolder.transform.localPosition = new Vector3(0, 0, 0);
        nodesHolder.transform.rotation = Quaternion.identity;

        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        ground.transform.position = new Vector3(0, -(restrictionObject.transform.localScale.y / 2), 0);

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = new Vector3(camera.transform.position.x, 20, camera.transform.position.z);

    }

    void MoveGround() {
        
    }

    // Vypíname/zapíname bounding box okolo obmedzovača
    public void BoundsControlStateChanged(bool enabledState)
    {
        boundsControl = gameObject.GetComponent<BoundsControl>();

        boundsControl.enabled = enabledState;

        if (enabledState) {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        } else {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

    }

    // Vypíname/zapíname možnosť manipulácie s obmedzovačom (rotácia/translácia)
    public void ObjectManipulatorStateChanged(bool enabledState)
    {
        objectManipulator = gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();

        objectManipulator.enabled = enabledState;

        if (enabledState) {
            BoundsControlStateChanged(true);
            gameObject.GetComponent<BoxCollider>().enabled = true;
        } else {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    
    // Preťažujeme v child komponentoch.
    public void CreateRestriction()
    {
        
    }
    
    // TODO - Premenovať metódu, Slider na na zmenu priesvitnosti obmedzovača
    public void OnSliderUpdated(SliderEventData eventData)
    {
        var trans = 0.5f;

        TargetRenderer = GetComponentInChildren<Renderer>();
        var col = gameObject.GetComponent<Renderer>().material.color;
        
        col.a = trans;

        if ((TargetRenderer != null) && (TargetRenderer.material != null))
        {
            col.a = eventData.NewValue;

            TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.g, TargetRenderer.sharedMaterial.color.g, TargetRenderer.sharedMaterial.color.b, col.a);

        }
    }

    // Prilepenie uzlov k obmedzovaču.
    public void AttachNodes()
    {
        GameObject[] graphNodes = GameObject.FindGameObjectsWithTag("sphereXR");
        List<GameObject> selectedNodes = new List<GameObject>();


        // Ukladáme vybrané uzly do poľa.
        foreach (GameObject graphNode in graphNodes)
        {
            if (graphNode.GetComponent<Interactable>().CurrentDimension == 1)
            {
                selectedNodes.Add(graphNode);
            }
        }

        // Api Call


        // foreach(GameObject selectedNode in selectedNodes)
        // {
        //     Debug.Log(selectedNode);
        // }
    
    }

    // Zmena veľkosť vnútorného obmedzovača.
    public void OnSliderInnerSizeUpdated(SliderEventData eventData)
    {
        innerRestriction.transform.localScale = new Vector3(eventData.NewValue, eventData.NewValue, eventData.NewValue);
    }

    public void UniformScalingChanged(bool state)
    {
        BoundsControl objectBounds = gameObject.GetComponent<BoundsControl>();

        if (state) {
            objectBounds.ScaleHandlesConfig.ScaleBehavior = HandleScaleMode.NonUniform;
        } else {
            objectBounds.ScaleHandlesConfig.ScaleBehavior = HandleScaleMode.Uniform;
        }
    }

    public void ShowRestrictionMenu(bool enabledState)
    {
        bool showingRestriction = GameObject.FindGameObjectWithTag("XRMenu").GetComponent<XRHandMenu>().showingRestriction;
        Debug.Log("Showing: " + showingRestriction);
        Debug.Log("State: " + enabledState);
        if (enabledState && showingRestriction)
        {
            menuVisuals.SetActive(enabledState);
        }
    }
}
