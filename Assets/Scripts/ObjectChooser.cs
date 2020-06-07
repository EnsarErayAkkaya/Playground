using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChooser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ObjectPlacementManager placementManager;
    [SerializeField] GameUIManager UIManager;

    [Header("")]
    public GameObject choosenObject;
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
                    Choose(hit.collider.gameObject);
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
        choosenObject = obj;
        UIManager.SetInteractible(obj);
    }
    void Unchoose()
    {
        choosenObject = null;
        UIManager.SetInteractible(null);
    }
}
