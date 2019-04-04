using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataIO;

public class DataVisualizor : MonoBehaviour {

    // Use this for initialization
    public GameObject dataPoint;


    void Start () {
       //DrawData();

    }

    List<GameObject> dataobjects = new List<GameObject>();

    Vector3[] dataPositions;

    float[] xvalues;
    float[] yvalues;
    float[] zvalues;


    public void LoadData(string xcolname, string ycolname, string zcolname, DataExtractor data)
    {
        dataPositions = new Vector3[data.dataLength];
        xvalues = new float[data.dataLength];
        yvalues = new float[data.dataLength];
        zvalues = new float[data.dataLength];
        float[][] threeAxies = { xvalues, yvalues, zvalues };


        string[] colnames = { xcolname, ycolname, zcolname };

        for (int i = 0; i < colnames.Length; i++)
        {
            string colname = colnames[i];
            if (data.GetDataType(colname) == typeof(string))
            {
                List<string> strs = data.GetStrColumn(colname);
                //TODO for axis setting the right string value to it
                List<string> strValue = new List<string>();
                ///list of string method indexof return the first value equal to the string
                /// so cannot use indexof to indentify which index it is.
                for (int j = 0; j < strs.Count; j++)
                {
                    string str = strs[j];
                    if (!strValue.Contains(str))
                    {strValue.Add(str);
                    }
                    threeAxies[i][j] = strValue.IndexOf(str);
                }

            }
            else
            {
                List<float> datas = data.GetColumn(colname);
                for (int j = 0; j < datas.Count; j++)
                {
                    threeAxies[i][j] = 1;
                    threeAxies[i][j] = datas[j];
                }
            }
        }
        for (int i = 0; i < data.dataLength; i++)
        {
            dataPositions[i] = new Vector3(xvalues[i], yvalues[i], zvalues[i]);
        }

        foreach (var vector3 in dataPositions)
        {
            dataobjects.Add(Instantiate(dataPoint, vector3, dataPoint.transform.rotation));
        }
    }

    void DrawData()
    {

    }

    // Update is called once per frame
    void Update () {
        //DrawData();

    }
}
