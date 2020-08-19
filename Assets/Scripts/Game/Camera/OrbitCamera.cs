using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;
    public Vector3 offSet;
	float currentZoom = 10f;
    public float ZoomSpeed = 4f , MinZoom = 5f , MaxZoom = 15f;
    void LateUpdate()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
		currentZoom = Mathf.Clamp(currentZoom,MinZoom,MaxZoom);

        if(Input.GetMouseButton(1))
        {
            offSet = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed,Vector3.up) * offSet;
        }
        transform.position = target.position - offSet * currentZoom;
        transform.LookAt(target);
    }
}
