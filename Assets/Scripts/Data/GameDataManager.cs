using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager: MonoBehaviour
{
    public static GameDataManager instance;
    public int currentlyPlayingLevelIndex;
    public ZenSceneDataManager zenSceneDataManager;
    public List<LevelData> levels;
    public List<RailData> allRails;
    public List<EnvironmentData> allEnvs;
    public List<PlaygroundData> allPlaygrounds;
    public List<TrainData> allTrains;

    void Awake()
    {
		if(instance != null)
		{
			Debug.LogWarning("More than one instance of DataManager found");
			return;
		}
		instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void AddNewPlayerRail(RailType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerRails.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerRails.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void AddNewPlayerEnvironment(EnvType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerEnvs.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerEnvs.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void AddNewPlayerPlayground(PlaygroundType t)
    {
        if( !SaveAndLoadGameData.instance.savedData.playerPlaygrounds.Any(s => s == t) )
        {
            SaveAndLoadGameData.instance.savedData.playerPlaygrounds.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }

    public void ChoosePlayground(PlaygroundType playgroundType)
    {
        if(SaveAndLoadGameData.instance.savedData.choosenPlayground != playgroundType )
        {
            SaveAndLoadGameData.instance.savedData.choosenPlayground = playgroundType;
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void AddNewPlayerTrain(TrainType t)
    {
        if(SaveAndLoadGameData.instance.savedData.playerTrains.Any(s => s != t))
        {
            SaveAndLoadGameData.instance.savedData.playerTrains.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
    public void SaveLevelMark(int _mark)
    {
        SaveAndLoadGameData.instance.savedData.unlockedLevels.First( s => s.levelIndex == currentlyPlayingLevelIndex).mark = _mark;
                
        SaveAndLoadGameData.instance.Save();
    }
    public void UnlockNextLevel()
    {
        if(levels.Count > currentlyPlayingLevelIndex )
        {
            if( SaveAndLoadGameData.instance.savedData.unlockedLevels.Any(s => s.levelIndex == currentlyPlayingLevelIndex+1) == false )
            {
                LevelData ld = levels[currentlyPlayingLevelIndex];
                ld.isUnlocked = true;
                SaveAndLoadGameData.instance.savedData.unlockedLevels.Add(ld);
                    
                SaveAndLoadGameData.instance.Save();
            }
        }
    }
}
