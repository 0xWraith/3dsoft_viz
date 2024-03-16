using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Softviz.Controllers;
using Softviz.MetaNodes.Magnets;

public class XRMagnet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect() {
        XRMagnetController.Instance.ConnectMagnetWithNodes();
    }
}
