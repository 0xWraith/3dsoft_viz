using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Softviz.Controllers;

/// <summary>
/// Class <c>ChangeLayoutAlgorithmController</c> represent Script to 'AlgorithmLayoutPanel' object in Unity
/// That object is interface for changing layouting of graph and you can turn it off/on by pressing 'L'
/// </summary>
public class ChangeLayoutAlgorithmController : MonoBehaviour
{

    [Header("UI Components")]
    [SerializeField]
    private Dropdown algorithmPicker;
    [SerializeField]
    private Dropdown functionModePicker;
    [SerializeField]
    private Dropdown variableModePicker;
    [SerializeField]
    private Dropdown buildingModePicker;


    [SerializeField] private GameObject cittySettingsPanel;

    private enum AlgorithmLayouting
    {
        [Description("FruchtermanReingold")]
        FruchtermanReingold,
        [Description("City")]
        City,
        [Description("CityFruchtermanReingold")]
        CityFruchtermanReingold
    }

    private enum FunctionMode
    {
        [Description("Cyclomatic")]
        Cyclomatic,
        [Description("LinesOfCode")]
        LinesOfCode,
        [Description("Statements")]
        Statements,
        [Description("StatementsCube")]
        StatementsCube        
    }


    private enum VariableMode
    {
        [Description("None")]
        None,
        [Description("Typed")]
        Typed
    }


    private enum BuildingMode
    {
        [Description("RowAlgorithm")]
        RowAlgorithm,
        [Description("SpiralAlgorithm")]
        SpiralAlgorithm
    }


    private AlgorithmLayouting layoutingSelection;
    private FunctionMode functionSelection;
    private VariableMode variableSelection;
    private BuildingMode buildingSelection;

    private void Awake()
    {
        EnumToDropDown.Populate(algorithmPicker, layoutingSelection);
        algorithmPicker.onValueChanged.AddListener(delegate { DropdownValueChanged(algorithmPicker); });

        cittySettingsPanel = GameObject.FindGameObjectWithTag(GameObjectTags.CitySettingsPanel);
        cittySettingsPanel?.SetActive(false);

        EnumToDropDown.Populate(functionModePicker, functionSelection);
        functionModePicker.onValueChanged.AddListener(delegate { DropdownValueChanged(functionModePicker); });

        EnumToDropDown.Populate(variableModePicker, variableSelection);
        variableModePicker.onValueChanged.AddListener(delegate { DropdownValueChanged(variableModePicker); });

        EnumToDropDown.Populate(buildingModePicker, buildingSelection);
        buildingModePicker.onValueChanged.AddListener(delegate { DropdownValueChanged(buildingModePicker); });
    }

    /// <summary>
    /// This feature is responsible for client interaction with the application. 
    /// It will perform the necessary functionality and update the graph configuration
    /// </summary>
    /// <param name="dropdown">Represent which dropdown is changed</param>
    private void DropdownValueChanged(Dropdown dropdown)
    {
        if(dropdown == algorithmPicker)
        {
            layoutingSelection = (AlgorithmLayouting)dropdown.value; //Convert dropwdown value to enum
            switch (layoutingSelection)
            {
                case AlgorithmLayouting.FruchtermanReingold:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualLayoutAlgorithm = Enums.LayoutAlgorithm.FruchtermanReingold; //zmenit na aktualny ENUM
                    VisibilityCittySettingsPanel(false);
                    break;
                case AlgorithmLayouting.City:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualLayoutAlgorithm = Enums.LayoutAlgorithm.City; //zmenit na aktualny ENUM
                    VisibilityCittySettingsPanel(true);
                    break;
                case AlgorithmLayouting.CityFruchtermanReingold:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualLayoutAlgorithm = Enums.LayoutAlgorithm.CityFruchtermanReingold; //zmenit na aktualny ENUM
                    VisibilityCittySettingsPanel(true);
                    break;
            }
        }
        if(dropdown == functionModePicker)
        {
            functionSelection = (FunctionMode)dropdown.value;
            switch (functionSelection)
            {
                case FunctionMode.LinesOfCode:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualFunctionType = Enums.FunctionType.LinesOfCode;
                    break;
                case FunctionMode.Statements:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualFunctionType = Enums.FunctionType.Statements;
                    break;
                case FunctionMode.StatementsCube:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualFunctionType = Enums.FunctionType.StatementsCube;
                    break;
                case FunctionMode.Cyclomatic:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualFunctionType = Enums.FunctionType.Cyclomatic;
                    break;
            }
        }
        if(dropdown == variableModePicker)
        {
            variableSelection = (VariableMode)dropdown.value;
            switch (variableSelection)
            {
                case VariableMode.None:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualVariableType = Enums.VariableType.None;
                    break;
                case VariableMode.Typed:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualVariableType = Enums.VariableType.Typed;
                    break;
            }
        }
        if (dropdown == buildingModePicker)
        {
            buildingSelection = (BuildingMode)dropdown.value;
            switch (buildingSelection)
            {
                case BuildingMode.RowAlgorithm:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualBuildingLayoutAlgorithm = Enums.BuildingLayoutAlgorithm.RowAlgorithm;
                    break;
                case BuildingMode.SpiralAlgorithm:
                    GraphController.Instance.UpdateHierarchy = true;
                    GraphController.Instance.GraphConfiguration.actualBuildingLayoutAlgorithm = Enums.BuildingLayoutAlgorithm.SpiralAlgorithm;
                    break;
            }
        }
        GraphController.Instance.UpdateGraphConfiguration();
    }

    /// <summary>
    /// This function is turning on/off part of interface, where we can change optional parameters.
    /// These optional parameters are visible when we choose City or CityFruchterman layout algoritmh
    /// </summary>
    /// <param name="flag"></param>
    private void VisibilityCittySettingsPanel(bool flag)
    {
        cittySettingsPanel?.SetActive(flag);
    }

}
