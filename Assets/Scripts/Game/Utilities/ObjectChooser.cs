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
    [SerializeField] CameraManager cameraManager;
    public Transform multipleObjectParent;

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
            Ray ray = cameraManager.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, maxDistance, choosenLayers,QueryTriggerInteraction.Collide))
            {
                if(hit.transform.parent != null && hit.transform.parent.GetComponent<Rail>() != null ) // raysa
                {
                    if( !isMulitipleSelected && choosenObject != null && hit.transform.parent.gameObject.Equals( choosenObject.gameObject ))
                    {
                        Rail r = hit.transform.parent.GetComponent<Rail>();
                        choosenObjects = railManager.GetConnectedRails(r);

                        foreach (var item in choosenObjects)
                        {
                            if(item.isStatic)
                            {
                                choosenObjects = null;
                                break;
                            }
                        }
                        if(choosenObject != null && choosenObjects != null && choosenObjects.Count > 1)
                        {
                            foreach (Transform child in multipleObjectParent)
                            {
                                child.SetParent(null, false);
                            }

                            isMulitipleSelected = true;

                            choosenObjects.Add(r);

                            multipleObjectParent.position = r.transform.position;

                            foreach (var item in choosenObjects)
                            {
                                item.transform.SetParent(multipleObjectParent);
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
                    if(hit.transform.GetComponent<InteractibleBase>() != null)
                    {
                        Choose(hit.transform.gameObject);
                    }
                    else if( hit.transform.parent.GetComponent<InteractibleBase>() != null)
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
                    UnchooseMultiple();
                    // Glow will end
                    // Buttons will disapper
                    //
                }
        }
        
    }
    public void Choose(GameObject obj)
    {
        Unchoose();
        UnchooseMultiple();
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
    public void UnchooseMultiple()
    {
        if(choosenObjects != null)
        {
            foreach (var item in choosenObjects)
            {
                if(item != null)
                {
                    item.Glow(false);
                    item.isSelected = false;
                    item.transform.SetParent(null);
                }
            }
            isMulitipleSelected = false;
            choosenObjects = null;
        }
        
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
            choosenObject.isSelected = false;
            choosenObject = null;
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
