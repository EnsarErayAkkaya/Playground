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
    [SerializeField] NavbarUIManager navbarUIManager;

    [Header("")]
    public InteractibleBase choosenObject;
    bool choosing = true;
    void FixedUpdate()
    {
        if( EventSystem.current.IsPointerOverGameObject() || choosing == false || placementManager.isPlacing)return;
        // if we are placing an object
        // we can not choose anything
         
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(hit.collider.tag == "Interactible")
                {
                    /* try
                    { */
                        if(hit.transform.GetComponent<InteractibleBase>() != null && hit.transform.GetComponent<InteractibleBase>() != choosenObject)
                        {
                            Choose(hit.transform.gameObject);
                        }
                        else if( hit.transform.parent.GetComponent<InteractibleBase>() != null && hit.transform.parent.GetComponent<InteractibleBase>() != choosenObject)
                        {
                            Choose(hit.transform.parent.gameObject);
                        }
                    /* }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.Message);
                    } */
                    // Choosen Object Will Glow
                    // Buttons will appear
                    //
                }
                else
                {
                    navbarUIManager.HideNavbar();
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
        Unchoose();
        if(obj == null )
            return;
        Debug.Log(obj.name);
        choosenObject = obj.GetComponent<InteractibleBase>();
        choosenObject.Glow( true );
        UIManager.SetInteractible(obj);
    }
    public void Unchoose()
    {
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
