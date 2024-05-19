using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Softviz.Graph;

public class SpationalMapCollisionDetection : MonoBehaviour {
    private Vector3 previousPosition;
    private Vector3 movingVector;
    void Start() {
    
    }
    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.collider.gameObject.name.Contains("SpatialMappingChunk")) {
                Debug.Log("Hit Spatial Map");
                transform.position += previousPosition + movingVector * -1f;
                return;
            }
        }
        movingVector = transform.position - previousPosition;
        previousPosition = transform.position;
    }
}
