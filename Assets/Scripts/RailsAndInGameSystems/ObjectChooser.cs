using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectChooser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ObjectPlacementManager placementManager;
    [SerializeField] GameUIManager UIManager;
    [SerializeField] RailManager railManager;

    [Header("")]
    public InteractibleBase choosenObject;
    bool choosing = true;
    void FixedUpdate()
    {
        if( EventSystem.current.IsPointerOverGameObject() || choosing == false )return;
        // if we are placing an object
        // we can not choose anything
        if(placementManager.isPlacing)
            return;
         
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(hit.collider.tag == "Interactible")
                {
                    try
                    {
                        
                        if(hit.transform.GetComponent<InteractibleBase>() != null && hit.transform.GetComponent<InteractibleBase>() != choosenObject)
                        {
                            Debug.Log(hit.collider.name + " 0");
                            Choose(hit.collider.gameObject);
                        }
                        else if( hit.transform.parent.GetComponent<InteractibleBase>() != null && hit.transform.parent.GetComponent<InteractibleBase>() != choosenObject)
                        {
                            Debug.Log(hit.collider.name + " 1");
                            Choose(hit.transform.parent.gameObject);
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                    // Choosen Object Will Glow
                    // Buttons will appear
                    //
                }
                else
                {
                    // When we click no where choosenObject will be null
                    Unchoose();
                    // Glow will end
                    // Buttons will disapper
                    //
                }
            }
        }
    }
    public void Choose(GameObject obj)
    {
        Debug.Log(obj.name + "ch");
        Unchoose();
        if(obj == null )
            return;
        
        choosenObject = obj.GetComponent<InteractibleBase>();
        choosenObject.Glow( true );
        UIManager.SetInteractible(obj);
    }
    public void Unchoose()
    {
        if(choosenObject != null)
            Debug.Log(choosenObject.name+ "un");
        if(choosenObject != null)
        {
            try
            {
                choosenObject.Glow( false );
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
            choosenObject = null;
            UIManager.SetInteractible(null);
        }
    }
    public bool AmITheChoosenOne(InteractibleBase i)
    {
        return (i == choosenObject) ? true : false;
    }
    public void CanChoose()
    {
        choosing = true;
    }
    public void CantChoose()
    {
        choosing = false;
    }
}
