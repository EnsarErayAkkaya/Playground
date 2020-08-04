using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    [Header("Managers")]

    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] RailManager railManager;
    [SerializeField] EnvironmentManager environmentManager;
    [SerializeField] RailWayChooser railWayChooser;
    [SerializeField] TrainManager trainManager;
    [SerializeField] NavbarUIManager navbarUI;

    [SerializeField] Button changeRailWayButton, setConnectionButton, deleteButton, rotateButton, playStopButton;
    [SerializeField] Image playImage, stopImage;
    bool isPlaying;
    

    InteractibleBase interactible;
   
    
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
    public void TrainCreateButtonClick(GameObject train)
    {
        if(interactible == null ||interactible.GetComponent<Rail>() == null)
            return;
        
        trainManager.CreateTrain(interactible.GetGameObject(), train);
    }
    public void ChangeRailWayButtonClick()
    {
        if(interactible == null)
            return;
        if( interactible.GetComponent<Rail>() != null )
        {
            railWayChooser.ChooseWay(interactible.GetComponent<Rail>());
            trainManager.StopAllTrains();
        }
    }
    public void SetConnectionButtonClick()
    {
        if(interactible == null)
            return;
        railManager.ExistingRailConnection(interactible.GetComponent<Rail>());
    }
    public void SetTrainSpeedButtonClick()
    {
        trainManager.ChangeSpeed();
    }
    public void PlayStopButtonClick()
    {
        if(isPlaying)
        {
            playStopButton.targetGraphic = stopImage;
            isPlaying = false;
            trainManager.StopAllTrains();
            playImage.gameObject.SetActive(true);
            stopImage.gameObject.SetActive(false);
        }
        else
        {
            playStopButton.targetGraphic = playImage;
            isPlaying = true;
            trainManager.StartTrains();
            playImage.gameObject.SetActive(false);
            stopImage.gameObject.SetActive(true);
        }
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
        else if(obj != null && !trainManager.isStarted)
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
        else if(obj != null && trainManager.isStarted)
        {
            // tren başlamışsa
            // butonları gizle
            interactible = obj.GetComponent<InteractibleBase>();
            deleteButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);

            //navbarı gizle
            navbarUI.gameObject.SetActive(false);
            
            if(!interactible.isStatic && interactible.GetComponent<Rail>() != null)
            {
                if(interactible.GetComponent<Rail>().GetOutputConnectionPoints().Length > 1 )
                {
                    changeRailWayButton.gameObject.SetActive(true);
                    interactible.GetComponent<SplineManager>().ShowTrack();
                }
            }
        }
    }
}
