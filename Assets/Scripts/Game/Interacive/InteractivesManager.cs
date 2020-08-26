using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivesManager : MonoBehaviour
{
    [SerializeField]PlaygroundManager playgroundManager;
    PlayGround p;
    [Header("baloons")]
    #region baloons
        public GameObject baloonPrefab;
        public int baloonCount;
        public float baloonStartingHeight;
    #endregion
    [Header("clouds")]
    #region clouds
        public GameObject cloudPrefab;
        public int cloudCount;
        public float cloudStartingHeight;
    #endregion
    void Start()
    {
        StartCoroutine( InitializeCoroutine() );
    }
    IEnumerator InitializeCoroutine()
    {
        yield return new WaitForSeconds(.3f);
        SetPlayGround();
        CreateBaloons();
        CreateClouds();
    }
    public void SetPlayGround()
    {
        p = playgroundManager.playground;
    }
    public void CreateClouds()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        for (int i = 0; i < cloudCount; i++)
        {
            Vector3 pos = new Vector3( Random.Range( (float)p.minX, (float)p.maxX ), cloudStartingHeight,  Random.Range( (float)p.minZ, (float)p.maxZ ));
            Instantiate(cloudPrefab,pos,Quaternion.identity).GetComponent<Cloud>().SetDirection(x,z);
        }
    }
    public void CreateBaloons()
    {
        for (int i = 0; i < baloonCount; i++)
        {
            Vector3 pos = new Vector3( Random.Range( (float)p.minX, (float)p.maxX ), baloonStartingHeight,  Random.Range( (float)p.minZ, (float)p.maxZ ));
            Instantiate(baloonPrefab,pos,Quaternion.identity);
        }
    }
}
