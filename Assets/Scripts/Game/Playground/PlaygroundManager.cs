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
    public Vector3 ClampPoisiton(Vector3 pos)
    {
        return new Vector3( Mathf.Clamp(pos.x, playground.minX, playground.maxX )
            , pos.y,
             Mathf.Clamp(pos.z, playground.minZ, playground.maxZ ));
    }
    public Vector3 ClampField(Vector3 pos, float minX,float maxX, float minZ,float maxZ)
    {
        return new Vector3( Mathf.Clamp(pos.x, playground.minX - minX, playground.maxX - maxX )
            , pos.y,
             Mathf.Clamp(pos.z, playground.minZ - minZ, playground.maxZ - maxZ ));
    }
}
