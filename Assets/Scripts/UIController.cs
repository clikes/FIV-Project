using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using DataIO;
using UnityEngine.UI;
using Cinemachine;
using static UnityEngine.UI.Dropdown;

public class UIController : MonoBehaviour {

    /// <summary>
    /// Reference to select panel's dropdowns
    /// </summary>
    public Dropdown[] Selectdps;

    public DataExtractor temp;

    public DataVisualizor dv;

    public GameObject SelectPanel;

    public GameObject MainPanel;

    public Text[] selectPanelTexts;

    public Text[] OffsetAndScaleTexts;

    public Slider[] OffsetAndScaleSliders;

    public GameObject slider;

    GameObject freecamera;

    string xcolname, ycolname, zcolname, sizename, colorname;

    string[] colnames = new string[5];



    string defaultOpt = "  -  ";

    void Start () {
        dv = GameObject.Find("DataVisualizor").GetComponent<DataVisualizor>();
        foreach (var dp in Selectdps)
        {
            dp.ClearOptions();
        }

        freecamera = GameObject.Find("CM FreeLook1");

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

        for (int i = 0; i < 6; i++)
        {
            OffsetAndScaleSliders[i].value = dv.OffsetAndScale[i];
            OffsetAndScaleSliders[i].onValueChanged.RemoveAllListeners();
            OffsetAndScaleSliders[i].onValueChanged.AddListener(delegate { OnOffsetAndSizeSliderChange(); });
            OffsetAndScaleTexts[i].text = dv.OffsetAndScale[i].ToString();
        }
        slider.SetActive(true);

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
        for (int i = 0; i < OffsetAndScaleTexts.Length; i++)
        {
            dv.OffsetAndScale[i] = OffsetAndScaleSliders[i].value;
            OffsetAndScaleTexts[i].text = dv.OffsetAndScale[i].ToString();
            //Debug.Log(dv.OffsetAndScale[i]);
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
            freecamera.SetActive(cameraMove);
        }
    }
}
