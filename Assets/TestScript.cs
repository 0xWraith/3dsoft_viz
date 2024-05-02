using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start() {
        Debug.Log("Hello World");
    }
    void Update() {

    }

    public void OnSpationalMapStateChange(bool state) {
        if (BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance == null) {
            Debug.LogError("Could not find EskyTrackerZed instance");
            return;
        }
        Debug.Log("Spatial Mapping State Changed to " + state);
        BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance.StartSpatialMappingTest = state;
        BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance.StopSpatialMappingTest = !state;
    }

    public void OnSpationalMapVizualizeStateChange(bool state) {
        if (BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance == null) {
            Debug.LogError("Could not find EskyTrackerZed instance");
            return;
        }
        Debug.Log("Spatial Mapping Visualization State Changed to " + state);
        BEERLabs.ProjectEsky.Tracking.EskyTrackerZed.zedInstance.UpdateVizualizationState(state);
    }
}
