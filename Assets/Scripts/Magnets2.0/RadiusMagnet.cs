using System;
using Communication;
using Microsoft.MixedReality.Toolkit.UI;
using Softviz.Controllers;
using UnityEngine;
using static MetaNodesManager;

public class RadiusMagnet : MonoBehaviour
{
    public int id;
    public Transform minRadiusSphere;
    public Transform maxRadiusSphere;
    public float minRadius;
    public float maxRadius;
    public TMPro.TMP_Text minRadiusText;
    public TMPro.TMP_Text maxRadiusText;
    public TMPro.TMP_Text magnetIdText;
    public Renderer[] visualRenderers = new Renderer[2];
    public float strength;
    public TMPro.TMP_Text strengthText;
    public Transform detailsMenu;
    private MetaNodesManager manager;
    public InteractionTimer logger;

    public void RadiusMagnetInit(MetaNodesManager metaNodesManager, InteractionTimer logs)
    {
        transform.localScale = new Vector3(
            transform.localScale.x / transform.parent.localScale.x,
            transform.localScale.y / transform.parent.localScale.y,
            transform.localScale.z / transform.parent.localScale.z
        );

        manager = metaNodesManager;
        logger = logs;

        MetaNodeData data = manager.NewMagnet();
        id = data.id;

        detailsMenu.localPosition += new Vector3(0, 0.5f * id, 0);
        detailsMenu.parent = manager.transform;

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

        minRadiusSphere.localScale = new Vector3(minRadius, minRadius, minRadius);
        maxRadiusSphere.localScale = new Vector3(maxRadius, maxRadius, maxRadius);
        // minRadiusSphere.localScale = new Vector3(minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4);
        // maxRadiusSphere.localScale = new Vector3(maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4);
        // minRadiusSphere.localScale = new Vector3(minRadius * transform.localScale.x, minRadius * transform.localScale.x, minRadius * transform.localScale.x);
        // maxRadiusSphere.localScale = new Vector3(maxRadius * transform.localScale.x, maxRadius * transform.localScale.x, maxRadius * transform.localScale.x);
        SetColor(false);
    }

    public void UIMinSliderChange(SliderEventData data)
    {
        minRadius = data.NewValue * 4;
        minRadiusText.text = String.Format("{0:0.00}", minRadius);
        minRadiusSphere.localScale = new Vector3(minRadius, minRadius, minRadius);
        // minRadiusSphere.localScale = new Vector3(minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4, minRadius * transform.localScale.x / 4);
        // minRadiusSphere.localScale = new Vector3(minRadius * transform.localScale.x, minRadius * transform.localScale.x, minRadius * transform.localScale.x);
    }

    public void UIMaxSliderChange(SliderEventData data)
    {
        maxRadius = data.NewValue * 4;
        maxRadiusText.text = String.Format("{0:0.00}", maxRadius);
        maxRadiusSphere.localScale = new Vector3(maxRadius, maxRadius, maxRadius);
        // maxRadiusSphere.localScale = new Vector3(maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4, maxRadius * transform.localScale.x / 4);
        // maxRadiusSphere.localScale   = new Vector3(maxRadius * transform.localScale.x, maxRadius * transform.localScale.x, maxRadius * transform.localScale.x);
    }

    public void UIStrengthChange(SliderEventData data)
    {
        strength = data.NewValue * 100f;
        strengthText.text = String.Format("{0:0.00}", strength);
    }

    public void UIDestroy()
    {
        API_out.DeleteMetaNode(1, id);
        manager.DeleteMagnet();
        Destroy(gameObject);
        Destroy(detailsMenu.gameObject);
    }

    public void UIManipulationStart()
    {
        logger.StartMagnetTimer(id);
    }

    public void UIManipulationEnd()
    {
        logger.EndMagnetTimer(id);
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
            minRadiusSphere.localScale.x / 2 * transform.localScale.x,
            // minRadius * transform.localScale.x,
            // minRadius * minRadiusSphere.localScale.x,
            //minRadiusSphere.localScale.x * transform.localScale.x,
            // max radius
            maxRadiusSphere.localScale.x / 2 * transform.localScale.x
        // maxRadius * transform.localScale.x
        // maxRadius * maxRadiusSphere.localScale.x
        //maxRadiusSphere.localScale.x * transform.localScale.x
        );
    }

    public void SetColor(bool active)
    {
        foreach (var renderer in visualRenderers)
        {
            Color color = active ? Color.red : Color.white;
            renderer.material.SetColor("_Color", color);
        }
    }
}