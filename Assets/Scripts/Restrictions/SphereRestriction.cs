using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Communication;

public class SphereRestriction : Restriction
{
    int counter = 1;
    private float lastUpdateSeconds;

    // 1 -> 
    // 2 -> Nodes will be attached on the surface of restriction
    // 3 -> 
    [Range(1, 3)]
    [SerializeField] protected int mode;

    [SerializeField] protected bool wildcard = false;

    public void SetWildCardValue(bool val)
    {
        this.wildcard = val;
    }

    new void Start()
    {
        base.Start();

        CreateRestriction();
        lastUpdateSeconds = 0;
    }

    void Update()
    {
        if (Time.time >= lastUpdateSeconds + 30f)
        {
            lastUpdateSeconds = Time.time;
            // DestroyRestriction();
        }

        counter++;
        if (counter % 50 == 0) {
            UpdateRestriction();
        }
    }

    public void SetModeValue(int val)
    {
        this.mode = val;
    }

    public void DestroyRestriction() {

        GameObject[] graphNodes = GameObject.FindGameObjectsWithTag("XRNode");
        List<int> selectedNodesXR = new List<int>();

        foreach (GameObject graphNode in graphNodes)
        {
            if (!wildcard)
            { // TODO - Refactor
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 1)
                {
                    if (graphNode.GetComponentInParent<NodeXR>())
                    {
                        selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                    }

                }
            }
            else
            {
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 0)
                {
                    if (graphNode.GetComponentInParent<NodeXR>())
                    {
                        selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                    }
                }
            }
        }

        API_out.RemoveNodesFromRestriction(
            1, // graphID
            restrictionId, // restrictionId
            selectedNodesXR.ToArray() // nodeIds
        );
        Destroy(gameObject);
    }

    public void UpdateRestriction()
    {
        GameObject[] graphNodes = GameObject.FindGameObjectsWithTag("XRNode");
        List<int> selectedNodesXR = new List<int>();

        // Ukladáme vybrané uzly do poľa.
        foreach (GameObject graphNode in graphNodes)
        {
            if (!wildcard)
            { // TODO - Refactor
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 1)
                {
                    if (graphNode.GetComponentInParent<NodeXR>())
                    {
                        selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                    }

                }
            }
            else
            {
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 0)
                {
                    if (graphNode.GetComponentInParent<NodeXR>())
                    {
                        selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                    }
                }
            }
        }
        // new Vector3(transform.position.x / (transform.localScale.x / 2), transform.position.y / (transform.localScale.y / 2), transform.position.z / (transform.localScale.z / 2)),
        API_out.UpdateSphereRestriction(
            1, // graphID
            restrictionId, // restrictionId
            selectedNodesXR.ToArray(), // nodeIds
            transform.localPosition,
            // new Vector3(transform.position.x / (transform.localScale.x / 2), transform.position.y / (transform.localScale.y / 2), transform.position.z / (transform.localScale.z / 2)),
            transform.GetChild(0).gameObject.transform.localScale.x * transform.localScale.x, // radiusMin
            transform.localScale.x, // radiusMax
            mode // TODO - Toto pride zo vstupu
        );
    }

    new public void CreateRestriction() {

        GameObject[] graphNodes = GameObject.FindGameObjectsWithTag("XRNode");
        List<int> selectedNodesXR = new List<int>();

        // Ukladáme vybrané uzly do poľa.
        foreach (GameObject graphNode in graphNodes)
        {
            if (!wildcard) { // TODO - Refactor
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 1)
                {
                    if (graphNode.GetComponentInParent<NodeXR>()) {
                        selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                    }
                    
                }
            } else {
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 0)
                {
                    if (graphNode.GetComponentInParent<NodeXR>())
                    {
                        selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                    }
                }
            }
        }

        // int[] selectedNodesXR = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        API_out.CreateSphereRestriction(
            1, // graphID
            restrictionId, // restrictionId
            selectedNodesXR.ToArray(), // nodeIds
            transform.localPosition,
            transform.GetChild(0).gameObject.transform.localScale.x * transform.localScale.x, // radiusMin
            transform.localScale.x, // radiusMax
            mode // TODO - Toto pride zo vstupu
        );

    }
    new public void ShowRestrictionMenu(bool enabledState)
    {
       
    }

    public void RemoveNodesFromRestriction()
    {
        GameObject[] graphNodes = GameObject.FindGameObjectsWithTag("XRNode");
        List<int> selectedNodesXR = new List<int>();

        // Ukladáme vybrané uzly do poľa.
        foreach (GameObject graphNode in graphNodes)
        {

            if (graphNode.GetComponent<Interactable>().CurrentDimension == 1)
            {
                if (graphNode.GetComponentInParent<NodeXR>())
                {
                    selectedNodesXR.Add(graphNode.GetComponentInParent<NodeXR>().id);
                }

            }
        }

        Debug.Log("");

        // int[] selectedNodesXR = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        API_out.RemoveNodesFromRestriction(
            1, // graphID
            restrictionId, // restrictionId
            selectedNodesXR.ToArray() // nodeIds
        );

    }

}
