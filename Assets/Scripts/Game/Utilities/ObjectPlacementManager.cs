using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementManager : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GameUIManager uIManager;
    [SerializeField] RailManager railManager;
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] PlaygroundManager playgroundManager;
    [SerializeField] CameraManager cameraManager;

    [Header("")]
    public bool isPlacing = false;
    public LayerMask placementLayer;

    PlacementType placementType;  
    GameObject placingObject;
    float height;
    [SerializeField] RailMover railMover;
    void Update()
    {
        if(isPlacing)
        {
            if(Input.GetMouseButtonDown(0))
            {
                ObjectPlaced();
            }
        }
    }
    void FixedUpdate()
    {
        if(isPlacing)
        {
            Ray ray = cameraManager.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, objectChooser.maxDistance, placementLayer, QueryTriggerInteraction.Collide))
            {
                placingObject.transform.position = new Vector3(hit.point.x, height, hit.point.z);
                if(placementType == PlacementType.RailSystem)
                {
                    placingObject.transform.position = playgroundManager.ClampField(placingObject.transform.position
                        , railMover.minX, railMover.maxX, railMover.minZ
                        ,railMover.maxZ );
                }
                else
                     placingObject.transform.position = playgroundManager.ClampPoisiton(placingObject.transform.position);
            }
        }
    }
    void ObjectPlaced()
    {
        isPlacing = false;
        if(placementType == PlacementType.Rail)
        {
            //placingObject.GetComponent<Rail>().Search();
            placingObject.GetComponent<CollidableBase>().ActivateColliders();
            objectChooser.Choose(placingObject);
        }
        else if(placementType == PlacementType.Env)
        {
            placingObject.GetComponent<CollidableBase>().ActivateColliders();
            objectChooser.Choose(placingObject);
        }
        else if(placementType == PlacementType.RailSystem)
        {
            placingObject.GetComponent<RailMover>().MovingComplated();
        }
        uIManager.buttonLock = false;
        placingObject = null;
    }
    /// <summary>
    /// Call this for place an object.
    /// </summary>
    public void PlaceMe(GameObject obj, PlacementType type)
    {
        if( type != PlacementType.RailSystem && obj.GetComponent<CollidableBase>().isStatic)
            return;    
        placingObject = obj;
        placementType = type;
        if(type == PlacementType.Rail )
        {
            height = railManager.railHeight;
        }
        else if(type == PlacementType.RailSystem)
        {
            height = objectChooser.choosenObject.transform.position.y;
            placingObject.GetComponent<RailMover>().StartMoving();
        }
        isPlacing = true;
    }
    public void PlaceMe(GameObject obj, PlacementType type, float _height)
    {
        placingObject = obj;
        placementType = type;
        height = _height;
        isPlacing = true;
    }
}
public enum PlacementType
{
    Rail, Env, RailSystem
}