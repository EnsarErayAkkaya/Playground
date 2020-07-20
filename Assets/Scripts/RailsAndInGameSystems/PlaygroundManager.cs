using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaygroundManager : MonoBehaviour
{
    void Start()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        PlaygroundType t = SaveAndLoadGameData.instance.savedData.choosenPlayground;
        GameObject g =  Instantiate(GameDataManager.instance.allPlaygrounds.First(s => s.playgroundType == t).playgroundGamePrefab);
        g.transform.parent = this.transform;
    }
}
