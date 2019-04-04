using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using DataIO;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class UIController : MonoBehaviour {

    /// <summary>
    /// Reference to select panel's dropdowns
    /// </summary>
    public Dropdown[] Selectdps;

    public DataExtractor temp;

    void Start () {
        foreach (var dp in Selectdps)
        {
            dp.ClearOptions();
        }
    }

    /// <summary>
    /// Clicks the import data button event.
    /// </summary>
    public void ClickImportData()
    {
        string filePath = EditorUtility.OpenFilePanel("Import File", "", "");
        if (filePath.Length == 0) return;
        FileReader.ReadFile(filePath);
        temp = FileReader.Datas[FileReader.Datas.Count-1];
        UpdateSelectPanel();
        GameObject.Find("DataVisualizor").GetComponent<DataVisualizor>().LoadData("age", "sex", "trestbps", temp);

    }

    /// <summary>
    /// Updates the select panel, each value change and initialize,
    /// when value change the list should change.
    /// </summary>
    public void UpdateSelectPanel()
    {
        List<OptionData> options = new List<OptionData>
        {
            new OptionData("  -  ")//default null value
        };
        for (int i = 0; i < temp.columnLength; i++)
        {
            options.Add(new OptionData(temp.columnName[i] + " " + temp.types[i].Name));
        }
        //List<OptionData>[] optionForEachDropDown = new List<OptionData>[Selectdps.Length];

        ///Selected options should not exist in other's options, 
        ///remove the selected options here
        for (int i = 0; i < Selectdps.Length; i++)
        {
            List<OptionData> buffer = new List<OptionData>();
            options.ForEach(opt => buffer.Add(opt));
            foreach (var dropdown in Selectdps)
            {
                if (dropdown.Equals(Selectdps[i])) continue;
                if (dropdown.value == 0) continue;
                buffer.Remove(dropdown.options[dropdown.value]);
            }

            ///clear before adding
            Selectdps[i].ClearOptions();
            Selectdps[i].AddOptions(buffer);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
