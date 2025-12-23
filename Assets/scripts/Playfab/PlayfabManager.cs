using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public playerScoreData[] playersData;
    public playerScoreData[] DailyplayersData;
    public bool LoggedIn;
    public bool GotLeaderboard;
    public bool GotDailyPlayers;

    public string TimeStatus;

    public string NameString;

    private System.DateTime playfabtimetemp;
    private System.DateTime systemtimetemp;

    public class playerScoreData
    {
        public string pos;
        public string name;
        public string value;
    }

    // Start is called before the first frame update
    public void PfStart()
    {
        
        LoggedIn = false;
        GotLeaderboard = false;
        GotDailyPlayers = false;
        TimeStatus = "Pending";

        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {

            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true}
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login / account create!");
        LoggedIn = true;

        NameString = "Unknown";
        GetCurrentTimeOnStart();

        if (result.InfoResultPayload.PlayerProfile != null)
        {
            NameString = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while Logging / create acc");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SubmitName(string name)
    {
        if (!LoggedIn)
        { return;   }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);


        

    }


    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Name Update");
    }


    public void SubmitContact( string contact)
    {
        if (!LoggedIn)
        { return;   }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Contact", contact }
            }

        };
        
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);

    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Data Update");
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> 
            {
                new StatisticUpdate 
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboaredUpdate, OnError);
    }

    void OnLeaderboaredUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leader board sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 100
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        List<playerScoreData> playersList = new List<playerScoreData>();

        foreach (var item in result.Leaderboard)
        {

            //playerScoreData itemToString = (item.Position + 1).ToString() + "  " + item.DisplayName + "  " + item.StatValue;
            playerScoreData itemToString = new playerScoreData();
            itemToString.pos = (item.Position + 1).ToString();
            itemToString.name = item.DisplayName;
            itemToString.value = item.StatValue.ToString();
            //Debug.Log(itemToString);
            playersList.Add(itemToString);

        }

        playersData = playersList.ToArray();

        GotLeaderboard = true;
    }


    public void SendDailyPlayers(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Daily Highscore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnDailyPlayersUpdate, OnError);
    }

    void OnDailyPlayersUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull Daily players sent");
    }

    public void GetDailyPlayers()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Daily Highscore",
            StartPosition = 0,
            MaxResultsCount = 15
        };

        PlayFabClientAPI.GetLeaderboard(request, OnDailyPlayersGet, OnError);
    }


    void OnDailyPlayersGet(GetLeaderboardResult result)
    {
        List<playerScoreData> playersList = new List<playerScoreData>();

        foreach (var item in result.Leaderboard)
        {

            //playerScoreData itemToString = (item.Position + 1).ToString() + "  " + item.DisplayName + "  " + item.StatValue;
            playerScoreData itemToString = new playerScoreData();
            itemToString.pos = (item.Position + 1).ToString();
            itemToString.name = item.DisplayName;
            itemToString.value = item.StatValue.ToString();

            //Debug.Log(itemToString);
            playersList.Add(itemToString);

        }

        DailyplayersData = playersList.ToArray();

        GotDailyPlayers = true;
    }

   /* public void GetCurrentTime()
    {
       
        PlayFabClientAPI.GetTime(new GetTimeRequest(), OnGetTimeSuccess, OnGetTimeFail);
    }*/
    
    public void GetCurrentTimeOnStart()
    {
        systemtimetemp = System.DateTime.UtcNow;
        playfabtimetemp = systemtimetemp;
        PlayFabClientAPI.GetTime(new GetTimeRequest(), OnGetTimeSuccess, OnGetTimeFail);
    }
    void OnGetTimeSuccess(GetTimeResult result)
    {
        
        playfabtimetemp = result.Time;
        TimeStatus = "Success";
    }

    void OnGetTimeFail(PlayFabError error)
    {

        TimeStatus = "Fail";
    }

    
    public System.DateTime playfabTime()
    {
        System.TimeSpan tt1 = System.DateTime.UtcNow - systemtimetemp;
        System.DateTime tt = playfabtimetemp + tt1; 
        
        return tt;
        
    }

}
