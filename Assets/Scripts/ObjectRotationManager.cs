using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationManager : MonoBehaviour
{
    [SerializeField] float rotateAngle;
    public bool isRotating;
    GameObject rotatingObject;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            isRotating = true;
        }
        if( isRotating )
        {
            rotatingObject.transform.RotateAround(transform.position, transform.up, rotateAngle);
            isRotating = false;
        }
    }
    public void SetRotatingObject(GameObject obj)
    {
        rotatingObject = obj;
    }
    public void CleanRotatingObject()
    {
        rotatingObject = null;
    }
}
