using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankMax : Target
{


    [SerializeField] Material tanDesTex;
    [SerializeField] Material tanDamTex;
    [SerializeField] Material tanDesTexPlane;
    [SerializeField] Mesh tankDesMesh;


    override public void TakeDamage(float amount)
    {
        if (!targetAlive)
        {
            return;
        }

        base.TakeDamage(amount);

        int starcount;

        if (amount <= 50)
        {
            starcount = 1;
        }
        else if (amount <= 55)
        {
            starcount = 2;
        }
        else if (amount <= 60)
        {
            starcount = 3;
        }
        else if (amount <= 65)
        {
            starcount = 4;
        }
        else if (amount <= 70)
        {
            starcount = 5;
        }
        else
        {
            starcount = 6;
        }



        base.Stars(starcount);




    }


    override public void Die()
    {
        base.Die();

        int desFromChild = 2;


        Transform sPlane = transform.GetChild(1);
        sPlane.GetComponent<Renderer>().material = tanDesTexPlane;


        Transform tank = transform.GetChild(0);

        tank.GetComponent<MeshFilter>().mesh = tankDesMesh;
        tank.GetComponent<Renderer>().material = tanDesTex;


        for (int i = desFromChild; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }


    override public void Damage()
    {
        Transform tank = transform.GetChild(0);
        tank.GetComponent<Renderer>().material = tanDamTex;

        if (transform.childCount < 4)
        {
            return;
        }

        Destroy(transform.GetChild(3).gameObject);
    }
}
