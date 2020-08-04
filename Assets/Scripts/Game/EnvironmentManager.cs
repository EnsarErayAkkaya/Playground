using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] List<EnvironmentObject> environments;
    [SerializeField] float rotateAngle = 90;

    public void CreateEnvironmentObject(GameObject env)
    {
        EnvironmentObject e = Instantiate(env).GetComponent<EnvironmentObject>();
        e.creationTime = Time.time;
        AddEnv( e );
    }
    public void RotateEnv(EnvironmentObject e)
    {
        if(!e.isStatic)
            e.transform.RotateAround(e.transform.position, e.transform.up, rotateAngle);
    }
    public void RemoveEnv(EnvironmentObject e)
    {
        environments.Remove(e);
    }
    public void AddEnv(EnvironmentObject e)
    {
        environments.Add(e);
    }
}
