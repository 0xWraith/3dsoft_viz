using Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Softviz.InputAction;

namespace Softviz.Controllers
{
    public class InputController : SingletonBase<InputController>
    {
        private List<IInputSubscription> subscriptions = new List<IInputSubscription>();

        private IDictionary<IInputElement, IInputSubscription> activatedInputs = new Dictionary<IInputElement, IInputSubscription>();

        protected override void Update()
        {
            foreach (var subscription in subscriptions)
            {
                bool isEventActive = false;
                if (subscription.InputEvent.IsActive())
                {
                    var isInputCombinationAlreadyActivated = subscription.InputEvent.Inputs.Any(activatedInputs.ContainsKey);

                    if (!isInputCombinationAlreadyActivated)
                    {
                        foreach (var key in subscription.InputEvent.Inputs)
                        {
                            activatedInputs[key] = subscription;
                        }

                        isEventActive = true;
                    }
                }

                subscription.ExecuteCallback(isEventActive);
            }

            activatedInputs.Clear();
        }

        public IInputSubscription Subscribe(IInputEvent inputEvent, Action activeCallback, Action inactiveCallback = null) => InternalSubscribe(new InputSubscription(inputEvent, activeCallback, inactiveCallback));

        public IInputSubscription Subscribe<T>(IInputEvent<T> inputEvent, Action<T> callback, Action inactiveCallback = null) => InternalSubscribe(new InputSubscription(inputEvent, () => callback.Invoke(inputEvent.GetValue()), inactiveCallback));

        public IInputSubscription Subscribe<T1, T2>(IInputEvent<T1, T2> inputEvent, Action<T1, T2> callback, Action inactiveCallback = null) => InternalSubscribe(new InputSubscription(inputEvent, () => callback.Invoke(inputEvent.GetFirstValue(), inputEvent.GetSecondValue()), inactiveCallback));

        public bool Unsubscribe(IInputSubscription subscription)
        {
            var wasSuccessful = subscriptions.Remove(subscription);

            if (wasSuccessful)
            {
                SortSubscriptions();
            }

            return wasSuccessful;
        }

        private IInputSubscription InternalSubscribe(IInputSubscription subscription)
        {
            var isInputAlreadyTaken = subscriptions.Any((s) => s.InputEvent.Equals(subscription.InputEvent));

            if (isInputAlreadyTaken)
            {
                throw new Exception("Already subscribed to input [" + subscription.InputEvent + "]");
            }

            subscriptions.Add(subscription);
            SortSubscriptions();

            return subscription;
        }

        /// <summary>
        /// Sorts subscriptions by input count in descending order
        /// </summary>
        private void SortSubscriptions()
        {
            subscriptions.Sort(Comparer<IInputSubscription>.Create((a, b) => b.InputEvent.GetInputCount().CompareTo(a.InputEvent.GetInputCount())));
        }
    }
}
