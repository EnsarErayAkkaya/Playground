using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : InteractibleBase
{
    ObjectPlacementManager placementManager;
    EnvironmentManager environmentManager;

    [SerializeField] float height;
    
    void Start()
    {
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        environmentManager = FindObjectOfType<EnvironmentManager>();
        mesh.GetComponent<Collider>().enabled = false;
     
        if(!isStatic)
            placementManager.PlaceMe(gameObject,PlacementType.Env, height);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Interactible"))
        {
            CollidableBase collidedObject = null;
            if(other.GetComponent<CollidableBase>() != null )
                collidedObject = other.GetComponent<CollidableBase>();
            else if(other.transform.parent.GetComponent<CollidableBase>() != null)
                collidedObject = other.transform.parent.GetComponent<CollidableBase>();
            else
                collidedObject = other.transform.parent.parent.GetComponent<CollidableBase>();

            if( lastCollided == null || (collidedObject.GetHashCode() != lastCollided.GetHashCode()) || Time.time - lastCollisionTime > .9f )
            {
                lastCollided =  collidedObject;
                lastCollisionTime = Time.time;
                if(!this.isStatic) // çarpıştığım obje statik ve ben değilsem
                {
                    if(this.creationTime > collidedObject.creationTime) // oluşmuşum ve çarpmışım
                    {
                        Destroy(gameObject);
                    }
                    else  if(this.lastEditTime > collidedObject.creationTime) // kıpırdamışım ve çarpmışım
                    {
                        Destroy(gameObject);
                        // get back to old pos
                    } 
                }   
            }       
            
        }
        
             
    }

    
    public override void Destroy()
    {
        // If this rail is static you cant delete it 
        if(isStatic)
            return;


        if(environmentManager == null)
            environmentManager = FindObjectOfType<EnvironmentManager>();
        
        // Remove from list
        environmentManager.RemoveEnv(this);

        Destroy(gameObject);    
    }
    public override void  Glow( bool b)
    {
        Debug.Log("c");
        if(b)
        {
            Debug.Log("a");
            mesh.material.SetInt("Vector1_5C3F79E1", 3);
        }
        else{
            Debug.Log("b");
            mesh.material.SetInt("Vector1_5C3F79E1", 0);
        }
    }
    public void ActivateCollider()
    {
        mesh.GetComponent<Collider>().enabled = true;;
    }

}
