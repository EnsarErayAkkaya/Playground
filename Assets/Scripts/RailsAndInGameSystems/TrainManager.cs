using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] LightManager lightManager;
    [SerializeField] float height;
    [SerializeField] LayerMask layerMask;
    public SpeedType speedType = SpeedType.x;
    public float normalSpeed, middleSpeed, fastSpeed;
    [SerializeField] List<Train> trains;
    
    bool chooseRail;
    public void ResumeStartedTrain()
    {
        foreach (Train item in trains)
        {
            item.ResumeTrain();
        }
    }
    public void StopAllTrains()
    {
        foreach (Train item in trains)
        {
            item.StopTrain();
        }
    }
    public void CreateTrain(GameObject choosenRail,GameObject trainPrefab)
    {
        if(choosenRail.GetComponent<Rail>() != null)
        {
            GameObject a = Instantiate(trainPrefab);
            a.transform.position = new Vector3(choosenRail.transform.position.x, height, choosenRail.transform.position.z);
            a.transform.rotation = choosenRail.transform.rotation;

            Train t = a.transform.GetChild(0).GetComponent<Train>();
            t.rail = choosenRail.GetComponent<Rail>();
            trains.Add(t);
        }
        else
        {
            Debug.Log("You should choose a rail");
        }
    }
    public void ChangeSpeed()
    {
        if(speedType == SpeedType.x) speedType = SpeedType.x2;   
        else if(speedType == SpeedType.x2) speedType = SpeedType.x3;
        else if(speedType == SpeedType.x3) speedType = SpeedType.x;
        foreach (Train item in trains)
        {
            item.SetSpeed();
        }   
    }
    public void RemoveTrain(Train t)
    {
        trains.Remove(t);
    }
}
