using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameover : MonoBehaviour
{
    JetGame jetgame;
    public int goScore;
    public float goMiles;
    public int goKills;
    [SerializeField] TextMeshProUGUI goScoreText;
    [SerializeField] TextMeshProUGUI goTotalMoneyText;

    [SerializeField] TextMeshProUGUI targetsCountText;
    [SerializeField] TextMeshProUGUI targetsMoneyText;
    [SerializeField] TextMeshProUGUI mileageText;
    [SerializeField] TextMeshProUGUI mileageMoneyText;

    [SerializeField] TextMeshProUGUI accountText;
    public GameObject hsImage;

    private float totMoney;

    // Start is called before the first frame update
    void Start()
    {
        jetgame = GameObject.FindObjectOfType<JetGame>();
        goScore = jetgame.score;
        goMiles = jetgame.miles;
        goKills = jetgame.TotalKills;
        hsImage.SetActive(goScore >= jetgame.highScore);
        goScoreText.text = "Score:" + goScore.ToString();
        jetgame.ResetScore();
        accountText.text = jetgame.Account.ToString("n0");

        targetsCountText.text = "Kills: " +goKills.ToString();
        float killsMoney = goKills * 100f;
        targetsMoneyText.text =  killsMoney.ToString("n0");

        mileageText.text = "Mileage: " + goMiles.ToString("F2");
        float mileMoney = goMiles * 100f;
        mileageMoneyText.text =  mileMoney.ToString("n0");

        totMoney = (goMiles * 100f) + (goKills * 100f);

        jetgame.UpdateAcc(totMoney);

        StartCoroutine(CountText());

        jetgame.LoadInterstitialAd();

        jetgame.Update_NextFunc = GotoMenu;
        jetgame.Update_AfterNextFunc = GotoMenu;
    }


    public void GotoMenu()
    {
        LoadingManager.LoadInstance.LoadScene("Menu");
    }

    public void PlayAgain()
    {
        LoadingManager.LoadInstance.LoadScene("Game");
    }

    private IEnumerator CountText()
    {
        float countTotMoney = 0;
        float plusval = 0.1f;
        while (countTotMoney < totMoney)
        {
            plusval = (totMoney - countTotMoney + 10f) /2f;

            countTotMoney += plusval;
            if (countTotMoney > totMoney)
            {
                countTotMoney = totMoney;
            }

            goTotalMoneyText.text = "Total: " + countTotMoney.ToString("n0");

            yield return new WaitForSeconds(0.1f);
        }

        accountText.text = jetgame.Account.ToString("n0");
        jetgame.SaveEconomy();
    }
}
