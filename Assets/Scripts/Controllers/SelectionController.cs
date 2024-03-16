using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Utils;
using UnityEngine.EventSystems;
using Softviz.InputAction.Desktop;

namespace Softviz.Controllers
{
    public class SelectionChangedArgs : EventArgs
    {
        public GameObject GameObject { get; protected set; }

        public SelectionChangedArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }

    public class SelectionController : SingletonBase<SelectionController>
    {
        private ISet<GameObject> selectedObjects = new HashSet<GameObject>();
        private ISet<GameObject> selectedMagnets = new HashSet<GameObject>();

        public event EventHandler<SelectionChangedArgs> SelectionChanged;

        protected override void Start()
        {
            var inputHandler = InputController.Instance;
            var inputAction = DesktopInputAction.Instance;

            inputHandler.Subscribe(inputAction.Select, () =>
            {
                // When you click on a button in a menu for example, you dont want to handle selection 
                if (EventSystem.current.currentSelectedGameObject?.GetComponentInParent<Canvas>() != null)
                {
                    return;
                }

                ClearSelection();
                HandleSelection();
            });
            inputHandler.Subscribe(inputAction.MultiSelect, () => HandleSelection());
        }

        private void HandleSelection()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                return;
            }

            bool handled = false;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f))
            {
                var go = hit.collider?.gameObject;
                if (go != null)
                {
                    var selectableObject = GetSelectableObject(go);
                    if (selectableObject != null)
                    {
                        if (!selectedObjects.Any(obj => obj.GetInstanceID() == go.GetInstanceID()))
                        {
                            Select(((MonoBehaviour)selectableObject).gameObject);
                        }
                        else
                        {
                            Unselect(((MonoBehaviour)selectableObject).gameObject);
                        }

                        handled = true;
                    }
                }
            }

            if (!handled)
            {
                ClearSelection();
            }
        }

        public void Select(GameObject go)
        {
            if (go.tag == GameObjectTags.Magnet)
            {
                selectedMagnets.Add(go);
                ClearObjectsSelection();
            }
            else
            {
                selectedObjects.Add(go);
                ClearMagnetsSelection();
            }

            //GetSelectableObject(go).OnObjectSelected();
            SelectionChanged?.Invoke(this, new SelectionChangedArgs(go));
            go.GetComponent<ISelectableObject>().OnObjectSelected();
        }

        public void Unselect(GameObject go)
        {
            if (go.tag == GameObjectTags.Magnet)
            {
                selectedMagnets.Remove(go);
            }
            else
            {
                selectedObjects.Remove(go);
            }

            //GetSelectableObject(go).OnObjectDeselect();
            SelectionChanged?.Invoke(this, new SelectionChangedArgs(go));
            go.GetComponent<ISelectableObject>().OnObjectDeselect();
        }

        public void ClearSelection()
        {
            ClearObjectsSelection();
            ClearMagnetsSelection();
        }

        private void ClearObjectsSelection()
        {
            while (selectedObjects.Any())
            {
                var obj = selectedObjects.First();
                Unselect(obj);
                selectedObjects.Remove(obj);
            }
        }

        private void ClearMagnetsSelection()
        {
            while (selectedMagnets.Any())
            {
                var obj = selectedMagnets.First();
                Unselect(obj);
                selectedMagnets.Remove(obj);
            }
        }

        public ISelectableObject GetSelectableObject(GameObject gameObject)
        {
            return gameObject.GetComponentInParent(typeof(ISelectableObject)) as ISelectableObject ??
                gameObject.GetComponentInChildren(typeof(ISelectableObject)) as ISelectableObject;
        }

        /// <summary>
        /// Returns all selected gameobjects of given type
        /// </summary>
        public IEnumerable<T> GetSelectedObjects<T>()
        {
            return selectedObjects.Select(a => a.GetComponent<T>()).Where(a => a != null);
        }

        /// <summary>
        /// Returns selected object of given type. If more object of given type exsists, first is returned
        /// </summary>
        public T GetSelectedObject<T>()
        {
            return selectedObjects.Select(a => a.GetComponent<T>()).FirstOrDefault(a => a != null);
        }
    }
}