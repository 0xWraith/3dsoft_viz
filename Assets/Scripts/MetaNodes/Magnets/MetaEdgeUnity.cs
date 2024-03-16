using Softviz.Graph;
using Utils;
using System;
using UnityEngine;
using Softviz.Controllers;

namespace Softviz.MetaNodes.Magnets
{
    public class MetaEdgeUnity : GraphObject, ISelectableObject
    {
        public bool isSelected;

        [SerializeField]
        private GameObject meshGamebject = default;

        public GameObject MeshGameObject
        {
            get => meshGamebject ?? gameObject;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Reference to meta node
        /// </summary>
        public MagnetUnity MetaNode { get; set; }

        /// <summary>
        /// Reference to node
        /// </summary>
        public Node Node { get; set; }

        private void Update()
        {

            ConnectNodes();

        }

        private void ConnectNodes()
        {
            Connect(MeshGameObject, GetSourceNodePosition(), GetDestinationNodePosition(), 0.5f);
        }

        /// <summary>
        /// Get meta node
        /// </summary>
        private Vector3 GetSourceNodePosition()
        {

            return Node.transform.position;

        }

        /// <summary>
        /// Get meta node position with offset
        /// </summary>
        private Vector3 GetDestinationNodePosition()
        {
            return MetaNode.transform.position + DestinationPointOffset;
        }

        private void Connect(GameObject meshGo, Vector3 sourcePosition, Vector3 destinationPosition, float scaleFactor)
        {
            meshGo.transform.rotation = Quaternion.identity;
            meshGo.transform.up = destinationPosition - sourcePosition;
            meshGo.transform.position =
                sourcePosition + meshGo.transform.up.normalized * Vector3.Distance(sourcePosition, destinationPosition) * 0.5f;

            meshGo.transform.localScale = new Vector3(
                0.05f,
                Vector3.Distance(sourcePosition, destinationPosition) * scaleFactor, // / Graph.transform.localScale.y, // TODO nemame graph.transform
                0.05f);
        }

        void ISelectableObject.OnObjectSelected()
        {
            SetSelectedColor();
            isSelected = true;
            MagnetController.Instance.MetaEdgesSelectionChanged();
        }

        void ISelectableObject.OnObjectDeselect()
        {
            SetNormalColor();
            isSelected = false;
            MagnetController.Instance.MetaEdgesSelectionChanged();
        }

        public void SetSelectedColor()
        {
            GetComponentInChildren<MeshRenderer>().material.color = new Color(1.0f, 0.5f, 0.0f);
        }

        public void SetNormalColor()
        {
            GetComponentInChildren<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
        }

        // offset for browser nodes (when edge can overlap content of browser)
        public Vector3 DestinationPointOffset { get; set; } = Vector3.zero;
        public Vector3 LabelPosition => throw new NotImplementedException();
    }
}
