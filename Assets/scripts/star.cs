using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 scaleChange = new Vector3(-0.1f, -0.1f, -0.1f);
        float movespeed = 2f;
        transform.position += Vector3.up * Time.deltaTime * movespeed;



        float starScale = transform.GetChild(0).localScale.x;
        //float starScale = transform.GetChild(0).localScale.x;
        if (starScale > 0)
        {
            starScale -=  4f *Time.deltaTime *Time.deltaTime;
            Vector3 newScale = new Vector3(starScale, starScale, starScale);
            transform.GetChild(0).localScale = newScale;
        }

        //transform.GetChild(0).localScale += scaleChange * Time.deltaTime;
    }
}
