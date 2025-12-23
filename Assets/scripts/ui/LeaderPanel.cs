using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderPanel : MonoBehaviour
{
    
    [SerializeField] GameObject ProfilePanel;
    [SerializeField] GameObject ProfileButton;


    [SerializeField] GameObject LeaderboardPanel;
    [SerializeField] GameObject LeaderboardButton;


    [SerializeField] GameObject EventsPanel;
    [SerializeField] GameObject EventsButton;


    [SerializeField] Color selectedbuttoncollor;

    public void setLeaderPanel(string type)
    {
        if (type == "profile")
        {
            ProfilePanel.SetActive(true);
            LeaderboardPanel.SetActive(false);
            EventsPanel.SetActive(false);

            ProfilePanel.GetComponent<ProfileContent>().submitProfile();

            ProfileButton.GetComponent<Image>().color = selectedbuttoncollor;
            LeaderboardButton.GetComponent<Image>().color = Color.white;
            EventsButton.GetComponent<Image>().color = Color.white;
        }
        else if (type == "leaderboard")
        {
            ProfilePanel.SetActive(false);
            LeaderboardPanel.SetActive(true);
            EventsPanel.SetActive(false);

            LeaderboardPanel.GetComponent<LeaderboardContent>().ResetChart();
            LeaderboardPanel.GetComponent<LeaderboardContent>().CreateLeaderboardChart();

            ProfileButton.GetComponent<Image>().color = Color.white;
            LeaderboardButton.GetComponent<Image>().color = selectedbuttoncollor;
            EventsButton.GetComponent<Image>().color = Color.white;
        }
        else if (type == "events")
        {
            ProfilePanel.SetActive(false);
            LeaderboardPanel.SetActive(false);
            EventsPanel.SetActive(true);

            EventsPanel.GetComponent<EventsContent>().ResetChart();
            EventsPanel.GetComponent<EventsContent>().CreateDailyPlayersChart();
            EventsPanel.GetComponent<EventsContent>().SetTimerTextCorotine();

            ProfileButton.GetComponent<Image>().color = Color.white;
            LeaderboardButton.GetComponent<Image>().color = Color.white;
            EventsButton.GetComponent<Image>().color = selectedbuttoncollor;
        }
    }
}
