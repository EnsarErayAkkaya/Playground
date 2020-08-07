using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour
{
    public Vector3 lastPosition;
    [SerializeField] ObjectChooser objectChooser;
    public void StartMoving()
    {
        foreach (InteractibleBase item in objectChooser.choosenObjects)
        {
            item.DisableColliders();
        }
    }
    public void IsThereCollision()
    {
        foreach (InteractibleBase item in objectChooser.choosenObjects)
        {
            item.ActivateColliders();
        }
        foreach (InteractibleBase item in objectChooser.choosenObjects)
        {
            if( Time.time - item.lastCollisionTime < 1f  )
            {
                Debug.Log("çarpışma bulundu");
                objectChooser.transform.position = lastPosition;
                break;
            }
        }
    }
    
}
