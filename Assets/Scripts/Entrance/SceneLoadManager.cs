using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public Transform cardParent;
    public GameObject cardPrefab;
    public EntranceUI entranceUI;
    void OnEnable()
    {
        SetScene();
    }
    void SetScene()
    {
        foreach (Transform item in cardParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in SaveAndLoadGameData.instance.savedData.zenSceneDatas)
        {
            GameObject g = Instantiate(cardPrefab);
            g.transform.parent = cardParent;

            SceneLoadCard card = g.GetComponent<SceneLoadCard>();
            
            card.date.text = item.dateTime.ToString();
            card.sceneData = item;

            card.delete.onClick.AddListener( delegate{ Delete(item); } );
            card.load.onClick.AddListener( delegate{ Load(item); } );
        }
    }
    public void Delete(ZenSceneData zenSceneData)
    {
        SaveAndLoadGameData.instance.savedData.zenSceneDatas.Remove(zenSceneData);
        SaveAndLoadGameData.instance.Save();
        SetScene();
    }
    public void Load(ZenSceneData zenSceneData)
    {
        GameDataManager.instance.zenSceneDataManager.LoadingScene = zenSceneData;
        GameDataManager.instance.zenSceneDataManager.isLoad = true;
        entranceUI.OpenZenScene();
    }
}
