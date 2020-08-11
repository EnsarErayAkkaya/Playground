using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCollisionController : MonoBehaviour
{
    [SerializeField] Rail rail;
    void OnTriggerEnter(Collider other)
    {
        CollidableBase collidable;
        if(other.transform.GetComponent<CollidableBase>() != null)
        {
            collidable = other.transform.GetComponent<CollidableBase>();
        }
        else if(other.transform.parent.GetComponent<CollidableBase>() != null)
        {
           collidable = other.transform.parent.GetComponent<CollidableBase>();
        }
        else if(other.transform.parent.parent.GetComponent<CollidableBase>() != null)
        {
            collidable = other.transform.parent.parent.GetComponent<CollidableBase>();
        }
        else
            collidable = null;
        Debug.Log(collidable.isMoving);
            
        if(collidable.isMoving == false )
        {
            rail.OnCollisionCallBack( collidable );
        }
        else
        {
            rail.animator.Play("InteractibleCollision");
        }
    }

}
