using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class JetGame : MonoBehaviour
{
    private static JetGame jetgame;
    public jetObject jetO;
    public PlayfabManager playfabManager;
    
    public int score;

    public float miles;

    public int TotalKills;

    public int highScore;

    public float Account;

    public int userJetIndex;

    public int turn;

    public int controlt;

    public int musict;

    public string UserName;

    public string Contact;



    private bool _registered;
    public bool Registered
    {
        get { return _registered; }
        set
        {
            _registered = value;

            Debug.LogError(value.ToString());
            if (value == false)
            {
                StartCoroutine(setRegister());
            }
        }
    }


    IEnumerator setRegister()
    {
        yield return new WaitForSeconds(0.5f);
        OnUnregisteredPlayerLogged?.Invoke();
    }
    //to check daily event

    public static System.DateTime sampleTime = new System.DateTime(2023, 01, 01, 18, 30, 00);
    public static System.TimeSpan sessionTime = new System.TimeSpan(18, 00, 00);
    public System.TimeSpan sessionTimeGet;
    public System.DateTime SessionFinishTime;
    public string lastDailyEventFinishTimeString;
    public string lastDailyGiftTimeString;

    public bool isDailySessionOn;
    public bool isDailySessionActivated;

    public Dictionary<string, string> NtE;
    public Dictionary<string, string> EtN;

    public delegate void OnAccountUpdatedDelegate();
    public event OnAccountUpdatedDelegate OnAccountUpdated;
    
    public delegate void OnAccountErrorDelegate();
    public event OnAccountErrorDelegate OnAccountError;

    public GameObject DailyRewardAdGo;
    public GameObject EventRewardAdGo;
    public GameObject InterstitialAdGo;

    public delegate void NextFunc();
    public NextFunc Update_NextFunc;
    public NextFunc Update_AfterNextFunc;

    public float DailyRewardAmount;

    public delegate void OnDailyRewardLoadDelegate();
    public event OnDailyRewardLoadDelegate OnDailyRewardLoad;

    public delegate void OnDailyRewardOpenDelegate();
    public event OnDailyRewardOpenDelegate OnDailyRewardOpen;

    private bool IsEventRewardRequested;
    public bool IsEventRewardLoaded;

    public delegate void OnEventRewardLoadDelegate();
    public event OnEventRewardLoadDelegate OnEventRewardLoad;

    public delegate void OnEventRewardOpenDelegate();
    public event OnEventRewardOpenDelegate OnEventRewardOpen;

    public delegate void UnregiteredPlayerLoggedDelegate();
    public event UnregiteredPlayerLoggedDelegate OnUnregisteredPlayerLogged;

    public int MenuTurn;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (jetgame == null)
        {
            jetgame = this;
            MenuTurn = 0;
            goBack = true;
                        
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private bool goBack;

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && goBack)
            {
                StartCoroutine(SetNextFunc());
            }
        }

#if UNITY_EDITOR


        if (Input.GetKeyDown(KeyCode.Q) && goBack)
        {

            StartCoroutine(SetNextFunc());

        }

#endif
    }

    IEnumerator SetNextFunc()
    {
        Update_NextFunc();
        goBack = false;

        yield return new WaitForSecondsRealtime(0.2f);
        goBack = true;

        Update_NextFunc = Update_AfterNextFunc;

    }


    public void jetStart()
    {

        setDic();
        playfabManager = this.GetComponent<PlayfabManager>();
        playfabManager.PfStart();
        LoadEconomy();
        setSessionFinishTime();
        SetTimerTextCorotine();
        SetBackGroundMusic();
        isDailySessionActivated = false;
        IsEventRewardRequested = false;
        ActivateDailySessionOnStart();
    }

    public void SetTimerTextCorotine()
    {
        Debug.Log("Checktime jet game");
        StartCoroutine(CheckTime());
    }

    public IEnumerator CheckTime()
    {

        bool timeOut = false;
        bool loaded = false;


        while (!loaded && !timeOut)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    loaded = (playfabManager.LoggedIn && this.playfabManager.TimeStatus == "Success");
                    if (loaded)
                    {
                        break;
                    }
                }
                catch
                {
                    loaded = false;
                }
                yield return new WaitForSeconds(1f);

                if (i >= 2)
                {
                    timeOut = true;
                }
            }


        }



        if (timeOut)
        {
            isDailySessionOn = false;
        }
        else
        {
            SetTimerText();
        }

    }


    public void setSessionFinishTime()
    {

        System.DateTime timenow = playfabManager.playfabTime();
        SessionFinishTime = new System.DateTime(timenow.Year, timenow.Month, timenow.Day, sampleTime.Hour, sampleTime.Minute, sampleTime.Second);
        sessionTimeGet = sessionTime;
    }

    public void SetTimerText()
    {
        if (!playfabManager.LoggedIn)
        {
            return;
        }

        System.DateTime timenow = playfabManager.playfabTime();
        SessionFinishTime = new System.DateTime(timenow.Year, timenow.Month, timenow.Day, sampleTime.Hour, sampleTime.Minute, sampleTime.Second);

        System.TimeSpan countTime = SessionFinishTime - timenow;
        if (countTime < System.TimeSpan.Zero)
        {
            isDailySessionOn = false;
            Debug.Log("TimeOut");
        }
        else if (countTime > sessionTime)
        {
            isDailySessionOn = false;
            Debug.Log("TimeOut next day");
        }
        else
        {
            isDailySessionOn = true;            
            Debug.Log("session on" + countTime.ToString() + "playfabtime" + timenow.ToString());
        }
    }


    public bool CheckDailySession()
    {
        System.DateTime lastFinishTime;
        try
        {
            lastFinishTime = System.DateTime.Parse(lastDailyEventFinishTimeString);
        }
        catch 
        {
            lastFinishTime = new System.DateTime(2023, 01, 01, 00, 00, 00);
        }

        Debug.Log(SessionFinishTime.ToString() + " aaaaaa" + SessionFinishTime.ToString());
        bool isSameSession = SessionFinishTime == lastFinishTime;
        return isSameSession;
    }

    public void ActivateDailySessionOnStart()
    {
        isDailySessionActivated = CheckDailySession();

        if (!CheckDailySession())
        {
            if (!CheckDailySession() && !IsEventRewardRequested)
            {
                LoadEventRewardAd();
            }
        }
    }

    public void ActivateDailySession()
    {
        lastDailyEventFinishTimeString = SessionFinishTime.ToString();
        isDailySessionActivated = true;
    }

    public bool CheckDailyGift()
    {
        System.DateTime lastGiftTime;
        try
        {
            lastGiftTime = System.DateTime.Parse(lastDailyGiftTimeString);
        }
        catch
        {
            lastGiftTime = new System.DateTime(2023, 01, 01, 00, 00, 00);
        }

        System.TimeSpan giftInterval = new System.TimeSpan(12,00,00);
        System.DateTime timenow = System.DateTime.UtcNow;


        bool giftReady = ((timenow - lastGiftTime) > giftInterval);
        return giftReady;
    }

    public void SetDailyGiftTime()
    {
        lastDailyGiftTimeString = System.DateTime.UtcNow.ToString();
    }



    public void ResetScore()
    {
        if (score > highScore)
        {
            highScore = (int)score;
        }

        if (playfabManager.LoggedIn)
        {

            playfabManager.SendLeaderboard((int)score);

            if (isDailySessionOn && isDailySessionActivated)
            {

                playfabManager.SendDailyPlayers((int)score);
            }
        }
        score = 0;
        miles = 0;
        TotalKills = 0;

        SetTimerTextCorotine();
    }

    public void GetLeaderboardToGame()
    {
        if (!playfabManager.LoggedIn)
        {
            return;
        }
        playfabManager.GetLeaderboard();
        playfabManager.GetDailyPlayers();
    }

    public void UpdateAcc(float val)
    {
        Account = Mathf.Max((Account + val), 0f);
        OnAccountUpdated?.Invoke();
    }

    public bool DoPay(float price)
    {
        if (Account < price)
        {
            OnAccountError();
                       
            return false;
        }


        UpdateAcc(price * -1f);
        return true;
    }

    void OnApplicationQuit()
    {
        if (Registered)
        {
            turn += 1;
        }
        SaveEconomy();
    }

    public void Registration()
    {
        Registered = true;
    }

    [System.Serializable]
    public class Economy
    {
        public string hsG_dj;
        public string Ty_adr;
        public string maj_Sb;
        public string u_hdjf;
        public string cj_sgy;
        public string bs_msh;
        public string SavedName;
        public string SavedContact;
    }

    public void LoadEconomy()
    {
        Economy geecon = new Economy();

        string economyString = PlayerPrefs.GetString("edata", JsonUtility.ToJson(geecon));

        geecon = JsonUtility.FromJson<Economy>(economyString);

        Debug.Log(economyString.ToString());

        //string highscoreText = geecon.hsG_dj;
        //string turnText = geecon.Ty_adr;
        //string accountText = geecon.maj_Sb;
        //string userjiText = geecon.u_hdjf;
        //string controlText = geecon.cj_sgy;
        //string bgmusicText = geecon.bs_msh;

        //string highscoreDec = dec(highscoreText);
        //string turnDec = dec(turnText);
        //string accountDec = dec(accountText);
        //string userjiDec = dec(userjiText);
        //string controlDec = dec(controlText);
        //string bgmusicDec = dec(bgmusicText);

        /*try
        {
            highScore = int.Parse(highscoreDec);
            turn = int.Parse(turnDec);
            Account = float.Parse(accountDec);
            userJetIndex = int.Parse(userjiDec);
            controlt = int.Parse(controlDec);
            UserName = geecon.SavedName;
            Contact = geecon.SavedContact;


        }
        catch (System.Exception e)
        {
            highScore = 0;
            turn = 0;
            Account = 10000;
            userJetIndex = 0;
            controlt = 0;
            UserName = "Unkown";
            Contact = "";


            print(e.ToString());
            return;
        }*/
        
        try
        {
            string highscoreText = geecon.hsG_dj;
            string highscoreDec = dec(highscoreText);
            highScore = int.Parse(highscoreDec);
        }
        catch (System.Exception e) 
        {   highScore = 0;            
            print(e.ToString());
        }


        try
        {
            string turnText = geecon.Ty_adr;
            string turnDec = dec(turnText);
            turn = int.Parse(turnDec);
            Registered = (turn >= 1 || Registered);
        }
        catch (System.Exception e)
        {   turn = 0;
            Registered = false;
            print(e.ToString());
        }

        try
        {
            string accountText = geecon.maj_Sb;
            string accountDec = dec(accountText);
            Account = float.Parse(accountDec);
        }
        catch (System.Exception e)
        {   Account = 10000;
            print(e.ToString());
        }

        try
        {
            string userjiText = geecon.u_hdjf;
            string userjiDec = dec(userjiText);
            userJetIndex = int.Parse(userjiDec);
        }
        catch (System.Exception e)
        {   userJetIndex = 0;
            print(e.ToString());
        }

        try
        {
            string controlText = geecon.cj_sgy;
            string controlDec = dec(controlText);
            controlt = int.Parse(controlDec); 
        }
        catch (System.Exception e)
        {   controlt = 1;
            print(e.ToString());
        }
        
        try
        {
            string bgmusicText = geecon.bs_msh;
            string bgmusicDec = dec(bgmusicText);
            musict = int.Parse(bgmusicDec); 
        }
        catch (System.Exception e)
        {   musict = 1;
            print(e.ToString());
        }

        try
        {             
            string nametemp = geecon.SavedName;
            if (nametemp == "")
            { nametemp = "Unkown"; }
            UserName = nametemp;
        }
        catch (System.Exception e)
        {
            UserName = "Unkown";
            print(e.ToString());
        }

        try{ Contact = geecon.SavedContact; }
        catch (System.Exception e)
        {
            Contact = "";
            print(e.ToString());
        }

        lastDailyEventFinishTimeString = PlayerPrefs.GetString("cftime");
        lastDailyGiftTimeString = PlayerPrefs.GetString("dgtime");


    }


    public void SaveEconomy()
    {
        string highScoreToString = highScore.ToString();
        string turnToString = turn.ToString();
        string accountToString = Account.ToString();
        string userjiToString = userJetIndex.ToString();
        string controlToString = controlt.ToString();
        string bgmusicToString = musict.ToString();

        string highScoreEnc = enc(highScoreToString);
        string turnEnc = enc(turnToString);
        string accountEnc = enc(accountToString);
        string userjiEnc = enc(userjiToString);
        string controlEnc = enc(controlToString);
        string bgmusicEnc = enc(bgmusicToString);

        Economy defaultEcon = new Economy();
        defaultEcon.hsG_dj = highScoreEnc;
        defaultEcon.Ty_adr = turnEnc;
        defaultEcon.maj_Sb = accountEnc;
        defaultEcon.u_hdjf = userjiEnc;
        defaultEcon.cj_sgy = controlEnc;
        defaultEcon.bs_msh = bgmusicEnc;
        defaultEcon.SavedName = UserName;
        defaultEcon.SavedContact = Contact;
        PlayerPrefs.SetString("edata", JsonUtility.ToJson(defaultEcon));
        PlayerPrefs.SetString("cftime", lastDailyEventFinishTimeString);
        PlayerPrefs.SetString("dgtime", lastDailyGiftTimeString);
        PlayerPrefs.Save();
    }



    void setDic()
    {
        if (NtE == null)
        {
            NtE = new Dictionary<string, string>();
            NtE.Add("0", "s");
            NtE.Add("1", "o");
            NtE.Add("2", "6");
            NtE.Add("3", "k");
            NtE.Add("4", "w");
            NtE.Add("5", "8");
            NtE.Add("6", "4");
            NtE.Add("7", "l");
            NtE.Add("8", "b");
            NtE.Add("9", "m");
            NtE.Add(".", "d");
        }


        if (EtN == null)
        {
            EtN = new Dictionary<string, string>();
            EtN.Add("s", "0");
            EtN.Add("o", "1");
            EtN.Add("6", "2");
            EtN.Add("k", "3");
            EtN.Add("w", "4");
            EtN.Add("8", "5");
            EtN.Add("4", "6");
            EtN.Add("l", "7");
            EtN.Add("b", "8");
            EtN.Add("m", "9");
            EtN.Add("d", ".");
        }


    }


    string enc(string text)
    {
        var result = new StringBuilder();

        for (int c = 0; c < text.Length; c++)
        {
            // take next character from string
            char character = text[c];


            string encharacter = NtE[character.ToString()];
            result.Append(encharacter[0]);
        }

        return result.ToString();
    }

    string dec(string text)
    {
        var result = new StringBuilder();

        for (int c = 0; c < text.Length; c++)
        {
            // take next character from string
            char character = text[c];


            string encharacter = EtN[character.ToString()];
            result.Append(encharacter[0]);
        }

        return result.ToString();
    }

    public void SetBackGroundMusic()
    {
        if (musict == 0)
        {
            this.GetComponent<AudioSource>().Stop();
        }
        else if(musict ==1)
        {
            if(!this.GetComponent<AudioSource>().isPlaying)
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void LoadInterstitialAd()
    {
        InterstitialAdGo.GetComponent<InterstitialAdControl>().RequestAndLoadInterstitialAd();
    }
    public void ShowInterstitialAd()
    {
        InterstitialAdGo.GetComponent<InterstitialAdControl>().ShowInterstitialAd();
    }

    public void LoadDailyRewardAd()
    {
        //Debug.Log("DLload");
        DailyRewardAdGo.GetComponent<DailyRewardAdControl>().RequestAndLoadRewardedAd();
    }
    public void ShowDailyRewardAd()
    {
        DailyRewardAdGo.GetComponent<DailyRewardAdControl>().ShowRewardedAd();
    }
    public void DailyRewardLoaded()
    {
        //Debug.Log("DLloded");
        OnDailyRewardLoad?.Invoke();

        int giftRandom = Random.Range(50, 100);
        int gift = giftRandom * 100;
        DailyRewardAmount = (float)gift;

    }
    public void DailyRewardOpened()
    {

        StartCoroutine(DailyRewardOpenedWait());
        
    }

    IEnumerator DailyRewardOpenedWait()
    {
        yield return new WaitForSeconds(0.5f);
        SetDailyGiftTime();

        UpdateAcc(DailyRewardAmount);

        OnDailyRewardOpen?.Invoke();
    }

    public void LoadEventRewardAd()
    {
        IsEventRewardLoaded = false;
        //Debug.Log("evet r load req");
        EventRewardAdGo.GetComponent<EventRewardAdControl>().RequestAndLoadRewardedInterstitialAd();
        
    }
    public void ShowEventRewardAd()
    {
        EventRewardAdGo.GetComponent<EventRewardAdControl>().ShowRewardedInterstitialAd();
    }
    public void EventRewardLoaded()
    {
        IsEventRewardLoaded = true;
        //Debug.Log("evet r loaded");
        OnEventRewardLoad?.Invoke();
        
    }
    public void EventRewardOpened()
    {
        //Debug.Log("1000 gift");
        OnEventRewardOpen?.Invoke();
        UpdateAcc(1000);
    }

}
