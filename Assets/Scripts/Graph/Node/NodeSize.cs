using UnityEngine;


namespace Softviz.Graph
{
    public class NodeSize : MonoBehaviour
    {
        private GameObject model;
        private GameObject labelCont;

        void Awake()
        {
            model = transform.Find("Model").gameObject;
            labelCont = transform.Find("LabelCont").gameObject;
        }

        public void SetSize(Vector3 size)
        {
            model.transform.localScale = size;
            labelCont.transform.localPosition = new Vector3(0, size.y / 2, 0);
        }
    }
}