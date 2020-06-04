using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] float rotateAngle;
    public bool isRotating;
    void Update()
    {
        if( isRotating && Input.GetKeyDown(KeyCode.R) )
        {
            transform.RotateAround(transform.position, transform.up, rotateAngle);
        }
    }
}
