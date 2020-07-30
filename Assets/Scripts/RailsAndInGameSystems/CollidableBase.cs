using UnityEngine;

public class CollidableBase : MonoBehaviour
{
    public float creationTime, lastCollisionTime, lastEditTime = 0;
    public bool isStatic;
    public CollidableBase lastCollided = null;
    public Collider[] colliders;
    public void DisableColliders()
    {
        foreach (var item in colliders)
        {
            item.enabled = false;
        }
    }
    public void ActivateColliders()
    {
        foreach (var item in colliders)
        {
            item.enabled = true;
        }
    }
}
