using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaclec : MonoBehaviour
{

    Target targetScript;
    player playerMovement;

   
    private void OnTriggerEnter(Collider collider)
    {

        targetScript = GetComponent<Target>();
        if (collider.gameObject.name == "player")
        {
            playerMovement = GameObject.FindObjectOfType<player>();
            playerMovement.Blast();
        }

        targetScript.Die();
    }
    
   

   


    

  
}
