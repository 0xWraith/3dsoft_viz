using Microsoft.MixedReality.Toolkit.Input;
using System;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.MetaNodes.Magnets;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Utilities;
using Softviz.Graph;
using Softviz.Controllers;
using System.Data;
using Communication;
using UnityEditor.PackageManager;
using System.Collections.Generic;

namespace XRInteraction
{

    /// <summary>
    /// Hand menu. In script we secure right rotation and summoning/closing of the menu.
    /// </summary>
    public class XRHandMenu : MonoBehaviour
    {
        [SerializeField] GameObject XRMagnetController;
        [SerializeField] GameObject sphereRestrictionWithMenu;
        [SerializeField] GameObject quadRestrictionWithMenu;

        public bool showingRestriction;

        private MeshRenderer targetObjectMesh = null;
        public MeshRenderer TargetObjectMesh
        {
            get => targetObjectMesh;
            set => targetObjectMesh = value;
        }
        private SpriteRenderer targetObjectSprite = null;
        public SpriteRenderer TargetObjectSprite
        {
            get => targetObjectSprite;
            set => targetObjectSprite = value;
        }

        public Transform graph;

        // Restrictions 2.0
        public RestrictionObject restrictionPrefab;
        public Transform         restrictionParent;
        public Transform         restrictionSpawnPoint;
        public MetaNodesManager  metaNodesManager;

        // Magnets 2.0
        public EdgeMagnet   edgeMagnetPrefab;
        public RadiusMagnet radiusMagnetPrefab;

        // Graph config
        public float          attractiveForce;
        public TMPro.TMP_Text attractiveForceText;
        public float          repulsiveForce;
        public TMPro.TMP_Text repulsiveForceText;
        public float          minNodeDist;
        public TMPro.TMP_Text minNodeDistText;

        public InteractionTimer logs;

        private void Start()
        {
        }
        
        #region Public Functions
        /// <summary>
        /// This will set the visibility, scale, and position of the Menu
        /// </summary>
        public void SummonMenu(GameObject container)
        {
            this.gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            transform.position = GameObject.Find(container.name + "/Anchor").transform.position;
            TargetObjectMesh = GameObject.Find(container.name + "/TargetObject (Mesh)").GetComponent<MeshRenderer>();
            TargetObjectSprite = GameObject.Find(container.name + "/TargetObject (Sprite)").GetComponent<SpriteRenderer>();
        }

        // public void AddRadiusMagnet()
        // {
        //     XRMagnetController.GetComponent<XRMagnetController>().AddStartRadiusMagnet();
        // }

        // public void AddEdgeMagnet()
        // {
        //     XRMagnetController.GetComponent<XRMagnetController>().AddEdgeMagnet();
        // }

        public void OnAttrativeForceSlider(SliderEventData data)
        {
            attractiveForce = data.NewValue * 10f;
            attractiveForceText.text = String.Format("{0:0.00}", attractiveForce);
        }

        public void OnRepulsiveForceSlider(SliderEventData data)
        {
            repulsiveForce = data.NewValue * 10f;
            repulsiveForceText.text = String.Format("{0:0.00}", repulsiveForce);
        }

        public void OnMinNodeDistSlider(SliderEventData data)
        {
            minNodeDist = data.NewValue * 10f;
            minNodeDistText.text = String.Format("{0:0.00}", minNodeDist);
        }

        public void ChangeGraphConfig()
        {
            API_out.SetAttractiveForce(attractiveForce);
            API_out.SetRepulsiveForce(repulsiveForce);
            API_out.SetMinNodeDistance(minNodeDist);
        }

        public void OnSliderMinRadiusChanged(SliderEventData eventData)
        {
            XRMagnetController.GetComponent<XRMagnetController>().OnSliderMinRadiusChanged(eventData);
        }

        public void OnSliderMaxRadiusChanged(SliderEventData eventData)
        {
            XRMagnetController.GetComponent<XRMagnetController>().OnSliderMaxRadiusChanged(eventData);
        }

        public void MenuLeftHandEnabled()
        {
            gameObject.GetComponent<SolverHandler>().TrackedHandness = Handedness.Right;
        }
        public void MenuRightHandEnabled()
        {
            gameObject.GetComponent<SolverHandler>().TrackedHandness = Handedness.Left;
        }

        public void SelectAllNodes()
        {
            XRGraphController xrGraph = (XRGraphController)GraphController.Instance;
            xrGraph.SelectAllNodes();
        }

        public void DeselectAllNodes()
        {
            XRGraphController xrGraph = (XRGraphController) GraphController.Instance;
            xrGraph.DeselectAllNodes();
        }

        public void AddRestrictionObject(int type) 
        {
            RestrictionObject restriction = Instantiate(restrictionPrefab, restrictionSpawnPoint.position, Quaternion.identity, restrictionParent);
            restriction.RestrictionInit(type, metaNodesManager, logs);
            metaNodesManager.restrictions.Add(restriction);
        }

        public void AddEdgeMagnet()
        {
            EdgeMagnet magnet = Instantiate(edgeMagnetPrefab, restrictionSpawnPoint.position, Quaternion.identity, restrictionParent);
            magnet.EdgeMagnetInit(metaNodesManager, logs);
        }

        public void AddRadiusMagnet()
        {
            RadiusMagnet magnet = Instantiate(radiusMagnetPrefab, restrictionSpawnPoint.position, Quaternion.identity, restrictionParent);
            magnet.RadiusMagnetInit(metaNodesManager, logs);
        }

        public void ResetGraph()
        {
            graph.localPosition = Vector3.zero;
            graph.localScale    = new Vector3(0.1f, 0.1f, 0.1f);
        }

        public void DisableNodeColliders(bool disabled)
        {
            var graph = (XRGraphController)GraphController.Instance;
            
            Dictionary<int, Node> nodes = graph.graph.Nodes;

            foreach (var node in nodes)
            {
                NodeXR xrNode = (NodeXR)node.Value;
                xrNode.model.GetComponent<BoxCollider>().enabled = !disabled;

                if (disabled) 
                {
                    var material = xrNode.model.GetComponent<Renderer>().material;
                    material.SetColor("_Color", Color.cyan);
                }
            }

            if (!disabled)
            {
                API_out.GetNodeColorColumn();
            }
        }

        // public void AddRestriction(int type)
        // {
        //     GameObject wrapper = GameObject.FindGameObjectWithTag("CommunicationWrapper");
        //     GameObject restriction;

        //     if (type == 1) {
        //         restriction = Instantiate(sphereRestrictionWithMenu, new Vector3(0, 0, 0), Quaternion.identity, wrapper.transform);
        //     } else {
        //         restriction = Instantiate(quadRestrictionWithMenu, new Vector3(0, 0, 0), Quaternion.identity, wrapper.transform);
        //     }
            

        //     restriction.transform.parent = wrapper.transform;
        //     restriction.transform.localPosition = Vector3.zero;
        //     restriction.transform.position = new Vector3(0, 0, 0);

        //     GameObject[] rwm = GameObject.FindGameObjectsWithTag("RestrictionWithMenu");

        //     foreach(var a in rwm)
        //     {
        //         a.transform.position = new Vector3(0, 0, 0);
        //         restriction.transform.localPosition = Vector3.zero;
        //     }
        
        // }

        public void StopUpdatingNodes() {
            
        }

        public void EnableShowingRestrictionMenu(bool enabledState)
        {
            showingRestriction = enabledState;
        }

        #endregion
    }
    
}
