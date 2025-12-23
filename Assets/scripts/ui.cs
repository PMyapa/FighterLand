using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ui : MonoBehaviour
{

    player playerMovement;
    Gun gunCode;


    public GameObject ComboGO;
    public TextMeshProUGUI comboCount;

    public TextMeshProUGUI CountDownText;


    private float playerFuel;
    public float playerAltitude = 50f;
    private float playerLife;

    [SerializeField] GameObject gameCanvas;

    [SerializeField] GameObject fuelWarnIcon;
    [SerializeField] GameObject lowHeightWarnIcon;
    [SerializeField] GameObject cloudWarnIcon;
    [SerializeField] GameObject dangerWarnIcon;

    [SerializeField] GameObject fuelWarnPop;
    [SerializeField] GameObject lowHeightWarnPop;
    [SerializeField] GameObject cloudWarnPop;
    [SerializeField] GameObject dangerWarnPop;

    [SerializeField] GameObject warningScreen;
    [SerializeField] GameObject cloudSimScreen;
    Animator cloud_Animator;


    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Pauseb;



    [SerializeField] TextMeshProUGUI altiText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject SuperSonicIndicator;

    private bool fuelBool ;
    public bool lowHeightBool ;
    private bool cloudBool ;
    private bool dangerBool ;


    public bool playerAlive ;

    private bool __playerSuperSonic;
    public bool PlayerSuperSonic
    {
        get { return __playerSuperSonic; }
        set
        {
            bool temp = value;

            if (temp != __playerSuperSonic)
            {
                SuperSonicIndicator.SetActive(temp);
            }

            __playerSuperSonic = temp;
        }
    }

    private AudioSource[] sources;


    private float time1;
    private float time2;
    private bool tp;


    JetGame jetgame;

    public void Start()
    {
        playerMovement = GameObject.FindObjectOfType<player>();
        gunCode = GameObject.FindObjectOfType<Gun>();

        fuelBool = false;
        lowHeightBool = false;
        cloudBool = false;
        dangerBool = false;

        cloud_Animator = cloudSimScreen.GetComponent<Animator>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
                
        jetgame = FindObjectOfType<JetGame>();

        jetgame.Update_NextFunc = pauseFunc;
        jetgame.Update_AfterNextFunc = resumeFunc;

        gameCanvas.SetActive(false);
        StartCoroutine(StartGameCount());
    }

    IEnumerator StartGameCount()
    {
        string countdown = "3";
        CountDownText.text = countdown;
        yield return new WaitForSeconds(0.7f);
        countdown = "2";
        CountDownText.text = countdown;
        yield return new WaitForSeconds(0.7f);
        countdown = "1";
        CountDownText.text = countdown;
        yield return new WaitForSeconds(0.7f);
        countdown = "Go!";
        CountDownText.text = countdown;
        gameCanvas.SetActive(true);
        playerMovement.startGame();

        yield return new WaitForSeconds(0.7f);
        CountDownText.gameObject.SetActive(false);

    }
  
    // Update is called once per frame
    void LateUpdate()
    {
        playerFuel = playerMovement.fuel;
        playerAltitude = playerMovement.altitude;
        playerLife = playerMovement.lifeRemain;

        altiText.text = playerMovement.preAltitude.ToString() + "m";
        speedText.text = playerMovement.preSpeed.ToString() + "mph";
        scoreText.text = playerMovement.scoreTotal.ToString();

        PlayerSuperSonic = playerMovement.SuperSonic;

        float fulewarnlim = playerMovement.fuelTankSize / 10f;

        playerAlive = playerMovement.alive;

        if (playerAlive)
        {
            if (playerFuel > 2f && playerFuel <= fulewarnlim && !fuelBool)
            {
                LowFuelPopup();

                fuelBool = true;
                fuelWarnIcon.SetActive(fuelBool);


            }
            else if (playerFuel > fulewarnlim && fuelBool)
            {
                fuelBool = false;
                fuelWarnIcon.SetActive(fuelBool);
            }

            if (playerLife > 2f && playerLife <= 10f && !dangerBool)
            {

                DangerPopup();

                dangerBool = true;
                dangerWarnIcon.SetActive(dangerBool);

            }
            else if (playerLife > 10f && dangerBool)
            {
                dangerBool = false;
                dangerWarnIcon.SetActive(dangerBool);
            }

            if (playerAltitude >= 70f && !cloudBool)
            {
                CloudPopup();

                cloudBool = true;
                cloudWarnIcon.SetActive(cloudBool);
                //cloudSimScreen.SetActive(cloudBool);
                cloud_Animator.SetBool("on", true);

            }
            else if (playerAltitude < 70f && cloudBool)
            {
                cloudBool = false;
                cloudWarnIcon.SetActive(cloudBool);
                //cloudSimScreen.SetActive(cloudBool);
                cloud_Animator.SetBool("on", false);
            }

            if (playerAltitude > 15f && playerAltitude <= 30f && !lowHeightBool)
            {
                LowHeightPopup();
                lowHeightBool = true;
                lowHeightWarnIcon.SetActive(lowHeightBool);


            }
            else if (playerAltitude > 30f && lowHeightBool)
            {
                lowHeightBool = false;
                lowHeightWarnIcon.SetActive(lowHeightBool);
            }

        }
        else
        {
            fuelBool = false;
            lowHeightBool = false;
            cloudBool = false;
            dangerBool = false;


            dangerWarnPop.SetActive(false);
            fuelWarnPop.SetActive(false);
            cloudWarnPop.SetActive(false);          
            lowHeightWarnPop.SetActive(false);
            

            
        }    

        



        if (fuelBool || lowHeightBool || dangerBool )
        {
            if (!warningScreen.activeSelf)
            {

                warningScreen.SetActive(true);
            }

        }
        else
        {
            if (warningScreen.activeSelf)
            {

                warningScreen.SetActive(false);
            }
        }



        int combo = gunCode.hitCounter;
        bool isgunCombo = gunCode.isSuperCombo;
        comboCount.text = "Combo x " + combo.ToString();

        if (ComboGO.activeSelf != isgunCombo)
        {
            ComboGO.SetActive(isgunCombo);
        }

    }

    public void pauseFunc()
    {
        jetgame.Update_NextFunc = resumeFunc;
        jetgame.Update_AfterNextFunc = resumeFunc;
        PauseMenu.SetActive(true);
        Pauseb.SetActive(false);
        Time.timeScale = 0;

        List<AudioSource> allplayingaudios = new List<AudioSource>();

        if (playerMovement.playerAudiosource.isPlaying)
        {
            allplayingaudios.Add(playerMovement.playerAudiosource);
        }

        if (playerMovement.engineSource.isPlaying)
        {
            allplayingaudios.Add(playerMovement.engineSource);
        }
        
        if (playerMovement.gettingShotSource.isPlaying)
        {
            allplayingaudios.Add(playerMovement.gettingShotSource);
        }

        chopper[] choppers = FindObjectsOfType(typeof(chopper)) as chopper[];

        if (choppers.Length > 0)
        {
            Debug.Log("choppers length" + choppers.Length.ToString());
            foreach (chopper cgo in choppers)
            {
                if (cgo.helicopterAudiosource.isPlaying)
                {
                    
                    allplayingaudios.Add(cgo.helicopterAudiosource);
                }
            }
        }



        //sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        sources = new AudioSource[allplayingaudios.Count];
        sources = allplayingaudios.ToArray();

        foreach (AudioSource audioSource in sources)
        {
            audioSource.Pause();
        }
    }

   
    
    public void resumeFunc()
    {
        jetgame.Update_NextFunc = pauseFunc;
        jetgame.Update_AfterNextFunc = pauseFunc;
        PauseMenu.SetActive(false);
        Pauseb.SetActive(true);
        Time.timeScale = 1;

        foreach (AudioSource audioSource in sources)
        {
            audioSource.Play();
        }
    }

    public void PlayAgainButton()
    {

        Time.timeScale = 1;
        LoadingManager.LoadInstance.LoadScene("Game");
    }
    public void MenuButton()
    {

        Time.timeScale = 1;
        LoadingManager.LoadInstance.LoadScene("Menu");
    }

    void DangerPopup()
    {
        dangerWarnPop.SetActive(true);
    }

    void LowFuelPopup()
    {
        fuelWarnPop.SetActive(true);
    }

    void CloudPopup()
    {
        cloudWarnPop.SetActive(true);
    }

    void LowHeightPopup()
    {
        lowHeightWarnPop.SetActive(true);
    }

    
}
