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
        if(savedData.playerRails.Count < 1)
        {
            savedData.playerRails.AddRange( new List<RailType>{ RailType.A, RailType.EL, RailType.ER, RailType.F1, RailType.A1, RailType.A2
                , RailType.A3, RailType.G1, RailType.G2, RailType.NUp, RailType.NDown});
            savedData.playerEnvs.AddRange( new List<EnvType>{EnvType.R0, EnvType.R1, EnvType.ST, EnvType.OT});
            savedData.playerTrains.Add( TrainType.A );
            savedData.choosenPlayground = PlaygroundType.PuzzleCarpet;
            savedData.unlockedLevels.Add(new LevelData(){
                levelIndex = 1,
                levelSceneIndex = 2,
                isUnlocked = true,
                mark = 0
            });
            savedData.playerPlaygrounds.AddRange( new List<PlaygroundType>{PlaygroundType.PuzzleCarpet, PlaygroundType.CarpetDull});
            Save();
        }
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
