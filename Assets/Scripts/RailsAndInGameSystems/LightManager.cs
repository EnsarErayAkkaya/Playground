using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light directionalLight;

    public void CloseLights()
    {
        directionalLight.intensity = 0;
    }
    public void OpenLights()
    {
        directionalLight.intensity = 1;
    }
}
