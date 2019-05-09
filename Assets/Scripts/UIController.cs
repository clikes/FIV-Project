using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using DataIO;
using UnityEngine.UI;
using Cinemachine;
using static UnityEngine.UI.Dropdown;
using TMPro;
using SFB;

public class UIController : MonoBehaviour {

    /// <summary>
    /// Reference to select panel's dropdowns
    /// </summary>
    public Dropdown[] Selectdps;

    public DataExtractor temp;

    public DataVisualizor dv;

    public DrawRectangle dr;

    public GameObject SelectPanel;

    public GameObject MainPanel;

    public TextMeshProUGUI DataCount;

    public TextMeshProUGUI[] OffsetAndScaleTexts;

    public TextMeshProUGUI[] DataPrompts;

    public Slider[] OffsetAndScaleSliders;

    public GameObject axises;

    public GameObject slider;

    public bool sliderInit = false;

    GameObject freecamera;

    string xcolname, ycolname, zcolname, sizename, colorname;

    string[] colnames = new string[5];

    string[] colnamesOrigin = new string[5];

    string defaultOpt = "  -  ";

    void Start () {
        dv = GameObject.Find("DataVisualizor").GetComponent<DataVisualizor>();
        foreach (var dp in Selectdps)
        {
            dp.ClearOptions();
        }

        freecamera = GameObject.Find("CM FreeLook1");

    }

    public void UpdateSelectDataCount()
    {
        DataCount.text = "Data Count: \n" + dv.dataobjects.Count;
    }

    public void UpdateSelectDataCount(int SelectedDataCounts)
    {
        DataCount.text = "Data Count: \n" + SelectedDataCounts + " / " + dv.dataobjects.Count;
    }

    public void UpdateDataPromptsValue(int[] indexes)
    {
        float x = 0, y = 0, z = 0;
        for (int i = 0; i < indexes.Length; i++)
        {
            x += dv.xvalues[indexes[i]];
            y += dv.yvalues[indexes[i]];
            z += dv.zvalues[indexes[i]];
        }
        x /= indexes.Length;
        y /= indexes.Length;
        z /= indexes.Length;
        DataPrompts[0].text = colnamesOrigin[0] + " AVG: \n" + x;
        DataPrompts[1].text = colnamesOrigin[1] + " AVG: \n" + y;
        DataPrompts[2].text = colnamesOrigin[2] + " AVG: \n" + z;
    }


    /// <summary>
    /// Clicks the import data button event.
    /// </summary>
    public void ClickImportData()
    {
        string[] filePath = StandaloneFileBrowser.OpenFilePanel("Import File", "", "", false);
        if (filePath[0].Length == 0) return;
        sliderInit = false;
        FileReader.ReadFile(filePath[0]);
        temp = FileReader.Datas[FileReader.Datas.Count-1];
        UpdateSelectPanel();
        //GameObject.Find("DataVisualizor").GetComponent<DataVisualizor>().LoadData("thalach", "chol", "trestbps", "age", "cp",  temp);
        MainPanel.SetActive(false);
        UpdateSelectPanel();
        SelectPanel.SetActive(true);
        

    }

    /// <summary>
    /// Check if the data can be visualize 
    /// </summary>
    /// <param name="xcolname">Xcolname.</param>
    /// <param name="ycolname">Ycolname.</param>
    /// <param name="zcolname">Zcolname.</param>
    /// <param name="sizename">Sizename.</param>
    /// <param name="colorname">Colorname.</param>
    /// <param name="data">Data.</param>
    public void OnClickSelectData()
    {
        //TODO check the data
        for (int i = 0; i < Selectdps.Length; i++)
        {
            Dropdown dp = Selectdps[i];
            DataPrompts[i].text = dp.options[dp.value].text;
            colnamesOrigin[i] = dp.options[dp.value].text;
            if (dp.options[dp.value].text == defaultOpt)
            {
                colnames[i] = "";
            }
            else
            {
                colnames[i] = dp.options[dp.value].text;
            }
        }
        dv.LoadData(colnames[0], colnames[1], colnames[2], colnames[3], colnames[4], temp);
        SelectPanel.SetActive(false);


        //sliderInit = true;
        slider.SetActive(true);
        axises.SetActive(true);
    }

    public void OnClickMainMenu()
    {
        MainPanel.SetActive(!MainPanel.activeSelf);
    }

    public void OnClickReset()
    {
        sliderInit = false;
        for (int i = 0; i < 6; i++)
        {
            dv.OffsetAndScale[i] = 1;
        }
        dv.AutoAdjustAxies();
        sliderInit = true; ;
    }

    public void OnClickExportData()
    {
        if (dr.indexes.Count == 0)
        {
            return;
        }
        string filePath = StandaloneFileBrowser.SaveFilePanel("Export File", "", "datas", "csv");
        DataExporter.ExportData(dr.indexes.ToArray(), dv, filePath);
    }

    /// <summary>
    /// Updates the select panel, each value change and initialize,
    /// when value change the list should change.
    /// </summary>
    public void UpdateSelectPanel()
    {
        List<OptionData> options = new List<OptionData>
        {
            new OptionData(defaultOpt)//default null value
        };
        for (int i = 0; i < temp.columnLength; i++)//+ " " + temp.types[i].Name
        {
            options.Add(new OptionData(temp.columnName[i]));
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

    public void OnOffsetAndSizeSliderChange()
    {
        Debug.Log("!");
        if (!sliderInit)
        {
            
            return;
        }
        Debug.Log("!!");
        for (int i = 0; i < OffsetAndScaleTexts.Length; i++)
        {
            dv.OffsetAndScale[i] = OffsetAndScaleSliders[i].value;
            OffsetAndScaleTexts[i].text = dv.OffsetAndScale[i].ToString();
           
        }
        dv.AdjustAxiesOffsetAndScale();
    }

    bool cameraMove = true;

    // Update is called once per frame
    void Update () {
        if (temp != null)
        {
            UpdateSelectPanel();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraMove = !cameraMove;
            dr.freezeData = !dr.freezeData;
            freecamera.SetActive(cameraMove);
        }
    }
}
