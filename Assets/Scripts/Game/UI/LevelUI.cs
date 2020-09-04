using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelUI : MonoBehaviour
{
    public LevelManager levelManager;
    [SerializeField] TextMeshProUGUI budgetText;
    [SerializeField] GameObject endUI;
    [SerializeField] Image markImage;
    [SerializeField] Button nextButton;
    public Sprite[] stars;
    void Start()
    {
        //SetBudgetText();
    }
    public bool SetBudget(int c)
    {
        bool a = levelManager.SetBudget(c);
        if(a)
        {
            SetBudgetText();
            return true;
        }
        else
        {
            Debug.Log("Not enough resources");
            return false;
        }
    }
    void SetBudgetText()
    {
        budgetText.text = levelManager.budget.ToString();
    }
    public void SetEndUI(int m)
    {
        endUI.SetActive(true);
        markImage.sprite = stars[m-1];
        if( GameDataManager.instance.levels.Any(s => s.levelIndex == GameDataManager.instance.currentlyPlayingLevelIndex+1) )
        {
            nextButton.gameObject.SetActive(true);
        }
    }
    public void NextLevelButtonClick()
    {
        LevelData ld = GameDataManager.instance.levels[GameDataManager.instance.currentlyPlayingLevelIndex];
        SceneManager.LoadScene(ld.levelSceneIndex);
    }
    public void RestartLevelButtonClick()
    {
        LevelData ld = GameDataManager.instance.levels[GameDataManager.instance.currentlyPlayingLevelIndex-1];
        SceneManager.LoadScene(ld.levelSceneIndex);
    }
}
