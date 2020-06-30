using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    IInteractible interactible;
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] RailManager railManager;
    [SerializeField] Button changeRailWayButton;
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
        railManager.NewRailConnection(interactible.GetGameObject().GetComponent<Rail>(), obj);
    }
    public void ChangeRailWayButtonClick()
    {
        if(interactible == null)
            return;
        if(interactible.GetGameObject().GetComponent<Rail>() != null)
            interactible.GetGameObject().GetComponent<Rail>().ChangeCurrentOption();
    }
    public void SetConnectionButtonClick()
    {
        if(interactible == null)
            return;
        railManager.ExistingRailConnection(interactible.GetGameObject().GetComponent<Rail>());
    }
    public void SetInteractible(GameObject obj)
    {
        try
        {
            Debug.Log("burada1");
            if(interactible.GetGameObject().GetComponent<Rail>() != null && interactible.GetGameObject().GetComponent<Rail>().GetConnectionPoints().Length > 2 )
            {
                Debug.Log("hiding tracks of " + interactible.GetGameObject().name );
                interactible.GetGameObject().GetComponent<Rail>().HideTracks();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        if(obj == null)
            interactible = null;
        else
        {         
            interactible = obj.GetComponent<IInteractible>();

            if(interactible.GetGameObject().GetComponent<Rail>() != null && interactible.GetGameObject().GetComponent<Rail>().GetConnectionPoints().Length > 2 )
            {
                changeRailWayButton.gameObject.SetActive(true);
                interactible.GetGameObject().GetComponent<Rail>().ShowActiveTrack();
            }
            else
            {
                changeRailWayButton.gameObject.SetActive(false);
            }
        }
    }
}
