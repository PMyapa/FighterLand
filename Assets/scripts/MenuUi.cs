using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuUi : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Selector;
    [SerializeField] GameObject Editor;

    [SerializeField] GameObject JetInfoPan;

    [SerializeField] GameObject exitPan;
    [SerializeField] GameObject confirmPan;
    [SerializeField] GameObject registerPan;
    [SerializeField] GameObject settingPan;
    [SerializeField] GameObject leaderboardPan;
    [SerializeField] GameObject accountboardPan;

    [SerializeField] GameObject menuBack;

    [SerializeField] TextMeshProUGUI accountText;
    [SerializeField] GameObject highScoreBox;
    [SerializeField] TextMeshProUGUI highScoreText;

    [SerializeField] Text speedti, fuelti, hpti, misileti;


    [SerializeField] TextMeshProUGUI jetPriceText;

    [SerializeField] GameObject NextJetButton;
    [SerializeField] GameObject PreviousJetButton;

    [SerializeField] GameObject Locker;
    [SerializeField] GameObject Editb;
    [SerializeField] GameObject Playb;

    [SerializeField] GameObject DailyGiftButton;

    public enum MenuType { Main , Select , Edit };
    public MenuType myMenu;


    static Vector3 MainPos = new Vector3(-1.08f, 0.4f, 2.8f);
    static Quaternion MainRot = Quaternion.Euler(3.56f, 155.7f, 0f);

    static Vector3 NextPos = new Vector3(0f, 1.05f, 2.69f);
    static Quaternion NextRot = Quaternion.Euler(22.5f, 180f, 0f);

    jetfix jfx;
    JetGame jetgame;

    // Start is called before the first frame update
    void Start()
    {
        if (LoadingManager.LoadInstance == null)
        {
            SceneManager.LoadScene("Load");
            return;

        }
        jetgame = GameObject.FindObjectOfType<JetGame>();
        jfx = GameObject.FindObjectOfType<jetfix>();
        SetMain();
        UpdateAccText();
        jetgame.OnAccountUpdated += UpdateAccText;
        jetgame.OnAccountError += OpenAccountBalance;
        jetgame.OnDailyRewardLoad += EnableDailyReward;
        jetgame.OnDailyRewardOpen += AfterDailyReward;
        jetgame.OnUnregisteredPlayerLogged += OpenRegister;

        //jetgame.InterstitialAdGo.GetComponent<InterstitialAdControl>().RequestAndLoadInterstitialAd();
        
        SetDailyReward(jetgame.CheckDailyGift());
        jetgame.LoadInterstitialAd();
    }

    private void OnDisable()
    {
        if (jetgame != null)
        {
            jetgame.SaveEconomy();

            jetgame.OnAccountUpdated -= UpdateAccText;
            jetgame.OnAccountError -= OpenAccountBalance;
            jetgame.OnDailyRewardLoad -= EnableDailyReward;
            jetgame.OnDailyRewardOpen -= AfterDailyReward;
            jetgame.OnUnregisteredPlayerLogged -= OpenRegister;
        }

        
    }

     
    public void UpdateAccText()
    {        
        accountText.text = jetgame.Account.ToString("n0");
    }

    void SetDailyReward(bool isTrue)
    {
        if (!isTrue)
        {
            DailyGiftButton.SetActive(false);
            return;
        }

        //Debug.Log("DLSET");
        DailyGiftButton.SetActive(true);
        DailyGiftButton.GetComponent<Button>().interactable = false;
        jetgame.LoadDailyRewardAd();
        
       
    }

    public void EnableDailyReward()
    {
        //Debug.Log("DLEnable");
        DailyGiftButton.GetComponent<Button>().interactable = true;
    }

    public void OpenDailyReward()
    {
        jetgame.ShowDailyRewardAd();
        
    }

    public void AfterDailyReward()
    {
        DailyGiftButton.SetActive(false);
        OpenAccounDailyGift();
    }

    public void AsktoExit()
    {
        exitPan.SetActive(true);
        jetgame.Update_AfterNextFunc = CancelExit;
    }

    public void CancelExit()
    {
        exitPan.SetActive(false);
        jetgame.Update_AfterNextFunc = AsktoExit;

    }

    public void OpenRegister()
    {
        registerPan.SetActive(true);

        jetgame.Update_NextFunc = OpenRegister;
        jetgame.Update_AfterNextFunc = OpenRegister;
    }

    public void DoRegistration()
    {
        jetgame.Registration();
        CloseRegister();
    }

    public void CloseRegister()
    {
        registerPan.SetActive(false);

        jetgame.Update_NextFunc = CloseRegister;
        jetgame.Update_AfterNextFunc = AsktoExit;
    }

    public void OpenSettings(string type)
    {
        settingPan.SetActive(true);

        jetgame.Update_NextFunc = CloseSettings;
        jetgame.Update_AfterNextFunc = AsktoExit;

        settingPan.GetComponent<Settings>().SetMenuType(type);
    }

    public void CloseSettings()
    {
        settingPan.SetActive(false);
        jetgame.Update_NextFunc = CloseSettings;
        jetgame.Update_AfterNextFunc = AsktoExit;
    }

    public void OpenLeaderboard(string type)
    {
        leaderboardPan.SetActive(true);
        jetgame.GetLeaderboardToGame();
        


        jetgame.Update_NextFunc = CloseLeaderBoard;
        jetgame.Update_AfterNextFunc = AsktoExit;


        leaderboardPan.GetComponent<LeaderPanel>().setLeaderPanel(type);

    }

    public void CloseLeaderBoard()
    {
        leaderboardPan.SetActive(false); 
        jetgame.Update_NextFunc = CloseLeaderBoard;
        jetgame.Update_AfterNextFunc = AsktoExit;
    }

    public void OpenAccountBalance()
    {
        accountboardPan.SetActive(true);
        accountboardPan.GetComponent<AccountBoard>().SetBalancePan();
        jetgame.Update_NextFunc = CloseAccountBoard;
    }

 
    public void OpenAccounDailyGift()
    {
        float amount = jetgame.DailyRewardAmount;
        accountboardPan.SetActive(true);
        accountboardPan.GetComponent<AccountBoard>().SetDailyGiftPan(amount.ToString());
        jetgame.Update_NextFunc = CloseAccountBoard;
    }

    public void CloseAccountBoard()
    {
        accountboardPan.SetActive(false);
        jetgame.Update_NextFunc = jetgame.Update_AfterNextFunc;
    }


    public void AsktoDiscard()
    {
        confirmPan.SetActive(true);
        jetgame.Update_AfterNextFunc = CancelDiscard;
    }
    
    public void CancelDiscard()
    {
        confirmPan.SetActive(false);
        jetgame.Update_AfterNextFunc = AsktoDiscard;
    }

    void SetJetInfoUI()
    {
        
        Locker.SetActive(jfx.jetsdata.jd[jfx.jetIndex].jk != jfx.jets[jfx.jetIndex].GetComponent<JetView>().Lock);
        Playb.GetComponent<Button>().interactable = (jfx.jetsdata.jd[jfx.jetIndex].jk == jfx.jets[jfx.jetIndex].GetComponent<JetView>().Lock);
        Editb.GetComponent<Button>().interactable = (jfx.jetsdata.jd[jfx.jetIndex].jk == jfx.jets[jfx.jetIndex].GetComponent<JetView>().Lock);


        float topspeedvar = jfx.jets[jfx.jetIndex].GetComponent<JetView>().TopSpeed * 50;
        speedti.text = topspeedvar.ToString() + " mph";
        fuelti.text = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Tank.ToString() + " gal";
        hpti.text = jfx.jets[jfx.jetIndex].GetComponent<JetView>().hp.ToString();
        misileti.text = jfx.jets[jfx.jetIndex].GetComponent<JetView>().misileCap.ToString();


        float jetPrice = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Price;
        jetPriceText.text = jetPrice.ToString("n0");

        NextJetButton.GetComponent<Button>().interactable = (jfx.JetOrderIndex < jfx.JetOrder.Length - 1);
        PreviousJetButton.GetComponent<Button>().interactable = (jfx.JetOrderIndex > 0);
    }

    public void SetMain()
    {
        myMenu = MenuType.Main;
        MainMenu.SetActive(true);
        Editor.SetActive(false);
        Selector.SetActive(false);

        JetInfoPan.SetActive(false);

        Camera.main.transform.position = MainPos;
        Camera.main.transform.rotation = MainRot;
        menuBack.SetActive(true);

        jfx.GetUserJet();

        jetgame.Update_NextFunc = AsktoExit;
        jetgame.Update_AfterNextFunc = AsktoExit;

        highScoreText.text = jetgame.highScore.ToString("n0");

        if (jetgame.highScore == 0 && highScoreBox.gameObject.activeSelf)
        {

            highScoreBox.SetActive(false);
        }

        jetgame.MenuTurn += 1;
    }


    public void SetSelect()
    {
        myMenu = MenuType.Select;
        MainMenu.SetActive(false);
        Editor.SetActive(false);
        Selector.SetActive(true);

        JetInfoPan.SetActive(true);

        Camera.main.transform.position = NextPos;
        Camera.main.transform.rotation = NextRot;

        menuBack.SetActive(false);
        SetJetInfoUI();


        jetgame.Update_NextFunc = SetMain;
        jetgame.Update_AfterNextFunc = SetMain;

        if (jetgame.MenuTurn > 3)
        {
            jetgame.ShowInterstitialAd();
        }
    }


    public void SetEdit()
    {
        myMenu = MenuType.Edit;
        Selector.SetActive(false);
        MainMenu.SetActive(false);
        Editor.SetActive(true);

        JetInfoPan.SetActive(false);

        menuBack.SetActive(false);

        jfx.SaveToJson();


        jfx.patternCost = 0f;
        jfx.themeCost = 0f;
        jfx.ModificationTotalCost();


        jetgame.Update_NextFunc = AsktoDiscard;
        jetgame.Update_AfterNextFunc = CancelExit;
    }


    public void unlockJet()
    {
        float jetPrice = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Price;


        if (jetgame.DoPay(jetPrice))
        {
            jfx.jetsdata.jd[jfx.jetIndex].jk = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Lock;
            jfx.SaveToJson();
            jfx.SetUserJet(true);

            SetJetInfoUI();
        }


    }

    public void NextJet()
    {
        jfx.nextJet();
        SetJetInfoUI();
    }

    public void PreviousJet()
    {
        jfx.preJet();
        SetJetInfoUI();
    }

    public void SaveModJet()
    {
        float _modificationCost = jfx.ModificationTotalCost();

        if (jetgame.DoPay(_modificationCost))
        {
            jfx.SaveToJson();
            jfx.patternCost = 0f;
            jfx.themeCost = 0f;
            jfx.ModificationTotalCost();
            SetSelect();
        }

        
    }

    public void DiscardModJet()
    {
        jfx.LoadFromJson();
        jfx.UpdateJetLook();
        jfx.patternCost = 0f;
        jfx.themeCost = 0f;
        jfx.ModificationTotalCost();
        SetSelect();
    }

}
