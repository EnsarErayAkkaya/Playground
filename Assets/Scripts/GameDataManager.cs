using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager: MonoBehaviour
{
    public static GameDataManager instance;
    public List<RailData> allRails;
    public List<EnvironmentData> allEnvs;

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
        if(SaveAndLoadGameData.instance.savedData.playerRails.Any(s => s != t))
        {
            SaveAndLoadGameData.instance.savedData.playerRails.Add(t);
            SaveAndLoadGameData.instance.Save();
        }
    }
}
