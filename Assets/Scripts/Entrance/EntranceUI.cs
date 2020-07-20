using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceUI : MonoBehaviour
{
    public Transform myContent;
    void Start()
    {
        foreach (RailType type in SaveAndLoadGameData.instance.savedData.playerRails)
        {
            GameObject a = Instantiate(GameDataManager.instance.allRails.Find(s => s.railType == type).railImage);
            a.transform.SetParent(myContent);
        }
        /* foreach (EnvType type in SaveAndLoadGameData.instance.savedData.playerEnvs)
        {
            GameObject a = Instantiate(GameDataManager.instance.allEnvs.Find(s => s.envType == type).envImage);
            a.transform.SetParent(myContent);
        } */
    }
    
    public void OpenZenScene()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenLevels()
    {
        //Open levels
    }
}
