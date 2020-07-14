using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager: MonoBehaviour
{
    public static GameDataManager instance;
    public List<RailData> allRails;

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
        SaveAndLoadGameData.instance.savedData.playerRails.Add(t);
    }
}
