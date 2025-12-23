using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountBoard : MonoBehaviour
{
    [SerializeField] GameObject DailyGiftPan;
    [SerializeField] GameObject BalancePan;
    [SerializeField] Text giftText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetBalancePan()
    {
        BalancePan.SetActive(true);
        DailyGiftPan.SetActive(false);
    }

    public void SetDailyGiftPan(string val)
    {
        BalancePan.SetActive(false);
        DailyGiftPan.SetActive(true);
        giftText.text = val;
    }
}
