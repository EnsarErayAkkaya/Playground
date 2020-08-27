using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : InteractiveContent
{
    PlayGround playGround;

    float X;
    float Z;
    
    public Mesh[] clouds;
    public MeshFilter meshFilter;

    float horizontalVelocity;
    public float minHorizontalVelocity;
    public float maxHorizontalVelocity;

    Vector3 pos;
    void Start()
    {
        meshFilter.mesh = clouds[Random.Range(0, clouds.Length)];
        horizontalVelocity = Random.Range(minHorizontalVelocity, maxHorizontalVelocity);
        pos = transform.position;
    }
    public void SetCloud(float x, float z, PlayGround p)
    {
        X = x;
        Z = z;
        playGround = p;
    }
    void Update()
    {        
        
        pos += new Vector3(X * horizontalVelocity, 0, Z * horizontalVelocity) * Time.deltaTime;
    
        pos.x = Mathf.Clamp( pos.x, playGround.minX, playGround.maxX );
        pos.z = Mathf.Clamp( pos.z, playGround.minZ, playGround.maxZ );
        
        if( pos.x == playGround.minX  )
        {
            pos.x = playGround.maxX;
        }
        else if(pos.x == playGround.maxX)
        {
            pos.x = playGround.minX;
        }

        if( pos.z == playGround.minZ  )
        {
            pos.z = playGround.maxZ;
        }
        else if(pos.z == playGround.maxZ)
        {
            pos.z = playGround.minZ;
        }

        transform.position = pos;
    }
    
    public override void Interact()
    {
        Debug.Log("Baloon overrided interraction");
    } 
}
