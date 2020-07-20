using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] float height;
    GameObject trainPrefab;
    
    void Start()
    {
        TrainType t = SaveAndLoadGameData.instance.savedData.choosenTrain;
        trainPrefab = GameDataManager.instance.allTrains.First(s => s.trainType == t).trainGamePrefab;
    }
    public void CreateTrain()
    {
        GameObject a = Instantiate(trainPrefab);
    }
}
