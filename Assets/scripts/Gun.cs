using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    JetGame jetgame;

    private float damage1 ;
    private float damage2 ;
    private float damage ;
    private float range = 500 ;
    private float playerAltitude;
    private float playerSpeed;

    public Camera fpsCam;

    public GameObject pointGO;

    [SerializeField] AudioClip shootsfx;
    [SerializeField] AudioSource source;

    [SerializeField] Button yourButton;

    public int misiles;
    public int misileCap;
    public float gunPoints;

    [SerializeField] Text gunptext;
    [SerializeField] GameObject gpPlusTextObject;
    [SerializeField] TextMeshProUGUI gpPlusText;

    public GameObject misileNoPop;

    [SerializeField] TextMeshProUGUI misileText;

    player playerShoot;

    public bool PlayerSuperSonic;

    public bool isSuperCombo;

    [SerializeField] private float maxTimeBetweenHits = 2;
    [SerializeField] private int hitsUntilSuperCombo = 3;
    [SerializeField] private float powerUpDuration = 2;

    public int hitCounter;
    private float lastHitTime;
    private float powerUpResetTimer;

    void Start()
    {
        playerShoot = GameObject.FindObjectOfType<player>();
       
        gunPoints = 50f;


    }

    // Update is called once per frame
    void Update()
    {


#if UNITY_STANDALONE_WIN

  
        if (Input.GetKeyDown(KeyCode.X))
        {
            
            Shoot();

        }

#endif


#if UNITY_EDITOR

  
        if (Input.GetKeyDown(KeyCode.X))
        {
            
            Shoot();

        }

#endif

        if (gunPoints > 99)
        {
            gunPoints = 0;
            misiles = misileCap;
            playerShoot.PlayrefillSfx();
        }


        float gpt = Mathf.Round(gunPoints);
        gunptext.text = gpt.ToString() + "%";

        misileText.text = misiles.ToString() ;
        //cooldown.fillAmount = gunPoints / 100f ;

        playerAltitude = playerShoot.altitude;
        playerSpeed = playerShoot.speed;

        if (playerAltitude > 70)
        {
            damage1 = 15;
        }
        else if (playerAltitude > 56)
        {
            damage1 = 18;
        }
        else if (playerAltitude > 44)
        {
            damage1 = 21;
        }
        else if (playerAltitude > 34)
        {
            damage1 = 24;
        }
        else if (playerAltitude > 26)
        {
            damage1 = 27;
        }
        else
        {
            damage1 = 30;
        }
        
        
        
        
        if (playerSpeed < 12)
        {
            damage2 = 30;
        }
        else if (playerSpeed < 14)
        {
            damage2 = 33;
        }
        else if (playerSpeed < 16)
        {
            damage2 = 36;
        }
        else if (playerSpeed < 18)
        {
            damage2 = 39;
        }
        else if (playerSpeed < 20)
        {
            damage2 = 42;
        }
        else
        {
            damage2 = 45;
        }



        damage = damage1 + damage2;

        PlayerSuperSonic = playerShoot.SuperSonic;

        if (isSuperCombo)
        {
            powerUpResetTimer -= Time.deltaTime;
            if (powerUpResetTimer <= 0 || playerShoot.gettingShot)
            {
                comboOff();
            }

        }




    }

    public void giftGUnPoints(float gunplus)
    {
        gunPoints += gunplus;
        gpPlusTextObject.SetActive(false);
        gpPlusTextObject.SetActive(true);
        float preGplus = Mathf.Round(gunplus);
        gpPlusText.text = preGplus.ToString() + "+";
    }

    public void awardMisile()
    {
        misiles = misileCap;
    }

    public void Shoot()
    {

        int layerMask1 = 1 << 8;
        int layerMask2 = 1 << 9;
        int layerMask3 = 1 << 10;
        int layerMask =  layerMask1 | layerMask2 | layerMask3 ;
        layerMask = ~ layerMask ;

        if (misiles >=1)
        {
            misiles -= 1;

            source.PlayOneShot(shootsfx);

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask) && playerShoot.alive && playerShoot.fueled)
            {
                //Debug.Log(hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);

                    if (PlayerSuperSonic)
                    {
                        addCombohit();
                    }

                    //playerShoot.Countakill();

                }

                GameObject hitGO = Instantiate(pointGO, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitGO, 1f);

            }
        }
        else
        {
            Debug.Log("no misiles");
            MisilePop();
        }
        



        
    }


    public void addCombohit()
    {
        if (Time.time - lastHitTime < maxTimeBetweenHits)
        {
            hitCounter++;

            if (hitCounter >= hitsUntilSuperCombo)
            {
                isSuperCombo = true;
                powerUpResetTimer = powerUpDuration;
            }
        }
        else
        {
            if (!isSuperCombo)
            {
                hitCounter = 1;
            }
            
        }

        lastHitTime = Time.time;
    }

    public void comboOff()
    {
        isSuperCombo = false;
        hitCounter = 0;
    }


    public void MisilePop()
    {
        misileNoPop.SetActive(true);
    }

}
