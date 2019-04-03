﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

public class FileReader :MonoBehaviour {


    /// <summary>
    /// Import csv file from filesystem
    /// </summary>
    /// <returns>The 2 data table.</returns>
    /// <param name="fileName">File name.</param>
    public static DataTable CSV2DataTable(string fileName)
    {
        DataTable dt = new DataTable();
        FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        StreamReader sr = new StreamReader(fs, new System.Text.UnicodeEncoding());
        ///Read file line by line
        string strLine = "";
        //the record of each line 
        string[] aryLine = null;

        int columnCount = 0;
        //first line is the name of column
        bool IsFirst = true;

        //read csv line by line
        while ((strLine = sr.ReadLine()) != null)
        {
            aryLine = strLine.Split(',');
            if (IsFirst == true)
            {
                IsFirst = false;
                columnCount = aryLine.Length;
                //create column
                for (int i = 0; i < columnCount; i++)
                {
                    DataColumn dc = new DataColumn(aryLine[i]);
                    dc.DataType = typeof(float);
                    dt.Columns.Add(dc);
                }
            }
            else
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < columnCount; j++)
                {
                    dr[j] = aryLine[j];
                }
                dt.Rows.Add(dr);
            }
        }


        //foreach (DataRow datarow in dt.Rows)
        //{
        //    for (int i = 0; i < columnCount; i++)
        //    {
        //        if (IsInt(datarow[i]))
        //        {

        //        }
        //    }
        //}

        sr.Close();
        fs.Close();
        return dt;
    }

    public static bool IsNumeric(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?/d*[.]?/d*$");
    }

    public static bool IsInt(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?/d*$");
    }

    public static bool IsBool(string value)
    {
        return Regex.IsMatch(value, @"^[01]?");
    }

    private void Start()
    {
        //int test = 1;
        DataTable dt = FileReader.CSV2DataTable("/Users/like/Documents/git/FIV Project/heart.csv");
        Debug.Log(dt.Rows[0][3]));
    }
}
