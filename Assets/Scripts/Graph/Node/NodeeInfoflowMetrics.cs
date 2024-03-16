using UnityEngine;

namespace Softviz.Graph
{

    public class NodeInfoflowMetrics : MonoBehaviour
    {
        public InfoflowMetrics infoflowMetrics;

        public void SetInfoflowMetrics(InfoflowMetrics metrics)
        {
            infoflowMetrics = metrics;    
        }
    }

}