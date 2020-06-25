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
    public IInteractible choosenObject;
    public Vector3 hitPoint;
    void FixedUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject() || railManager.choosingConnectionPoints)return;
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
                hitPoint = hit.point;
                if(hit.collider.tag == "Interactible")
                {
                    if(hit.collider.GetComponent<IInteractible>() != choosenObject)
                    {
                        Choose(hit.collider.gameObject);
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
        Unchoose();
        choosenObject = obj.GetComponent<IInteractible>();
        UIManager.SetInteractible(obj);
        choosenObject.Glow( true );
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
    public bool AmITheChoosenOne(IInteractible i)
    {
        return (i == choosenObject) ? true : false;
    }
}
