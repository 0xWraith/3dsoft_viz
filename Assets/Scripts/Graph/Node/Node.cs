using UnityEngine;
using Utils;

namespace Softviz.Graph
{

    ///<summary>
    ///This class represents graph vertex. 
    ///</summary>
    public class Node : MonoBehaviour, ISelectableObject
    {
        public enum AvailableShapes { sphere, box };
        [SerializeField]
        public int id;
        [SerializeField]
        private bool isFiltered;

        public void Initialize(int id, Vector3 position)
        {
            this.id = id;
            // position = GameObject.FindGameObjectWithTag("CommunicationWrapper").transform.InverseTransformPoint(position);

            UpdatePosition(position);
        }

        private void Update() {
            var label = GetComponent<NodeLabel>().label;
            label.transform.rotation = Quaternion.LookRotation(label.transform.position - Camera.main.transform.position);
        }

        public virtual void UpdatePosition(Vector3 position)
        {
            // Debug.Log("A");
            var comp = GetComponent<NodePosition>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodePosition)) as NodePosition;
            }

            // position = GameObject.FindGameObjectWithTag("CommunicationWrapper").transform.InverseTransformPoint(position);

            comp.SetPosition(position);
        }

        public void UpdateShape(AvailableShapes shape)
        {
            var comp = GetComponent<NodeShape>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeShape)) as NodeShape;
            }
            comp.SetShape(shape);
        }

        public void UpdateColor(Color color)
        {
            var comp = GetComponent<NodeColor>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeColor)) as NodeColor;
            }
            comp.SetColor(color);
        }

        public void UpdateSize(Vector3 size)
        {
            var comp = GetComponent<NodeSize>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeSize)) as NodeSize;
            }
            comp.SetSize(size);
        }


        public void UpdateVisibility(bool visibility)
        {
            //TODO
        }


        public void UpdateMetrics(Metrics metrics)
        {
            var comp = GetComponent<NodeMetrics>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeMetrics)) as NodeMetrics;
            }
            comp.SetMetrics(metrics);
        }

        public void UpdateInfoflowMetrics(InfoflowMetrics infoflowMetrics)
        {
            var comp = GetComponent<NodeInfoflowMetrics>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeInfoflowMetrics)) as NodeInfoflowMetrics;
            }
            comp.SetInfoflowMetrics(infoflowMetrics);
        }

        public void UpdateLabel(string label)
        {
            var comp = GetComponent<NodeLabel>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeLabel)) as NodeLabel;
                comp.SetLabel(label);
                comp.label.SetActive(false);
            }
        }

        public void UpdateType(string type)
        {
            var comp = GetComponent<NodeType>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeType)) as NodeType;
            }
            comp.SetNodeType(type);
        }

        public void UpdateBody(string body)
        {
            //TODO
        }


        public void UpdateIsFiltered(bool iIsFiltered)
        {
            isFiltered = iIsFiltered;
            this.gameObject.SetActive(!isFiltered);
        }

        public void OnObjectSelected()
        {
            var comp = GetComponent<NodeColor>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeColor)) as NodeColor;
            }
            comp.SetShader(Shader.Find(Enums.Shaders.HighLight.ToString()));
        }

        public void OnObjectDeselect()
        {
            var comp = GetComponent<NodeColor>();
            if (comp == null)
            {
                comp = gameObject.AddComponent(typeof(NodeColor)) as NodeColor;
            }
            comp.SetShader(Shader.Find(Enums.Shaders.Standard.ToString()));
        }
    }
}