using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCollectable : MonoBehaviour
{       
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Train>() != null)
        {
            OnCollision();
        }
        else if(other.transform.parent.GetComponent<Train>() != null)
        {
           OnCollision();
        }
    }
    void OnCollision()
    {
        FindObjectOfType<LevelManager>().TrainCollect();
        Destroy(gameObject);
    }
}
