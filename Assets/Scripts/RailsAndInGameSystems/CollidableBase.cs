using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableBase : MonoBehaviour
{
    public float creationTime, lastCollisionTime, lastEditTime = 0;
    public bool isStatic;
    public CollidableBase lastCollided = null;
}
