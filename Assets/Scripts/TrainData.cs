using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class TrainData 
{
    public GameObject trainGamePrefab;
    public GameObject trainUIPrefab;
    public TrainType trainType;
}  

public enum TrainType
{
    A
}