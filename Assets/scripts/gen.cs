using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class gen : MonoBehaviour
{
    //float[] xpSet = { -3.2f, -1.28f, 1.28f, 3.2f } ;
    float[] xpSet = { -3.3f, -1.1f, 1.1f, 3.3f } ;
    public GameObject[] myPrefab;
    public GameObject reward;
    public bool tc;
    public bool rc;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR

        Vector3 p ;
        rc = false;

        for (int i = 0; i < 20; i++)
        {
            p.z = 2 + 8 * i;



            for (int j = 0; j < 4; j++)
            {
                p.x = xpSet[j];
                p.y = 0.12f;

                float random = Random.Range(0f, 1f);

                if (random < 0.4 && !tc)
                {
                    tc = true;

                    if (random < 0.25)
                    {
                        if (random > 0.2 && !rc)
                        {
                            GameObject GOt = PrefabUtility.InstantiatePrefab(reward) as GameObject;
                            GOt.transform.position = p;
                            GOt.transform.SetParent(transform);
                            rc = true;
                        }
                        else
                        {

                            GameObject GOt = PrefabUtility.InstantiatePrefab(myPrefab[0]) as GameObject;
                            GOt.transform.position = p;
                            GOt.transform.SetParent(transform);
                        }

                    }
                    else if (random < 0.36)
                    {
                        GameObject GOt = PrefabUtility.InstantiatePrefab(myPrefab[1]) as GameObject;
                        GOt.transform.position = p;
                        GOt.transform.SetParent(transform);
                    }
                    else if (random < 0.4)
                    {
                        GameObject GOt = PrefabUtility.InstantiatePrefab(myPrefab[2]) as GameObject;
                        GOt.transform.position = p;
                        GOt.transform.SetParent(transform);
                    }
                }
                else
                {
                    tc = false;
                }
                 
                
            }
        }


#endif


    }


}
