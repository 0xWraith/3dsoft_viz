using UnityEngine;

namespace Softviz.Graph
{

public class NodeMetrics : MonoBehaviour {

    public Metrics metrics;

    public void SetMetrics(Metrics metricsIn) {
        metrics = metricsIn;    
    }    
}

}