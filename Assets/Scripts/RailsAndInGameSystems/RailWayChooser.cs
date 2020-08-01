using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RailWayChooser : MonoBehaviour
{
    [SerializeField] ObjectChooser objectChooser;
    [SerializeField] LightManager lightManager;
    [SerializeField] TrainManager trainManager;
    [SerializeField] LayerMask layer;

    bool choosingInput,choosingOutput;
    Rail rail;

    RailConnectionPoint startPoint, endPoint;
    private bool mouseReleased;

    void Update()
    {  
        if(Input.GetMouseButtonUp(0))
        {
            mouseReleased = true;
        }
    }
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, layer))
        {
            if(choosingInput == true && !EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButtonDown(0) && mouseReleased == true)
                {
                    mouseReleased = false;
                    //highlight ı bitir
                    rail.DownlightConnectionPoints();

                    startPoint = null;
                    foreach (RailConnectionPoint rcp in rail.GetInputConnectionPoints())
                    {
                        if( startPoint == null || Vector3.Distance(hit.point, startPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                        {
                            startPoint = rcp;
                        }
                    }
                    choosingInput = false;
                    
                    ChooseOutputPoint();
                }
            }
            if(choosingOutput == true && !EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButtonDown(0) && mouseReleased == true)
                {
                    mouseReleased = false;
                    //highlight ı bitir
                    rail.DownlightConnectionPoints();

                    endPoint = null;
                    foreach (RailConnectionPoint rcp in rail.GetOutputConnectionPoints())
                    {
                        if( endPoint == null || Vector3.Distance(hit.point, endPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                        {
                            endPoint = rcp;
                        }
                    }
                    objectChooser.CanChoose();
                    lightManager.OpenLights();

                    choosingOutput = false;

                    rail.GetComponent<SplineManager>().SetSpline(startPoint, endPoint);
                    trainManager.ResumeStartedTrain();
                }
            }
        }
    }

    public void ChooseWay(Rail r)
    {
        rail = r;
        RailConnectionPoint[] inputs = rail.GetInputConnectionPoints();
        if(inputs.Length > 1)
        {
            objectChooser.CantChoose();
            lightManager.CloseLights();
            rail.HighlightConnectionPoints( inputs );
            choosingInput = true;
        }
        else if(inputs.Length == 1)
        {
            startPoint = inputs[0];
            ChooseOutputPoint();
        }
    }
    public void ChooseOutputPoint()
    {
        lightManager.CloseLights();
        RailConnectionPoint[] outputs = rail.GetOutputConnectionPoints();
        if(outputs.Length > 1)
        {
            rail.HighlightConnectionPoints( outputs );
            choosingOutput = true;
        }
        else if(outputs.Length == 1)
        {
            endPoint = outputs[0];
            lightManager.OpenLights();
            objectChooser.CanChoose();
            rail.GetComponent<SplineManager>().SetSpline(startPoint, endPoint);
            trainManager.ResumeStartedTrain();
        }
    }
}
