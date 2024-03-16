using Utils;
using System.Linq;
using UnityEngine;
using Softviz.Controllers;
using Softviz.MetaNodes;

namespace Softviz.MetaNodes.Magnets
{
    public class MagnetUnity : MonoBehaviour, ISelectableObject
    {
        public bool isSelected = false;
        public bool isHidden = false;
        public int id;
        public MetaNode metaNode;


        void ISelectableObject.OnObjectSelected()
        {
            SetSelectedColor();
            isSelected = true;
            MagnetController.Instance.MagnetSelectionChanged();
        }

        void ISelectableObject.OnObjectDeselect()
        {
            SetNormalColor();
            isSelected = false;
            MagnetController.Instance.MagnetSelectionChanged();
        }

        public void SetSelectedColor()
        {
            var redMagnetPartRenderers = GetComponentsInChildren<MeshRenderer>().Where(c => c.tag == GameObjectTags.RedMagnetPart);
            foreach (var mr in redMagnetPartRenderers)
            {
                mr.material.color = new Color(1.0f, 0.5f, 0.0f);
            }
        }

        public void SetNormalColor()
        {
            var redMagnetPartRenderers = GetComponentsInChildren<MeshRenderer>().Where(c => c.tag == GameObjectTags.RedMagnetPart);

            foreach (var mr in redMagnetPartRenderers)
            {
                mr.material.color = new Color(1.0f, 0.0f, 0.0f);
            }
        }
    }
}
