using System;
using System.Collections;
using System.Collections.Generic;

public class FIVDataColumn
{
    /// <summary>
    /// Title of column
    /// </summary>
    public readonly string Title;
    /// <summary>
    /// Gets or sets the type of the data, after the read of file, 
    /// filereader will automately set the type of data, user can change it after.
    /// </summary>
    /// <value>The type of the data.</value>
    public DataType Type { get; set; }

    /// <summary>
    /// Data collections, using object for generic data storage
    /// </summary>
    /// <value>The data.</value>
    public List<object> Data { get; private set; }
    public int Length { get; private set; }

    public FIVDataColumn(string Title)
    {
        this.Title = Title;
        Data = new List<object>();
    }

    /// <summary>
    /// Adds the data.
    /// </summary>
    /// <param name="data">Data.</param>
    public void AddData(object data)
    {
        Data.Add(data);
        Length = Data.Count;
    }



}

