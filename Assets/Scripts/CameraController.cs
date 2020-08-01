using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera camera;

    [Header("Rotation")]
    [SerializeField] float speedH = 2.0f;
    [SerializeField] float speedV = 2.0f;
    [SerializeField] float yaw = 0;
    [SerializeField] float pitch = 0;
    
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    public string movingAxis = "Mouse ScrollWheel";
    private float ScrollWheel
    {
        get { return Input.GetAxis(movingAxis); }
    }

    void Start()
    {
        yaw = camera.transform.rotation.eulerAngles.x;
        pitch = camera.transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        Rotation();
        Move();
    }
    void Rotation()
    {
        if(Input.GetMouseButton(1))
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            camera.transform.eulerAngles = new Vector3(pitch, yaw, 0); 
        }
    }
    void Move()
    {
        if(ScrollWheel > 0)
        {
            transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
        }
        else if(ScrollWheel < 0)
        {
            transform.position -= this.transform.forward * moveSpeed * Time.deltaTime;
        }
        
    }
}
