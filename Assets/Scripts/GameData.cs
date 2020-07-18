using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    public List<RailType> playerRails;
    public List<EnvironmentData> playerEnvs;
    
    public GameData()
    {
        playerRails = new List<RailType>();
        playerEnvs = new List<EnvironmentData>();
    }
    
}

