using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavbarUIManager : MonoBehaviour
{
    [SerializeField] GameUIManager uIManager;
    [SerializeField] Button railsContentButton, envsContentButton, trainsContentButton;
    [SerializeField] ScrollRect contentScrollRect;

    [SerializeField] Transform railsContent, envsContent, trainsContent;
    [SerializeField] Image navbarImage;
    

    [Header("Navbar Poses")]

    [SerializeField] Vector3 hidePos;
    [SerializeField] Vector3 showPos;

    [Header("Content Colors")]

    [SerializeField] Color railColor;
    [SerializeField] Color envColor;
    [SerializeField] Color trainColor;

    
    bool navbarShowing;
    RectTransform navbarTransform;
   
    void Awake()
    {
        navbarTransform = transform.GetComponent<RectTransform>();
    }

    void Start()
    {
        if(SaveAndLoadGameData.instance != null)
        {
            // Clean
            foreach (Transform child in railsContent)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in envsContent)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in trainsContent)
            {
                Destroy(child.gameObject);
            }
            // fill
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerRails)
            {
                RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);
                GameObject e = Instantiate(data.railButton);
                e.transform.parent = railsContent;
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.RailButtonClick(data.railPrefab); } );
            }
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerEnvs)
            {
                EnvironmentData data = GameDataManager.instance.allEnvs.Find(s => s.envType == item);
                GameObject e = Instantiate(data.envButton);
                e.transform.parent = envsContent;
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.EnvironmentCreateButtonClick(data.envPrefab); } );
            }
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerTrains)
            {
                TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);
                GameObject e = Instantiate(data.trainButton);
                e.transform.parent = trainsContent;
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.TrainCreateButtonClick(data.trainPrefab); } );
            }
        }
    }

    public void RailsContentButtonClick()
    {
        ShowNavbar();

        navbarImage.color = railColor;

        railsContent.gameObject.SetActive(true);

        contentScrollRect.content = railsContent.GetComponent<RectTransform>();
        
        envsContent.gameObject.SetActive(false);
        trainsContent.gameObject.SetActive(false);
    }
    public void EnvsContentButtonClick()
    {
        ShowNavbar();

        navbarImage.color = envColor;

        envsContent.gameObject.SetActive(true);

        contentScrollRect.content = envsContent.GetComponent<RectTransform>();

        railsContent.gameObject.SetActive(false);
        trainsContent.gameObject.SetActive(false);
    }
    public void TrainsContentButtonClick()
    {
        ShowNavbar();

        navbarImage.color = trainColor;

        trainsContent.gameObject.SetActive(true);

        contentScrollRect.content = trainsContent.GetComponent<RectTransform>();

        envsContent.gameObject.SetActive(false);
        railsContent.gameObject.SetActive(false);
    }
    public void ShowNavbar()
    {
        navbarShowing = true;
        RefreshPosition();
    }
    public void HideNavbar()
    {
        navbarShowing = false;
        RefreshPosition();        
    }
    void RefreshPosition()
    {
        if (navbarShowing)
        {
            navbarTransform.localPosition = showPos;
        }
        else
        {
           navbarTransform.localPosition = hidePos;
        }
    }
}
