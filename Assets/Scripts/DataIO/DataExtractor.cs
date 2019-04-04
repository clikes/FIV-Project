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

        private DataTable dataTable;

        public int Length { get; private set; }


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

            Length = types.Count;
        }

        public List<int> GetIntColumn(string columnName)
        {
            List<object> col = new List<object>();

            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == columnName)
                {
                    if (column.DataType != typeof(int))
                    {
                        throw new FormatException("The column " + columnName + " is not match type Int");
                    }
                }
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                col.Add(dr[columnName]);
            }

            List<int> colConverted = col.ConvertAll(item => (int)item);

            return colConverted;
        }

        public List<float> GetFloatColumn(string columnName)
        {
            List<object> col = new List<object>();

            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == columnName)
                {
                    if (column.DataType != typeof(float))
                    {
                        throw new FormatException("The column " + columnName + " is not match type float");
                    }
                }
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                col.Add(dr[columnName]);
            }

            List<float> colConverted = col.ConvertAll(item => (float)item);

            return colConverted;
        }

        public List<string> GetStringColumn(string columnName)
        {
            List<object> col = new List<object>();

            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName == columnName)
                {
                    if (column.DataType != typeof(string))
                    {
                        throw new FormatException("The column " + columnName + " is not match type string");
                    }
                }
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                col.Add(dr[columnName]);
            }

            List<string> colConverted = col.ConvertAll(item => (string)item);

            return colConverted;
        }

    }
}