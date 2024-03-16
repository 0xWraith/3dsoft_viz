using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Utils
{
    class EnumToDropDown
    {
        public static void Populate(Dropdown dropdown, Enum targetEnum)//You can populate any dropdown with any enum with this method
        {
            List<Dropdown.OptionData> newOptions = new List<Dropdown.OptionData>();

            foreach (Enum enumValue in Enum.GetValues(targetEnum.GetType()))//Populate new Options
            {
                newOptions.Add(new Dropdown.OptionData(enumValue.DisplayName()));
            }

            dropdown.ClearOptions();//Clear old options
            dropdown.AddOptions(newOptions);//Add new options
        }

    }
}