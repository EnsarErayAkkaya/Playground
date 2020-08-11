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
    public Transform objectParent;

    [Header("")]
    public InteractibleBase choosenObject;
    public List<InteractibleBase> choosenObjects;
    public bool isMulitipleSelected;
    public float maxDistance = 100;
    public LayerMask choosenLayers;
    bool choosing = true;

    void FixedUpdate()
    {
        if( EventSystem.current.IsPointerOverGameObject() || choosing == false || placementManager.isPlacing)return;
        // if we are placing an object
        // we can not choose anything
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, maxDistance, choosenLayers,QueryTriggerInteraction.Collide))
            {                
                if(hit.transform.parent.GetComponent<Rail>() != null ) // raysa
                {
                    if( !isMulitipleSelected && choosenObject != null && hit.transform.parent.gameObject.Equals( choosenObject.gameObject ))
                    {
                        Rail r = hit.transform.parent.GetComponent<Rail>();
                        choosenObjects = railManager.GetConnectedRails(r);
                        
                        if(choosenObject != null && choosenObjects.Count > 1)
                        {
                            foreach (Transform child in objectParent)
                            {
                                child.SetParent(null, false);
                            }

                            isMulitipleSelected = true;

                            choosenObjects.Add(r);

                            objectParent.position = r.transform.position;

                            foreach (var item in choosenObjects)
                            {
                                item.transform.SetParent(objectParent);
                            }
                            ChooseMultiple();
                        }
                    }
                    else
                    {
                        Choose(hit.transform.parent.gameObject);
                    }
                }
                else
                {
                    if(hit.transform.GetComponent<InteractibleBase>() != null && hit.transform.GetComponent<InteractibleBase>() != choosenObject)
                    {
                        Choose(hit.transform.gameObject);
                    }
                    else if( hit.transform.parent.GetComponent<InteractibleBase>() != null && hit.transform.parent.GetComponent<InteractibleBase>() != choosenObject)
                    {
                        Choose(hit.transform.parent.gameObject);
                    }
                }
            }
            else
                {
                    navbarUIManager.HideNavbar();// burdan taşı
                    // When we click no where choosenObject will be null
                    Unchoose();
                    // Glow will end
                    // Buttons will disapper
                    //
                }
        }
        
    }
    public void Choose(GameObject obj)
    {
        Unchoose();
        if(obj == null )
            return;
        choosenObject = obj.GetComponent<InteractibleBase>();
        choosenObject.Glow( true );
        choosenObject.isSelected = true;
        UIManager.SetInteractible(obj);
    }
    public void ChooseMultiple()
    {
        foreach (var item in choosenObjects)
        {
            item.Glow( true );
        }
        UIManager.SetUIMultiple(choosenObjects);
    }
    public void Unchoose()
    {
        if(isMulitipleSelected)
        {
            foreach (var item in choosenObjects)
            {
                item.Glow(false);
                item.isSelected = false;
                item.transform.SetParent(null);
            }
            isMulitipleSelected = false;
            choosenObjects = null;
            choosenObject = null;
            UIManager.SetInteractible(null);
        }
        else
        {
            if(choosenObject != null)
            {
                try
                {
                    choosenObject.isSelected = false;
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
