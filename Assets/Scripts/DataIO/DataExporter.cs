using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class DataExporter
{
    // Start is called before the first frame update

    public static void ExportData(int[] indexes, DataVisualizor dv, string filepath)
    {
        DataTable exportTable = new DataTable();
        exportTable = dv.de.dataTable.Clone();
        exportTable.Clone();
        exportTable.Clear();
        foreach (var item in indexes)
        {
            exportTable.ImportRow(dv.de.dataTable.Rows[item]);

        }
        ExportCSV(exportTable, filepath);
    }

    #region public static void ExportCSV(DataTable dataTable, string fileName) 导出CSV格式文件
    /// <summary>
    /// 导出CSV格式文件
    /// </summary>
    /// <param name="dataTable">数据表</param>
    /// <param name="fileName">文件名</param>
    public static void ExportCSV(DataTable dataTable, string fileName)
    {
        StreamWriter streamWriter = new StreamWriter(fileName, false, System.Text.Encoding.GetEncoding("gb2312"));
        streamWriter.WriteLine(GetCSVFormatData(dataTable).ToString());
        streamWriter.Flush();
        streamWriter.Close();
    }
    #endregion

    #region public static StringBuilder GetCSVFormatData(DataTable dataTable) 通过DataTable获得CSV格式数据
    /// <summary>
    /// 通过DataTable获得CSV格式数据
    /// </summary>
    /// <param name="dataTable">数据表</param>
    /// <returns>CSV字符串数据</returns>
    public static StringBuilder GetCSVFormatData(DataTable dataTable)
    {
        StringBuilder StringBuilder = new StringBuilder();
        // 写出表头
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            StringBuilder.Append(dataTable.Columns[i]);
            if (i != dataTable.Columns.Count-1)
            {
                StringBuilder.Append(",");
            }
        }
        StringBuilder.Append("\n");
        // 写出数据
        int count = 0;
        foreach (DataRowView dataRowView in dataTable.DefaultView)
        {
            count++;
            foreach (DataColumn DataColumn in dataTable.Columns)
            {
                string field = dataRowView[DataColumn.ColumnName].ToString();

                if (field.IndexOf('"') >= 0)
                {
                    field = field.Replace("\"", "\"\"");
                }
                field = field.Replace("  ", " ");
                if (field.IndexOf(',') >= 0 || field.IndexOf('"') >= 0 || field.IndexOf('<') >= 0 || field.IndexOf('>') >= 0 || field.IndexOf("'") >= 0)
                {
                    field = "\"" + field + "\"";
                }
                StringBuilder.Append(field + ",");
                field = string.Empty;
            }
            if (count != dataTable.Rows.Count)
            {
                StringBuilder.Append("\n");
            }
        }
        return StringBuilder;
    }
    #endregion


}
