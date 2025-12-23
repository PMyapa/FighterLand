using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsContent : MonoBehaviour
{
    [SerializeField] GameObject playersTextsPrefab;
    [SerializeField] GameObject DailyPlayersContentArea;
    [SerializeField] GameObject ListHead;
    [SerializeField] GameObject[] playersTexts;

    public Text CountDownText;
    public Button EventButton;
    public Text EventButtonText;
    public GameObject EventButtonAdSymbol;
    private System.DateTime FinishTime;
    private System.TimeSpan eventTimeSpan;
    private System.DateTime eventEndTime;
    public PlayfabManager.playerScoreData[] chartElements;

    [SerializeField] GameObject rewardPop;

    JetGame jetGame;

    public bool eventReady;

    // Start is called before the first frame update
    void Start()
    {
        jetGame = FindObjectOfType<JetGame>();
    }

    public void LateUpdate()
    {
        TimerUpadte();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        jetGame.OnEventRewardLoad -= ShowEventRewardAd;
        jetGame.OnEventRewardOpen -= OnAFterEventReward;
    }

    void TimerUpadte()
    {
        if (!eventReady)
        {
            return;
        }

        System.TimeSpan ctu = eventEndTime - System.DateTime.UtcNow;
        CountDownText.text =  ctu.Hours + "h " + ctu.Minutes + "m " + ctu.Seconds + "s ";

        if(ctu < System.TimeSpan.Zero)
        {
            SetTimerText();
        }
    }

    public void SetTimerTextCorotine()
    {
        StartCoroutine(CheckTime());
    }

    public IEnumerator CheckTime()
    {

        SetTimerText();

        

        bool timeOut = false;
        bool loaded = false;


        while (!loaded && !timeOut)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    loaded = (jetGame.playfabManager.LoggedIn && jetGame.playfabManager.TimeStatus == "Success");
                    if (loaded)
                    {
                        break;
                    }
                }
                catch
                {
                    loaded = false;
                }
                yield return new WaitForSeconds(2f);

                if (i >= 2)
                {
                    timeOut = true;
                }
            }


        }

        

        if (timeOut)
        {
            jetGame.isDailySessionOn = false;
            eventReady = false;
            CountDownText.text = "error";
            ListHead.gameObject.GetComponent<Text>().text = "error";
        }
        else
        {
            SetTimerText();
        }

    }


    public void SetTimerText()
    {
        if (jetGame == null)
        {
            jetGame = FindObjectOfType<JetGame>();
            Debug.Log("Start func jet game not calling");
        }

        if (!jetGame.playfabManager.LoggedIn)
        {
            eventReady = false;
            CountDownText.text = "error";
            EventButton.gameObject.SetActive(false);
            ListHead.gameObject.GetComponent<Text>().text = "error";
            return;
        }

        eventReady = true;

        System.DateTime timenow = jetGame.playfabManager.playfabTime();
        FinishTime = jetGame.SessionFinishTime;
        System.TimeSpan sessionTime = jetGame.sessionTimeGet;

        System.TimeSpan countTime = FinishTime - timenow;
        if (countTime < System.TimeSpan.Zero)
        {
            jetGame.isDailySessionOn = false;
            eventTimeSpan = FinishTime + new System.TimeSpan(24, 00, 00) - sessionTime - timenow;
            eventEndTime = System.DateTime.UtcNow + eventTimeSpan;
            EventButton.gameObject.SetActive(true);
            EventButton.interactable = false;
            EventButton.image.color = Color.red;
            EventButtonText.text = "Next Session";
            EventButtonAdSymbol.SetActive(false);
            ListHead.gameObject.GetComponent<Text>().text = "Results";
            Debug.Log("TimeOut");
        }
        else if (countTime > sessionTime)
        {
            jetGame.isDailySessionOn = false;
            eventTimeSpan = countTime - sessionTime;
            eventEndTime = System.DateTime.UtcNow + eventTimeSpan;
            EventButton.gameObject.SetActive(true);
            EventButton.interactable = false;
            EventButton.image.color = Color.red;
            EventButtonText.text = "Next Session";
            EventButtonAdSymbol.SetActive(false);
            ListHead.gameObject.GetComponent<Text>().text = "Results";
            Debug.Log("TimeOut next day");
        }
        else
        {
            jetGame.isDailySessionOn = true;
            eventTimeSpan = countTime;
            eventEndTime = FinishTime;
            SetEventButton();
            ListHead.gameObject.GetComponent<Text>().text = "Top Players";
            Debug.Log("session on   " + countTime.ToString() + "playfabtime    " + timenow.ToString()+ "__   " + FinishTime.ToString());
        }
        TimerUpadte();
    }

    void SetEventButton()
    {
        if (jetGame.CheckDailySession())
        {
            EventButton.interactable = false;
            EventButton.image.color = Color.green;
            EventButtonText.text = "Joined";
            EventButtonAdSymbol.SetActive(false);
        }
        else
        {
            EventButton.interactable = true;
            EventButton.GetComponent<Button>().onClick.RemoveAllListeners();
            EventButton.GetComponent<Button>().onClick.AddListener(() => { OnEventButtonClick(); });
            EventButton.image.color = Color.green;
            EventButtonText.text = "Join";
            EventButtonAdSymbol.SetActive(true);
        }
    }

    void ShowWhenLoadedEventReward()
    {
        if (jetGame.IsEventRewardLoaded)
        {
            jetGame.ShowEventRewardAd();
            jetGame.OnEventRewardLoad -= ShowEventRewardAd;
        }
        else
        {
            jetGame.OnEventRewardLoad += ShowEventRewardAd;
        }

        jetGame.OnEventRewardOpen += OnAFterEventReward;
    }

    public void ShowEventRewardAd()
    {
        jetGame.ShowEventRewardAd();
    }

    public void OnAFterEventReward()
    {
        rewardPop.SetActive(true);
    }

    void OnEventButtonClick()
    {
        //Debug.Log("ONJOIN");
        jetGame.ActivateDailySession(); 
        EventButton.interactable = false;
        EventButton.image.color = Color.green;
        EventButtonText.text = "Joined";
        EventButtonAdSymbol.SetActive(false);
        ShowWhenLoadedEventReward();
    }

    public void CreateDailyPlayersChart()
    {
        StartCoroutine(CreateLeaderboardChartProcess());
    }


    public IEnumerator CreateLeaderboardChartProcess()
    {
        playersTexts = new GameObject[1];
        playersTexts[0] = Instantiate(playersTextsPrefab, DailyPlayersContentArea.transform);
        playersTexts[0].transform.GetChild(0).GetComponent<Text>().text = "";
        playersTexts[0].transform.GetChild(1).GetComponent<Text>().text = "Loading..";
        playersTexts[0].transform.GetChild(2).GetComponent<Text>().text = "";

        bool timeOut = false;
        bool loaded = false;




        while (!loaded && !timeOut)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    loaded = (jetGame.playfabManager.LoggedIn && jetGame.playfabManager.GotDailyPlayers);
                    if (loaded)
                    {
                        break;
                    }
                }
                catch
                {
                    loaded = false;
                }
                yield return new WaitForSeconds(2f);

                if (i >= 2)
                {
                    timeOut = true;
                }
            }


        }

        ResetChart();

        if (timeOut)
        {
            playersTexts = new GameObject[1];

            playersTexts[0] = Instantiate(playersTextsPrefab, DailyPlayersContentArea.transform);
            playersTexts[0].transform.GetChild(0).GetComponent<Text>().text = "";
            playersTexts[0].transform.GetChild(1).GetComponent<Text>().text = "Connection Error";
            playersTexts[0].transform.GetChild(2).GetComponent<Text>().text = "";
        }
        else
        {
            int chartLenght = jetGame.playfabManager.DailyplayersData.Length;

            playersTexts = new GameObject[chartLenght];
            chartElements = new PlayfabManager.playerScoreData[chartLenght];

            for (int i = 0; i < chartLenght; i++)
            {
                chartElements[i] = jetGame.playfabManager.DailyplayersData[i];
                playersTexts[i] = Instantiate(playersTextsPrefab, DailyPlayersContentArea.transform);
                playersTexts[i].transform.GetChild(0).GetComponent<Text>().text = chartElements[i].pos;
                playersTexts[i].transform.GetChild(1).GetComponent<Text>().text = chartElements[i].name;
                playersTexts[i].transform.GetChild(2).GetComponent<Text>().text = chartElements[i].value;
            }

        }

    }

    public void ResetChart()
    {
        foreach (GameObject go in playersTexts)
        {
            Destroy(go);
        }
        
    }

}
