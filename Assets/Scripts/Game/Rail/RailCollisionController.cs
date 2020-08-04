using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCollisionController : MonoBehaviour
{
    [SerializeField] Rail rail;
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Interactible"))
        {
            if(other.transform.GetComponent<CollidableBase>() != null)
            {
                rail.OnCollisionCallBack( other.transform.GetComponent<CollidableBase>() );
            }
            else if(other.transform.parent.GetComponent<CollidableBase>() != null)
            {
                rail.OnCollisionCallBack( other.transform.parent.GetComponent<CollidableBase>() );
            }
            else if(other.transform.parent.parent.GetComponent<CollidableBase>() != null)
            {
                rail.OnCollisionCallBack( other.transform.parent.parent.GetComponent<CollidableBase>() );
            }
        }
    }

}
