using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using DataIO;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class UIController : MonoBehaviour {

    public Dropdown[] Selectdps;

    public DataExtractor temp;

    void Start () {
        foreach (var dp in Selectdps)
        {
            dp.ClearOptions();
        }
    }

    public void ClickImportData()
    {
        string filePath = EditorUtility.OpenFilePanel("Import File", "", "csv");
        FileReader.ReadFile(filePath);
        temp = FileReader.Datas[FileReader.Datas.Count-1];
        UpdateSelectPanel();
    }

    void UpdateSelectPanel()
    {
        List<OptionData> options = new List<OptionData>();
        options.Add(new OptionData("-"));
        foreach (var item in temp.columnName)
        {
            options.Add(new OptionData(item));
        }
        foreach (var item in Selectdps)
        {
            item.AddOptions(options);
        }
        //Selectdp1.AddOptions(options);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
