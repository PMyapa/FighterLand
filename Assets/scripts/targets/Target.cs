using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health ;
    [SerializeField] GameObject Star;
    [SerializeField] GameObject GStar;
    public bool targetAlive;
    targetMovement tarm;


    [SerializeField] AudioClip blastsfx;
    [SerializeField] AudioSource targetAudiosource;

    player Player;
    Gun guncode;

    private void Start()
    {
        targetAlive = true;
    }

    virtual public void TakeDamage(float amount)
    {

        health -= amount;

        targetAudiosource.PlayOneShot(blastsfx);

        if (health <= 0f)
        {
            Die();
        }
        else
        {
            Damage();
        }
                
       
    }


    virtual public void Die()
    {
        //Destroy(transform.parent.gameObject);
        targetAlive = false;
        tarm = this.transform.parent.GetComponent<targetMovement>();
        tarm.tmDie();

        player p = GameObject.FindObjectOfType<player>();
        p.Countakill();
    }


    

    virtual public void Damage()
    {
       
    }

    virtual public void Stars(int nos)
    {
        Player = GameObject.FindObjectOfType<player>();
        guncode = GameObject.FindObjectOfType<Gun>();

        Vector3 p = this.transform.position;
        Quaternion r = Quaternion.identity;
        Quaternion q = r;
        float assingdq = 0.2f;
        q.z -= (nos * assingdq *0.5f);
        //float fuelGift = 0.25f * nos;
        float fuelGift = 1f * nos;
        float hpGift = 1f * nos;
        float scoreGift = 100f * nos;
        float gunGift = 1f * nos;
        GameObject starpf = Star;
        int combo = 1;

        if (guncode.isSuperCombo)
        {
            starpf = GStar;
            combo = guncode.hitCounter;
        }
        else
        {
            starpf = Star;
            combo = 1;
        }

        for (int i = 0; i < nos; i++)
        {
            q.z += assingdq ;
            GameObject starGO = Instantiate(starpf, p, q, transform.parent);
            Destroy(starGO, 1f);

            
        }
                

        Player.giftFuelPoints(fuelGift * combo);
        Player.giftLifePoints(hpGift * combo);
        Player.giftScore(scoreGift * combo);
        guncode.giftGUnPoints(gunGift * combo);
    }
}
