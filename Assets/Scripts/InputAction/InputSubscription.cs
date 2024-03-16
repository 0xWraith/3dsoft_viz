using Utils;
using System;

namespace Softviz.InputAction
{
    public interface IInputSubscription
    {
        IInputEvent InputEvent { get; }
        void ExecuteCallback(bool isInputActive);
        int GetInputCount();
    }

    internal class InputSubscription : IInputSubscription
    {
        private readonly Action activeCallback;
        private readonly Action inactiveCallback;

        internal InputSubscription(IInputEvent inputEvent, Action activeCallback, Action inactiveCallback)
        {
            this.InputEvent = ObjectUtils.AssureNotNull(inputEvent);
            this.activeCallback = ObjectUtils.AssureNotNull(activeCallback);
            this.inactiveCallback = inactiveCallback;
        }

        public IInputEvent InputEvent { get; }

        public void ExecuteCallback(bool isInputActive)
        {
            if (isInputActive)
            {
                activeCallback.Invoke();
            }
            else
            {
                inactiveCallback?.Invoke();
            }
        }

        public int GetInputCount() => InputEvent.GetInputCount();
    }
}
