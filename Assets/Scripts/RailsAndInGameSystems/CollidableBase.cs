using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableBase : MonoBehaviour
{
    public float creationTime;
    public bool isStatic;
    public CollidableBase lastCollided = null;
    public float lastCollisionTime;
}
