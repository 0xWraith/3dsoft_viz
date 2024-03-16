using UnityEngine;

namespace Softviz.Graph.VisualMapping
{
    /// <summary>
    /// Trieda, ktorá slúži ako komponent pre hranu. Tento komponent sa využíva pre nastavovanie popisku hrany.
    /// </summary>
    public class EdgeLabel : MonoBehaviour
    {
        /// <summary>
        /// Aktuálne nastavený popisok hrany.
        /// </summary>
        [SerializeField]
        private GameObject label;

        /// <summary>
        /// Metóda, ktorá nastaví popisok pre hranu podľa zadaného stringu.
        /// </summary>
        /// <param name="iLabel">Popisok, ktorý sa má hrane nastaviť.</param>
        public void SetLabel(string iLabel)
        {
            // Ak má nejaký starý popisok, odstránime ho.
            if (label != null)
            {
                Destroy(label);
            }

            var edge = GetComponent<Edge>();
            var shapeRend = edge.ShapeRend;

            // Ak má nejaký shape renderer, čiže ak má hrana nastavený nejaký tvar, tak chceme vytvoriť popisok.
            if (shapeRend != null)
            {
                // Vytvoríme objekt s textprefabom, na pozíciu trošku nad hranou a nastavíme hranu ako parenta.
                label = Instantiate(Resources.Load<GameObject>("Prefabs/Graph/TextPrefab"), this.transform.position - new Vector3(0, .2f, 0), Quaternion.identity, edge.transform);
                label.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                label.transform.rotation = Quaternion.Euler(0f, edge.transform.rotation.eulerAngles.y, edge.transform.rotation.eulerAngles.z);
                // Nastavíme text vytvorenému objektu. Text bude mať maximálne 30 znakov.
                int maxLetters = (30 < iLabel.Length) ? 30 : iLabel.Length;
                label.GetComponent<TextMesh>().text = iLabel.Substring(0, maxLetters);
            }
        }
    }
}