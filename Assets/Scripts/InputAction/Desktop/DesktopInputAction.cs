using Utils;
using System;
using UnityEngine;

namespace Softviz.InputAction.Desktop
{

    public interface IDesktopInputAction : IBaseInputAction
    {
        IInputEvent<float, float> Orbit { get; }
        IInputEvent<float, float> CameraLook { get; }
        IInputEvent<float> ScrollWheelZoom { get; }
        IInputEvent ScaleUp { get; }
        IInputEvent ScaleDown { get; }

        IInputEvent FocusSourceNode { get; }
        IInputEvent FocusTargetNode { get; }
        IInputEvent FocusSelected { get; }
    }

    public class DesktopInputAction : Singleton<DesktopInputAction>, IDesktopInputAction
    {
        public IInputEvent<float> CameraYaw => new InputEvent<float>(DesktopAxes.MouseX);
        public IInputEvent<float> CameraPitch => new InputEvent<float>(DesktopAxes.MouseY);

        public IInputEvent<float, float> CameraLook => new InputEvent<float, float>(DesktopAxes.MouseX, DesktopAxes.MouseY);

        public IInputEvent<float> CameraRoll { get { throw new NotImplementedException(); } }
        public IInputEvent Focus => new InputEvent(DesktopKeys.RightMouseButton);
        public IInputEvent ZoomToFit => new InputEvent((Key)KeyCode.Z);
        public IInputEvent<float> ScrollWheelZoom => new InputEvent<float>(DesktopAxes.MouseScrollWheel);

        public IInputEvent Select => new InputEvent(DesktopKeys.LeftMouseButton);
        public IInputEvent MultiSelect => new InputEvent((Key)KeyCode.LeftControl, DesktopKeys.LeftMouseButton);
        public IInputEvent ToggleAllLabels => new InputEvent((Key)KeyCode.O);

        public IInputEvent<float> MoveHorizontal => new InputEvent<float>(DesktopAxes.MovementHorizontal);
        public IInputEvent<float> MoveVertical => new InputEvent<float>(DesktopAxes.MovementVertical);
        public IInputEvent<float> UpDown => new InputEvent<float>(DesktopAxes.UpDown);

        public IInputEvent Menu => new InputEvent((Key)KeyCode.M);
        public IInputEvent ToggleEdgeDetection = new InputEvent((Key)KeyCode.K);
        public IInputEvent Layout => new InputEvent((Key)KeyCode.L);
        public IInputEvent Translate { get { throw new NotImplementedException(); } }
        public IInputEvent Rotate { get { throw new NotImplementedException(); } }

        public IInputEvent ScaleUp => new InputEvent(false, (Key)KeyCode.KeypadPlus);
        public IInputEvent ScaleDown => new InputEvent(false, (Key)KeyCode.KeypadMinus);

        public IInputEvent<float, float> Orbit => new InputEvent<float, float>(DesktopAxes.MouseX, DesktopAxes.MouseY, DesktopKeys.MiddleMouseButton);

        public IInputEvent FocusSourceNode => new InputEvent((Key)KeyCode.PageDown);
        public IInputEvent FocusTargetNode => new InputEvent((Key)KeyCode.PageUp);
        public IInputEvent FocusSelected => new InputEvent((Key)KeyCode.Home);
    }
}
