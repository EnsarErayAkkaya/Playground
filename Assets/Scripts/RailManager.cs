using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RailManager : MonoBehaviour
{
    [SerializeField] GameObject prefab,prefab1,prefab2;
    public float connectionDistance, railHeight;
    [SerializeField] List<Rail> rails;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(prefab);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(prefab1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(prefab2);
        }
    }

    public void RemoveRail(Rail r)
    {
        rails.Remove(r);
    }
    public void AddRail(Rail r)
    {
        rails.Add(r);
    }
    public bool IsFirstRail()
    {
        if(rails.Count > 0)
            return false;
        else{
            return true;
        }
    }
    public Rail GetFirstRail()
    {
        return rails.Find(s => s.isFirst);
    }
}
