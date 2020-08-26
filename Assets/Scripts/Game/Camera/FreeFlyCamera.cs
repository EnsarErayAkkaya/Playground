using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    Camera cam;
    public HouseManager houseManager;

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
        cam = GetComponent<Camera>();
        yaw = cam.transform.rotation.eulerAngles.x;
        pitch = cam.transform.rotation.eulerAngles.y;
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
            cam.transform.eulerAngles = new Vector3(pitch, yaw, 0); 
        }
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 pos = transform.position; 
        pos += (this.transform.forward * v + this.transform.right * h) * moveSpeed * Time.deltaTime;
        
        if(ScrollWheel > 0)
        {
            pos += this.transform.forward * moveSpeed * Time.deltaTime;
        }
        else if(ScrollWheel < 0)
        {
            pos -= this.transform.forward * moveSpeed * Time.deltaTime;
        }
        
        pos.x = Mathf.Clamp(pos.x,houseManager.minX,houseManager.maxX);
        pos.y = Mathf.Clamp(pos.y,houseManager.minY,houseManager.maxY);
        pos.z = Mathf.Clamp(pos.z,houseManager.minZ,houseManager.maxZ);
        
        transform.position = pos;
    }
}
