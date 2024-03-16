using Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Softviz.InputAction
{
    /// <summary>
    /// A single input eg. keystroke
    /// </summary>
    public interface IInputElement
    {
        bool IsActive();
    }

    /// <summary>
    /// A single parametrized input eg. mouse movement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInputElement<T> : IInputElement
    {
        T GetValue();
    }

    /// <summary>
    /// A combination of multiple <see cref="IInputElement"/>, which have to be active simultaneously.
    /// </summary>
    public interface IInputEvent : IInputElement
    {
        IList<IInputElement> Inputs { get; }
        int GetInputCount();
    }

    /// <summary>
    /// A combination of multiple <see cref="IInputElement"/>, which have to be active simultaneously.
    /// Supports one generic parameter.
    /// </summary>
    public interface IInputEvent<T> : IInputEvent, IInputElement<T> { }

    /// <summary>
    /// A combination of multiple <see cref="IInputElement"/>, which have to be active simultaneously.
    /// Supports two generic parameters.
    /// </summary>
    public interface IInputEvent<T1, T2> : IInputEvent<T1>
    {
        T1 GetFirstValue();
        T2 GetSecondValue();
    }

    #region basic implementations

    internal class InputEvent : IInputEvent
    {
        private readonly bool isOneTime;
        private bool wasActive = false;

        internal InputEvent(params IInputElement[] inputs) : this(true, inputs) { }

        internal InputEvent(bool isOneTime, params IInputElement[] inputs)
        {
            this.isOneTime = isOneTime;
            this.Inputs = ObjectUtils.AssureNotNull(inputs);
        }

        public IList<IInputElement> Inputs { get; }

        public virtual bool IsActive()
        {
            var isActive = Inputs.All((i) => i.IsActive());

            if (!isOneTime)
            {
                return isActive;
            }

            if (!wasActive && isActive)
            {
                wasActive = isActive;
                return true;
            }
            else
            {
                wasActive = isActive;
                return false;
            }
        }

        public virtual int GetInputCount() => Inputs.Count;

        public override bool Equals(object obj)
        {
            var @event = obj as InputEvent;
            return @event != null &&
                   Inputs.Count == @event.Inputs.Count && Inputs.All(@event.Inputs.Contains);
        }

        public override int GetHashCode()
        {
            return -687309960 + EqualityComparer<IList<IInputElement>>.Default.GetHashCode(Inputs);
        }

        public override string ToString() => String.Join(", ", Inputs);
    }

    internal class InputEvent<T> : InputEvent, IInputEvent<T>
    {
        private readonly IInputElement<T> parametrizedInput;

        //internal InputEvent(IInputElement<T> parametrizedInput, params IInputElement[] inputs) : base(false, new IInputElement[] { })
        internal InputEvent(IInputElement<T> parametrizedInput, params IInputElement[] inputs) : base(false, inputs.Append(parametrizedInput))
        {
            IInputElement a = (IInputElement)parametrizedInput;

            this.parametrizedInput = ObjectUtils.AssureNotNull(parametrizedInput);
        }

        public T GetValue() => parametrizedInput.GetValue();
    }

    internal class InputEvent<T1, T2> : InputEvent<T1>, IInputEvent<T1, T2>
    {
        private readonly IInputElement<T2> secondParametrizedInput;

        internal InputEvent(IInputElement<T1> firstParametrizedInput, IInputElement<T2> secondParametrizedInput, params IInputElement[] inputs) : base(firstParametrizedInput, inputs.Append(secondParametrizedInput))
        {
            this.secondParametrizedInput = ObjectUtils.AssureNotNull(secondParametrizedInput);
        }

        public T1 GetFirstValue() => GetValue();

        public T2 GetSecondValue() => secondParametrizedInput.GetValue();
    }

    #endregion


}

public static class CollectionExtensions
{
    public static T[] Concat<T>(this T[] x, T[] y)
    {
        if (x == null) throw new ArgumentNullException("x");
        if (y == null) throw new ArgumentNullException("y");
        int oldLen = x.Length;
        Array.Resize<T>(ref x, x.Length + y.Length);
        Array.Copy(y, 0, x, oldLen, y.Length);
        return x;
    }

    public static T[] Append<T>(this T[] x, T y)
    {
        if (x == null) throw new ArgumentNullException("x");
        if (y == null) throw new ArgumentNullException("y");
        int oldLen = x.Length;
        Array.Resize<T>(ref x, x.Length + 1);
        x[oldLen] = y;
        return x;
    }
}
