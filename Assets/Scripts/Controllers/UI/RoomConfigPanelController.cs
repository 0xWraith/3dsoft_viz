using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using Utils;

namespace Softviz.Controllers.UI
{
    public class RoomConfigPanelController : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        private Button toggleWalls;
        [SerializeField]
        private Button toggleFurniture;
        [SerializeField]
        private Button toggleFloor;
        [SerializeField]
        private Dropdown backgroundPicker;
        [SerializeField]
        private Dropdown tablePicker;

        [SerializeField]
        private Dropdown templatePicker;

        [Header("Game Objects")]
        public GameObject walls;
        public GameObject floor;
        public GameObject officeTable;
        public GameObject roundTable;
        public GameObject rectangleTable;
        public GameObject meetingTable;
        public GameObject furiture;
        public Camera mainCamera;

        [Header("Floor/Background Materials")]
        public Material naturalBackgroundMat;
        public Material whiteBackgroundMat;
        public Material blackBackgroundMat;
        public Material floorMaterial;

        private GameObject[] tables;
        private enum TableTypes
        {
            [Description("")]
            None,
            [Description("Office Table")]
            officeTable,
            [Description("Round Table")]
            roundTable,
            [Description("Rectangle Table")]
            rectangleTable,
            [Description("Meeting Table")]
            meetingTable
        }
        private TableTypes activeTable;
        private enum BackgroundTypes
        {
            [Description("Natural Background")]
            naturalBackground,
            [Description("White Background")]
            whiteBackground,
            [Description("Black Background")]
            blackBackground
        }
        private BackgroundTypes activeBackground;
        private enum Templates
        {
            [Description("")]
            None,
            [Description("Empty Room")]
            emptyRoom,
            [Description("Default Room")]
            defaultRoom,
            [Description("Office Room")]
            officeRoom,
            [Description("Meeting Room")]
            meetingRoom,
            [Description("Dark Room")]
            darkRoom,
            [Description("Natural Room")]
            naturalRoom,
            [Description("Dark Office Room")]
            darkOfficeRoom,
            [Description("Natural Office Room")]
            naturalOfficeRoom,
            [Description("Dark Meeting Room")]
            darkMeetingRoom,
            [Description("Natural Meeting Room")]
            naturalMeetingRoom
        }
        private Templates activeTemplate;
        private bool template = true;
        private void Awake()
        {
            EnumToDropDown.Populate(tablePicker, activeTable);
            EnumToDropDown.Populate(backgroundPicker, activeBackground);
            EnumToDropDown.Populate(templatePicker, activeTemplate);

            tablePicker.onValueChanged.AddListener(delegate { DropdownValueChanged(tablePicker); });
            backgroundPicker.onValueChanged.AddListener(delegate { DropdownValueChanged(backgroundPicker); });
            templatePicker.onValueChanged.AddListener(delegate { DropdownValueChanged(templatePicker); });

            toggleFurniture.onClick.AddListener(delegate { ButtonWasToggled(toggleFurniture); });
            toggleWalls.onClick.AddListener(delegate { ButtonWasToggled(toggleWalls); });
            toggleFloor.onClick.AddListener(delegate { ButtonWasToggled(toggleFloor); });
        }
        // Start is called before the first frame update
        void Start()
        {
            mainCamera.clearFlags = CameraClearFlags.Skybox;
            tables = new GameObject[] { officeTable, roundTable, rectangleTable, meetingTable };
            templatePicker.value = ((int)Templates.defaultRoom);
        }
        private void DropdownValueChanged(Dropdown dropdown)
        {
            if (dropdown == backgroundPicker)
            {
                BackgroundPickerValueChanged(dropdown);
            }

            if (dropdown == tablePicker)
            {
                if ((TableTypes)dropdown.value == TableTypes.None)
                {
                    tablePicker.SetValueWithoutNotify((int)activeTable);
                    return;
                }
                TablePickerValueChanged(dropdown);
            }

            if (dropdown == templatePicker)
            {
                if ((Templates)dropdown.value == Templates.None)
                {
                    templatePicker.SetValueWithoutNotify((int)activeTemplate);
                    return;
                }
                TemplatePickerValueChanged(dropdown);
            }
        }
        private void BackgroundPickerValueChanged(Dropdown dropdown)
        {

            activeBackground = (BackgroundTypes)dropdown.value;

            if (!template)
            {
                templatePicker.SetValueWithoutNotify((int)Templates.None);
            }

            switch (activeBackground)
            {
                case BackgroundTypes.naturalBackground:
                    RenderSettings.skybox = naturalBackgroundMat;
                    break;
                case BackgroundTypes.whiteBackground:
                    RenderSettings.skybox = whiteBackgroundMat;
                    break;
                case BackgroundTypes.blackBackground:
                    RenderSettings.skybox = blackBackgroundMat;
                    break;
            }
        }
        private void TablePickerValueChanged(Dropdown dropdown)
        {
            activeTable = (TableTypes)dropdown.value;

            if (!template)
            {
                templatePicker.SetValueWithoutNotify((int)Templates.None);
            }

            HideTables();

            switch (activeTable)
            {
                case TableTypes.officeTable:
                    officeTable.SetActive(true);
                    break;
                case TableTypes.roundTable:
                    roundTable.SetActive(true);
                    break;
                case TableTypes.rectangleTable:
                    rectangleTable.SetActive(true);
                    break;
                case TableTypes.meetingTable:
                    meetingTable.SetActive(true);
                    furiture.SetActive(false);
                    break;
            }
        }
        private void TemplatePickerValueChanged(Dropdown dropdown)
        {
            activeTemplate = (Templates)dropdown.value;

            HideAllRoomObjects();

            template = true;
            SetTemplate();
            template = false;
        }
        private void SetTemplate()
        {
            switch (activeTemplate)
            {
                case Templates.officeRoom:
                    ToggleIndoorRoomObjects();
                    furiture.SetActive(true);
                    tablePicker.value = ((int)TableTypes.officeTable);
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
                case Templates.meetingRoom:
                    ToggleIndoorRoomObjects();
                    tablePicker.value = ((int)TableTypes.meetingTable);
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
                case Templates.darkMeetingRoom:
                    tablePicker.value = ((int)TableTypes.meetingTable);
                    backgroundPicker.value = ((int)BackgroundTypes.blackBackground);
                    break;
                case Templates.darkOfficeRoom:
                    furiture.SetActive(true);
                    tablePicker.value = ((int)TableTypes.officeTable);
                    backgroundPicker.value = ((int)BackgroundTypes.blackBackground);
                    break;
                case Templates.naturalMeetingRoom:
                    tablePicker.value = ((int)TableTypes.meetingTable);
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
                case Templates.naturalOfficeRoom:
                    furiture.SetActive(true);
                    tablePicker.value = ((int)TableTypes.officeTable);
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
                case Templates.darkRoom:
                    tablePicker.value = ((int)TableTypes.roundTable);
                    backgroundPicker.value = ((int)BackgroundTypes.blackBackground);
                    break;
                case Templates.naturalRoom:
                    tablePicker.value = ((int)TableTypes.roundTable);
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
                case Templates.defaultRoom:
                    ToggleIndoorRoomObjects();
                    tablePicker.value = ((int)TableTypes.roundTable);
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
                case Templates.emptyRoom:
                    backgroundPicker.value = ((int)BackgroundTypes.naturalBackground);
                    break;
            }
        }
        private void ToggleIndoorRoomObjects()
        {
            walls.SetActive(true);
            floor.SetActive(true);
        }
        private void HideAllRoomObjects()
        {
            HideTables();
            walls.SetActive(false);
            floor.SetActive(false);
            furiture.SetActive(false);
            tablePicker.SetValueWithoutNotify((int)TableTypes.None);
        }
        private void HideTables()
        {
            foreach (GameObject table in tables)
            {
                table.SetActive(false);
            }
        }

        private void ButtonWasToggled(Button button)
        {
            templatePicker.value = ((int)Templates.None);
            if (button == toggleWalls)
            {
                walls.SetActive(!walls.activeSelf);
            }
            if (button == toggleFurniture)
            {
                furiture.SetActive(!furiture.activeSelf);
            }
            if (button == toggleFloor)
            {
                floor.SetActive(!floor.activeSelf);
            }
        }
    }
}