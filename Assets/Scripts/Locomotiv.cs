using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotiv : MonoBehaviour
{
    public Vector3[] poses;
    [SerializeField] GameObject[] vagons;
    [SerializeField]float getNextPosTime, vagonDistance, moveSpeed;
    float posTakeTime;
    int posesIndex;
    [SerializeField] int rotationLerpModifier;
    public bool move;

    void LateUpdate()
    {
        if(!move)
            return;

        posTakeTime -= Time.deltaTime;
        for (int i = 0; i < vagons.Length; i++)
        {
            vagons[i].transform.position = Vector3.Lerp( vagons[i].transform.transform.position, poses[0], moveSpeed * Time.deltaTime);
            vagons[i].transform.rotation = Quaternion.Lerp( vagons[i].transform.rotation, Quaternion.LookRotation( poses[0] ), rotationLerpModifier * Time.deltaTime );
        }
        if(posTakeTime <= 0)
        {
            posTakeTime = getNextPosTime;
            posesIndex++;

            if(posesIndex > 4)
            {
                posesIndex = 4;
                for (int i = 1; i < poses.Length; i++)
                {
                    poses[i-1] = poses[i];
                }
            }
            poses[posesIndex] = transform.position;
        }

    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Vector3 size = new Vector3(.3f,.3f,.3f);
        for (int i = 0; i < poses.Length; i++)
        {
            Gizmos.DrawCube(poses[i], size);
        }
    }
}
