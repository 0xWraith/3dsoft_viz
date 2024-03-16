using Softviz.InputAction.Desktop;
using UnityEngine;
using Utils;


namespace Softviz.Controllers.UI
{
    public class SceneController : MonoBehaviour
    {

        [SerializeField] private GameObject magnetPanel;
        [SerializeField] private GameObject roomConfigPanel;

        [SerializeField] private GameObject algorithmLayoutPanel;

        private GameObject[] cameras;

        // Use this for initialization
        void Start()
        {
            magnetPanel = GameObject.FindGameObjectWithTag(GameObjectTags.MagnetPanel);
            magnetPanel?.SetActive(false);
            roomConfigPanel = GameObject.FindGameObjectWithTag(GameObjectTags.RoomConfigPanel);
            roomConfigPanel?.SetActive(false);
            cameras = GameObject.FindGameObjectsWithTag(GameObjectTags.MainCamera);

            algorithmLayoutPanel = GameObject.FindGameObjectWithTag(GameObjectTags.AlgorithmLayoutPanel);
            algorithmLayoutPanel?.SetActive(false);

            var inputHandler = InputController.Instance;

            var inputAction = DesktopInputAction.Instance;
            inputHandler.Subscribe(inputAction.Menu, ToggleMenu);

            inputHandler.Subscribe(inputAction.Layout, ToggleLayout);
        }

        public void SetMouseLookEnabled(bool enabled)
        {
            foreach (var camera in cameras)
            {
                var mouseLook = camera.GetComponent<MouseLook>();
                if (mouseLook != null)
                {
                    mouseLook.enabled = enabled;
                }
            }
        }

        private void ToggleMenu()
        {
            bool showPanel;

            if (magnetPanel?.activeSelf == true)
            {
                roomConfigPanel.SetActive(false);
                magnetPanel.SetActive(false);
                showPanel = false;
            }
            else
            {
                roomConfigPanel.SetActive(true);
                magnetPanel.SetActive(true);
                showPanel = true;
            }

            SetMouseLookEnabled(!showPanel);
        }

        private void ToggleLayout()
        {
            bool showPanel;
            if (algorithmLayoutPanel?.activeSelf == true)
            {
                algorithmLayoutPanel.SetActive(false);
                showPanel = false;
            }
            else
            {
                algorithmLayoutPanel.SetActive(true);
                showPanel = true;
            }

            SetMouseLookEnabled(!showPanel);
        }
    }
}
