using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChooser : MonoBehaviour
{
    [SerializeField,Header("References")] ObjectPlacementManager placementManager;
    [Header("")]
    public GameObject choosenObject;
    void FixedUpdate()
    {
        // if we are placing an object
        // we can not choose a new one
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
                    choosenObject = hit.collider.gameObject;
                    // Choosen Object Will Glow
                    // 
                }
            }
        }
    }
}
