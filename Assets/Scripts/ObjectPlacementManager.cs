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

    void Update()
    {
        if(isPlacing)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isPlacing = false;
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
            if(Physics.Raycast(ray,out hit))
            {
                placingObject.transform.position = new Vector3(hit.point.x, height, hit.point.z);
            }
        }
    }
    void ObjectPlaced()
    {
        if(placementType == PlacementType.Rail)
        {
            placingObject.GetComponent<Rail>().Search();
        }
        if(placingObject.tag == "Interactible")
        {
            objectChooser.Choose(placingObject);
        }
        placingObject = null;
    }
    /// <summary>
    /// Call this for place an object.
    /// </summary>
    public void PlaceMe(GameObject obj, PlacementType type)
    {
        placingObject = obj;
        if(type == PlacementType.Rail)
        {
            height = railManager.railHeight;
        }

        isPlacing = true;
    }
}
public enum PlacementType
{
    Rail
}