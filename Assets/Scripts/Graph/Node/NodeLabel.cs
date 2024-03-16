using UnityEngine;


namespace Softviz.Graph
{
    public class NodeLabel : MonoBehaviour
    {
        private GameObject labelCont;
        private GameObject labelGO;

        void Awake()
        {
            labelCont = transform.Find("LabelCont").gameObject;
            labelGO = Instantiate(Resources.Load<GameObject>("Prefabs/Graph/TextPrefab"), labelCont.transform);
            labelGO.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
        }

        public void SetLabel(string label)
        {
            labelGO.GetComponent<TextMesh>().text = label.Substring(0, Mathf.Min(30, label.Length));
        }
    }
}
