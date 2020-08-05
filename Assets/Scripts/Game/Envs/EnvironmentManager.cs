using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] List<EnvironmentObject> environments;
    [SerializeField] float rotateAngle = 90;
    void Start()
    {
        foreach (EnvironmentObject item in FindObjectsOfType<EnvironmentObject>().ToList())
        {
            environments.Add(item);
        }
    }

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
    public List<EnvironmentObject> GetEnvironments()
    {
        return environments;
    }
}
