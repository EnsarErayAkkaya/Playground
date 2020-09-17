using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class TrainData 
{
    public GameObject trainPrefab;
    public GameObject trainButton;
    public TrainType trainType;
    public int cost;
}  

public enum TrainType
{
    A, AA
}