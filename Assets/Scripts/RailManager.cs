using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class RailManager : MonoBehaviour
{
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] GameObject prefab,prefab1,prefab2;
    GameObject creatingRail;
    Rail connectingRail;
    public float connectionDistance, railHeight;
    public bool choosingConnectionPoints;
    bool startChoosePoint,startChoosePoint2;
    [SerializeField] List<Rail> rails;
    void Awake()
    {
        List<Rail> rs = FindObjectsOfType<Rail>().ToList();
        if(rs.Count > 0)
        {
            Rail r = rs.First(s => s.isFirst);
            rails.Add(r);
            rs.Remove(r);
            foreach (Rail item in rs)
            {
                rails.Add(item);
            }
        }
       
    }
    void Update()
    {
        if(startChoosePoint == true && !EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetMouseButtonDown(0))
            {
                //highlight ı bitir
                connectingRail.NotHighlightConnectionPoints();

                Vector3 clickPos = objectChooser.hitPoint;

                RailConnectionPoint closestPoint = null;
                foreach (RailConnectionPoint rcp in connectingRail.GetFreeConnectionPoints())
                {
                    if( closestPoint == null || Vector3.Distance(clickPos, closestPoint.point) 
                                > Vector3.Distance(clickPos, rcp.point) )
                    {
                        closestPoint = rcp;
                    }
                }
                Connect( closestPoint );
                startChoosePoint = false;
            }
        }
      /*   if(startChoosePoint2 == true && !EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetMouseButtonDown(0))
            {
                //highlight ı bitir
                connectingRail.NotHighlightConnectionPoints();

                Vector3 clickPos = Input.mousePosition;

                RailConnectionPoint closestPoint = null;
                foreach (RailConnectionPoint rcp in connectingRail.GetFreeConnectionPoints())
                {
                    if( closestPoint == null || Vector3.Distance(clickPos, closestPoint.GetWorldPos()) 
                                > Vector3.Distance(clickPos, rcp.GetWorldPos()) )
                    {
                        closestPoint = rcp;
                    }
                }
                Connect( closestPoint );
                startChoosePoint = false;
            }
        } */
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(prefab);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(prefab1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(prefab2);
        }
    }

    void Connect(RailConnectionPoint rcp)
    {
        // TODO
        // Bir ray seçilecek ve bir bağlantı noktası seçilecek
        // ve rayın uygun noktası seçilen bağlantı noktasına bağlanacak
        
        // Ray değilse dön 
        if(creatingRail.GetComponent<Rail>() == null ) { Debug.LogError("Attached wrong object to Rail button"); return;}

        Rail r = Instantiate( creatingRail ).GetComponent<Rail>();

        if(rcp.isInput == false) // bağlanılan nokta çıkış ise
        {
            Debug.Log("çıkışa bağlanıyor");
            // direk girişi seçili noktaya bağla
            rcp.connectedPoint = r.GetFreeConnectionPoints().First(s => s.isInput == true);
            
            RailConnection(rcp);

            // açıyı ayarla 
            rcp.connectedPoint.transform.rotation = Quaternion.Euler(rcp.rail.transform.rotation.eulerAngles + new Vector3(0, rcp.extraAngle,0));

        }
        else // bağlanılan nokta giriş ise
        {
            Debug.Log("girişe bağlanıyor");
            // çıkışı girişe bağla
            rcp.connectedPoint = r.GetFreeConnectionPoints().First(s => s.isInput == false);
            rcp.connectedPoint.connectedPoint = rcp;

            RailConnection(rcp);

            // açıyı ayarla
            rcp.connectedPoint.transform.rotation = Quaternion.Euler(rcp.rail.transform.rotation.eulerAngles - new Vector3(0, rcp.connectedPoint.extraAngle,0));
            
        }
        
        // parentları düzenle
        rcp.connectedPoint.rail.transform.parent = null; // railın parentını tamizle
        rcp.connectedPoint.transform.parent = rcp.connectedPoint.rail.transform; // noktayı railın çocuğu yap
        
    }
    void RailConnection(RailConnectionPoint rcp)
    {
        rcp.connectedPoint.connectedPoint = rcp;
        
        // rcp noktası bağlanılan nokta ve rcp.connectedPoint bağlanan noktadır
        rcp.connectedPoint.transform.parent = null; // parentı çıkar
        rcp.connectedPoint.rail.transform.parent = rcp.connectedPoint.transform; // raili noktasının çocuğu yap
        rcp.connectedPoint.point = /*  rcp.rail.transform.position + */ rcp.point; // noktaların pozisyonunu birleştir
    }
    
    
    public void HighlightConnectionPoints(Rail r, GameObject nextRail)
    {
        creatingRail = nextRail;
        connectingRail = r;
        if(connectingRail.GetFreeConnectionPoints().Length > 1)
        {
            r.HighlightConnectionPoints();
            startChoosePoint = true;
        }
        else if(connectingRail.GetFreeConnectionPoints().Length == 1){
            Connect( r.GetFreeConnectionPoints()[0] );
        }
        else{
            Debug.Log("Bağlanılacak bir nokta yok");
        }
    }



    public void RemoveRail(Rail r)
    {
        rails.Remove(r);
    }
    public void AddRail(Rail r)
    {
        rails.Add(r);
    }
    public bool IsFirstRail()
    {
        if(rails.Count > 0)
            return false;
        else{
            return true;
        }
    }
    public Rail GetFirstRail()
    {
        return rails.Find(s => s.isFirst);
    }
    public Rail GetLastRail()
    {
        return rails[rails.Count-1];
    }
}
