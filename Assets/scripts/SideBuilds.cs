using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBuilds : MonoBehaviour
{
    public GameObject buildBase;
    public float floorheights;

    public int maxHeight;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 p = this.transform.position ;
        p.y += 5f * floorheights;
        int buildHeight = Random.Range(0, maxHeight);


        for (int i = 0; i< buildHeight; i++)
        {
            p.y += floorheights;
            
           GameObject buildFloors= Instantiate(buildBase, p, transform.rotation);
           buildFloors.transform.localScale = this.transform.localScale;
           buildFloors.transform.parent = this.transform;

           



        }

        


    }

  
}
