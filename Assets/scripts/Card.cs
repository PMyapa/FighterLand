using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    player playerMovement;
    Gun gunCode;
    public float movementStartDis;
    public enum awardType { none ,life, fuel , ammo};
    public awardType thisAward;
    public GameObject fuelAward, lifeAward, misileAward , rays;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<player>();
        gunCode = GameObject.FindObjectOfType<Gun>();
        thisAward = awardType.none;
    }

    public void Update()
    {
        Vector3 pp = playerMovement.transform.position;
        Vector3 tp = this.transform.position;

        float distance = DisToTank(pp, tp);

        if (distance < movementStartDis &&  distance > 40f && thisAward == awardType.none)
        {
            
            if (playerMovement.fuel <= 20f)
            {
                fuelAward.SetActive(true);
                rays.SetActive(true);
                thisAward = awardType.fuel;
                
            }
            else if (playerMovement.lifeRemain <= 20f)
            {
                lifeAward.SetActive(true);
                rays.SetActive(true);
                thisAward = awardType.life;
                
            }
            else if (gunCode.misiles < 1)
            {
                misileAward.SetActive(true);
                rays.SetActive(true);
                thisAward = awardType.ammo;
                
            }
        }
        


    }

    float DisToTank(Vector3 a, Vector3 b)
    {
        float xx = Mathf.Abs(a.x - b.x);
        float zz = Mathf.Abs(a.z - b.z);

        float sqDis = (xx * xx) + (zz * zz);
        return Mathf.Sqrt(sqDis);
    }
}
