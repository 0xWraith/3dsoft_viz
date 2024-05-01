using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start() {

    }
    void Update() {

    }

    public void OnSpationalMapStateChange(bool state) {
        if (BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance == null) {
            Debug.LogError("Could not find EskyTrackerZed instance");
            return;
        }
        BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance.StartSpatialMappingTest = state;
        BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance.StopSpatialMappingTest = !state;
    }
}
