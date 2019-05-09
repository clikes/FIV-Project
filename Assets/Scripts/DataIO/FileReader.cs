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
            Debug.Log(GetType(fileName));
            StreamReader sr = new StreamReader(fs, GetType(fileName));
            ///Read file line by line
            string strLine = "";
            //the record of each line 
            string[] aryLine = null;

            int columnCount = 0;
            //first line is the name of column
            bool IsFirst = true;
            bool IsFirstData = true;
            int count = 0;
            //read csv line by line
            while ((strLine = sr.ReadLine()) != null)
            {
                count++;
                //Debug.Log(++count);
                //Debug.Log(strLine);
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

            Debug.Log(count);

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

        /// <summary>
        /// Open source code from https://www.cnblogs.com/lovko/archive/2008/12/26/1363002.html
        /// </summary>
        /// <returns>The type.</returns>
        /// <param name="FILE_NAME">File name.</param>
        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        public static System.Text.Encoding GetType(FileStream fs)
        {
            /*byte[] Unicode=new byte[]{0xFF,0xFE};  
            byte[] UnicodeBIG=new byte[]{0xFE,0xFF};  
            byte[] UTF8=new byte[]{0xEF,0xBB,0xBF};*/

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            byte[] ss = r.ReadBytes(3);
            r.Close();
            //编码类型 Coding=编码类型.ASCII;   
            if (ss[0] >= 0xEF)
            {
                if (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
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
          
            //Debug.Log(strs.IndexOf(test3));
        }
    }
}