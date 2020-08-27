using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public GameObject levelButton;
    public Transform levelsContent;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
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
            lb.Set(item.levelIndex, item.mark, item.isUnlocked);
            lb.transform.parent = levelsContent;
            lb.GetComponent<Button>().onClick.AddListener( delegate{ LevelButtonOnClick(item.levelSceneIndex); } );
        }
    }

    public void LevelButtonOnClick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
