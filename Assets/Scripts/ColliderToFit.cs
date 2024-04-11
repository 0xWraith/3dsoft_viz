using UnityEngine;
using UnityEditor;
using System.Collections;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.Graph;

namespace XRInteraction
{
    // We are checking colliders of nodes in graph so we can dynamically resize collider of the main graph
    public class ColliderToFit : MonoBehaviour
    {
        void Update() {
            FitColliderToChildren();
        }
        [SerializeField] GameObject parentObject;

        public BoxCollider bc;
        private Vector3    offset;


        private void Start() 
        {
            offset = transform.localPosition + transform.parent.localPosition;    
        }

        public void FitColliderToChildren()
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            bool hasBounds = false;

            GameObject[] renderers = GameObject.FindGameObjectsWithTag("XRNode");
            foreach (GameObject render in renderers)
            {
                if (hasBounds)
                {
                    bounds.Encapsulate(render.GetComponent<Renderer>().bounds);
                }
                else
                {
                    bounds = render.GetComponent<Renderer>().bounds;
                    hasBounds = true;
                }
            }

            // Setting final collider size
            if (hasBounds)
            {
                bc.center = bounds.center - offset; // /*bounds.center -*/ parentObject.transform.localPosition;
                bc.size = bounds.size;
            }
            else
            {
                bc.size = bc.center = Vector3.zero;
                bc.size = Vector3.zero;
            }
        }
    }
}