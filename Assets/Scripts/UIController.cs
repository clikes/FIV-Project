using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;



public class UIController : MonoBehaviour {

	void Start () {

    }

    public void ClickImportData()
    {
        string filePath = EditorUtility.OpenFilePanel("Find File", Application.dataPath, "*.*");




    }


    // Update is called once per frame
    void Update () {
		
	}
}
