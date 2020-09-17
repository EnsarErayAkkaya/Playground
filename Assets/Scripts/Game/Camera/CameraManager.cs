using UnityEngine;
public class CameraManager : MonoBehaviour
{
    TrainManager trainManager;
    public Camera rtsCamera;
    public Camera trainViewCamera;
    public Camera izoCamera;
    
    ViewStyle[] styles = { ViewStyle.RTS, ViewStyle.TRAINVIEW, ViewStyle.IZO };
    ViewStyle selectedStyle = new ViewStyle();
    private int activeIndex;
    public Camera activeCamera;

    private void Start() 
    {
        trainManager = FindObjectOfType<TrainManager>();    
    }


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
                izoCamera.gameObject.SetActive(false);
                activeCamera = rtsCamera;
            break;
            case ViewStyle.IZO:
                rtsCamera.gameObject.SetActive(false);
                trainViewCamera.gameObject.SetActive(false);
                izoCamera.gameObject.SetActive(true);
                activeCamera = izoCamera;
            break;
            case ViewStyle.TRAINVIEW:
                if(trainManager.trains.Count > 0)
                {
                    trainViewCamera.GetComponent<OrbitCamera>().target = trainManager.trains[0].transform;
                    rtsCamera.gameObject.SetActive(false);
                    trainViewCamera.gameObject.SetActive(true);
                    izoCamera.gameObject.SetActive(false);
                    activeCamera = trainViewCamera;
                }
                else
                {
                    Debug.Log("no train");
                    ChangeStyle();
                }
            break;
            default:
                rtsCamera.gameObject.SetActive(true);
                trainViewCamera.gameObject.SetActive(false);
                izoCamera.gameObject.SetActive(false);
                activeCamera = rtsCamera;
            break;
        }
    }
}
public enum ViewStyle
{
    RTS, TRAINVIEW, IZO 
}
