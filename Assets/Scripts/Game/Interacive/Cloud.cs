using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : InteractiveContent
{
    float X;
    float Z;
    
    public Mesh[] clouds;
    public MeshFilter meshFilter;

    float horizontalVelocity;
    public float minHorizontalVelocity;
    public float maxHorizontalVelocity;

    void Start()
    {
        meshFilter.mesh = clouds[Random.Range(0, clouds.Length)];

        horizontalVelocity = Random.Range(minHorizontalVelocity, maxHorizontalVelocity);
    }
    public void SetDirection(float x, float z)
    {
        X = x;
        Z = z;
    }
    void Update()
    {        
        transform.position += new Vector3(X * horizontalVelocity, 0, Z * horizontalVelocity) * Time.deltaTime;
    }
    
    public override void Interact()
    {
        Debug.Log("Baloon overrided interraction");
    } 
}
