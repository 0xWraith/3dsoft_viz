using UnityEngine;
using Utils;

namespace Softviz.Graph
{
    /// <summary>
    /// Abstraktná trieda, ktorá predstavuje všeobecný objekt grafu, s ktorým sa dá manipulovať. Implementuje metódy rozhrania ISelectableObject.
    /// </summary>
    public abstract class GraphObject : MonoBehaviour, ISelectableObject
    {
        /// <summary>
        /// Privátny renderer pre tvar objektu.
        /// </summary>
        [SerializeField]
        private Renderer shapeRend;

        /// <summary>
        /// Verejný renderer pre tvar objektu.
        /// </summary>
        public Renderer ShapeRend { get => shapeRend; set => shapeRend = value; }

        /// <summary>
        /// Privátne id objektu.
        /// </summary>
        public int id;

        /// <summary>
        /// Unity metóda. Po kliknutí na objekt chceme objekt označiť alebo odznačiť.
        /// </summary>
        private void OnMouseDown()
        {
            Debug.Log("Clicked on " + transform.name + " " + id.ToString());
            //SelectionManager.Instance.OnGraphObjectClick(this);
        }

        /// <summary>
        /// Metóda, ktorá označí objekt.
        /// </summary>
        public void OnObjectSelected()
        {
            Debug.Log("OnObjectSelected " + transform.name + " " + id.ToString());

            // Shader sa nastaví na highlighted.
            shapeRend.material.shader = Shader.Find(Enums.Shaders.HighLight.ToString());
        }

        /// <summary>
        /// Metóda, ktorá odznačí objekt.
        /// </summary>
        public void OnObjectDeselect()
        {
            Debug.Log("OnObjectDeselected " + transform.name + " " + id.ToString());

            // Shader sa nastaví na standard.
            shapeRend.material.shader = Shader.Find(Enums.Shaders.Standard.ToString());
        }
    }
}
