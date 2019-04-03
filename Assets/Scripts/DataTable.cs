using System;
using System.Collections.Generic;

public enum DataType
{
    NULL,
    INT,
    DOUBLE,
    STRING
}

public class FIVDataTable {
    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    /// <value>The name of the table.</value>
    public string TableName { get; private set; }
    /// <summary>
    /// Gets the columns.
    /// </summary>
    /// <value>The columns.</value>
    public List<FIVDataColumn> Columns { get; private set; }
    /// <summary>
    /// Gets the count of columns.
    /// </summary>
    /// <value>Tthe count of columns.</value>
    public int Count { get; private set; }

    public FIVDataTable()
    {
        TableName = null;
        Columns = new List<FIVDataColumn>();
    }

    /// <summary>
    /// Adds the column with the type of this columns
    /// </summary>
    /// <param name="title">Title.</param>
    public void AddColumn(string title)
    {
        Columns.Add(new FIVDataColumn(title));
        Count = Columns.Count;
    }

}
