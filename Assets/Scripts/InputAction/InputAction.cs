namespace Softviz.InputAction
{
    public interface IBaseInputAction : IBaseMovementInputAction, IBaseCameraInputAction, IBaseGraphInputAction, IBaseGuiInputAction { }

    public interface IBaseMovementInputAction
    {
        IInputEvent<float> MoveHorizontal { get; }
        IInputEvent<float> MoveVertical { get; }
        IInputEvent<float> UpDown { get; }
    }

    public interface IBaseCameraInputAction
    {
        IInputEvent<float> CameraYaw { get; }
        IInputEvent<float> CameraPitch { get; }
        IInputEvent<float> CameraRoll { get; }

        IInputEvent Focus { get; }
        IInputEvent ZoomToFit { get; }
    }

    public interface IBaseGraphInputAction
    {
        IInputEvent Select { get; }
        IInputEvent MultiSelect { get; }

        IInputEvent ToggleAllLabels { get; }
        IInputEvent Translate { get; }
        IInputEvent Rotate { get; }
    }

    public interface IBaseGuiInputAction
    {
        IInputEvent Menu { get; }
    }
}
