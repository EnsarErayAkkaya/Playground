using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveAndLoadGameData : MonoBehaviour
{
    public static SaveAndLoadGameData instance;
    public GameData savedData = new GameData();
    void Awake()
    {
		if(instance != null)
		{
			Debug.LogWarning("More than one instance of Saver found");
			return;
		}
		instance = this;
        Load();
        DontDestroyOnLoad(this.gameObject);
    }
                
    //it's static so we can call it from anywhere
    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create (Application.persistentDataPath + "/gameData.gd"); //you can call it anything you want
        bf.Serialize(file, savedData);
        Debug.Log("Game saved");
        file.Close();
    }   
     
    public void Load() {
        if(File.Exists(Application.persistentDataPath + "/gameData.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.gd", FileMode.Open);
            savedData = (GameData)bf.Deserialize(file);
            file.Close();
        }
    }
}
