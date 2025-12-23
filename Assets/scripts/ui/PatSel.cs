using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatSel : MonoBehaviour
{

    public GameObject patButtonPrefab;
    public GameObject[] patButtons;

    jetfix jetfx;

    void Awake()
    {
        jetfx = FindObjectOfType<jetfix>();

        int patL = jetfx.allPatterns.Length;
        patButtons = new GameObject[patL];

        for (int i = 0; i < patL; i++)
        {

            patButtons[i] = (GameObject)Instantiate(patButtonPrefab, this.transform);
            patButtons[i].GetComponent<Image>().sprite = jetfx.allPatterns[i].patSpr;
            int x = i;
            patButtons[i].GetComponent<Button>().onClick.AddListener(() => { setPattern(x); });
        }

    }


    public void setPattern(int val)
    {
        if (jetfx.jetsdata.jd[jetfx.jetIndex].pat == val)
        {
            return;
        }

        jetfx.jetsdata.jd[jetfx.jetIndex].pat = val;
        jetfx.UpdateJetLook();

        jetfx.patternCost = 5000f;
        jetfx.ModificationTotalCost();
    }
}
