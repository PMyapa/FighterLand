using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardContent : MonoBehaviour
{
    public GameObject playersTextsPrefab;
    public GameObject LeaderboardContentArea;
    public GameObject[] playersTexts;

    public PlayfabManager.playerScoreData[] chartElements;

    JetGame jetGame;

    // Start is called before the first frame update
    void Start()
    {
        jetGame = FindObjectOfType<JetGame>();
    }

    

    public void CreateLeaderboardChart()
    {
        StartCoroutine(CreateLeaderboardChartProcess());

    }
    public IEnumerator CreateLeaderboardChartProcess()
    {

        playersTexts = new GameObject[1];
        playersTexts[0] = Instantiate(playersTextsPrefab, LeaderboardContentArea.transform);
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
                    loaded = (jetGame.playfabManager.LoggedIn && jetGame.playfabManager.GotLeaderboard);
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

        if (timeOut )
        {
            

            playersTexts = new GameObject[1];

            playersTexts[0] = Instantiate(playersTextsPrefab, LeaderboardContentArea.transform);
            playersTexts[0].transform.GetChild(0).GetComponent<Text>().text = "";
            playersTexts[0].transform.GetChild(1).GetComponent<Text>().text = "Connection Error";
            playersTexts[0].transform.GetChild(2).GetComponent<Text>().text = "";
        }
        else
        {
            int chartLenght = jetGame.playfabManager.playersData.Length;

            playersTexts = new GameObject[chartLenght];
            chartElements = new PlayfabManager.playerScoreData[chartLenght];

            for (int i = 0; i < chartLenght; i++)
            {
                chartElements[i] = jetGame.playfabManager.playersData[i];
                playersTexts[i] = Instantiate(playersTextsPrefab, LeaderboardContentArea.transform);
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
