using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneLoadCard : MonoBehaviour
{
    public ZenSceneData sceneData;
    public TextMeshProUGUI date;
    public Image image;
    public Button delete;
    public Button load;
    public SceneLoadCard(System.DateTime d)
    {
        date.text = d.ToString();
    }
}
