using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class RailManager : MonoBehaviour
{
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] LightManager lightManager;


    // connectingRail yeni ve var olan ray bağlantısı yaparken bağlanan rayı işaret eder
    Rail connectingRail, newCreatedRail;

    // var olan ray bağlantısı yaparken bağlanılan noktayı işaret eder
    RailConnectionPoint connectingPoint,secondPointForExistingConnection;

    // objectPlacementManagerda kullanılan rayların yüksekliğini deopalayan değişken 
    public float railHeight;

    // şu anda seçimyapıtığını gösteren ve bu sırada obje seçimi yapılamaması için object chooser da kullanılan değişken
    public bool choosingConnectionPoints; // !! //

    // nokta seçiliyor mu 
    bool startChoosePointForConnection, startChoosePointForExistingConnection, willStartChoosePointForExistingConnection, mouseRelased;
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
        if(Input.GetMouseButtonUp(0))
        {
            mouseRelased = true;
        }
        if(startChoosePointForConnection == true && !EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetMouseButtonDown(0) && mouseRelased == true )
            {
                mouseRelased = false;
                
                //highlight ı bitir
                connectingRail.DownlightConnectionPoints();

                Vector3 clickPos = objectChooser.hitPoint;

                if(willStartChoosePointForExistingConnection)
                {
                    foreach (RailConnectionPoint rcp in connectingRail.GetConnectionPoints())
                    {
                        if( secondPointForExistingConnection == null || Vector3.Distance(clickPos, secondPointForExistingConnection.point) 
                                    > Vector3.Distance(clickPos, rcp.point) )
                        {
                            secondPointForExistingConnection = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    HighlightRailsForExistingConnection();
                }
                else
                {
                    mouseRelased = false;
                    
                    lightManager.OpenLights();
                    foreach (RailConnectionPoint rcp in connectingRail.GetFreeConnectionPoints())
                    {
                        if( connectingPoint == null || Vector3.Distance(clickPos, connectingPoint.point) 
                                    > Vector3.Distance(clickPos, rcp.point) )
                        {
                            connectingPoint = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    Connect();
                }
            }
        }
        
        if(startChoosePointForExistingConnection == true && !EventSystem.current.IsPointerOverGameObject())
        {   
            if(Input.GetMouseButtonDown(0) && mouseRelased == true)
            {
                DownlightRails();
                lightManager.OpenLights();

                Vector3 clickPos = objectChooser.hitPoint;

                if(secondPointForExistingConnection.isInput == false) // çıkışsa
                {
                    foreach (Rail rail in rails)
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeConnectionPoints().Where(s => s.isInput))
                        {
                            if( connectingPoint == null || Vector3.Distance(clickPos, connectingPoint.point) 
                                    > Vector3.Distance(clickPos, rcp.point) )
                            {
                                connectingPoint = rcp;
                            }
                        } 
                    }
                }
                else
                {
                    foreach (Rail rail in rails)
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeConnectionPoints().Where(s => !s.isInput))
                        {
                            if( connectingPoint == null || Vector3.Distance(clickPos, connectingPoint.point) 
                                    > Vector3.Distance(clickPos, rcp.point) )
                            {
                                connectingPoint = rcp;
                            }
                        } 
                    }
                }
                connectingRail.CleanConnections();

                startChoosePointForExistingConnection = false;

                Connect( );
            }
        }
    }

    void Connect()
    {
        // TODO
        // Bir ray seçilecek ve bir bağlantı noktası seçilecek
        // ve rayın uygun noktası seçilen bağlantı noktasına bağlanacak
        

        if(connectingPoint.isInput == false) // bağlanılan nokta çıkış ise
        {
            // direk girişi seçili noktaya bağla

            if(secondPointForExistingConnection == null)
            {
                connectingPoint.connectedPoint = newCreatedRail.GetFreeConnectionPoints().First(s => s.isInput == true);
                AddRail(newCreatedRail);
            }
            else{
                connectingPoint.connectedPoint = secondPointForExistingConnection;
            }

            RailConnection();

            // açıyı ayarla 
            connectingPoint.connectedPoint.transform.rotation = Quaternion.Euler(connectingPoint.rail.transform.rotation.eulerAngles + new Vector3(0, connectingPoint.extraAngle,0));

        }
        else // bağlanılan nokta giriş ise
        {
            // çıkışı girişe bağla
            
            if(secondPointForExistingConnection == null)
            {
                connectingPoint.connectedPoint = newCreatedRail.GetFreeConnectionPoints().First(s => s.isInput == false);
                AddRail(newCreatedRail);
            }
            else{
                connectingPoint.connectedPoint = secondPointForExistingConnection;
            }

            RailConnection();

            // açıyı ayarla
            connectingPoint.connectedPoint.transform.rotation = Quaternion.Euler(connectingPoint.rail.transform.rotation.eulerAngles - new Vector3(0, connectingPoint.connectedPoint.extraAngle,0));
            
        }
        
        // parentları düzenle
        connectingPoint.connectedPoint.rail.transform.parent = null; // railın parentını tamizle
        connectingPoint.connectedPoint.transform.parent = connectingPoint.connectedPoint.rail.transform; // noktayı railın çocuğu yap
        
        
        connectingPoint = null;
        newCreatedRail = null;
        secondPointForExistingConnection = null;
    }
    void RailConnection()
    {
        connectingPoint.connectedPoint.connectedPoint = connectingPoint;
        
        // connectingPoint noktası bağlanılan nokta ve connectingPoint.connectedPoint bağlanan noktadır
        connectingPoint.connectedPoint.transform.parent = null; // parentı çıkar
        connectingPoint.connectedPoint.rail.transform.parent = connectingPoint.connectedPoint.transform; // raili noktasının çocuğu yap
        connectingPoint.connectedPoint.point = /*  connectingPoint.rail.transform.position + */ connectingPoint.point; // noktaların pozisyonunu birleştir
    }
    public void NewRailConnection(Rail r, GameObject nextRail)
    {
        // Ray değilse dön 
        if(nextRail.GetComponent<Rail>() == null ) 
        { 
            Debug.LogError("Attached wrong object to Rail button"); 
            return;
        }        
        connectingRail = r;

        if(connectingRail.GetFreeConnectionPoints().Length > 1)
        {
            r.HighlightConnectionPoints();
            lightManager.CloseLights();
            startChoosePointForConnection = true;
            newCreatedRail = Instantiate( nextRail ).GetComponent<Rail>();        
        }
        else if(connectingRail.GetFreeConnectionPoints().Length == 1){
            connectingPoint = r.GetFreeConnectionPoints()[0];
            newCreatedRail = Instantiate( nextRail ).GetComponent<Rail>();        
            Connect();
        }
        else{
            Debug.Log("Bağlanılacak bir nokta yok");
        }
    }

    public void ExistingRailConnection(Rail firstRail)
    {
        connectingRail = firstRail;
        if(connectingRail.GetConnectionPoints().Length > 1)
        {
            connectingRail.HighlightConnectionPoints(connectingRail.GetConnectionPoints());
            lightManager.CloseLights();
            startChoosePointForConnection = true;
            willStartChoosePointForExistingConnection = true;
        }
        else if(connectingRail.GetConnectionPoints().Length == 1)
        {
            lightManager.CloseLights();
            HighlightRailsForExistingConnection();
        }
        else
        {
            Debug.Log("Seçebileceğin bir nokta yok");
        }
    }
    public void HighlightRailsForExistingConnection()
    {
        //lightManager.CloseLights();
        if(secondPointForExistingConnection.isInput == false) // çıkışsa
        {
            foreach (Rail rail in rails)
            {
                rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints().Where(s => s.isInput).ToArray());
            }
        }
        else
        {
            foreach (Rail rail in rails) // girişse
            {
                rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints().Where(s => !s.isInput).ToArray());
            }
        }
        startChoosePointForExistingConnection = true;
        willStartChoosePointForExistingConnection = false;
    }
    public void DownlightRails()
    {
        foreach (Rail rail in rails)
        {
            rail.DownlightConnectionPoints();
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
