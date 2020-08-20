using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public GameObject envPrefab;
    public EnvType envType;
    public GameObject envButton;
    public GameObject envImage;
}
public enum EnvType
{
    R0,R1,OT,ST
}