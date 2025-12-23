using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{


    player playerMovement;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "player")
        {

            playerMovement = GameObject.FindObjectOfType<player>();

            playerMovement.Blast();
        }

        //kill player
    }


    

  
}
