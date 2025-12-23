using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class magicpointbar : MonoBehaviour
{
    public GameObject mp;
    public GameObject mp1;
    public GameObject mp2;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR

        Vector3 p = this.transform.position;
        
        for (int i = 0; i < 8; i++)
        {
            p.z = p.z + 1f;
            //Instantiate(mp, p, Quaternion.identity, transform);
            if(i <5)
            {
                GameObject GOt = PrefabUtility.InstantiatePrefab(mp) as GameObject;
                GOt.transform.position = p;
                GOt.transform.SetParent(transform);
            }
            else if(i <8)
            {
                GameObject GOt = PrefabUtility.InstantiatePrefab(mp1) as GameObject;
                GOt.transform.position = p;
                GOt.transform.SetParent(transform);
            }
            else
            {
                GameObject GOt = PrefabUtility.InstantiatePrefab(mp2) as GameObject;
                GOt.transform.position = p;
                GOt.transform.SetParent(transform);
            }
            
        }
    

    
#endif

    }
}
