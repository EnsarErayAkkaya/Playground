using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    InteractibleBase interactible;
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] RailManager railManager;
    [SerializeField] EnvironmentManager environmentManager;
    [SerializeField] RailWayChooser railWayChooser;
    [SerializeField] Train train;
    [SerializeField] Button changeRailWayButton, setConnectionButton, deleteButton, rotateButton;
    [SerializeField] Transform railButtonsContent;
   
    void Start()
    {
        if(SaveAndLoadGameData.instance != null)
        {
            // Clean
            foreach (Transform child in railButtonsContent)
            {
                Destroy(child.gameObject);
            }
            // fill
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerRails)
            {
                RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);
                GameObject e = Instantiate(data.railButton);
                e.transform.parent = railButtonsContent;
                e.GetComponent<Button>().onClick.AddListener( delegate{ RailButtonClick(data.railPrefab); } );
            }
        }
    }
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
        if(interactible.GetComponent<Rail>() != null)
            railManager.RotateRail(interactible.GetComponent<Rail>());
        else if(interactible.GetComponent<EnvironmentObject>() != null)
            environmentManager.RotateEnv(interactible.GetComponent<EnvironmentObject>());
    }
    public void RailButtonClick(GameObject obj)
    {
        if(interactible != null)
            railManager.NewRailConnection(interactible.GetComponent<Rail>(), obj);
        else
            railManager.CreateFloatingRail(obj);
    }
    public void EnvironmentCreateButtonClick(GameObject obj)
    {
        environmentManager.CreateEnvironmentObject(obj);
    }
    public void ChangeRailWayButtonClick()
    {
        if(interactible == null)
            return;
        if( interactible.GetComponent<Rail>() != null )
            railWayChooser.ChooseWay(interactible.GetComponent<Rail>());
    }
    public void SetConnectionButtonClick()
    {
        if(interactible == null)
            return;
        railManager.ExistingRailConnection(interactible.GetComponent<Rail>());
    }
    public void SetTrainSpeedButtonClick()
    {
        train.ChangeSpeed();
    }
    public void SetInteractible(GameObject obj)
    {
        try
        {
            if(interactible.GetComponent<Rail>() != null && interactible.GetComponent<Rail>().GetOutputConnectionPoints().Length > 1 )
            {
                interactible.GetComponent<SplineManager>().HideTracks();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        if(obj == null)
        {
            interactible = null;
            deleteButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            changeRailWayButton.gameObject.SetActive(false);              
        }
        else
        {         
            interactible = obj.GetComponent<InteractibleBase>();
            if(interactible.isStatic)
            {
                deleteButton.gameObject.SetActive(false);
                rotateButton.gameObject.SetActive(false);
                setConnectionButton.gameObject.SetActive(false);
                changeRailWayButton.gameObject.SetActive(false);                
            }
            else
            {
                deleteButton.gameObject.SetActive(true);
                rotateButton.gameObject.SetActive(true);
                if(interactible.GetComponent<Rail>() != null)
                {
                    setConnectionButton.gameObject.SetActive(true);
                    if(interactible.GetComponent<Rail>().GetOutputConnectionPoints().Length > 1 )
                    {
                        changeRailWayButton.gameObject.SetActive(true);
                        interactible.GetComponent<SplineManager>().ShowTrack();
                    }
                    else
                    {
                        changeRailWayButton.gameObject.SetActive(false);
                    }
                }
            }            
        }
    }
}
