using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveContent : MonoBehaviour 
{
    public virtual void Interact()
    {
        Debug.Log(gameObject.name + " interacted.");
    }
}
