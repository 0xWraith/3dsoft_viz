using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePositionChange : MonoBehaviour
{
    public bool changePositions = true;

    public void SetChangePositions(bool iChangePositions) {
        this.changePositions = iChangePositions;
    }
}
