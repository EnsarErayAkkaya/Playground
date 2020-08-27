using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : InteractiveContent
{
    PlayGround playGround;
    float X;
    float Z;

    float verticalVelocity;
    public float minVerticalVelocity;
    public float maxVerticalVelocity;
    
    public float minMaxHeight;
    public float maxMaxHeight;
    float maxHeight;

    float horizontalVelocity;
    public float minHorizontalVelocity;
    public float maxHorizontalVelocity;

    float lastDirectionChangeTime;
    public float directionChangeInterval;

    Vector3 pos;

    void Start()
    {
        pos = transform.position;
        maxHeight = Random.Range(minMaxHeight, maxMaxHeight);
        verticalVelocity = Random.Range(minVerticalVelocity, maxVerticalVelocity);
        horizontalVelocity = Random.Range(minHorizontalVelocity, maxHorizontalVelocity);
    }
    public void SetBaloon(PlayGround p)
    {
        playGround = p;
    }
    void Update()
    {
        if( pos.y < maxHeight )
        {
            if(Time.time - lastDirectionChangeTime > directionChangeInterval)
            {
                lastDirectionChangeTime = Time.time;
                X = Random.Range(-1.0f,1.0f);
                Z = Random.Range(-1.0f,1.0f);
            }
            pos += new Vector3(X * horizontalVelocity, verticalVelocity, Z*horizontalVelocity) * Time.deltaTime;
            transform.position = pos;
        }
        else if(pos.y >= maxHeight)
        {
            if(Time.time - lastDirectionChangeTime > directionChangeInterval)
            {
                lastDirectionChangeTime = Time.time;
                X = Random.Range( -0.5f, 0.5f );
                Z = Random.Range( -0.5f, 0.5f );
            }
            pos.x = Mathf.Clamp( pos.x, playGround.minX, playGround.maxX );
            pos.z = Mathf.Clamp( pos.z, playGround.minZ, playGround.maxZ );

            pos += new Vector3(X * horizontalVelocity, 0, Z * horizontalVelocity) * Time.deltaTime;
            transform.position = pos;
        }      
    }

    public override void Interact()
    {
        Debug.Log("Baloon overrided interraction");
    } 
}
