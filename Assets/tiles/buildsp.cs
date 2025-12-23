using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildsp : MonoBehaviour
{
    public Mesh[] bv2s;
    public Material[] bv2mats;

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
            int r2 = Random.Range(0, bv2mats.Length);


            this.transform.GetChild(i).gameObject.GetComponent<MeshFilter>().mesh = bv2s[r1];
            this.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = bv2mats[r2];
        }
    }
}
