using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

namespace Restrictions
{
    /// <summary>
    /// Táto trieda spravuje obmedzovače v scéne, nechceme viazať všeobecné akcie a funkcie obmedzovača na triedy, ktoré nesú informáciu len o jednom obmedzovači.
    /// </summary>
    public class RestrictionManager : MonoBehaviour
    {
        public GameObject restrictionWithMenuPrefab;

        static GameObject[] restrictions;

        GameObject restriction;

        public void AddRestriction(int type)
        {
            GameObject graph = GameObject.FindGameObjectWithTag("Graph");
            graph.transform.localScale = new Vector3(1, 1, 1);
            graph.transform.localPosition = new Vector3(0, 0, 0);
            graph.transform.rotation = Quaternion.identity;

            GameObject nodesHolder = GameObject.FindGameObjectWithTag("nodesHolder");
            nodesHolder.transform.localScale = new Vector3(1, 1, 1);
            nodesHolder.transform.localPosition = new Vector3(0, 0, 0);
            nodesHolder.transform.rotation = Quaternion.identity;

            GameObject MRTKContent = GameObject.FindGameObjectWithTag("MRTKSceneContent");
            MRTKContent.transform.position = new Vector3(MRTKContent.transform.position.x, MRTKContent.transform.position.y, 20);

            GameObject wrapper = GameObject.FindGameObjectWithTag("CommunicationWrapper");

            restriction = Instantiate(restrictionWithMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity, wrapper.transform);

            restriction.transform.parent = wrapper.transform;

            restriction.transform.localPosition = new Vector3(0, 0, 0);
            restrictions.Append(restriction);
        }

        public void Test()
        {
            Debug.Log("Test!!");
        }

        public void Update() {
            foreach (var rr in restrictions) {
                rr.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

}
