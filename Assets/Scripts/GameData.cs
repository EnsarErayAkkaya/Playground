using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{ 
    public PlaygroundType choosenPlayground;
    public List<RailType> playerRails;
    public List<EnvType> playerEnvs;
    public List<PlaygroundType> playerPlaygrounds;
    public List<TrainType> playerTrains;
   
    public GameData()
    {
        playerRails = new List<RailType>();
        playerEnvs = new List<EnvType>();
    }
    
}

