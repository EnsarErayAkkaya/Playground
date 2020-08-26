using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : InteractiveContent
{
    float X;
    float Z;

    float verticalVelocity;
    public float minVerticalVelocity;
    public float maxVerticalVelocity;
    public float maxHeight;

    float horizontalVelocity;
    public float minHorizontalVelocity;
    public float maxHorizontalVelocity;

    float lastDirectionChangeTime;
    public float directionChangeInterval;
    void Start()
    {
        verticalVelocity = Random.Range(minVerticalVelocity, maxVerticalVelocity);
        horizontalVelocity = Random.Range(minHorizontalVelocity, maxHorizontalVelocity);
    }
    void Update()
    {
        if( transform.position.y < maxHeight )
        {
            if(Time.time - lastDirectionChangeTime > directionChangeInterval)
            {
                lastDirectionChangeTime = Time.time;
                X = Random.Range(-1.0f,1.0f);
                Z = Random.Range(-1.0f,1.0f);
            }
            transform.position += new Vector3(X * horizontalVelocity, verticalVelocity, Z*horizontalVelocity) * Time.deltaTime;
        }
        else if(transform.position.y >= maxHeight)
        {
            if(Time.time - lastDirectionChangeTime > directionChangeInterval)
            {
                lastDirectionChangeTime = Time.time;
                X = Random.Range( -0.5f, 0.5f );
                Z = Random.Range( -0.5f, 0.5f );
            }
            transform.position += new Vector3(X * horizontalVelocity, 0, Z * horizontalVelocity) * Time.deltaTime;
        }      
    }

    public override void Interact()
    {
        Debug.Log("Baloon overrided interraction");
    } 
}
