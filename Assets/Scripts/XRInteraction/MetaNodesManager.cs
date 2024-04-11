using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MetaNodesManager : MonoBehaviour
{
    public int magnetsCounter;
    public int restrictionsCounter;
    public Color[] colors; 


    public struct MetaNodeData
    {
        public int   id;
        public Color color;

        public MetaNodeData(int id, Color color)
        {
            this.id    = id;
            this.color = color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        magnetsCounter = 0;
        restrictionsCounter = 0;
        colors = new Color[5]{Color.blue, Color.cyan, Color.green, Color.magenta, Color.yellow};
    }

    public MetaNodeData NewMagnet() 
    {
        magnetsCounter += 1;

        int color_id = magnetsCounter > 5 ? Random.Range(0, 5) : magnetsCounter - 1;  

        return new MetaNodeData(magnetsCounter - 1, colors[color_id]);
    }

    public MetaNodeData NewRestriction()
    {
        restrictionsCounter += 1;

        int color_id = restrictionsCounter > 5 ? Random.Range(0, 5) : 4 - (restrictionsCounter - 1);

        return new MetaNodeData(restrictionsCounter - 1, colors[color_id]);
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
