using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataIO;
using System;

public class DataVisualizor : MonoBehaviour {

    // Use this for initialization
    public GameObject dataPoint;
    GameObject Parent;



    public bool IsShowPoint = true;

    

    List<GameObject> dataobjects = new List<GameObject>();

    Vector3[] dataPositions;

    Color[] dataColor;

    float[] xvalues, yvalues, zvalues, sizevalues, colorvalues;
    string[] colnames;

    public void LoadData(string xcolname, string ycolname, string zcolname, string sizename, string colorname, DataExtractor data)
    {
        dataPositions = new Vector3[data.dataLength];

        xvalues = new float[data.dataLength];
        yvalues = new float[data.dataLength];
        zvalues = new float[data.dataLength];
        sizevalues = new float[data.dataLength];
        colorvalues = new float[data.dataLength];

        float[][] values = { xvalues, yvalues, zvalues, sizevalues, colorvalues };


        string[] buffer = { xcolname, ycolname, zcolname, sizename, colorname };
        colnames = buffer;

        ///read data into x,y,z values;
        for (int i = 0; i < colnames.Length; i++)
        {
            string colname = colnames[i];
            ///jump to next if the value is empty string
            if (colname.Length <= 0)
            {
                //List<float> datas = data.GetColumn(colname);
                for (int j = 0; j < data.dataLength; j++)
                {
                    //values[i][j] = 1;
                    values[i][j] = 0;
                }
            }
            else if (data.GetDataType(colname) == typeof(string))
            {
                List<string> strs = data.GetStrColumn(colname);
                //TODO for axis setting the right string value to it
                List<string> strValue = new List<string>();
                ///List<string>'s method 'indexof' return the first value equal to the string
                /// so cannot use indexof to indentify which index it is.
                for (int j = 0; j < strs.Count; j++)
                {
                    string str = strs[j];
                    if (!strValue.Contains(str))
                    {
                        strValue.Add(str);
                    }
                    values[i][j] = strValue.IndexOf(str);
                }

            }
            else
            {
                List<float> datas = data.GetColumn(colname);
                Debug.Log(colname);
                for (int j = 0; j < datas.Count; j++)
                {
                    //values[i][j] = 1;
                    values[i][j] = datas[j];
                }
            }
        }
        PrepareData();
    }

    void PrepareData()
    {
        dataPositions = new Vector3[dataPositions.Length];
        dataColor = new Color[dataPositions.Length];
        dataobjects.Clear();

        for (int i = 0; i < dataPositions.Length; i++)
        {
            dataPositions[i] = new Vector3(xvalues[i], yvalues[i], zvalues[i]);



            dataobjects.Add(Instantiate(dataPoint, dataPositions[i], dataPoint.transform.rotation, Parent.transform));

            if (colnames[4].Length == 0)
            {
                dataobjects[i].GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                dataobjects[i].GetComponent<Renderer>().material.color = new Color(1, 0, 50 / colorvalues[i]);
            }

            if (colnames[3].Length != 0)
            {
                dataobjects[i].transform.localScale = new Vector3(sizevalues[i], sizevalues[i], sizevalues[i]);
            }
        }

    }

    public void AdjustAxiesOffset(float x, float y, float z)
    {

        for (int i = 0; i < dataPositions.Length; i++)
        {
            dataPositions[i].x += x;
            dataPositions[i].y += y;
            dataPositions[i].z += z;
            dataobjects[i].transform.position = dataPositions[i];
        }
    }

    void DrawData()
    {
        foreach (var vector3 in dataPositions)
        {
            dataobjects.Add(Instantiate(dataPoint, vector3, dataPoint.transform.rotation));
        }
    }

    public void SetPointVisable(bool isvisable)
    {
        Parent.SetActive(isvisable);
    }

    void tester()
    {
        float x, y, z,r = 5;
        float w = 100;
        float h = 100;
            
           // for (r = 5; r > 0; r -= 0.2f)
           // {
                for (int i = 0; i <= 360; i++)
                {
                    float angle = Mathf.PI * i / 180;
                    x = r * (2 * Mathf.Sin(angle) + Mathf.Sin(2 * angle));
                    y = r * (2 * Mathf.Cos(angle) + Mathf.Cos(2 * angle));
                    dataobjects.Add(Instantiate(dataPoint, new Vector3(x, y, 0), dataPoint.transform.rotation));

                }
            //}
        
       
       
    }

    void Start()
    {
        Parent = GameObject.Find("Points");
        //DrawData();
        //for (int i = 0; i < 100000; i++)
        //{
        //    dataobjects.Add(Instantiate(dataPoint, new Vector3(Random.Range(1, 100), Random.Range(1, 100), Random.Range(1, 100)), dataPoint.transform.rotation));
        //}
        //tester();
    }

    // Update is called once per frame
    void Update () {

    }
}
