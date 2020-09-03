using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelIndexText;
    public Image starImage;
    public Image background;
    public Sprite[] stars;
    public Color locked;
    public Color unLocked;
    public void Set(int lvlIndex, int mark, bool isUnlocked )
    {
        levelIndexText.text = lvlIndex.ToString();

        if(mark > 0)
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = stars[mark-1];
        }
        if(isUnlocked)
        {
            background.color = unLocked;
        }
        else
            background.color = locked;
    }
}
