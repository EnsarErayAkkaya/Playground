using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    public List<RailType> playerRails;
    
    public GameData()
    {
        playerRails = new List<RailType>();
    }
    
}

