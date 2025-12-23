using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class gen1 : MonoBehaviour
{
    float[] xpSet = { -3.2f, -1.28f, 1.28f, 3.2f } ;
    float[] ypSet = { 2.4f, 3.2f, 4f, 4.8f } ;
    public GameObject myPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR

        Vector3 p ;
        for (int i = 0; i < 20; i++)
        {
            p.z = 2 + 8 * i;


            for (int j = 0; j < 4; j++)
            {
                int Ran = Random.Range(0, 4);

                p.x = xpSet[j];
                p.y = ypSet[Ran];

                float random = Random.Range(0f, 1f);
               
                if (random < 0.05)
                {
                    GameObject GOt = PrefabUtility.InstantiatePrefab(myPrefab) as GameObject;
                    GOt.transform.position = p;
                    GOt.transform.SetParent(transform);
                }
                
                
            }
        }


#endif


    }


}
