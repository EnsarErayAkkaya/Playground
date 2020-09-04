using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RailData
{
    public GameObject railPrefab;
    public RailType railType;
    public GameObject railButton;
    public int cost;
}
public enum RailType
{
    A, A2, A3, EL, ER, EEL, EER, F1, G2, H, L, NUp, NDown, P, T, G1
}
