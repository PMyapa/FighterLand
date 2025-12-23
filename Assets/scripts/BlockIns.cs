using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIns : MonoBehaviour
{

    public GameObject block;



    public void SpawnBlock(float c , float m)
    {
        block.GetComponent<targetGenerator>().createtg(c , m);
    }       
   
}
