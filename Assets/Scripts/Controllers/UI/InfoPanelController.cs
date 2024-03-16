using UnityEngine;


namespace Softviz.Controllers.UI
{
    public class InfoPanelController : MonoBehaviour
    {
        public GameObject placeMagnetInfo;
        public GameObject changeMagnetPosInfo;
        public GameObject connectMagnetInfo;

        void Start()
        {
            placeMagnetInfo.SetActive(false);
            changeMagnetPosInfo.SetActive(false);
            connectMagnetInfo.SetActive(false);

        }

        public void ShowPlaceMangetInfo(bool show)
        {
            placeMagnetInfo.SetActive(show);
        }

        public void ShowChangeMagnetPositionInfo(bool show)
        {
            changeMagnetPosInfo.SetActive(show);
        }

        public void ShowConnectMangetInfo(bool show)
        {
            connectMagnetInfo.SetActive(show);
        }
    }
}