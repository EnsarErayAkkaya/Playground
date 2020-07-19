using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    public List<RailType> playerRails;
    public List<EnvType> playerEnvs;
    public List<PlaygroundType> playerPlaygrounds;
    public PlaygroundType choosenPlayground;
    
    public GameData()
    {
        playerRails = new List<RailType>();
        playerEnvs = new List<EnvType>();
    }
    
}

