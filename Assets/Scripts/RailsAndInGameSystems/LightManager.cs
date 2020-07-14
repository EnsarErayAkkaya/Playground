using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light directionalLight;

    public void CloseLights()
    {
        directionalLight.enabled = false;
    }
    public void OpenLights()
    {
        directionalLight.enabled = true;
    }
}
