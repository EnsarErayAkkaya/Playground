using UnityEngine;

[System.Serializable]
public class Vector3Ser
{
    public float x,y,z;
    public Vector3Ser(float x, float y , float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public static implicit operator Vector3(Vector3Ser v)  
    {  
        return new Vector3(v.x,v.y,v.z);
    }
    public static implicit operator Vector3Ser(Vector3 v)  
    {  
        return new Vector3Ser(v.x,v.y,v.z);
    }
}