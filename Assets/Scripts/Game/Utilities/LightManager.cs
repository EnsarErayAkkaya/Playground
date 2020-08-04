using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light light;

    public void CloseLights()
    {
        light.intensity = 0;
    }
    public void OpenLights()
    {
        light.intensity = 1;
    }
}
