using UnityEngine;
using Softviz.InputAction.Desktop;
using Softviz.Controllers;

namespace Utils
{
    public class MouseLook : BaseScript
    {
        public float cameraSensitivity = 0.15f;
        public float speed = 30;

        private Vector3 lastMouse = new Vector3(255, 255, 255);
        public enum ModifierKeys
        {
            leftShift = KeyCode.LeftShift,
            middleMouseButton = KeyCode.Mouse2,
            leftControl = KeyCode.LeftControl,
            none = KeyCode.None
        }

        public ModifierKeys modifierKey = ModifierKeys.middleMouseButton;

        public bool isCameraLocked = false;

        protected override void OnDisable()
        {
            SetEnabled(false);
        }

        protected override void OnEnable()
        {
            SetEnabled(true);
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                isCameraLocked = false;
            }
            else
            {
                isCameraLocked = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        protected override void Start()
        {
            var inputHandler = InputController.Instance;
            var inputAction = DesktopInputAction.Instance;

            inputHandler.Subscribe(inputAction.CameraLook, (xAxisValue, yAxisValue) =>
            {
                if (!isCameraLocked && (Input.GetKey((KeyCode)modifierKey) || modifierKey == ModifierKeys.none)) // <-- rozsireny podmienka na look
                {
                    lastMouse = Input.mousePosition - lastMouse;
                    lastMouse = new Vector3(-lastMouse.y * cameraSensitivity, lastMouse.x * cameraSensitivity, 0);
                    lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
                    transform.eulerAngles = lastMouse;
                }
                lastMouse = Input.mousePosition;
            }); ;

            inputHandler.Subscribe(inputAction.MoveVertical, (axisValue) =>
            {
                transform.position += transform.forward * speed * axisValue * Time.deltaTime;
            });

            inputHandler.Subscribe(inputAction.MoveHorizontal, (axisValue) =>
            {
                transform.position += transform.right* speed * axisValue* Time.deltaTime;
            });

            inputHandler.Subscribe(inputAction.UpDown, (axisValue) =>
            {
                transform.position += transform.up* speed * axisValue * Time.deltaTime;
            });
        }
    }
}
