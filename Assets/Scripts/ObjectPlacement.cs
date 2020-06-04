using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public PlacementType PlacementType;

    bool isPlacing;
    float height;
    [SerializeField] bool isStatic;
    void Start()
    {
        if(PlacementType == PlacementType.Rail)
        {
            height = FindObjectOfType<RailManager>().railHeight;
        }

        if(isStatic)
            isPlacing = false;
        else
        {
            isPlacing = true;
        }
    }
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
                transform.position = new Vector3(hit.point.x, height, hit.point.z);
            }
        }
    }
    public void ObjectPlaced()
    {
        if(PlacementType == PlacementType.Rail)
        {
            GetComponent<Rail>().isSearching = true;
        }
    }
}
public enum PlacementType
{
    Rail
}