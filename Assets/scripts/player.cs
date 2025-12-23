using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    Gun gun;

    JetGame jetgame;
    //public GameObject theJet;
    private Material theJetMat;
    private Mesh theJetMesh;

    public bool alive = true;
    public bool fueled = true;
    public bool gameStarted;


    public float BasicSpeed;
    public float speed;
    public float preSpeed;
    private float topSpeed;
    private float rot;
    [SerializeField] Rigidbody rb;

    private bool __superSonic;
    public bool SuperSonic
    {
        get { return __superSonic; }
        set
        {
            bool temp = value;

            if (temp != __superSonic)
            {

            }

            __superSonic = temp;
        }
    }
    public float mileage;

    private int killCount;

    public float horizontalInput;
    public float horizontalMultiplier;
    public float angfp;
    private float angf1;
    public float verticalInput;
    [SerializeField] float ale;
    [SerializeField] float testangf;

    public float altitude;
    public float preAltitude;/*
    [SerializeField] TextMeshProUGUI altiText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scoreText;*/

    public ParticleSystem blast;

    public float fuelTankSize;
    public float fuel;
    private float fuelBurn;
    private float fuelBurnRate;
    [SerializeField] float fuelBurnRateMultiplier;
    [SerializeField] GameObject fuelPlusTextObject;
    [SerializeField] TextMeshProUGUI fuelPlusText;

    public float fuelPoints;
    [SerializeField] Text fptext;

    public float life = 100f;
    public float lifeRemain;
    public bool gettingShot;

    private float lifePoints;
    [SerializeField] Text lptext;
    [SerializeField] GameObject lifePlusTextObject;
    [SerializeField] TextMeshProUGUI lifePlusText;


    private float score;
    public float scoreTotal;
    [SerializeField] GameObject scorePlusTextObject;
    [SerializeField] TextMeshProUGUI scorePlusText;
    //public godata SetGameOverData;

    float unbalanceAngle = 0f;

    private float antiRotate;
    private float antiRoatatewhenUp;
    private float resetAngle;
    private float anglex;


    private int mpcount;
    private float mgpc;
    private float mfpc;
    private float mhpc;
    private int mpctemp;

    private float jetEngineSoundVol;
    [SerializeField] float maxVol;
    [SerializeField] float minVol;

    [SerializeField] AudioClip mpsfx;
    [SerializeField] AudioClip blastsfx;
    [SerializeField] AudioClip refillsfx;
    public AudioSource playerAudiosource;
    public AudioSource engineSource;
    public AudioSource gettingShotSource;

    // Start is called before the first frame update
    void Start()
    {
        jetgame = GameObject.FindObjectOfType<JetGame>();
        gun = gameObject.GetComponent<Gun>();

        if (jetgame == null)
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        gameStarted = false;
        speed = BasicSpeed;

        GameObject jetGO = (GameObject)Instantiate(jetgame.jetO.jet, this.transform.position, Quaternion.identity, this.transform);
        jetGO.transform.GetChild(1).gameObject.SetActive(true);
        jetGO.transform.GetChild(0).gameObject.SetActive(false);

        jetGO.GetComponent<Renderer>().material = jetgame.jetO.jetMat;

        gun.misileCap = jetGO.GetComponent<JetView>().misileCap;
        gun.misiles = jetGO.GetComponent<JetView>().misileCap;

        //topSpeed = jetgame.jetO.TopSpeed;
        topSpeed = jetGO.GetComponent<JetView>().TopSpeed;
        rot = jetGO.GetComponent<JetView>().Rotary;
        fuelTankSize = jetGO.GetComponent<JetView>().Tank;
        life = jetGO.GetComponent<JetView>().hp;


        fuel = fuelTankSize;
        fuelPoints = 50;

        lifePoints = 50;

        lifeRemain = life;
        gettingShot = false;

        score = 0;
        scoreTotal = 0;

        mileage = 0;
        killCount = 0;

        mpcount = 0;
        mgpc = 0;
        mfpc = 0;
        mhpc = 0;
    }

    public void startGame()
    {
        gameStarted = true;
    }

    public void FixedUpdate()
    {

        if (!alive) return;
        

        altitude = Mathf.Round(rb.position.y * 10);
        preAltitude = 2 * altitude;

        if (altitude <= 20)
        {
            Blast();
        }


        int shotStregnth;

        if (altitude > 70)
        {
            shotStregnth = 160;
        }
        else if (altitude > 56)
        {
            shotStregnth = 180;
        }
        else if (altitude > 44)
        {
            shotStregnth = 200;
        }
        else if (altitude > 34)
        {
            shotStregnth = 240;
        }
        else if (altitude > 26)
        {
            shotStregnth = 280;
        }
        else
        {
            shotStregnth = 320;
        }


        if (lifeRemain < 0)
        {
            Blast();
        }
        else
        {
            if (gettingShot)
            {
                lifeRemain -= Time.deltaTime * shotStregnth;
            }
        }


        if (gettingShot)
        {
            if (speed > BasicSpeed)
            {
                speed -= Time.deltaTime * 10;
            }
            else
            {
                speed = BasicSpeed;

            }
        }
        else
        {
            if (speed < topSpeed)
            {
                float acceleration = (Mathf.Abs(topSpeed - speed) / 60) + 0.04f;

                speed += Time.deltaTime * acceleration;
            }

        }


        preSpeed = Mathf.Round(speed * 50);



        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;


        float hobsss = Mathf.Sign(horizontalInput);
        float hoabs = Mathf.Abs(hobsss);


        angf1 = Vector3.Angle(transform.up, Vector3.up);

        testangf = angf1;
        angfp = Mathf.Abs(angf1 * .01f);

        float clampa = angfp - 0.7f;
        // modify the variable to keep y within minY to maxY
        clampa = Mathf.Clamp(clampa, 0, 1f);
        // and now set the transform position to our modified vector
        angfp = clampa;


        float sideperfection = 4f + (rot * 3f);


        Vector3 horizontalMove = transform.up * angfp * hoabs * sideperfection * speed * Time.fixedDeltaTime * horizontalMultiplier;







        rb.MovePosition(rb.position + forwardMove + horizontalMove);



        antiRotate = 100 / (angf1 + 0.01f);
        antiRoatatewhenUp = 1f;
        resetAngle = 0f;
        anglex = 0f;



        if (!fueled)
        {
            //when no fuel

            unbalanceAngle += 1f;

            var gravityAngle = 0.38f;
            var varAngle = gravityAngle + unbalanceAngle;

            anglex = Mathf.LerpAngle(rb.transform.eulerAngles.x, varAngle, 10f * Time.deltaTime);
            antiRoatatewhenUp = 2f;

            var currentAngle = Mathf.LerpAngle(rb.transform.eulerAngles.z, resetAngle, antiRotate * antiRoatatewhenUp * Time.deltaTime);

            rb.transform.rotation = Quaternion.Euler(anglex, 0, currentAngle);
        }
        else
        {
            //when fuel


            unbalanceAngle = 0f;


            if (horizontalInput > -0.5 && horizontalInput < 0.5)
            {
                //when NOT turn right of left


                if (altitude <= 80f)
                {

                    //when under clouds and NOT turn right of left

                    var gravityAngle = 0.38f;
                    var varAngle = gravityAngle + unbalanceAngle;

                    if (verticalInput > -0.5 && verticalInput < 0.5)
                    {
                        //when NOT go Up or down

                        //anglex = 0;
                        anglex = Mathf.LerpAngle(rb.transform.eulerAngles.x, varAngle, 10f * Time.deltaTime);
                        antiRoatatewhenUp = 1f;
                    }
                    else
                    {
                        //when go up or down

                        anglex = rb.transform.rotation.eulerAngles.x;
                        antiRoatatewhenUp = 4f;
                    }


                    var currentAngle = Mathf.LerpAngle(rb.transform.eulerAngles.z, resetAngle, antiRotate * antiRoatatewhenUp * Time.deltaTime);
                    rb.transform.rotation = Quaternion.Euler(anglex, 0, currentAngle);
                    
                }
                else
                {

                    //when over clouds and NOT turn right of left

                    var gravityAngle = 1f;

                    var varAngle = gravityAngle + unbalanceAngle;

                    var upskyresest = 1f;

                    if (verticalInput > -0.5 && verticalInput < 0.5)
                    {
                        antiRoatatewhenUp = 1f;
                        upskyresest = altitude - 79;

                    }
                    else if (verticalInput < -0.9)
                    {
                        antiRoatatewhenUp = 4f;
                        upskyresest = 1f;

                    }
                    else
                    {
                        antiRoatatewhenUp = 1f;
                        upskyresest = 20f;
                    }

                    var currentAngle = Mathf.LerpAngle(rb.transform.eulerAngles.z, resetAngle, antiRotate * antiRoatatewhenUp * Time.deltaTime);
                    anglex = Mathf.LerpAngle(rb.transform.eulerAngles.x, varAngle, upskyresest * Time.deltaTime);


                    rb.transform.rotation = Quaternion.Euler(anglex, 0, currentAngle);                   
                    
                }

                Goupdown();


            }
            else
            {

                //when turn right of left
                var gravityAngle = 0.38f;
                var varAngle = gravityAngle + unbalanceAngle;


                if (altitude <= 80)
                {
                    //when under clouds
                    gravityAngle = 0.38f;
                    varAngle = gravityAngle + unbalanceAngle;
                }
                else
                {
                    //when over clouds
                    gravityAngle = 1f;
                    varAngle = gravityAngle + unbalanceAngle;
                }

                antiRoatatewhenUp = 1f;

                if (verticalInput > -0.5 && verticalInput < 0.5)
                {
                    //when Not go up or down                   

                }
                else
                {
                    //when go up or down
                }

                

                var currentAngle = Mathf.LerpAngle(rb.transform.eulerAngles.z, resetAngle, antiRotate * antiRoatatewhenUp * Time.deltaTime);
                anglex = Mathf.LerpAngle(rb.transform.eulerAngles.x, varAngle, 10f * Time.deltaTime);



                rb.transform.rotation = Quaternion.Euler(anglex, 0, currentAngle);


            }


            Gorightleft();
        }




    }
 

    void Goupdown()
    {
        //___properties for up rotate input
        ale = rb.transform.rotation.z;

        float upInput;


        if (ale > -0.1 && ale < 0.1)
        {

            if (altitude > 80 && verticalInput > 0)
            {



                upInput = 0;

            }
            else
            {

                upInput = -(10 + (rot * 50) )* verticalInput;

            }


        }
        else
        {
            upInput = 0;

        }

        //--


        rb.transform.Rotate(Vector3.right * upInput * Time.deltaTime, Space.Self);
    }

    void Gorightleft()
    {
        //___properties for side rotate input
        float anglimit = 1f;

        float smoothingAngleStart = 70f;
        float smoothungAngleEnd = 95f;
        float smoothRange = smoothungAngleEnd - smoothingAngleStart;

        if (angf1 < smoothingAngleStart)
        {
            anglimit = 1.1f;

        }
        else if (angf1 < smoothungAngleEnd)
        {
            float anglimitTemp = (smoothungAngleEnd - angf1 + 0.1f) / smoothRange;
            anglimit = anglimitTemp;
        }
        else
        {
            anglimit = 0;
        }

        rb.transform.Rotate(Vector3.forward * -(500 +( rot * 200)) * horizontalInput * anglimit * Time.deltaTime, Space.Self);

    }


    public IEnumerator mpCounter()
    {


        while (mpcount >= 1)
        {
            mpctemp = mpcount;

            yield return new WaitForSeconds(0.1f);


            if (mpcount == mpctemp)
            {
                giftmp();
                mpcount = 0;
                mpctemp = 0;

            }
        }


    }

    public void GetMp(float g, float f, float h)
    {
        playerAudiosource.PlayOneShot(mpsfx);

        mpcount += 1;
        mgpc += g;
        mfpc += f;
        mhpc += h;

        if (mpcount <= 1)
        {
            StartCoroutine(mpCounter());
        }

    }

    void giftmp()
    {
        if (mgpc > 0)
        {
            gun.giftGUnPoints(mgpc);
            mgpc = 0;
        }

        if (mfpc > 0)
        {
            giftFuelPoints(mfpc);
            mfpc = 0;
        }

        if (mhpc > 0)
        {
            giftLifePoints(mhpc);
            mhpc = 0;
        }
    }

    public void giftFuel(float fplus)
    {

        float fuelPlus1 = fuelTankSize - fuel;
        float fuelPlus2 = fplus;

        float fuelPlus = Mathf.Min(fuelPlus1, fuelPlus2);

        fuel += fuelPlus;

        fuelPlusTextObject.SetActive(false);
        fuelPlusTextObject.SetActive(true);
        float preFplus = Mathf.Round(fuelPlus * 100f);
        fuelPlusText.text = preFplus.ToString() + "+";

    }

    public void giftFuelPoints(float fp)
    {
        fuelPoints += fp;
        fuelPlusTextObject.SetActive(false);
        fuelPlusTextObject.SetActive(true);
        float preFplus = Mathf.Round(fp);
        fuelPlusText.text = preFplus.ToString() + "+";
    }


    public void awardFuel()
    {
        fuel = fuelTankSize;
    }


    public void giftLifePoints(float lp)
    {
        lifePoints += lp;
        lifePlusTextObject.SetActive(false);
        lifePlusTextObject.SetActive(true);
        float preLplus = Mathf.Round(lp);
        lifePlusText.text = preLplus.ToString() + "+";
    }

    public void awardLife()
    {
        lifeRemain = life;
    }

    public void giftScore(float scrplus)
    {
        score += scrplus / 10f;
        scorePlusTextObject.SetActive(false);
        scorePlusTextObject.SetActive(true);
        scorePlusText.text = scrplus.ToString() + "+";
    }

    public void Blast()
    {
        alive = false;
        engineSource.Stop();

        playerAudiosource.PlayOneShot(blastsfx);
        blast.Play();

        Invoke("GameOver", 0.7f);
        /*GameManager gameManager = gManager.GetComponent<GameManager>();
        gameManager.FinalScore();*/

    }

    public void NoFuel()
    {
        fueled = false;
        engineSource.Stop();


        Invoke("GameOver", 1.2f);
        /*GameManager gameManager = gManager.GetComponent<GameManager>();
        gameManager.FinalScore();*/

    }

    public void StartGettingShot()
    {
        gettingShot = true;
        /* (speed > BasicSpeed)
        {
            speed = BasicSpeed;
        }
        */


        gettingShotSource.Play();
    }

    public void StopGettingShot()
    {
        gettingShot = false;
        gettingShotSource.Pause();

    }

    public void Countakill()
    {
        killCount += 1;
    }

    public void GameOver()
    {
        jetgame.score = (int)scoreTotal;
        jetgame.miles = mileage;
        jetgame.TotalKills = killCount;
        SceneManager.LoadScene("gameover");
    }

    public void PlayrefillSfx()
    {
        playerAudiosource.PlayOneShot(refillsfx);
    }

    // Update is called once per frame
    void Update()
    {


        /*altiText.text = preAltitude.ToString() + "m";
        speedText.text = preSpeed.ToString() + "mph";*/

        if (gameStarted)
        {

            score += speed * Time.deltaTime;
            mileage += speed * 0.02f * Time.deltaTime;
        }


        scoreTotal = 10f * Mathf.Round(score);


        /*scoreText.text = scoreTotal.ToString();*/


        if (fuelPoints > 99)
        {
            fuelPoints = 0;
            fuel = fuelTankSize;
            PlayrefillSfx();
        }

        float fpt = Mathf.Round(fuelPoints);
        fptext.text = fpt.ToString() + "%";

        if (lifePoints > 99)
        {
            lifePoints = 0;
            lifeRemain = life;
            PlayrefillSfx();
        }

        float lpt = Mathf.Round(lifePoints);
        lptext.text = lpt.ToString() + "%";

        float upperSkyFuelBurnRate = 1f;
        if (altitude > 70)
        {
            upperSkyFuelBurnRate = 3f;

        }
        else if (altitude > 56)
        {
            upperSkyFuelBurnRate = 2f;

        }
        else if (altitude > 44)
        {
            upperSkyFuelBurnRate = 2f;

        }
        else if (altitude > 34)
        {
            upperSkyFuelBurnRate = 2f;

        }
        else if (altitude > 26)
        {
            upperSkyFuelBurnRate = 2f;

        }
        else
        {
            upperSkyFuelBurnRate = 3.6f;
        }

        float verticleburnrate = 1f;

        if (verticalInput > 0)
        {
            verticleburnrate = 1f + 4f * verticalInput;
        }
        else
        {
            verticleburnrate = 1f;
        }

        fuelBurnRate = fuelBurnRateMultiplier * speed * altitude * upperSkyFuelBurnRate * verticleburnrate * Time.deltaTime;
        fuelBurn += fuelBurnRate;

        fuel -= fuelBurnRate;
        if (fuel <= 0.001 && fueled)
        {
            //fuel = fuelTankSize;
            NoFuel();
        }


       /* if (speed >= 15.2f)
        {
            SuperSonic = true;
        }
        else
        {
            SuperSonic = false;
        }*/

        if (speed < BasicSpeed)
        {
            jetEngineSoundVol = Mathf.Max(maxVol, 0.5f);
            SuperSonic = false;
        }
        else if (speed < 15.2f)
        {
            float jetEngineSoundVoltemp = ((maxVol * 15.2f) - (minVol * BasicSpeed) + ((minVol - maxVol) * speed)) / (15.2f - BasicSpeed);
            jetEngineSoundVol = jetEngineSoundVoltemp;
            SuperSonic = false;
        }
        else if (speed >= 15.2f)
        {
            jetEngineSoundVol = Mathf.Max(minVol - 0.05f, 0);
            SuperSonic = true;
        }
             

        engineSource.volume = jetEngineSoundVol;
    }

    public void controlType(float HInput, float VInput)
    {
        horizontalInput = HInput;
        verticalInput = VInput;
    }
}
