using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class UIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void ClickImportData()
    {
       
           
        

 
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Open"))
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            openFileName.title = "窗口标题";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

            if (LocalDialog.GetSaveFileName(openFileName))
            {
                Debug.Log(openFileName.file);
                Debug.Log(openFileName.fileTitle);
            }
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
