using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace DataIO
{

    public class FileReader : MonoBehaviour
    {

        public static readonly List<DataExtractor> Datas = new List<DataExtractor>();

        public static void ReadFile(string fileName)
        {
            Datas.Add(new DataExtractor(CSV2DataTable(fileName)));

        }

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
            bool IsFirstData = true;

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
                        //dc.DataType = typeof(double);
                        dt.Columns.Add(dc);
                        dt.Columns[0].DataType = typeof(int);
                    }
                }
                else
                {
                    ///Determine data type  
                    if (IsFirstData)
                    {
                        IsFirstData = false;
                        for (int j = 0; j < columnCount; j++)
                        {
                            if (IsInt(aryLine[j]))
                            {
                                dt.Columns[j].DataType = typeof(int);
                            }
                            else if (IsNumeric(aryLine[j]))
                            {
                                dt.Columns[j].DataType = typeof(float);
                            }
                            else
                            {
                                dt.Columns[j].DataType = typeof(string);
                            }
                        }
                    }
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

        //public static DataTable ChangeColumnType(DataTable oldtb, Type toType, string columnName)
        //{
        //    DataTable newtb = oldtb.Clone();
        //    newtb.Columns[columnName].DataType = toType;
        //    foreach (var item in collection)
        //    {
        //        Convert.
        //    }
        //    return null;
        //}

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        public static bool IsBool(string value)
        {
            return Regex.IsMatch(value, @"^[01]?");
        }

        private void Start()
        {
            List<string> strs = new List<string>();
            List<string> strs2 = new List<string>();
            string test1 = "2222";
            string test2 = "2222";
            string test3 = "2222";
            string test4 = "2222";
            strs.Add(test1);
            strs.Add(test2);
            strs.Add(test3);
            strs.Add(test4);
            strs2.Add(test1);
            //Debug.Log(strs.IndexOf(test3));
        }
    }
}