using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaygroundData 
{
    public GameObject playgroundGamePrefab;
    public GameObject playgroundUIPrefab;
    public PlaygroundType playgroundType;
}
public enum PlaygroundType
{
    PuzzleCarpet,CarpetDull
}

