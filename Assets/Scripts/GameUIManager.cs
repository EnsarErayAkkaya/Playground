using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    IInteractible interactible;
    public void DeleteButtonClick()
    {
        if(interactible == null)
            return;
        interactible.Destroy();
    }
    public void RotateButtonClick()
    {
        if(interactible == null)
            return;
        interactible.Rotate();
    }
    public void SetInteractible(GameObject obj)
    {
        if(obj == null)
            interactible = null;
        else
        {
            interactible = obj.GetComponent<IInteractible>();
        }
    }
}
