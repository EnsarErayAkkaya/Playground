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
    RailConnectionPoint connectingPoint,connectionChangingPoint, lastEditedRail;
    Vector3 lastEditedRailPos,lastEditedRailAngle;

    // objectPlacementManagerda kullanılan rayların yüksekliğini deopalayan değişken 
    public float railHeight;

    // nokta seçiliyor mu 
    bool startChoosePointForConnection, startChoosePointForExistingConnection, willStartChoosePointForExistingConnection, mouseReleased;
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
            mouseReleased = true;
        }
        if(startChoosePointForConnection == true && !EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetMouseButtonDown(0) && mouseReleased == true)
            {
                mouseReleased = false;
                //highlight ı bitir
                connectingRail.DownlightConnectionPoints();

                Vector3 clickPos = objectChooser.hitPoint;

                if(willStartChoosePointForExistingConnection)
                {
                    foreach (RailConnectionPoint rcp in connectingRail.GetConnectionPoints())
                    {
                        if( connectionChangingPoint == null || Vector3.Distance(clickPos, connectionChangingPoint.point) 
                                    > Vector3.Distance(clickPos, rcp.point) )
                        {
                            connectionChangingPoint = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    HighlightRailsForExistingConnection();
                }
                else
                {                    
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
                    
                    objectChooser.CanChoose();

                    Connect();
                }
            }
        }
        
        if(startChoosePointForExistingConnection == true && !EventSystem.current.IsPointerOverGameObject())
        {   
            if(Input.GetMouseButtonDown(0) && mouseReleased== true)
            {
                DownlightRails();
                lightManager.OpenLights();

                Vector3 clickPos = objectChooser.hitPoint;

                if(connectionChangingPoint.isInput == false) // çıkışsa
                {
                    foreach (Rail rail in rails.Where(s => s != connectingRail))
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
                    foreach (Rail rail in rails.Where(s => s != connectingRail))
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
                
                lastEditedRail = connectionChangingPoint;
                lastEditedRailPos = connectionChangingPoint.point;
                lastEditedRailAngle = connectionChangingPoint.transform.rotation.eulerAngles;

                connectionChangingPoint.rail.CleanConnections();

                startChoosePointForExistingConnection = false;
                
                objectChooser.CanChoose();

                Connect( );
            }
        }
    }

    void Connect()
    {        
        if(connectingPoint.isInput == false) // bağlanılan nokta çıkış ise
        {
            // direk girişi seçili noktaya bağla

            if(connectionChangingPoint == null)
            {
                connectingPoint.connectedPoint = newCreatedRail.GetFreeConnectionPoints().First(s => s.isInput == true);
                AddRail(newCreatedRail);
            }
            else{
                connectingPoint.connectedPoint = connectionChangingPoint;
            }

            RailConnection(connectingPoint);

            // açıyı ayarla 
            connectingPoint.connectedPoint.transform.rotation = Quaternion.Euler(connectingPoint.rail.transform.rotation.eulerAngles + new Vector3(0, connectingPoint.extraAngle,0));

        }
        else // bağlanılan nokta giriş ise
        {
            // çıkışı girişe bağla
            
            if(connectionChangingPoint == null)
            {
                connectingPoint.connectedPoint = newCreatedRail.GetFreeConnectionPoints().First(s => s.isInput == false);
                AddRail(newCreatedRail);
            }
            else
            {
                connectingPoint.connectedPoint = connectionChangingPoint;
            }

            RailConnection(connectingPoint);

            // açıyı ayarla
            connectingPoint.connectedPoint.transform.rotation = Quaternion.Euler(connectingPoint.rail.transform.rotation.eulerAngles - new Vector3(0, connectingPoint.connectedPoint.extraAngle,0));
            
        }

        ConnectCollidingPoints();

        objectChooser.Choose(connectingPoint.connectedPoint.rail.gameObject);
        
        // parentları düzenle
        connectingPoint.connectedPoint.rail.transform.parent = null; // railın parentını tamizle
        connectingPoint.connectedPoint.transform.parent = connectingPoint.connectedPoint.rail.transform; // noktayı railın çocuğu yap
        
        if(newCreatedRail != null)
            newCreatedRail.ShowObject();
        
        connectingPoint = null;
        newCreatedRail = null;
        connectingRail = null;
        connectionChangingPoint = null;
    }
    void RailConnection(RailConnectionPoint a)
    {
        a.connectedPoint.connectedPoint = connectingPoint;
        
        // connectingPoint noktası bağlanılan nokta ve connectingPoint.connectedPoint bağlanan noktadır
        a.connectedPoint.transform.parent = null; // parentı çıkar
        a.connectedPoint.rail.transform.parent = a.connectedPoint.transform; // raili noktasının çocuğu yap
        a.connectedPoint.point = a.point; // noktaların pozisyonunu birleştir
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
            objectChooser.CantChoose();

            connectingRail.HighlightConnectionPoints();

            lightManager.CloseLights();

            startChoosePointForConnection = true;

            newCreatedRail = Instantiate( nextRail ).GetComponent<Rail>();
            newCreatedRail.HideObject();
            newCreatedRail.creationTime = Time.time;   
        }
        else if(connectingRail.GetFreeConnectionPoints().Length == 1){
            connectingPoint = r.GetFreeConnectionPoints()[0];
            newCreatedRail = Instantiate( nextRail ).GetComponent<Rail>();
            newCreatedRail.HideObject();
            newCreatedRail.creationTime = Time.time;           
            Connect();
        }
        else{
            Debug.Log("Bağlanılacak bir nokta yok");
        }
    }
    // it controls free points on connectingRail
    // if a point too close (< 0.1f) it connect points
    // and contuniues with next free point
    void ConnectCollidingPoints()
    {
        if(connectionChangingPoint != null)
        {
            ConnectCollidingRailPoints(connectionChangingPoint.rail);
        }
        else
        {
            ConnectCollidingRailPoints(newCreatedRail);
        }   
    }
    void ConnectCollidingRailPoints(Rail r)
    {
        foreach (RailConnectionPoint firstPoint in r.GetFreeConnectionPoints())
        {
            bool found = false;

            for (int i = 0; i < rails.Where(s => s != r).ToArray().Length; i++)
            {
                foreach (var secondPoint in rails[i].GetFreeConnectionPoints())
                {
                    if(Vector3.Distance(firstPoint.point,secondPoint.point) < 0.1f )
                    {
                        firstPoint.connectedPoint = secondPoint;
                        secondPoint.connectedPoint = firstPoint;
                        found = true;
                        break;
                    }
                }
                if(found)
                    break;
            }
        }
    }


    public void ExistingRailConnection(Rail firstRail)
    {
        connectingRail = firstRail;
        if(connectingRail.GetConnectionPoints().Length > 1)
        {
            objectChooser.CantChoose();

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
        int i = 0;
        if(connectionChangingPoint.isInput == false) // çıkışsa
        {
            foreach (Rail rail in rails.Where(s => s != connectingRail))
            {
                i += rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints().Where(s => s.isInput).ToArray());
            }
        }
        else
        {
            foreach (Rail rail in rails.Where(s => s != connectingRail)) // girişse
            {
                i += rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints().Where(s => !s.isInput).ToArray());
            }
        }
        if(i > 0)
        {
            objectChooser.CantChoose();
            startChoosePointForExistingConnection = true;
        }
        else
        {
            lightManager.OpenLights();
            DownlightRails();
            objectChooser.CanChoose();
            Debug.Log("Uygun bağlanılacak bir nokta yok");
        }
        willStartChoosePointForExistingConnection = false;
    }
    public void GetRailBackToOldPosition()
    {
        MoveRailToPosition(lastEditedRailPos, lastEditedRailAngle, lastEditedRail);
        ConnectCollidingRailPoints(lastEditedRail.rail);
        objectChooser.Choose(lastEditedRail.rail.gameObject);
    }
    void MoveRailToPosition(Vector3 pos,Vector3 rotation, RailConnectionPoint rcp)
    {
        rcp.rail.CleanConnections();
        // connectingPoint noktası bağlanılan nokta ve connectingPoint.connectedPoint bağlanan noktadır
        rcp.transform.parent = null; // parentı çıkar
        rcp.rail.transform.parent = rcp.transform; // raili noktasının çocuğu yap
        
        rcp.point = pos; // eski pozisyona getir

        rcp.transform.rotation = Quaternion.Euler(rotation); // eski açıya döndür

        rcp.rail.transform.parent = null; // railın parentını temizle
        rcp.transform.parent = rcp.rail.transform; // noktayı railın çocuğu yap

    }
    void ConnectTwoPoints(RailConnectionPoint a, RailConnectionPoint b)
    {
        a.connectedPoint = b;
        RailConnection(a);
        // parentları düzenle
        a.connectedPoint.rail.transform.parent = null; // railın parentını temizle
        a.connectedPoint.transform.parent = a.connectedPoint.rail.transform; // noktayı railın çocuğu yap
    }
    public void DownlightRails()
    {
        foreach (Rail rail in rails.Where(s => s != connectingRail))
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
    public Rail GetLastEditedRail()
    {
        if(lastEditedRail != null)
            return lastEditedRail.rail;
        else
            return null;
    }
}
