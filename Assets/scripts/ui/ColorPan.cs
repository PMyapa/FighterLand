using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPan : MonoBehaviour
{
    public GameObject colorButtonPrefab;
    public GameObject[] colorButtons;

    

    jetfix jetfx;

    public delegate void SetColorDelegate(int col);
    public event SetColorDelegate setColorEvent;



    // Start is called before the first frame update
    void Awake()
    {
        jetfx = FindObjectOfType<jetfix>();

        int colorsL = jetfx.allColors.Length;
        colorButtons = new GameObject[colorsL];

        for (int i = 0; i < colorsL; i++)
        {
            
            colorButtons[i] = (GameObject)Instantiate(colorButtonPrefab, this.transform);
            colorButtons[i].GetComponent<Image>().color = jetfx.allColors[i];
            int x = i;
            colorButtons[i].GetComponent<Button>().onClick.AddListener(() => { SetColor(x); });
        }
        
    }

  
  

    void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);

    }

    public void SetColor1Delegate()
    {
        setColorEvent = setCol1;
        //cb1.GetComponent<Button>().interactable = true;
    }

    public void setCol1(int col)
    {
        if (jetfx.jetsdata.jd[jetfx.jetIndex].col1 == col)
        {
            return;
        }
        jetfx.jetsdata.jd[jetfx.jetIndex].col1 = col;
        //jetfx.UpdateJet(jetfx.jetIndex);
        jetfx.UpdateJetLook();
        jetfx.patternCost = 5000f;
        jetfx.ModificationTotalCost();
    }

    public void SetColor2Delegate()
    {
        setColorEvent = setCol2;
        //cb2.GetComponent<Button>().interactable = true;
    }

    public void setCol2(int col)
    {
        if (jetfx.jetsdata.jd[jetfx.jetIndex].col2 == col)
        {
            return;
        }
        jetfx.jetsdata.jd[jetfx.jetIndex].col2 = col;
        //jetfx.UpdateJet(jetfx.jetIndex);
        jetfx.UpdateJetLook();        
        jetfx.patternCost = 5000f;
        jetfx.ModificationTotalCost();
    }

    public void SetColor(int col)
    {
        setColorEvent(col);
    }
}
