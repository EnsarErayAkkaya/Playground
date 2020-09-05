using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameUIManager : MonoBehaviour
{
    [Header("Managers")]

    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] RailManager railManager;
    [SerializeField] EnvironmentManager environmentManager;
    [SerializeField] RailWayChooser railWayChooser;
    [SerializeField] TrainManager trainManager;
    [SerializeField] ObjectPlacementManager placementManager;
    [SerializeField] CameraManager cameras;
    [SerializeField] NavbarUIManager navbarUI;
    [SerializeField] LevelUI levelUI;

    [SerializeField] Button changeRailWayButton, setConnectionButton, deleteButton, rotateButton, saveButton;
    [SerializeField] Button moveButton, playStopButton, trainSpeedButton, changeCamera, cleanButton, flipButton;
    [SerializeField] Image playImage, stopImage;
    bool isPlaying, isMultiple;
    public bool buttonLock = false;
    
    InteractibleBase interactible;
    List<InteractibleBase> interactibles;
    
    void Start()
    {
        if(FindObjectsOfType<Train>().Length > 0)
        {
            playStopButton.gameObject.SetActive(true);
        }
    }   
    public void DeleteButtonClick()
    {
        if(interactible == null || buttonLock)
            return;
        
        if(isMultiple)
        {
            foreach (var item in interactibles)
            {
                if(levelUI != null)
                    levelUI.SetBudget( item.cost );
                item.Destroy();
            }
        }
        else
        {
            if(levelUI != null)
                levelUI.SetBudget( interactible.cost );

            interactible.Destroy();

            objectChooser.Unchoose();

            if( trainManager.trains.Count <= 0 )
            {
                playStopButton.gameObject.SetActive(false);
            }
        }
    }
    public void RotateButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        if(isMultiple)
        {
            foreach (var item in interactibles)
            {
                railManager.RotateRail(item.GetComponent<Rail>());
            }
        }
        else
        {
            if(interactible.GetComponent<Rail>() != null)
                railManager.RotateRail(interactible.GetComponent<Rail>());
            else if(interactible.GetComponent<EnvironmentObject>() != null)
                environmentManager.RotateEnv(interactible.GetComponent<EnvironmentObject>());
        }
    }
    public void FlipRailButtonClick()
    {
        if( interactible != null && interactible.GetComponent<Rail>() != null)
            railManager.FlipRail(interactible.GetComponent<Rail>());
    }
    public void RailButtonClick(GameObject obj, int cost)
    {
        if(buttonLock) return;
        
        if( levelUI != null && levelUI.SetBudget(-cost))
        {
            buttonLock = true;
            if( interactible != null && interactible.GetComponent<Rail>() != null)
                railManager.NewRailConnection(interactible.GetComponent<Rail>(), obj, cost);
            else
                railManager.CreateFloatingRail(obj, cost);
        }
        else if( levelUI == null )
        {
            buttonLock = true;
            if( interactible != null && interactible.GetComponent<Rail>() != null)
                railManager.NewRailConnection(interactible.GetComponent<Rail>(), obj, cost);
            else
                railManager.CreateFloatingRail(obj, cost);
        }
    }
    public void EnvironmentCreateButtonClick(GameObject obj, int cost)
    {
        if(buttonLock) return;
        
        if( levelUI != null && levelUI.SetBudget(-cost))
        {
            buttonLock = true;
            environmentManager.CreateEnvironmentObject(obj, cost);
        }
        else if( levelUI == null )
        {
            buttonLock = true;
            environmentManager.CreateEnvironmentObject(obj, cost);
        }
    }
    public void TrainCreateButtonClick(GameObject train, int cost)
    {
        if(interactible == null ||interactible.GetComponent<Rail>() == null || buttonLock)
            return;
            
        if( levelUI != null && levelUI.SetBudget(-cost))
        {
            buttonLock = true;
            trainManager.CreateTrain(interactible.GetGameObject(), train, cost);

            if( trainManager.trains.Count > 0 )
            {
                playStopButton.gameObject.SetActive(true);
            }
        }
        else if( levelUI == null )
        {
            buttonLock = true;  
            trainManager.CreateTrain(interactible.GetGameObject(), train, cost);

            if( trainManager.trains.Count > 0 )
            {
                playStopButton.gameObject.SetActive(true);
            }
        }
        buttonLock = false;   
    }
    public void ChangeRailWayButtonClick()
    {
        if(interactible == null || buttonLock)
            return;
        if( interactible.GetComponent<Rail>() != null )
        {
            buttonLock = true;
            railWayChooser.ChangeRailway(interactible.GetComponent<Rail>());
        }
        buttonLock = false; 
    }
    public void MoveButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        if( isMultiple )
        {   
            buttonLock = true;
            placementManager.PlaceMe(objectChooser.multipleObjectParent.gameObject, PlacementType.RailSystem);
        }
    }
    public void SetConnectionButtonClick()
    {
        if(interactible == null || buttonLock)
            return;
        buttonLock = true;

        railManager.ExistingRailConnection(interactible.GetComponent<Rail>());
    }
    public void SetTrainSpeedButtonClick()
    {
        trainManager.ChangeSpeed();
    }
    public void SaveButtonClick()
    {
        GameDataManager.instance.zenSceneDataManager.SaveZenSceneData();
    }
    public void SetCamerasButton()
    {
        cameras.ChangeStyle();
    }
    public void RestartButtonClick()
    {
        if(buttonLock) return;
        
        trainManager.isStarted = false;

        isPlaying = false;
        
        playStopButton.gameObject.SetActive(false);

        navbarUI.gameObject.SetActive(true);

        while(trainManager.trains.Count > 0)
        {
            trainManager.trains[0].StopTrain();
            trainManager.trains[0].Destroy();
        }
        while(railManager.GetRails().Count > 0)
        {
            railManager.GetRails()[0].Destroy();
        }
        while(environmentManager.environments.Count > 0)
        {
            environmentManager.environments[0].Destroy();
        }
        SetUI(null);
    }
    public void PlayStopButtonClick()
    {
        if(buttonLock) return;

        if(isPlaying == false && trainManager.isStarted == false )
        {
            changeRailWayButton.GetComponent<RectTransform>().localPosition = Vector2.zero;
            // butonları gizle
            deleteButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);

            //navbarı gizle
            navbarUI.gameObject.SetActive(false);

            if( FindObjectOfType<LevelManager>() == null )
                saveButton.gameObject.SetActive(true);
        }
        if(isPlaying)
        {
            isPlaying = false;
            trainManager.StopAllTrains();
            playImage.gameObject.SetActive(true);
            stopImage.gameObject.SetActive(false);
        }
        else
        {
            playStopButton.targetGraphic = stopImage;
            isPlaying = true;
            trainManager.StartTrains();
            playImage.gameObject.SetActive(false);
            stopImage.gameObject.SetActive(true);
        }
        SetUI(null);
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

        SetUI(obj);
    }
    public void SetUI(GameObject obj)
    {
        isMultiple = false;
        moveButton.gameObject.SetActive(false);
        if(obj == null)
        {
            interactible = null;
            interactibles = null;

            deleteButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            changeRailWayButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);

            if(trainManager.isStarted)
            {
                //navbarı gizle
                navbarUI.gameObject.SetActive(false);
                //trainSpeedButton.gameObject.SetActive(true);
            }
        }
        else if(obj != null && !trainManager.isStarted)
        {         
            interactible = obj.GetComponent<InteractibleBase>();

            navbarUI.gameObject.SetActive(true);//navbarı aç

            if(interactible.isStatic)
            {
                deleteButton.gameObject.SetActive(false);
                rotateButton.gameObject.SetActive(false);
                setConnectionButton.gameObject.SetActive(false);
                flipButton.gameObject.SetActive(false);

                if(interactible.GetComponent<Rail>() != null)
                {
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
                else
                    changeRailWayButton.gameObject.SetActive(false);
            }
            else
            {
                deleteButton.gameObject.SetActive(true);
                rotateButton.gameObject.SetActive(true);
                flipButton.gameObject.SetActive(true);
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
                else if( interactible.GetComponent<Train>() != null )
                {
                    deleteButton.gameObject.SetActive(true);

                    rotateButton.gameObject.SetActive(false);
                    setConnectionButton.gameObject.SetActive(false);
                    changeRailWayButton.gameObject.SetActive(false);
                    flipButton.gameObject.SetActive(false);
                }
            }            
        }
        else if(obj != null && trainManager.isStarted)
        {
            // tren başlamışsa
            interactible = obj.GetComponent<InteractibleBase>();
            
            if(interactible.GetComponent<Rail>() != null)
            {
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
    public void SetUIMultiple( List<InteractibleBase> objects )
    {
        if(objects != null)
        {
            interactibles = objects;

            isMultiple = true;

            navbarUI.gameObject.SetActive(false);// gizle

            deleteButton.gameObject.SetActive(true);
            moveButton.gameObject.SetActive(true);

            rotateButton.gameObject.SetActive(false);
            changeRailWayButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);
        }
        
    } 
}
