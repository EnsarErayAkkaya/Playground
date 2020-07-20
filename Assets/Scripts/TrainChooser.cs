using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainChooser : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] Button chooseButton;
    List<GameObject> trains = new List<GameObject>();
    List<TrainType> trainTypes = new List<TrainType>();
    int index;
    void Start()
    {
        // Clean
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        index = 0;
        // fill
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerTrains)
        {
            TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);

            GameObject obj = Instantiate(data.trainUIPrefab);
            obj.transform.parent = this.transform;

            trains.Add(obj);
            trainTypes.Add(item);

            obj.SetActive(false);
        }
        trains[index].SetActive(true);
    }
    void OnDisable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        for (int i = 0; i < trains.Count; i++)
        {
            if(i == index)
                trains[i].SetActive(true);
            else
                trains[i].SetActive(false);
        }
    }
    void Update()
    {
        Camera.main.transform.RotateAround(Vector3.zero,Vector3.up, rotateSpeed * Time.deltaTime);
    }
    public void RightButton()
    {
        trains[index].SetActive(false);
        index++;

        if (index == trains.Count)
            index = 0;

        trains[index].SetActive(true);
        ButtonCheck();
    }
    public void LeftButton()
    {
        trains[index].SetActive(false);
        index--;

        if (index < 0)
            index = trains.Count - 1;

        trains[index].SetActive(true);
        ButtonCheck();
    }
    public void ButtonCheck()
    {
        if (SaveAndLoadGameData.instance.savedData.choosenTrain == trainTypes[index])
        {
            chooseButton.interactable = false;
        }
        else
        {
            chooseButton.interactable = true;
        }
    }
    public void ChooseTrain()
    {
        GameDataManager.instance.ChooseTrain(trainTypes[index]);
    }
}
