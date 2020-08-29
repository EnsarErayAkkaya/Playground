using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public GameObject levelButton;
    public Transform levelsContent;

    void Start()
    {
        Set();
    }

    public void Set()
    {
        foreach (Transform child in levelsContent)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in GameDataManager.instance.levels)
        {
            LevelButton lb = Instantiate(levelButton).GetComponent<LevelButton>();

            if(SaveAndLoadGameData.instance.savedData.unlockedLevels.Any(s => s.levelIndex == item.levelIndex))
            {
                LevelData ld = SaveAndLoadGameData.instance.savedData.unlockedLevels.First(s => s.levelIndex == item.levelIndex);
                
                lb.Set(item.levelIndex, ld.mark, ld.isUnlocked );
                
                if(ld.isUnlocked)
                    lb.GetComponent<Button>().onClick.AddListener( delegate{ LevelButtonOnClick(item.levelSceneIndex, item.levelIndex); } );
            }
            else
            {
                lb.Set(item.levelIndex, "", false );
            }

            lb.transform.parent = levelsContent;
            
        }
    }

    public void LevelButtonOnClick(int sceneIndex, int levelIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        GameDataManager.instance.currentlyPlayingLevelIndex = levelIndex;
    }
}
