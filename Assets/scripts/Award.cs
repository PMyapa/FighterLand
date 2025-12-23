using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award : MonoBehaviour
{
    player playerMovement;
    Gun gubCode;
    Card myCard;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<player>();
        gubCode = GameObject.FindObjectOfType<Gun>();
        myCard = this.transform.parent.GetComponent<Card>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "player")
        {
            //Debug.Log("awrd");


            if (myCard.thisAward == Card.awardType.fuel)
            {
                playerMovement.awardFuel();
            }
            else if (myCard.thisAward == Card.awardType.life)
            {
                playerMovement.awardLife();
            }
            else if (myCard.thisAward == Card.awardType.ammo)
            {
                gubCode.awardMisile();
            }

            Destroy(transform.parent.gameObject);
        }

        //kill player
    }

}
