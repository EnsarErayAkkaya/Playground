using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChooser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ObjectPlacementManager placementManager;
    [SerializeField] GameUIManager UIManager;

    [Header("")]
    public IInteractible choosenObject;
    void FixedUpdate()
    {
        // if we are placing an object
        // we can not choose anything
        if(placementManager.isPlacing)
            return;
         
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider.tag == "Interactible")
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(hit.collider.GetComponent<IInteractible>() != choosenObject)
                    {
                        Choose(hit.collider.gameObject);
                    }
                    // Choosen Object Will Glow
                    // Buttons will appear
                    //
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0))
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
        UIManager.SetInteractible(choosenObject);
        choosenObject.Glow( true );
    }
    void Unchoose()
    {
        if(choosenObject != null)
        {
            choosenObject.Glow( false );
            choosenObject = null;
            UIManager.SetInteractible(null);
        }
    }
    public bool AmITheChoosenOne(IInteractible i)
    {
        return (i == choosenObject) ? true : false;
    }
}
