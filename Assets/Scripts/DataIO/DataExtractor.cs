using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
namespace DataIO
{
    public class DataExtractor
    {

        public List<string> columnName;

        public List<Type> types;

        public DataTable dataTable;

        public int dataLength { get; private set; }
        public int columnLength { get; private set; }


        public DataExtractor(DataTable dataTable)
        {
            this.dataTable = dataTable;
            columnName = new List<string>();

            types = new List<Type>();

            foreach (DataColumn col in dataTable.Columns)
            {
                columnName.Add(col.ColumnName);
                types.Add(col.DataType);
            }

            columnLength = dataTable.Columns.Count;
            dataLength = dataTable.Rows.Count;
        }

        //public List<float> GetColumn(string columnName)
        //{
        //    List<object> col = new List<object>();
        //    if (dataTable.Columns[columnName].DataType == typeof(string))
        //    {
        //        List<string> strs = new List<string>();
        //        foreach (DataRow dr in dataTable.Rows)
        //        {
        //            if (!strs.Contains((string)dr[columnName]))
        //            {
        //                strs.Add((string)dr[columnName]);
        //            }

        //        }

        //    }
        //}
        /// <summary>
        /// Gets the list of column with int or float value.
        /// </summary>
        /// <returns>The column.</returns>
        /// <param name="columnName">Column name.</param>
        public List<float> GetColumn(string columnName)
        {
            List<object> col = new List<object>();

            if (dataTable.Columns[columnName].DataType == typeof(string))
            {
                throw new FormatException("This method cannot get column with type string.");
            }

            ///datatable use object to store the value,
            ///should convert the data later
            foreach (DataRow dr in dataTable.Rows)
            {
                col.Add(dr[columnName]);
            }

            List<float> colConverted = col.ConvertAll(Convert.ToSingle);
            return colConverted;
        }

        public List<string> GetStrColumn(string columnName)
        {
            List<object> col = new List<object>();

            if (dataTable.Columns[columnName].DataType == typeof(string))
            {
                throw new FormatException("This method can only get column with type string.");
            }

            ///datatable use object to store the value,
            ///should convert the data later
            foreach (DataRow dr in dataTable.Rows)
            {
                col.Add(dr[columnName]);
            }

            List<string> colConverted = col.ConvertAll(item => (string)item);
            return colConverted;
        }

        public Type GetDataType(string columnName)
        {
            return dataTable.Columns[columnName].DataType;
        }

    }
}