using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaygroundManager : MonoBehaviour
{
    public PlayGround playground;
    void Start()
    {
        PlayGround pl = FindObjectOfType<PlayGround>();
        if(pl == null)
        {
             foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            PlaygroundType t = SaveAndLoadGameData.instance.savedData.choosenPlayground;
            playground =  Instantiate(GameDataManager.instance.allPlaygrounds.First(s => s.playgroundType == t).playgroundGamePrefab).GetComponent<PlayGround>();
            playground.transform.parent = this.transform;
        }
        else
        {
            playground = pl;
        }
       
    }
}
