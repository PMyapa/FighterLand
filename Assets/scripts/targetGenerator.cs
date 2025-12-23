using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetGenerator : MonoBehaviour
{

    float[] xpSet = { -3.3f, -1.1f, 1.1f, 3.3f };
    float[] ypSet = { 2.4f, 3.2f, 4f, 4.8f, 5.6f, 6.4f };
    public GameObject[] myPrefab;
    public GameObject[] magicPrefab;
    copterp[] copters = new copterp[4];
    public bool tc;

    // Start is called before the first frame update
    public void createtg(float c , float m)
    {
        Vector3 p;
        Vector3 q = this.transform.position;
        tc = false;

        for (int i = 0; i < 20; i++)
        {
            p.z = q.z + 2 + 8 * i;



            for (int j = 0; j < 4; j++)
            {
                p.x = q.x + xpSet[j];

                float random = Random.Range(0f, 1f);
                float random1 = Random.Range(0f, 1f);
                int Ran1 = Random.Range(0, 4);
                int Ran2 = Random.Range(2, 6);

                if (copters[j] == null)
                {
                    copters[j] = new copterp();
                }

                float targetsMul = 0.14f - 0.01f*c;
                float targetsMul_1 = targetsMul - 0.01f;
                float targetsMul_0 = targetsMul - 0.04f;


                if (random < (targetsMul* c) && !tc)
                {

                    p.y = q.y + 0.12f;
                    tc = true;

                    if (random < (targetsMul_0 * c))
                    {                      
                        GameObject GOt = Instantiate(myPrefab[0], p, Quaternion.identity);
                        GOt.transform.SetParent(transform);                        

                    }
                    else if (random < (targetsMul_1*c))
                    {
                        GameObject GOt = Instantiate(myPrefab[1], p, Quaternion.identity);
                        GOt.transform.position = p;
                        GOt.transform.SetParent(transform);
                    }
                    else if (random < (targetsMul*c))
                    {
                        GameObject GOt = Instantiate(myPrefab[2], p, Quaternion.identity);
                        GOt.transform.position = p;
                        GOt.transform.SetParent(transform);
                    }
                }
                else
                {
                    tc = false;
                }

               
                if (random1 < 0.05)
                {
                    p.y = q.y + ypSet[Ran1];

                    if (i < 18 && m > 10f)
                    {
                        copters[j].gaps.Add(i + 1);
                        copters[j].gaps.Add(i + 2);
                        copters[j].gaps.Add(i + 3);


                        Vector3 pp = p;
                        pp.z = p.z + 24;
                        GameObject GOt = Instantiate(myPrefab[3], pp, Quaternion.identity);
                        GOt.transform.SetParent(transform);
                    }


                }
                else if (random1 < 0.2)
                {
                    p.y = q.y + ypSet[Ran2];

                    if (!copters[j].gaps.Contains(i))
                    {
                        if (m < 10)
                        {

                            copters[j].gaps.Add(i + 1);
                            GameObject GOtm = Instantiate(magicPrefab[0], p, Quaternion.identity);
                            GOtm.transform.SetParent(transform);
                        }
                        else if (m < 20)
                        {

                            copters[j].gaps.Add(i + 1);
                            GameObject GOtm = Instantiate(magicPrefab[1], p, Quaternion.identity);
                            GOtm.transform.SetParent(transform);
                        }
                        else
                        {

                            copters[j].gaps.Add(i + 1);
                            GameObject GOtm = Instantiate(magicPrefab[2], p, Quaternion.identity);
                            GOtm.transform.SetParent(transform);
                        }


                    }

                }



            



            }

        }

    }

  
    public class copterp
    {
        //public Vector3[] cops;
        public List<int> gaps = new List<int>();
    }

}
