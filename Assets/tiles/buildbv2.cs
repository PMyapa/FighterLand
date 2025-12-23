using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildbv2 : MonoBehaviour
{
    public Mesh[] bv2s;

    // Start is called before the first frame update
    void Start()
    {
        setbv2();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setbv2()
    {


        int childc = this.transform.childCount;

        for (int i = 0; i < childc; i++)
        {
            int r1 = Random.Range(0, bv2s.Length);


            this.transform.GetChild(i).gameObject.GetComponent<MeshFilter>().mesh = bv2s[r1];
        }
    }
}
