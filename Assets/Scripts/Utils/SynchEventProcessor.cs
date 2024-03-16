using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public class SynchEventProcessor : MonoBehaviour
    {
        // Queue of functions that will be called in the current frame
        private static List<System.Action> functions = new List<System.Action>();

        void Start()
        {
            StartCoroutine(Processor());
        }

        public static void Enqueue(System.Action func)
        {
            lock (functions)
            {
                functions.Add(func);
            }
        }

        private IEnumerator Processor()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                while (functions.Count > 0)
                {
                    functions[0]();
                    lock (functions)
                    {
                        functions.RemoveAt(0);
                    }
                }
            }
        }
    }
}