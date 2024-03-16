using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaNodesManager : MonoBehaviour
{
    public int magnetsCounter;
    public int restrictionsCounter;

    // Start is called before the first frame update
    void Start()
    {
        magnetsCounter = 0;
        restrictionsCounter = 0;
    }

    public int NewMagnet() 
    {
        magnetsCounter += 1;
        return magnetsCounter - 1;
    }

    public int NewRestriction()
    {
        restrictionsCounter += 1;
        return restrictionsCounter - 1;
    }

    public void DeleteMagnet()
    {
        magnetsCounter -= 1;
    }

    public void DeleteRestriction()
    {
        restrictionsCounter -= 1;
    }

    public void IncrementCounter()
    {

    }

    public int GetMetaNodeCounter()
    {
        return restrictionsCounter;
    }
}
