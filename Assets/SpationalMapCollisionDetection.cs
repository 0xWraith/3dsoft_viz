using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Softviz.Graph;

public class SpationalMapCollisionDetection : MonoBehaviour {

    public BoxCollider boundsControlCollider;
    public BoxCollider spatialMapCollider;

    private int SPATIAL_MAP_LAYER = -1;
    private LayerMask SPATIAL_MAP_LAYER_MASK;

    // [SerializeField]
    // public Transform nodesHolder;

    private Vector3 previousPosition;
    // private Vector3 nodesHolderPreviousPosition;

    void Start() {
        SPATIAL_MAP_LAYER = LayerMask.NameToLayer("SPATIAL_MAP_LAYER");

        if (SPATIAL_MAP_LAYER != -1) {
            SPATIAL_MAP_LAYER_MASK = 1 << SPATIAL_MAP_LAYER;
        } else {
            Debug.LogError("Could not find layer 'SPATIAL_MAP_LAYER' in the project settings. Please create a layer with this name and assign it to the spatial map objects in the scene.");
        }
    }

    void Update() {
        // if (boundsControlCollider != null && spatialMapCollider != null && SPATIAL_MAP_LAYER != -1) {
        //     spatialMapCollider.center = boundsControlCollider.center;
        //     spatialMapCollider.size = boundsControlCollider.size;


        //     bool isColliding = Physics.CheckBox(spatialMapCollider.center, spatialMapCollider.size / 2, Quaternion.identity, SPATIAL_MAP_LAYER_MASK);

        //     // Handle the result
        //     if (isColliding)
        //     {
        //         // nodesHolder.position = nodesHolderPreviousPosition;
        //         //Print name of the object that is colliding with the spatial map
        //         Debug.Log(gameObject.name + " is colliding with the spatial map!");
        //         Debug.Log("spatialMapCollider is hitting something on the 'WorldReconstruction' layer!");
        //         return;
        //     }
        // }
        // if (nodesHolder != null)
        //     nodesHolderPreviousPosition = nodesHolder.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.collider.gameObject.name.Contains("SpatialMappingChunk")) {
                Debug.Log("Hit Spatial Map");
                //When the object hits the spatial map, move it back to the previous position
                transform.position = previousPosition;
                return;
            }
        }
        previousPosition = transform.position;
    }
}
