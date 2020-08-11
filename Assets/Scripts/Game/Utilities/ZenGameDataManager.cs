using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenGameDataManager : MonoBehaviour
{
    [SerializeField] RailManager railManager;        
    void Awake()
    {
        GameDataManager.instance.zenSceneDataManager.LoadZenSceneData();
        foreach (Rail item in railManager.GetRails())
        {
            railManager.ConnectCollidingRailPoints(item);
        }
    }
    

}
