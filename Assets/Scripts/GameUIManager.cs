using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    IInteractible interactible;
    GameObject choosenObj;
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] RailManager railManager;
    public void DeleteButtonClick()
    {
        if(interactible == null)
            return;
        interactible.Destroy();
        objectChooser.Unchoose();
    }
    public void RotateButtonClick()
    {
        if(interactible == null)
            return;
        interactible.Rotate();
    }
    public void RailButtonClick(GameObject obj)
    {
        if(interactible == null)
            return;
        // Bağlantı noktalarını açığa çıkar ve se.
        //choosenObj.GetComponent<Rail>().HighlightConnectionPoints(obj);
        railManager.HighlightConnectionPoints(choosenObj.GetComponent<Rail>(), obj);
    }
    public void SetInteractible(GameObject obj)
    {
        if(obj == null)
            interactible = null;
        else
        {
            interactible = obj.GetComponent<IInteractible>();
            this.choosenObj = obj;
        }
    }
}
