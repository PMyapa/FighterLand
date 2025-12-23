using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow1 : MonoBehaviour
{
    public Transform targetObject1;
    public Vector3 cameraOffset1;
    //public Transform LookTarget;
    [SerializeField] float Camangle1;
    [SerializeField] float Camchase1; 


    // Start is called before the first frame update
    private void Start()
    {
        // cameraOffset = transform.position - targetObject.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetpos;

        //targetpos.y = 4f;
        targetpos = targetObject1.position;
        targetpos.y = 30;


        
        transform.position = targetpos;


        

        transform.rotation = Quaternion.Euler(90, 0, 0);

        
    }
}
