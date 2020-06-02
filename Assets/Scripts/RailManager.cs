using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RailManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public float lineHeight, connectionDistance;
    public List<Rail> rails;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rail r = Instantiate(prefab).GetComponent<Rail>();
        }
    }
}
