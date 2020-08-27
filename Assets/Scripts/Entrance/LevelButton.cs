using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelIndexText;
    public TextMeshProUGUI levelMark;
    public Image background;
    public Color locked;
    public Color unLocked;
    public void Set(int lvlIndex, string mark, bool isUnlocked )
    {
        levelIndexText.text = lvlIndex.ToString();
        levelMark.text = mark;
        if(isUnlocked)
        {
            background.color = unLocked;
        }
        else
            background.color = locked;
    }
}
