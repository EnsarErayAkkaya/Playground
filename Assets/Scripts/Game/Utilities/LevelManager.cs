using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level geçildiğinide ödülleri verecek oyun sonu ekranını tetikleyecek
    [SerializeField] RailManager railManager;
    
    public List<RailType> levelRails;
    public List<EnvType> levelEnvs;
    public List<TrainType> levelTrains;

    public RailType[] levelRailPrize;
    public Rail targetRail;
    public int targetedTrainCount;
    int reachedTrainCount = 0;
    GameDataManager gdm;
    string mark = "";
    void Start()
    {
        gdm = GameDataManager.instance;
        railManager.AddCreatedRails();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Train")
        {
            Debug.Log("Level finished - train on tunnel");
            GivePrizes();
        }
    }
    public void TrainReachedTarget(Rail r)
    {
        if(r == targetRail)
            reachedTrainCount++;
        
        if(reachedTrainCount == targetedTrainCount)
        {
            EndLevel();
            SaveLevelData();
        }
    }
    void GivePrizes()
    {
        foreach (RailType item in levelRailPrize)
        {
            gdm.AddNewPlayerRail(item);
        }
    }
    public void SaveLevelData()
    {
        //currentLevel
        gdm.SaveLevelMark(CalculateMark());

        //unlock next Level
        gdm.UnlockNextLevel();
    }
    public string CalculateMark()
    {
        return "NAN";
    }
    public void EndLevel()
    {
        // Call End level UI
    }
}
