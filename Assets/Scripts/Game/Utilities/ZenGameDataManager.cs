using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenGameDataManager : MonoBehaviour
{
    [SerializeField] RailManager railManager;
    ///
    /// ------------- UNITY STUFF --------------------
    ///
        
    void Awake()
    {
        GameDataManager.instance.zenSceneDataManager.LoadZenSceneData();
        railManager.AddCreatedRails();
        foreach (Rail item in railManager.GetRails())
        {
            FindObjectOfType<RailManager>().ConnectCollidingRailPoints(item);
        }
    }
    
    ///
    /// -------------- ---------- ---------------------
    ///
}
