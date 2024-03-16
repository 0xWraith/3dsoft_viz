using System.Collections.Generic;
using UnityEngine;

public class MultipleNodeSelection : MonoBehaviour
{
    public List<NodeXR> nodes = new List<NodeXR>();

    void FixedUpdate() 
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, 1 << 3);

        foreach (Collider hit in hits)
        {
            NodeXR node = hit.GetComponentInParent<NodeXR>();
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
                node.SelectedToggle();
            }
        }
    }

    public void ResetSelector() 
    {
        nodes.Clear();
    }
}