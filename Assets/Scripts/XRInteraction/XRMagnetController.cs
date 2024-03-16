using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Softviz.Graph;
using Communication;
using Softviz.Controllers;
using Microsoft.MixedReality.Toolkit.UI;
using Utils;
using Softviz.Controllers.UI;

// Overloading class for XR adjustments
namespace Softviz.MetaNodes.Magnets
{
    public class XRMagnetController : MagnetController
    {
        private long counterr = 0;
        private float lastSLiderMinValue = 30f; // TODO - We should multiply it by graph scale
        private float lastSLiderMaxValue = 30f; // TODO - We should multiply it by graph scale
        private float sliderMultiplier = 25f; // MRTK slider offers range from 0 to 1f so we need to transform the scale

        protected override void Start()
        { 
            mainCam = Camera.main;
        }

        private void HandleMagnetMovement()
        {

            var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();
            var metaNode = currentMagnetUnity.metaNode;

            var position = currentMagnetObject.transform.position;

            metaNode.Position = new System.Numerics.Vector3(position.x, position.y, position.z);
        }

        // Connecting edge magnet with selected nodes
        public override void ConnectMagnetWithNodes()
        {
            GameObject[] graphNodes = GameObject.FindGameObjectsWithTag("XRNode");
            List<GameObject> selectedNodesXR = new List<GameObject>();

            Debug.Log(graphNodes);

            // Ukladáme vybrané uzly do poľa.
            foreach (GameObject graphNode in graphNodes)
            {
                if (graphNode.GetComponent<Interactable>().CurrentDimension == 1)
                {
                    selectedNodesXR.Add(graphNode);
                }
            }

            foreach (var node in selectedNodesXR)
            {
                GameObject metaEdgeGameObject = Instantiate(metaEdgePrefab, gameObject.transform);

                GameObject[] magnetsToConnect = GameObject.FindGameObjectsWithTag("Magnet");

                foreach (var magnetToConn in magnetsToConnect)
                {
                    if (!magnetToConn.GetComponent<EdgeMagnetUnity>()) {
                        continue;
                    }
                    metaEdgesInScene.Add(metaEdgeGameObject);
                    var metaEdgeUnity = metaEdgeGameObject.GetComponent<MetaEdgeUnity>();
                    metaEdgeUnity.MetaNode = magnetToConn.GetComponent<MagnetUnity>();
                    metaEdgeUnity.Node = node.GetComponentInParent<NodeXR>();
                    magnetToConn.GetComponent<EdgeMagnetUnity>().listOfMetaEdges.Add(metaEdgeUnity);

                    if (metaEdgeUnity.Node)
                    {
                        API_out.CreateMetaEdge(
                            1, // graphID
                            1, // metaEdgeID
                            node.GetComponentInParent<NodeXR>().id, // from
                            2, // fromType
                            magnetToConn.GetComponent<MagnetUnity>().id, // to
                            1 // toType
                        );
                    }
                }
                

            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            counterr += 1;
            // MagnetSelectionChanged();
            
            if (currentMagnetObject)
            {
                if (counterr % 50 == 0)
                {
                    if (currentMagnetObject)
                    {
                        HandleMagnetMovement();
                        AddMagnetToGraph(); 
                    }

                }
            }

            
        }

        new public void MagnetSelectionChanged()
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
                }
                else if (selectedMagnet is RadiusMagnetUnity)
                {
                    var radiusMetaNode = (RadiusMetaNode)selectedMagnet.metaNode;
                    lastSLiderMaxValue = (float)radiusMetaNode.MaxRadius * 2;
                    lastSLiderMinValue = (float)radiusMetaNode.MinRadius * 2.0f / lastSLiderMaxValue;
                }

                API_out.UpdateMetaNode(1, 0, currentMagnetObject.transform.position.x, currentMagnetObject.transform.position.y, currentMagnetObject.transform.position.z,
                    1, 100f, lastSLiderMinValue, lastSLiderMaxValue
                );
            }

        }

        public void OnSliderMinRadiusChanged(SliderEventData eventData)
        {
            if (currentMagnetObject) {
                lastSLiderMinValue = eventData.NewValue * sliderMultiplier/10;
                MinRadiusValueChanged(eventData.NewValue * sliderMultiplier/10);
            }
            
        }

        public void OnSliderMaxRadiusChanged(SliderEventData eventData)
        {
            if (currentMagnetObject) {
                lastSLiderMaxValue = eventData.NewValue * sliderMultiplier;
                MaxRadiusValueChanged(eventData.NewValue * sliderMultiplier);
            }
        }

        new public void MaxRadiusValueChanged(float value)
        {
            var maxRadius = value;
            var minRadius = lastSLiderMinValue * maxRadius;
            var radiusMagnet = currentMagnetObject.GetComponent<RadiusMagnetUnity>();
            radiusMagnet.maxRadiusSphere.transform.localScale = new Vector3(maxRadius, maxRadius, maxRadius);
            radiusMagnet.minRadiusSphere.transform.localScale = new Vector3(minRadius, minRadius, minRadius);

            var metaNode = currentMagnetObject.GetComponent<MagnetUnity>().metaNode;
            var radiusMetaNode = ((RadiusMetaNode)metaNode);
            radiusMetaNode.MaxRadius = maxRadius / 2.0f;
            radiusMetaNode.MinRadius = minRadius / 2.0f;

            API_out.UpdateMetaNode(1, radiusMagnet.id, radiusMagnet.metaNode.Position.X, radiusMagnet.metaNode.Position.Y, radiusMagnet.metaNode.Position.Z,
                                                            1, (float)radiusMagnet.metaNode.Strength, lastSLiderMinValue, lastSLiderMaxValue);
            

        }

        new protected void AddMagnetToGraph()
        {
            var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();

            if (currentMagnetUnity is RadiusMagnetUnity)
            {
                API_out.UpdateMetaNode(1, currentMagnetUnity.id, currentMagnetUnity.metaNode.Position.X, currentMagnetUnity.metaNode.Position.Y, currentMagnetUnity.metaNode.Position.Z,
                    1, (float)currentMagnetUnity.metaNode.Strength, lastSLiderMinValue, lastSLiderMaxValue
                );
            }
            else
            {
                API_out.UpdateMetaNode(1, currentMagnetUnity.id, currentMagnetUnity.metaNode.Position.X, currentMagnetUnity.metaNode.Position.Y, currentMagnetUnity.metaNode.Position.Z,
                    0, (float)currentMagnetUnity.metaNode.Strength, 0, 0
                );
            }
        }

        new public void MinRadiusValueChanged(float value)
        {
            var maxRadius = lastSLiderMaxValue;
            var minRadius = value * maxRadius;

            var radiusMagnet = currentMagnetObject.GetComponent<RadiusMagnetUnity>();
            radiusMagnet.minRadiusSphere.transform.localScale = new Vector3(minRadius, minRadius, minRadius);
            var metaNode = currentMagnetObject.GetComponent<MagnetUnity>().metaNode;

            ((RadiusMetaNode)metaNode).MinRadius = minRadius / 2.0f;

            if (counterr % 12 == 0) {
                API_out.UpdateMetaNode(1, radiusMagnet.id, radiusMagnet.metaNode.Position.X, radiusMagnet.metaNode.Position.Y, radiusMagnet.metaNode.Position.Z,
                                                        1, (float)radiusMagnet.metaNode.Strength, lastSLiderMinValue, lastSLiderMaxValue);
            }
        }

        public void AddEdgeMagnet()
        {
            StartEdgeMagnetSpawn();
            FinishAddingMagnet();
            base.SelectAllMagnets();
            FinishChangingPosition();
        }

        public void AddStartRadiusMagnet()
        {
            StartRadiusMagnetSpawn();
            FinishAddingMagnet();
            base.SelectAllMagnets();
            FinishChangingPosition();
        }

        new public void StartRadiusMagnetSpawn()
        {
            foreach (var m in selectedMagnets)
            {
                m.GetComponent<ISelectableObject>().OnObjectDeselect();
            }
            var metaNode = new RadiusMetaNode
            {
                Strength = 100f,
                MaxRadius = 10f,
                MinRadius = 5f
            };

            StartMagnetSpawn(metaNode, radiusMagnetPrefab, 1);
        }

        /// <summary>
        /// Inserts magnet into scene as game object and creates metanode in layout manager
        /// </summary>
        new public void StartEdgeMagnetSpawn()
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

        new public void StartMagnetSpawn(MetaNode metaNode, GameObject prefab, int type)
        {
            currentMagnetObject = Instantiate(prefab, base.GetSpawnPosition(), mainCam.transform.rotation);
            var position = currentMagnetObject.transform.position;
            metaNode.Position = new System.Numerics.Vector3(position.x, position.y, position.z);
            base.ChangeMagnetOpacity(0.5f);

            var id = magnetCount++;
            var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();
            currentMagnetUnity.id = id;
            currentMagnetUnity.metaNode = metaNode;

            API_out.CreateMetaNode(1, currentMagnetUnity.id, currentMagnetUnity.metaNode.Position.X, currentMagnetUnity.metaNode.Position.Y, currentMagnetUnity.metaNode.Position.Z,
                type, (float)currentMagnetUnity.metaNode.Strength, minRadiusSlider.GetComponent<Slider>().value, maxRadiusSlider.GetComponent<Slider>().value
            );
        }

        new protected void FinishAddingMagnet()
        {
            AddMagnetToGraph();
            currentMagnetObject.GetComponent<SphereCollider>().enabled = true;
            ChangeMagnetOpacity(1.0f);
            magnetsInScene.Add(currentMagnetObject);
            showHiddenMagnetsButton.GetComponent<Button>().interactable = true;

            var currentMagnetUnity = currentMagnetObject.GetComponent<MagnetUnity>();
        }

        /// <summary>
        /// Hides info label from info panel, shows magnet panel and disables mouse look
        /// </summary>
        private void ReturnToMagnetUI()
        {
            magnetPanel.SetActive(true);
            roomConfigPanel.SetActive(true);
        }

        new protected void FinishChangingPosition()
        {
            AddMagnetToGraph();
            ChangeMagnetOpacity(1.0f);
        }
    }
}