using UnityEngine;
public class CameraManager : MonoBehaviour
{
    public TrainManager trainManager;
    public Camera rtsCamera;
    public Camera trainViewCamera;
    public Camera freeFlyCamera;
    
    ViewStyle[] styles = { ViewStyle.RTS, ViewStyle.TRAINVIEW, ViewStyle.FREEFLY };
    ViewStyle selectedStyle = new ViewStyle();
    private int activeIndex;
    public Camera activeCamera;


    public void ChangeStyle()
    {
        activeIndex++;
        if( activeIndex >= styles.Length )
        {
            activeIndex = 0;
        }
        selectedStyle = styles[activeIndex];
        switch (selectedStyle)
        {
            case ViewStyle.RTS:
                rtsCamera.gameObject.SetActive(true);
                trainViewCamera.gameObject.SetActive(false);
                freeFlyCamera.gameObject.SetActive(false);
                activeCamera = rtsCamera;
            break;
            case ViewStyle.FREEFLY:
                 freeFlyCamera.transform.position = activeCamera.transform.position;
                freeFlyCamera.transform.rotation = activeCamera.transform.rotation;
                rtsCamera.gameObject.SetActive(false);
                trainViewCamera.gameObject.SetActive(false);
                freeFlyCamera.gameObject.SetActive(true);
                activeCamera = freeFlyCamera;
            break;
            case ViewStyle.TRAINVIEW:
                if(trainManager.trains.Count > 0)
                {
                    trainViewCamera.GetComponent<OrbitCamera>().target = trainManager.trains[0].transform;
                    rtsCamera.gameObject.SetActive(false);
                    trainViewCamera.gameObject.SetActive(true);
                    freeFlyCamera.gameObject.SetActive(false);
                    activeCamera = trainViewCamera;
                }
                else
                {
                    Debug.Log("no train");
                    ChangeStyle();
                }
            break;
            default:
                rtsCamera.gameObject.SetActive(false);
                trainViewCamera.gameObject.SetActive(false);
                freeFlyCamera.gameObject.SetActive(true);
                activeCamera = freeFlyCamera;
            break;
        }
    }
}
public enum ViewStyle
{
    RTS, TRAINVIEW, FREEFLY 
}
