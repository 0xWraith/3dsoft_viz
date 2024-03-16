using UnityEngine;

namespace Utils
{
    public abstract class BaseScript : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void OnDisable() { }
        protected virtual void OnEnable() { }
    }
}
