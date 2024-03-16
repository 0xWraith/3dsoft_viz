using UnityEngine;


namespace Softviz.Graph
{
    public class NodeShape : MonoBehaviour
    {
        private GameObject model;
        private MeshFilter meshFilter;
        private Mesh sphereShape;
        private Mesh cubeShape;

        void Awake()
        {
            model = transform.Find("Model").gameObject;
            meshFilter = model.GetComponent<MeshFilter>();
            sphereShape = Resources.Load<Mesh>("Meshes/Sphere");
            cubeShape = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        }

        public void SetShape(Node.AvailableShapes shape)
        {
            var collider = model.GetComponent<Collider>();
            if (collider != null) Destroy(collider);

            switch (shape)
            {
                case Node.AvailableShapes.sphere:
                    {
                        meshFilter.mesh = sphereShape;
                        model.AddComponent<SphereCollider>();
                        break;
                    }
                case Node.AvailableShapes.box:
                    {
                        meshFilter.mesh = cubeShape;
                        model.AddComponent<BoxCollider>();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
