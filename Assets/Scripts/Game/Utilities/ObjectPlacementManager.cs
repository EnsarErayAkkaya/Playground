using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementManager : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] RailManager railManager;
    [SerializeField] ObjectChooser objectChooser;

    [Header("")]
    public bool isPlacing;
    PlacementType placementType;  
    float height;
    GameObject placingObject;
    public LayerMask placementLayer;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, objectChooser.maxDistance, placementLayer, QueryTriggerInteraction.Collide))
            {
                placingObject.transform.position = new Vector3(hit.point.x, height, hit.point.z);
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
            placingObject.GetComponent<EnvironmentObject>().ActivateCollider();
            objectChooser.Choose(placingObject);
        }
        else if(placementType == PlacementType.RailSystem)
        {
            placingObject.GetComponent<RailMover>().MovingComplated();
        }
        placingObject = null;
    }
    /// <summary>
    /// Call this for place an object.
    /// </summary>
    public void PlaceMe(GameObject obj, PlacementType type)
    {
        placingObject = obj;
        placementType = type;
        if(type == PlacementType.Rail )
        {
            height = railManager.railHeight;
        }
        else if(type == PlacementType.RailSystem)
        {
            height = railManager.railHeight;
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