using System;
using Communication;
using Leap.Unity.Attributes;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.Controllers;
using UnityEngine;

public class RadiusMagnet : MonoBehaviour
{
    public int            id; 
    public Transform      minRadiusSphere;
    public Transform      maxRadiusSphere;
    public float          minRadius;
    public float          maxRadius;
    public TMPro.TMP_Text minRadiusText;
    public TMPro.TMP_Text maxRadiusText;
    public TMPro.TMP_Text magnetIdText;
    public Renderer[]     visualRenderers = new Renderer[2];
    public float          strength;
    public TMPro.TMP_Text strengthText;
    public Transform      detailsMenu;

    public void RadiusMagnetInit(MetaNodesManager manager)
    {
        transform.localScale = new Vector3(
            transform.localScale.x / transform.parent.localScale.x,
            transform.localScale.y / transform.parent.localScale.y,
            transform.localScale.z / transform.parent.localScale.z
        );

        detailsMenu.parent = manager.transform;

        id = manager.NewMagnet();

        magnetIdText.text = "Magnet ID: " + id;

        minRadius = .5f;
        maxRadius = 1f;

        API_out.CreateMetaNode(
            1,
            id,
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z,
            // type
            1,
            // strength
            0,
            // min radius
            0, 
            // max radius
            0
        );

        minRadiusSphere.localScale = new Vector3(minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4);
        maxRadiusSphere.localScale = new Vector3(maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4);
        SetColor(false);
    }

    public void UIMinSliderChange(SliderEventData data)
    {
        minRadius = data.NewValue * 4;
        minRadiusText.text = String.Format("{0:0.00}", minRadius); 
        minRadiusSphere.localScale = new Vector3(minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4);
    }

    public void UIMaxSliderChange(SliderEventData data)
    {
        maxRadius = data.NewValue * 4;
        maxRadiusText.text = String.Format("{0:0.00}", maxRadius);
        maxRadiusSphere.localScale = new Vector3(maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4);
    }

    public void UIStrengthChange(SliderEventData data)
    {
        strength = data.NewValue * 100f;
        strengthText.text = String.Format("{0:0.00}", strength);
    }

    public void UpdateMagnet() 
    {
        API_out.UpdateMetaNode(
            1,
            id,
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z,
            // type
            1,
            // strength
            strength,
            // min radius
            minRadius * transform.localScale.x,
            // max radius
            maxRadius * transform.localScale.x
        );
    }   

    public void SetColor(bool active)
    {
        foreach (var renderer in visualRenderers)
        {
            renderer.material.color = active ? Color.red : Color.white;
        }
    }
} 