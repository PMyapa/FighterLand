using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaclet : MonoBehaviour
{


    player playerMovement;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "player")
        {
            playerMovement = GameObject.FindObjectOfType<player>();
            playerMovement.StartGettingShot();
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "player")
        {
            playerMovement = GameObject.FindObjectOfType<player>();
            playerMovement.StopGettingShot();
        }
    }

   


    

  
}
