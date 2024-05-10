using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpationalMapCollisionDetection : MonoBehaviour {

    private GameObject spatialMap;
    private Vector3 previousPosition;

    void Start() {
        Debug.Log("Hello from SpationalMapCollisionDetection");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Trigger");
        //Move object back to previous positio when it enters the trigger area

    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited Trigger");
        // Do something when an object exits the trigger area
    }

    void Update() {
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
