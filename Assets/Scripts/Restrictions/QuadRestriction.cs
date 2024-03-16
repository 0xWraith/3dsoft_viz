using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Communication;

public class QuadRestriction : Restriction
{
    int counter = 1;
    private float lastUpdateSeconds;

    [SerializeField] protected bool wildcard = false;

    public void SetWildCardValue(bool val)
    {
        this.wildcard = val;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        CreateRestriction();
        lastUpdateSeconds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Time.time);
        // TODO docasne riesenie
        if (Time.time >= lastUpdateSeconds + 30f)
        {
            lastUpdateSeconds = Time.time;
            // DestroyRestriction();
        }

        counter++;
        if (counter % 50 == 0)
        {
            UpdateRestriction();
        }
    }

    new public void CreateRestriction()
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

        // API_out.CreateBoxRestriction(
        //     1, // graphID
        //     restrictionId, // restrictionId
        //     selectedNodesXR.ToArray(), // nodeIds
        //     transform.position,
        //     transform.localScale * 2, // radiusMax
        //     base.mode // TODO - Toto pride zo vstupu
        // );
        // Debug.Log(transform.localScale);

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

        // int[] selectedNodesXR = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        // API_out.UpdateBoxRestriction(
        //     1, // graphID
        //     restrictionId, // restrictionId
        //     selectedNodesXR.ToArray(), // nodeIds
        //     transform.position,
        //     transform.localScale * 2, // radiusMax
        //     base.mode // TODO - Toto pride zo vstupu
        // );
        Debug.Log(transform.localScale);

    }

}
