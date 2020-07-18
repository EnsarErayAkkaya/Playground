using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RailData
{
    public GameObject railPrefab;
    public RailType railType;
    public GameObject railButton;
    public GameObject railImage;
}
public enum RailType
{
    A, EL, ER, F1, G2, NUp, NDown
}
