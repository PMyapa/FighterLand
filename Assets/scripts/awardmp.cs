using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class awardmp : MonoBehaviour
{
    player playerMovement;
    Gun gubCode;

    [System.Serializable]
    public enum mptype {gp, fp, hp }

    public mptype myp;

   
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "player")
        {

            playerMovement = GameObject.FindObjectOfType<player>();
            if (myp == mptype.gp)
            {
                playerMovement.GetMp(1f,0,0);
            }
            else if (myp == mptype.fp)
            {
                playerMovement.GetMp(0,1f,0);
            }
            if (myp == mptype.hp)
            {
                playerMovement.GetMp(0,0,1f);
            }
            
            Destroy(gameObject);
        }

    }

}
