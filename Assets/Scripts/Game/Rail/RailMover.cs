using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RailMover : MonoBehaviour
{
    public Vector3 lastPosition;
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] PlaygroundManager playgroundManager;
    public bool moving;
    public List<InteractibleBase> movingObjects;
    public float minX, maxX, minZ, maxZ;
    public void StartMoving()
    {
        minX = 0; maxX = 0; minZ = 0; maxZ = 0;

        movingObjects = null;
        lastPosition = transform.position;
        moving = true;
        movingObjects = objectChooser.choosenObjects;
        foreach (InteractibleBase item in movingObjects)
        {
            item.DisableColliders();
            if(item.transform.localPosition.x < minX )
            {
                minX = item.transform.localPosition.x;
            }
            else if(item.transform.localPosition.x > maxX )
            {
                maxX = item.transform.localPosition.x;
            }

            if(item.transform.localPosition.z < minZ )
            {
                minZ = item.transform.localPosition.z;
            }
            else if(item.transform.localPosition.z > maxZ )
            {
                maxZ = item.transform.localPosition.z;
            }
        }
    }
    public void MovingComplated()
    {
        foreach (InteractibleBase item in movingObjects)
        {
            item.isMoving = true;
            item.ActivateColliders();
        }
        StartCoroutine(CompleteChecks());
    }
    public void ChildCollidedCallBack()
    {
        if(moving)
        {
            objectChooser.objectParent.transform.position = lastPosition;
            moving = false;
            
        }
    }
    IEnumerator CompleteChecks()
    {
        yield return new WaitForSeconds(1f);
        foreach (InteractibleBase item in movingObjects)
        {
            item.isMoving = false;
        }
        if(moving)
        {
            moving = false;
            foreach (InteractibleBase item in movingObjects)
            {
                item.isMoving = false;
            }
        }
    }
    
   
}
