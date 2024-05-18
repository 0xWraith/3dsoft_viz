using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    private bool generated = false;
    private GameObject[] AllNodes;
    private int[] SelectedIndexes;
    private Material HighlightMaterial;
    private int NumOfSelectedNodes = 10;
    
    void Start()
    {
        // Get highlight material
        HighlightMaterial = Resources.Load("HighlightMaterialOn", typeof(Material)) as Material;
    }
    
    void Update()
    {
        // Turn on node highlight
        if (Input.GetKeyUp(KeyCode.RightShift) && generated==false)
        {
            generated = true;
            
            // Get all nodes
            AllNodes = GameObject.FindGameObjectsWithTag("XRNode");

            // Select random nodes
            System.Random rand = new System.Random();
            SelectedIndexes = new int[NumOfSelectedNodes];
            for(int i = 0; i < NumOfSelectedNodes; i++)
                SelectedIndexes[i] = rand.Next(0, AllNodes.Length);
            
            // Change material of selected nodes
            for (int i = 0; i < NumOfSelectedNodes; i++)
            {
                AllNodes[SelectedIndexes[i]].GetComponent<Renderer>().material = HighlightMaterial;
            }
        }
    }
}