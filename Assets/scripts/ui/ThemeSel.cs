using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSel : MonoBehaviour
{
    public GameObject thmButtonPrefab;
    public GameObject[] themeButtons;

    jetfix jetfx;

    void Awake()
    {
        jetfx = FindObjectOfType<jetfix>();

        int ThmL = jetfx.allThemes.Length;
        themeButtons = new GameObject[ThmL];

        for (int i = 0; i < ThmL; i++)
        {

            themeButtons[i] = (GameObject)Instantiate(thmButtonPrefab, this.transform);
            themeButtons[i].GetComponent<Image>().sprite = jetfx.allThemes[i].themeSpr;
            int x = i;
            themeButtons[i].GetComponent<Button>().onClick.AddListener(() => { setTheme(x); });
        }

    }


    public void setTheme(int val)
    {
        string inttothemename = jetfx.allThemes[val].themeName;
        if (jetfx.jetsdata.jd[jetfx.jetIndex].thm == inttothemename)
        {
            return;
        }
        jetfx.jetsdata.jd[jetfx.jetIndex].thm = inttothemename;
        jetfx.UpdateJetLook();


        jetfx.themeCost = 10000f;
        jetfx.ModificationTotalCost();
    }
}
