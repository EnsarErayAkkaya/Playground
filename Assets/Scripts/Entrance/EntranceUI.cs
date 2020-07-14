using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceUI : MonoBehaviour
{
    public Transform myRailsContent;
    void Start()
    {
        foreach (RailType type in SaveAndLoadGameData.instance.savedData.playerRails)
        {
            GameObject a = Instantiate(GameDataManager.instance.allRails.Find(s => s.railType == type).railImage);
            a.transform.SetParent(myRailsContent);
        }
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
