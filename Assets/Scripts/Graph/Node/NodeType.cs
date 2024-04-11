using System;
using UnityEngine;


namespace Softviz.Graph
{
    public class NodeType : MonoBehaviour
    {
        public string type;

        public void SetNodeType(string inType)
        {
            type =  inType;
        }
    }
}