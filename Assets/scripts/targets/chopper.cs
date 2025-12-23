using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chopper : Target
{

    [SerializeField] ParticleSystem copBlast;

    public AudioSource helicopterAudiosource;

    override public void TakeDamage(float amount)
    {
        if (!targetAlive)
        {
            return;
        }

        base.TakeDamage(amount);

        base.Stars(7);


    }


    override public void Die()
    {
        base.Die();

        helicopterAudiosource.Stop();
        copBlast.Play();

           

        for (int i = 0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }


    override public void Damage()
    {
        Transform tank = transform.GetChild(0);
    }
}
