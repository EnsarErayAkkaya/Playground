using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotiv : MonoBehaviour
{
    [System.Serializable]
    public struct LocomotivePosition 
    {
        public Vector3 pose;
        public Quaternion rotation;
    }
    [SerializeField] Train train;
    public LocomotivePosition[] poses;
    [SerializeField] GameObject vagon;
    [SerializeField]float getNextPosTime, currentSpeed;
    float posTakeTime;
    int posesIndex, posesCount;
    [SerializeField] int rotationLerpModifier;
    public bool move;
    [SerializeField] float normalSpeed, middleSpeed, fastSpeed;
    SpeedType speedType
    {
        get{ return train.speedType; }
    }

    void Start()
    {
        SetSpeed();
        posTakeTime = 0;
    }
    void LateUpdate()
    {
        if(!move )
            return;

        posTakeTime -= Time.deltaTime;

        vagon.transform.position = Vector3.Lerp( vagon.transform.transform.position, poses[0].pose, currentSpeed * Time.deltaTime);
        vagon.transform.rotation = Quaternion.Lerp( vagon.transform.rotation, poses[0].rotation, rotationLerpModifier * Time.deltaTime );
    
        if(posTakeTime <= 0)
        {
            posTakeTime = getNextPosTime;

            if(posesIndex > posesCount)
            {
                posesIndex = posesCount;
                for (int i = 1; i <= posesCount; i++)
                {
                    poses[i-1] = poses[i];
                }
            }
            poses[posesIndex].pose = transform.position;
            poses[posesIndex].rotation = transform.rotation;
            posesIndex++;
        }

    }
    public void SetSpeed()
    {
        if(speedType == SpeedType.x)
        {
            currentSpeed = normalSpeed;
        }
        else if(speedType == SpeedType.x2)
        {
            currentSpeed = middleSpeed;
        }
        else if(speedType == SpeedType.x3)
        {
            currentSpeed = fastSpeed;
        }
    }
    void OnDrawGizmos()
    {
        if(move)
        {
            Vector3 size = new Vector3(.3f,.3f,.3f);
            for (int i = 0; i < poses.Length; i++)
            {
                Gizmos.DrawCube(poses[i].pose, size);
            }
        }
    }
}
