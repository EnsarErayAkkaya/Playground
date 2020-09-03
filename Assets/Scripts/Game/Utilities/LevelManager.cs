using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level geçildiğinide ödülleri verecek oyun sonu ekranını tetikleyecek
    [SerializeField] RailManager railManager;
    [SerializeField] LevelUI levelUI;
    public int budget;
    int startingBudget;
    
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
        startingBudget = budget;
        gdm = GameDataManager.instance;
        railManager.AddCreatedRails();
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
    public int CalculateMark()
    {
        int x = 0;
        if(budget > 0)
        {
            x = 3;
        }
        else
        {
            x = 1;
        }
        levelUI.SetEndUI(x);
        return x;
    }
    public void EndLevel()
    {
        GivePrizes();
        SaveLevelData();
    }
    public bool SetBudget(int c)
    {
        int temp = budget;
        temp += c;
        if(temp < 0)
        {
            return false;
        }
        else
        {
            budget = temp;
            if(budget > startingBudget)
                budget = startingBudget;
            return true;
        }
    }
}
