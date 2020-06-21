using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotiv : MonoBehaviour
{
    public LocomotivePosition[] poses;
    [SerializeField] GameObject vagon;
    [SerializeField]float getNextPosTime, moveSpeed;
    float posTakeTime;
    int posesIndex;
    [SerializeField] int rotationLerpModifier;
    public bool move;

    void LateUpdate()
    {
        if(!move )
            return;

        posTakeTime -= Time.deltaTime;

        vagon.transform.position = Vector3.Lerp( vagon.transform.transform.position, poses[0].pose, moveSpeed * Time.deltaTime);
        vagon.transform.rotation = Quaternion.Lerp( vagon.transform.rotation, poses[0].rotation, rotationLerpModifier * Time.deltaTime );
    
        if(posTakeTime <= 0)
        {
            posTakeTime = getNextPosTime;

            if(posesIndex > 2)
            {
                posesIndex = 2;
                for (int i = 1; i < poses.Length; i++)
                {
                    poses[i-1] = poses[i];
                }
            }
            poses[posesIndex].pose = transform.position;
            poses[posesIndex].rotation = transform.rotation;
            posesIndex++;
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
