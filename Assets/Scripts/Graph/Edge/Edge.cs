using UnityEngine;
using Softviz.Graph.VisualMapping;
using System;
using Utils;

namespace Softviz.Graph
{
    public class Edge : GraphObject
    {
        public GameObject go;
        private Node source;
        private Node destination;
        private bool isParent;
        private bool isFiltred;
        private string type;

        public bool IsParent { get => isParent; }
        public string Type { get => type; }
        public bool IsFiltred { get => isFiltred; }
        public Node Source { get => source; }
        public Node Destination { get => destination; }
        public Graph parentGraph;

        private bool showLabel = true;
        public bool ShowLabel
        {
            get { return showLabel; }
            set
            {
                if (value != showLabel)
                {
                    showLabel = value;
                    onShowLabelChanged?.Invoke(this, new OnPropertyChanged<bool>(showLabel));
                }
            }
        }
        public EventHandler<OnPropertyChanged<bool>> onShowLabelChanged;


        public void Initialize(int id, Node source, Node destination, Graph iParentGraph)
        {
            this.id = id;
            this.ShapeRend = go.GetComponent<Renderer>();
            this.source = source;
            this.destination = destination;
            parentGraph = iParentGraph;
            Vector3 sourcePosition = this.source.gameObject.transform.position;
            Vector3 destinationPosition = this.destination.gameObject.transform.position;
            Connect(this.go, sourcePosition, destinationPosition, 0.5f);
            name = "Edge( " + this.source.id.ToString() + ", " + this.destination.id.ToString() + ")";
        }

        private void Update()
        {
            Vector3 sourcePosition = source.gameObject.transform.position;
            Vector3 destinationPosition = destination.gameObject.transform.position;
            Connect(go, sourcePosition, destinationPosition, 0.5f);
            if (!source.isActiveAndEnabled || !destination.isActiveAndEnabled)
            {
                go.SetActive(false);
            }
            if (source.isActiveAndEnabled && destination.isActiveAndEnabled)
            {
                go.SetActive(true);
            }
        }

        private void Connect(GameObject meshGo, Vector3 sourcePosition, Vector3 destinationPosition, float scaleFactor)
        {
            transform.rotation = Quaternion.identity;
            meshGo.transform.up = destinationPosition - sourcePosition;
            transform.position =
                sourcePosition + meshGo.transform.up.normalized * Vector3.Distance(sourcePosition, destinationPosition) * 0.5f;

            meshGo.transform.localScale = new Vector3(
                0.25f * parentGraph.GetNodesHolder().transform.localScale.x,
                Vector3.Distance(sourcePosition, destinationPosition) * scaleFactor / 1f,
                0.25f * parentGraph.GetNodesHolder().transform.localScale.z);    
        }

        public void UpdateLabel(string label)
        {
            var en = GetComponent<EdgeLabel>();
            if (en == null)
            {
                en = gameObject.AddComponent(typeof(EdgeLabel)) as EdgeLabel;
            }
            // en.SetLabel(label);
        }

        public void UpdateColor(Color color)
        {
            var nc = GetComponent<EdgeColor>();
            if (nc == null)
            {
                nc = gameObject.AddComponent(typeof(EdgeColor)) as EdgeColor;
            }
            nc.SetColorEdge(color);
        }

        public void UpdateIsParent(bool iIsParent)
        {
            isParent = iIsParent;
        }

        public void UpdateIsFiltred(bool iIsFiltred)
        {
            isFiltred = iIsFiltred;
            this.gameObject.SetActive(!isFiltred);
        }

        private Vector3 GetDestinationNodePosition()
        {
            return destination.transform.position + DestinationPointOffset;
        }

        public Vector3 DestinationPointOffset { get; set; } = Vector3.zero;

    }
}