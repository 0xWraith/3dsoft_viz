using System;
using UnityEngine;


namespace Softviz.Graph
{
    public class NodeLabel : MonoBehaviour
    {
        public GameObject label;
        // private GameObject labelGO;

        void Awake()
        {
            // labelCont = transform.Find("LabelCont").gameObject;
            // labelGO = Instantiate(Resources.Load<GameObject>("Prefabs/Graph/TextPrefab"), labelCont.transform);
            // labelGO.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
        }

        public void SetLabel(string iLabel)
        {
            // labelGO.GetComponent<TextMesh>().text = label.Substring(0, Mathf.Min(30, label.Length));
            // Ak má nejaký starý popisok, odstránime ho.
            if (label != null)
            {
                Destroy(label);
            }

            var node = GetComponent<NodeXR>();
            var shapeRend = node.model.GetComponent<Renderer>();

            if (shapeRend != null)
            {
                // Vytvoríme objekt s textprefabom, na pozíciu trošku nad hranou a nastavíme hranu ako parenta.
                label = Instantiate(Resources.Load<GameObject>("Prefabs/MRTKScene/VRTextPrefab"), this.transform.position + new Vector3(0, 0f, -0.1f), Quaternion.identity, node.transform);
                label.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                label.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);// Quaternion.Euler(0f, node.transform.rotation.eulerAngles.y, node.transform.rotation.eulerAngles.z);
                // Nastavíme text vytvorenému objektu. Text bude mať maximálne 30 znakov.
                int maxLetters = (30 < iLabel.Length) ? 30 : iLabel.Length;
                label.GetComponent<TextMesh>().text = iLabel.Substring(0, maxLetters);
            }
        }
    }
}
