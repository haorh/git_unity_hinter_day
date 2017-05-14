using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;

public class SaveAndLoad : MonoBehaviour {

    public static SaveAndLoad instance;

    public delegate void SaveDelegate(object sender, EventArgs args);
    public delegate void LoadDelegate(object sender, EventArgs args);
    public static event SaveDelegate SaveEvent;
    public static event LoadDelegate LoadEvent;

    // Use this for initialization
    void Start () {
        instance = this;     
    }	

	// Update is called once per frame
	void Update () {
        
	}

    public void FireSaveEvent()
    {
        
    }

    public void FireLoadEvent()
    {
    }

    public void SaveData(string fileName, object list)
    {
        if (!Directory.Exists("SaveFile"))
            Directory.CreateDirectory("SaveFile");

        if(SaveEvent != null)
            SaveEvent(this, EventArgs.Empty);
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream SaveObjects = File.Create("SaveFile/" + fileName + ".binary");

        formatter.Serialize(SaveObjects, list);

        SaveObjects.Close();
    }

    public void LoadData(string fileName,object list)
    {
        BinaryFormatter formatter = new BinaryFormatter();        
        FileStream saveObjects = File.Open("SaveFile/" + fileName + ".binary", FileMode.Open);

        list = (object)formatter.Deserialize(saveObjects);

        if(LoadEvent != null)
            LoadEvent(list, null);        

        saveObjects.Close();
    }
}
