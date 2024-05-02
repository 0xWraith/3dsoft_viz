using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpationalMapCollisionDetection : MonoBehaviour {

    private GameObject spatialMap;

    void Start() {
        spatialMap = GameObject.Find("Mesh");

        if (spatialMap == null) {
            Debug.LogError("Could not find spatial map");
        } else {
            Debug.Log("Found spatial map");
        }
    }

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.collider.gameObject.name.Contains("SpatialMappingChunk")) {
                Debug.Log("Hit Spatial Map");
            }
        }     
    }
}
