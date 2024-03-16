using UnityEngine;

namespace Softviz.Graph
{
    public class NodeColor : MonoBehaviour
    {
        private Material material;

        void Awake()
        {
            material = transform.Find("Model").gameObject.GetComponent<Renderer>().material;
        }

        public void SetColor(Color color)
        {
            material.color = color;

            if (color.a < 1f)
            {
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
        }

        public void SetShader(Shader shader)
        {
            material.shader = shader;
        }
    }
}