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

        public Transform arrow;

        public bool IsParent { get => isParent; }
        public string Type { get => type; }
        public bool IsFiltred { get => isFiltred; }
        public Node Source { get => source; }
        public Node Destination { get => destination; }
        public Graph parentGraph;

        private bool showLabel = true;
        // public bool ShowLabel
        // {
        //     get { return showLabel; }
        //     set
        //     {
        //         if (value != showLabel)
        //         {
        //             showLabel = value;
        //             onShowLabelChanged?.Invoke(this, new OnPropertyChanged<bool>(showLabel));
        //         }
        //     }
        // }
        // public EventHandler<OnPropertyChanged<bool>> onShowLabelChanged;


        public void Initialize(int id, Node source, Node destination, Graph iParentGraph)
        {
            this.id = id;
            this.ShapeRend = go.GetComponent<Renderer>();
            this.source = source;
            this.destination = destination;
            parentGraph = iParentGraph;

            Vector3 sourcePosition = this.source.gameObject.transform.position;
            Vector3 destinationPosition = this.destination.gameObject.transform.position;
            Connect(sourcePosition, destinationPosition, 0.5f);

            name = "Edge( " + this.source.id.ToString() + ", " + this.destination.id.ToString() + ")";
            
            arrow.localPosition = Vector3.zero;
        }

        private void Update()
        {
            Vector3 sourcePosition = source.gameObject.transform.position;
            Vector3 destinationPosition = destination.gameObject.transform.position;
            Connect(sourcePosition, destinationPosition, 0.5f);

            if (!source.isActiveAndEnabled || !destination.isActiveAndEnabled)
            {
                go.SetActive(false);
            }
            if (source.isActiveAndEnabled && destination.isActiveAndEnabled)
            {
                go.SetActive(true);
            }

            //speed(hrumy): There's no need to calculate this everyframe. Needs rework.
            arrow.rotation = Quaternion.LookRotation(arrow.position - destination.transform.position);
            arrow.localScale = new Vector3(
                0.25f * parentGraph.GetNodesHolder().transform.localScale.x,
                0.25f * parentGraph.GetNodesHolder().transform.localScale.y,
                0.25f * parentGraph.GetNodesHolder().transform.localScale.z
            );

            var label = GetComponent<EdgeLabel>().label;
            label.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            label.transform.localScale = new Vector3(
                0.1f * parentGraph.GetNodesHolder().transform.localScale.x *
                    Vector3.Distance(sourcePosition, destinationPosition),
                0.1f * parentGraph.GetNodesHolder().transform.localScale.y * 
                    Vector3.Distance(sourcePosition, destinationPosition),
                0.1f * parentGraph.GetNodesHolder().transform.localScale.z
            );
        }

        private void Connect(Vector3 sourcePosition, Vector3 destinationPosition, float scaleFactor)
        {
            transform.rotation = Quaternion.identity;
            go.transform.up = destinationPosition - sourcePosition;
            transform.position =
                sourcePosition + 0.5f * Vector3.Distance(sourcePosition, destinationPosition) * go.transform.up.normalized;

            go.transform.localScale = new Vector3(
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
                en.SetLabel(label);
                en.label.SetActive(false);
            }
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