using UnityEngine;

namespace Softviz.Graph.VisualMapping
{
    /// <summary>
    /// Trieda, ktorá slúži ako komponent pre uzol a pre hranu. Tento komponent sa využíva pre nastavovanie farby uzlov a hrán.
    /// </summary>
    public class EdgeColor : MonoBehaviour
    {
        /// <summary>
        /// Aktuálne nastavená farba.
        /// </summary>
        [SerializeField]
        public Color color;

        /// <summary>
        /// Metóda, ktorá nastaví farbu hrany podľa zadanej farby.
        /// </summary>
        /// <param name="iColor">Farba, ktorú si má hrana nastaviť.</param>
        public void SetColorEdge(Color iColor)
        {
            // Nastavíme farbu.
            this.color = iColor;

            // Musíme upraviť aj renderer. (Prevzatá časť zo starého projektu.)
            var renderer = GetComponent<Edge>().ShapeRend;

            if (renderer != null)
            {
                renderer.material.color = iColor;

                if (iColor.a < 1f)
                {
                    renderer.material.SetFloat("_Mode", 3);
                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    renderer.material.SetInt("_ZWrite", 0);
                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.DisableKeyword("_ALPHABLEND_ON");
                    renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    renderer.material.renderQueue = 3000;
                }
            }
        }
    }
}
