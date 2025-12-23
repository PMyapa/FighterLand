using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class locker : MonoBehaviour
{
    public TextMeshProUGUI jetPriceText;
    public GameObject notenough;

    jetfix jfx;
    JetGame jetgame;

    private float jetPrice;

    // Start is called before the first frame update
    void Start()
    {
        jfx = GameObject.FindObjectOfType<jetfix>();
        jetgame = GameObject.FindObjectOfType<JetGame>();
        jetPrice = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Price;
        jetPriceText.text = jetPrice.ToString("n0");
    }

    // Update is called once per frame
    void Update()
    {
        jetPrice = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Price;
        jetPriceText.text = jetPrice.ToString("n0");
    }

    public void unlockJet()
    {
        /*if (jetgame.Account < jetPrice)
        {
            notenough.gameObject.SetActive(true);
            Debug.Log("Insufficient account balance ");
            return;
        }

        jetgame.Account = jetgame.Account - jetPrice;*/

        if (jetgame.DoPay(jetPrice))
        {
            jfx.jetsdata.jd[jfx.jetIndex].jk = jfx.jets[jfx.jetIndex].GetComponent<JetView>().Lock;
            jfx.SaveToJson();
            jfx.SetUserJet(true);
        }

        
    }
}
