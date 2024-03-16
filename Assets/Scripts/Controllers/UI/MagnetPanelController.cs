using Softviz.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Softviz.Controllers.UI
{
    public class MagnetPanelController : MonoBehaviour
    {
        [SerializeField]
        private Slider maxRadiusSlider;
        [SerializeField]
        private Slider minRadiusSlider;
        [SerializeField]
        private Button edgeMagnetBtn;
        [SerializeField]
        private Button distanceMagnetBtn;
        [SerializeField]
        private Button conditionMagnetBtn;
        [SerializeField]
        private Button selectAllMagnetsBtn;
        [SerializeField]
        private Button deleteMagnetBtn;
        [SerializeField]
        private Button changePositionBtn;
        [SerializeField]
        private Button connectMagnetBtn;
        [SerializeField]
        private Button deleteConnectionBtn;
        [SerializeField]
        private Button showHiddenMagnetsBtn;
        [SerializeField]
        private Button hideSelectedMagnetsBtn;
        [SerializeField]
        private Dropdown magnetTypeDropdown;

        private void Start()
        {
            maxRadiusSlider.onValueChanged.AddListener(MagnetController.Instance.MaxRadiusValueChanged);
            minRadiusSlider.onValueChanged.AddListener(MagnetController.Instance.MinRadiusValueChanged);
            edgeMagnetBtn.onClick.AddListener(MagnetController.Instance.StartEdgeMagnetSpawn);
            distanceMagnetBtn.onClick.AddListener(MagnetController.Instance.StartRadiusMagnetSpawn);
            selectAllMagnetsBtn.onClick.AddListener(MagnetController.Instance.SelectAllMagnets);
            deleteMagnetBtn.onClick.AddListener(MagnetController.Instance.DeleteMagnet);
            changePositionBtn.onClick.AddListener(MagnetController.Instance.StartChangingMagnetPosition);
            connectMagnetBtn.onClick.AddListener(MagnetController.Instance.GetMagnetToConnect);
            deleteConnectionBtn.onClick.AddListener(MagnetController.Instance.DeleteMetaEdge);
            showHiddenMagnetsBtn.onClick.AddListener(MagnetController.Instance.ShowHiddenMagnets);
            hideSelectedMagnetsBtn.onClick.AddListener(MagnetController.Instance.HideSelectedMagnets);
        }
    }
}
