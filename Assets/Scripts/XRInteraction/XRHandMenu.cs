using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.MetaNodes.Magnets;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Utilities;
using Softviz.Graph;
using Softviz.Controllers;
using System.Data;

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

        // Restrictions 2.0
        public RestrictionObject restrictionPrefab;
        public Transform         restrictionParent;
        public Transform         restrictionSpawnPoint;
        public MetaNodesManager  metaNodesManager;

        // Magnets 2.0
        public EdgeMagnet   edgeMagnetPrefab;
        public RadiusMagnet radiusMagnetPrefab;

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z)) 
            {
                AddRadiusMagnet();
            }   
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
            restriction.RestrictionInit(type, metaNodesManager);
        }

        public void AddEdgeMagnet()
        {
            EdgeMagnet magnet = Instantiate(edgeMagnetPrefab, restrictionSpawnPoint.position, Quaternion.identity, restrictionParent);
            magnet.EdgeMagnetInit(metaNodesManager);
        }

        public void AddRadiusMagnet()
        {
            RadiusMagnet magnet = Instantiate(radiusMagnetPrefab, restrictionSpawnPoint.position, Quaternion.identity, restrictionParent);
            magnet.RadiusMagnetInit(metaNodesManager);
        }

        public void AddRestriction(int type)
        {
            GameObject wrapper = GameObject.FindGameObjectWithTag("CommunicationWrapper");
            GameObject restriction;

            if (type == 1) {
                restriction = Instantiate(sphereRestrictionWithMenu, new Vector3(0, 0, 0), Quaternion.identity, wrapper.transform);
            } else {
                restriction = Instantiate(quadRestrictionWithMenu, new Vector3(0, 0, 0), Quaternion.identity, wrapper.transform);
            }
            

            restriction.transform.parent = wrapper.transform;
            restriction.transform.localPosition = Vector3.zero;
            restriction.transform.position = new Vector3(0, 0, 0);

            GameObject[] rwm = GameObject.FindGameObjectsWithTag("RestrictionWithMenu");

            foreach(var a in rwm)
            {
                a.transform.position = new Vector3(0, 0, 0);
                restriction.transform.localPosition = Vector3.zero;
            }
        
        }

        public void StopUpdatingNodes() {
            
        }

        public void EnableShowingRestrictionMenu(bool enabledState)
        {
            showingRestriction = enabledState;
        }

        #endregion
    }
    
}
