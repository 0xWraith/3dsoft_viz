using Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Softviz.InputAction.Desktop
{
    internal class Key : IInputElement
    {
        private readonly KeyCode keyCode;

        internal Key(KeyCode keyCode)
        {
            this.keyCode = keyCode;
        }

        public static implicit operator Key(KeyCode keyCode) => new Key(keyCode);

        public bool IsActive() => Input.GetKey(keyCode);

        public override bool Equals(object obj)
        {
            var wrapper = obj as Key;
            return wrapper != null &&
                   keyCode == wrapper.keyCode;
        }

        public override int GetHashCode() => 301885969 + keyCode.GetHashCode();

        public override string ToString() => keyCode.ToString();
    }

    internal class Axis : IInputElement<float>
    {
        private readonly string axis;

        internal Axis(string axis)
        {
            this.axis = AssureAxisIsValid(ObjectUtils.AssureNotNull(axis));
        }

        public static implicit operator Axis(string axis) => new Axis(axis);

        public bool IsActive() => true;

        public float GetValue() => Input.GetAxis(axis);

        private static string AssureAxisIsValid(string axis)
        {
            // Input.GetAxis will throw if axis is not valid
            Input.GetAxis(axis);
            return axis;
        }

        public override bool Equals(object obj)
        {
            var wrapper = obj as Axis;
            return wrapper != null &&
                   axis == wrapper.axis;
        }

        public override int GetHashCode() => 46456852 + EqualityComparer<string>.Default.GetHashCode(axis);

        public override string ToString() => axis.ToString();
    }
}
