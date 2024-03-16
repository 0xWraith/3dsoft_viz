using System.Collections.Generic;
using Softviz.Controllers.UI;
using UnityEngine.UI;
using Softviz.Graph;
using Communication;
using UnityEngine;
using System.Linq;
using Utils;
using Softviz.MetaNodes.Magnets;
using Softviz.MetaNodes;

namespace Softviz.Controllers
{
    public class MagnetController : SingletonBase<MagnetController>
    {
        private long counter = 0;

        public GameObject magnetPanel;
        public GameObject roomConfigPanel;
        protected InfoPanelController infoPanelController;
        private SceneController sceneController;
        protected Camera mainCam;

        public GameObject edgeMagnetPrefab;
        public GameObject radiusMagnetPrefab;
        public GameObject metaEdgePrefab;

        public GameObject deleteMagnetButton;
        public GameObject changePositionButton;
        public GameObject connectMagnetButton;
        public GameObject deleteMetaEdgeButton;
        public GameObject hideSelectedMagnetsButton;
        public GameObject showHiddenMagnetsButton;

        public GameObject maxRadiusSlider;
        public GameObject minRadiusSlider;

        protected GameObject magnetToConnect;
        protected GameObject currentMagnetObject;

        protected List<Node> selectedNodes;
        protected List<GameObject> metaEdgesInScene = new List<GameObject>();
        protected List<GameObject> magnetsInScene = new List<GameObject>();
        protected List<MagnetUnity> selectedMagnets = new List<MagnetUnity>();
        private List<MetaEdgeUnity> selectedMetaEdges = new List<MetaEdgeUnity>();

        private bool draggable = false;
        private bool isMoving = false;
        private bool connectingMode = false;
        protected int magnetCount = 0;
        private bool ignoreSliderEvent = false;
        // at beginning there are no hidden magnets
        public bool hiddenMagnetsFlag = false;

        protected override void Start()
        {
            mainCam = Camera.main;
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            infoPanelController = GameObject.Find("InfoPanel").GetComponent<InfoPanelController>();
        }

        /// <summary>
        /// Hides selected magnets with their metaedges
        /// </summary>
        public void HideSelectedMagnets()
        {
            ToggleMagnetsVisibility(selectedMagnets, false);
            foreach (var m in selectedMagnets)
            {
                m.GetComponent<ISelectableObject>().OnObjectDeselect();
            }
            hiddenMagnetsFlag = true;
        }

        /// <summary>
        /// Shows hidden magnets with their metaedges
        /// </summary>
        public void ShowHiddenMagnets()
        {
            ToggleMagnetsVisibility(magnetsInScene.Select(m => m.GetComponent<MagnetUnity>()).ToList(), true);
            hiddenMagnetsFlag = false;
        }

        /// <summary>
        /// Hides or shows magnets passed as parameter with their metaedges
        /// </summary>
        public void ToggleMagnetsVisibility(List<MagnetUnity> magnetsList, bool toggle)
        {
            foreach (var magnet in magnetsList)
            {
                var renderers = magnet.GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    renderer.enabled = toggle;
                }

                if (magnet is EdgeMagnetUnity)
                {
                    foreach (var metaEdge in ((EdgeMagnetUnity)magnet).listOfMetaEdges)
                    {
                        metaEdge.GetComponentInChildren<Renderer>().enabled = toggle;
                    }
                }

                magnet.isHidden = !toggle;
            }
        }

        /// <summary>
        /// Gets selected magnet and saves it for further connection
        /// </summary>
        public void GetMagnetToConnect()
        {
            infoPanelController.ShowConnectMangetInfo(true);
            magnetPanel.SetActive(false);
            roomConfigPanel.SetActive(false);
            magnetToConnect = selectedMagnets.FirstOrDefault().gameObject;
            connectingMode = true;
        }

        /// <summary>
        /// Connects magnet with selected metaedges
        /// </summary>
        public virtual void ConnectMagnetWithNodes()
        {
            Debug.Log("CONNECTING a");
            ReturnToMagnetUI();
            connectingMode = false;

            selectedNodes = SelectionController.Instance.GetSelectedObjects<Node>().ToList();

            foreach (var node in selectedNodes)
            {
                GameObject metaEdgeGameObject = Instantiate(metaEdgePrefab, gameObject.transform);
                
                // TODO fix this to use Dict params (graphId, iMetaEdgeId)
                API_out.CreateMetaEdge(
                    1, // graphID
                    1, // metaEdgeID
                    node.id, // from
                    2, // fromType
                    magnetToConnect.GetComponent<MagnetUnity>().id, // to
                    1 // toType
                );

                metaEdgesInScene.Add(metaEdgeGameObject);
                var metaEdgeUnity = metaEdgeGameObject.GetComponent<MetaEdgeUnity>();
                metaEdgeUnity.MetaNode = magnetToConnect.GetComponent<MagnetUnity>();
                metaEdgeUnity.Node = node;
                magnetToConnect.GetComponent<EdgeMagnetUnity>().listOfMetaEdges.Add(metaEdgeUnity);

            }
        }

        /// <summary>
        /// Enables / disables button for magnet deletion and also sets proper text label
        /// </summary>
        public void MagnetSelectionChanged()
        {
            selectedMagnets = magnetsInScene.Select(m => m.GetComponent<MagnetUnity>()).Where(m => m.isSelected).ToList();
            var numOfSelected = selectedMagnets.Count();

            showHiddenMagnetsButton.GetComponent<Button>().interactable = hiddenMagnetsFlag == false ? false : true;

            deleteMagnetButton.GetComponent<Button>().interactable = numOfSelected > 0;
            deleteMagnetButton.GetComponentInChildren<Text>().text = numOfSelected > 1 ? "Delete magnets" : "Delete magnet";

            changePositionButton.GetComponent<Button>().interactable = numOfSelected == 1;

            hideSelectedMagnetsButton.GetComponent<Button>().interactable = numOfSelected > 0;
            hideSelectedMagnetsButton.GetComponentInChildren<Text>().text = numOfSelected > 1 ? "Hide selected magnets" : "Hide selected magnet";

            if (numOfSelected == 1)
            {
                var selectedMagnet = selectedMagnets.FirstOrDefault();
                currentMagnetObject = selectedMagnet.gameObject;

                if (selectedMagnet is EdgeMagnetUnity)
                {
                    connectMagnetButton.GetComponent<Button>().interactable = true;
                }
                else if (selectedMagnet is RadiusMagnetUnity)
                {
                    var radiusMetaNode = (RadiusMetaNode)selectedMagnet.metaNode;
                    maxRadiusSlider.GetComponent<Slider>().interactable = true;
                    minRadiusSlider.GetComponent<Slider>().interactable = true;
                    ignoreSliderEvent = true;
                    maxRadiusSlider.GetComponent<Slider>().value = (float)radiusMetaNode.MaxRadius * 2;
                    minRadiusSlider.GetComponent<Slider>().value = (float)radiusMetaNode.MinRadius * 2.0f / maxRadiusSlider.GetComponent<Slider>().value;
                    ignoreSliderEvent = false;
                }
            }
            else
            {
                connectMagnetButton.GetComponent<Button>().interactable = false;
                maxRadiusSlider.GetComponent<Slider>().interactable = false;
                minRadiusSlider.GetComponent<Slider>().interactable = false;
                ignoreSliderEvent = true;
                maxRadiusSlider.GetComponent<Slider>().value = 0;
                minRadiusSlider.GetComponent<Slider>().value = 0;
                ignoreSliderEvent = false;
            }
        }

        /// <summary>
        /// Enables / disables button for metaedge deletion and also sets proper text label
        /// </summary>
        public void MetaEdgesSelectionChanged()
        {
            selectedMetaEdges = metaEdgesInScene.Select(me => me.GetComponent<MetaEdgeUnity>()).Where(me => me.isSelected).ToList();
            var numOfSelected = selectedMetaEdges.Count();

            deleteMetaEdgeButton.GetComponent<Button>().interactable = numOfSelected > 0;
            deleteMetaEdgeButton.GetComponentInChildren<Text>().text = numOfSelected > 1 ? "Delete connections" : "Delete connection";
        }

        /// <summary>
        /// Iterates over all selected metaedges and removes them
        /// </summary>
        public void DeleteMetaEdge()
        {
            foreach (var metaEdge in selectedMetaEdges)
            {
                SelectionController.Instance.Unselect(metaEdge.gameObject);
                metaEdgesInScene.Remove(metaEdge.gameObject);
                ((EdgeMagnetUnity)metaEdge.MetaNode).listOfMetaEdges.Remove(metaEdge);
                Destroy(metaEdge.gameObject);
                // TODO fix this to use Dict params (graphId, iMetaEdgeId)
                // API_out.DeleteMetaEdge(1, metaEdge.Node.id, metaEdge.MetaNode.id);

            }
        }

        /// <summary>
        /// Iterates over all selected magnets and removes them
        /// </summary>
        public void DeleteMagnet()
        {
            foreach (var magnet in selectedMagnets)
            {

                foreach (var metaEdgeToRemove in metaEdgesInScene.Where(e => e.GetComponent<MetaEdgeUnity>().MetaNode.id == magnet.id))
                {
                    Destroy(metaEdgeToRemove);
                }
                metaEdgesInScene.RemoveAll(e => e.GetComponent<MetaEdgeUnity>().MetaNode.id == magnet.id);

                SelectionController.Instance.Unselect(magnet.gameObject);
                magnetsInScene.Remove(magnet.gameObject);
                Destroy(magnet.gameObject);
                showHiddenMagnetsButton.GetComponent<Button>().interactable = magnetsInScene.Count() > 0;

                API_out.DeleteMetaNode(1, magnet.id);
            }
        }

        /// <summary>
        /// Changes object opacity, which is passed as a parameter to function
        /// </summary>
        /// <param name="opacity"></param>
        protected void ChangeMagnetOpacity(float opacity)
        {
            var renderers = currentMagnetObject.GetComponentsInChildren<Renderer>();
            // Iterates through all child elements of object
            foreach (var r in renderers)
            {
                if (r.gameObject.name == "maxRadiusSphere" || r.gameObject.name == "minRadiusSphere")
                {
                    continue;
                }
                // var col = r.material.color;
                // col.a = opacity;
                // r.material.color = col;
            }
        }

        protected override void Update()
        {
            counter += 1;
            bool enterPressed = Input.GetKeyUp(KeyCode.Return);
            bool escapePressed = Input.GetKeyUp(KeyCode.Escape);
            bool mPressed = Input.GetKeyUp(KeyCode.M);

            // if (Input.GetKeyUp(KeyCode.T))
            // {
            //     StartEdgeMagnetSpawn();
            // }

            if (enterPressed && draggable)
            {
                FinishAddingMagnet();
            }

            if ((enterPressed || escapePressed || mPressed) && isMoving)
            {
                FinishChangingPosition();
            }

            if ((escapePressed || mPressed) && draggable)
            {
                CancelAddingMagnet();
            }

            if (draggable || isMoving)
            {
                MoveMagnetWithCamera();
            }

            if (enterPressed && connectingMode)
            {
                ConnectMagnetWithNodes();
            }

            if ((escapePressed || mPressed) && connectingMode)
            {
                ReturnToMagnetUI();
                connectingMode = false;
            }

            if (counter % 50 == 0) {
                if (currentMagnetObject) {
                    // AddMagnetToGraph();
                    // MoveMagnetWithCamera();
                    // API_out.UpdateNodes();
                }
            }

            // if (counter % 100 == 0)
            // {
            //     if (currentMagnetObject) {
            //         MoveMagnetWithCamera();
            //         AddMagnetToGraph();
            //     }
                
            // }

            MagnetSelectionChanged();
        }

        /// <summary>
        /// Hides info label from info panel, shows magnet panel and disables mouse look
        /// </summary>
        private void ReturnToMagnetUI()
        {
            infoPanelController.ShowPlaceMangetInfo(false);
            infoPanelController.ShowChangeMagnetPositionInfo(false);
            infoPanelController.ShowConnectMangetInfo(false);
            magnetPanel.SetActive(true);
            roomConfigPanel.SetActive(true);
            // sceneController.SetMouseLookEnabled(false);
        }

        protected void AddMagnetToGraph()
        {
            var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();

            if (currentMagnetUnity is RadiusMagnetUnity)
            {
                API_out.UpdateMetaNode(1, currentMagnetUnity.id, currentMagnetUnity.metaNode.Position.X, currentMagnetUnity.metaNode.Position.Y, currentMagnetUnity.metaNode.Position.Z,
                    1, (float)currentMagnetUnity.metaNode.Strength, minRadiusSlider.GetComponent<Slider>().value, maxRadiusSlider.GetComponent<Slider>().value
                );
            }
            else
            {
                API_out.UpdateMetaNode(1, currentMagnetUnity.id, currentMagnetUnity.metaNode.Position.X, currentMagnetUnity.metaNode.Position.Y, currentMagnetUnity.metaNode.Position.Z,
                    0, (float)currentMagnetUnity.metaNode.Strength, 0, 0
                );
            }
        }

        protected void FinishAddingMagnet()
        {
            AddMagnetToGraph();
            ReturnToMagnetUI();
            draggable = false;
            currentMagnetObject.GetComponent<SphereCollider>().enabled = true;
            ChangeMagnetOpacity(1.0f);
            magnetsInScene.Add(currentMagnetObject);
            showHiddenMagnetsButton.GetComponent<Button>().interactable = true;
        }

        protected void FinishChangingPosition()
        {
            AddMagnetToGraph();
            ReturnToMagnetUI();
            isMoving = false;
            ChangeMagnetOpacity(1.0f);
        }

        /// <summary>
        /// Updates game objects position as well as magnets position in layout manager based on moving camera
        /// </summary>
        protected void MoveMagnetWithCamera()
        {
            currentMagnetObject.transform.position = GetSpawnPosition();
            currentMagnetObject.transform.rotation = mainCam.transform.rotation;
            var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();
            var metaNode = currentMagnetUnity.metaNode;

            var position = currentMagnetObject.transform.position;
            metaNode.Position = new System.Numerics.Vector3(position.x, position.y, position.z);
        }

        /// <summary>
        /// Destroys newly created game object and also removes magnet from layout manager
        /// </summary>
        private void CancelAddingMagnet()
        {
            ReturnToMagnetUI();
            draggable = false;

            Destroy(currentMagnetObject);
            var id = magnetCount - 1;
            magnetCount--;
            API_out.DeleteMetaNode(1, id);
        }

        /// <summary>
        /// Invoked from magnet UI to set magnet to moving state
        /// </summary>
        public void StartChangingMagnetPosition()
        {
            magnetPanel.SetActive(false);
            roomConfigPanel.SetActive(false);
            infoPanelController.ShowChangeMagnetPositionInfo(true);
            sceneController.SetMouseLookEnabled(true);
            currentMagnetObject = selectedMagnets.FirstOrDefault().gameObject;
            ChangeMagnetOpacity(0.5f);
            isMoving = true;
        }

        /// <summary>
        /// Returns position in front of Main camera
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetSpawnPosition()
        {
            return mainCam.transform.position + mainCam.transform.forward * 4.0f;
        }

        /// <summary>
        /// Inserts magnet into scene as game object and creates metanode in layout manager
        /// </summary>
        public void StartEdgeMagnetSpawn()
        {
            foreach (var m in selectedMagnets)
            {
                m.GetComponent<ISelectableObject>().OnObjectDeselect();
            }
            var metaNode = new EdgeMetaNode
            {
                Strength = 100f
            };

            StartMagnetSpawn(metaNode, edgeMagnetPrefab, 0);
        }

        public void StartRadiusMagnetSpawn()
        {
            foreach (var m in selectedMagnets)
            {
                m.GetComponent<ISelectableObject>().OnObjectDeselect();
            }
            var metaNode = new RadiusMetaNode
            {
                Strength = 100f,
                MaxRadius = 1f,
                MinRadius = 0f
            };

            StartMagnetSpawn(metaNode, radiusMagnetPrefab, 1);
        }

        public void StartMagnetSpawn(MetaNode metaNode, GameObject prefab, int type)
        {
            infoPanelController.ShowPlaceMangetInfo(true);
            magnetPanel.SetActive(false);
            roomConfigPanel.SetActive(false);
            sceneController.SetMouseLookEnabled(true);
            if (!draggable && !isMoving)
            {
                draggable = true;
                currentMagnetObject = Instantiate(prefab, GetSpawnPosition(), mainCam.transform.rotation);
                var position = currentMagnetObject.transform.position;
                metaNode.Position = new System.Numerics.Vector3(position.x, position.y, position.z);
                ChangeMagnetOpacity(0.5f);

                var id = magnetCount++;
                var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();
                currentMagnetUnity.id = id;
                currentMagnetUnity.metaNode = metaNode;
                
                API_out.CreateMetaNode(1, currentMagnetUnity.id, currentMagnetUnity.metaNode.Position.X, currentMagnetUnity.metaNode.Position.Y, currentMagnetUnity.metaNode.Position.Z,
                    type, (float)currentMagnetUnity.metaNode.Strength, minRadiusSlider.GetComponent<Slider>().value, maxRadiusSlider.GetComponent<Slider>().value
                );
            }
        }

        public void MaxRadiusValueChanged(float value)
        {
            if (ignoreSliderEvent)
            {
                return;
            }

            var maxRadius = value;
            var minRadius = minRadiusSlider.GetComponent<Slider>().value * maxRadius;
            var radiusMagnet = currentMagnetObject.GetComponent<RadiusMagnetUnity>();
            radiusMagnet.maxRadiusSphere.transform.localScale = new Vector3(maxRadius, maxRadius, maxRadius);
            radiusMagnet.minRadiusSphere.transform.localScale = new Vector3(minRadius, minRadius, minRadius);

            var metaNode = currentMagnetObject.GetComponent<MagnetUnity>().metaNode;
            var radiusMetaNode = ((RadiusMetaNode)metaNode);
            radiusMetaNode.MaxRadius = maxRadius / 2.0f;
            radiusMetaNode.MinRadius = minRadius / 2.0f;

            API_out.UpdateMetaNode(1, radiusMagnet.id, radiusMagnet.metaNode.Position.X, radiusMagnet.metaNode.Position.Y, radiusMagnet.metaNode.Position.Z,
                                                                1, (float)radiusMagnet.metaNode.Strength, minRadiusSlider.GetComponent<Slider>().value, maxRadiusSlider.GetComponent<Slider>().value);

        }

        public void MinRadiusValueChanged(float value)
        {
            if (ignoreSliderEvent)
            {
                return;
            }

            var maxRadius = maxRadiusSlider.GetComponent<Slider>().value;
            var minRadius = value * maxRadius;

            var radiusMagnet = currentMagnetObject.GetComponent<RadiusMagnetUnity>();
            radiusMagnet.minRadiusSphere.transform.localScale = new Vector3(minRadius, minRadius, minRadius);
            var metaNode = currentMagnetObject.GetComponent<MagnetUnity>().metaNode;

            ((RadiusMetaNode)metaNode).MinRadius = minRadius / 2.0f;

            API_out.UpdateMetaNode(1, radiusMagnet.id, radiusMagnet.metaNode.Position.X, radiusMagnet.metaNode.Position.Y, radiusMagnet.metaNode.Position.Z,
                                                    1, (float)radiusMagnet.metaNode.Strength, minRadiusSlider.GetComponent<Slider>().value, maxRadiusSlider.GetComponent<Slider>().value);
        }

        public void SelectAllMagnets()
        {
            magnetsInScene.ForEach(m => SelectionController.Instance.Select(m));
        }
    }
}

