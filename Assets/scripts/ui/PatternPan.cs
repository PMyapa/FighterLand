using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternPan : MonoBehaviour
{
    [SerializeField] GameObject colorPanel;
    [SerializeField] GameObject colorPanelscroll;
    [SerializeField] GameObject patternSelectionPanel;
    [SerializeField] GameObject patternSelectionPanelscroll;

    [SerializeField] GameObject colorb1;
    [SerializeField] GameObject colorb2;
    [SerializeField] GameObject patternb;
    
    [SerializeField] GameObject colorb1Oline;
    [SerializeField] GameObject colorb2Oline;
    [SerializeField] GameObject patternb2Oline;

    jetfix jetfx;

    private int _patternattribute;
    public int PatternAttribute
    {
        get
        {
            return _patternattribute;
        }

        set
        {
            if (value == _patternattribute)
            {
                return;
            }
            _patternattribute = value;
            SetPatternAttribute(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        jetfx = FindObjectOfType<jetfix>();
        
    }

    void OnEnable()
    {
        PatternAttribute = 0;
        SetPatternAttribute(0);
    }

  
    void LateUpdate()
    {
        if (jetfx != null)
        {
            colorb1.GetComponent<Image>().color = jetfx.allColors[jetfx.jetsdata.jd[jetfx.jetIndex].col1];
            colorb2.GetComponent<Image>().color = jetfx.allColors[jetfx.jetsdata.jd[jetfx.jetIndex].col2];
            patternb.GetComponent<Image>().sprite = jetfx.allPatterns[jetfx.jetsdata.jd[jetfx.jetIndex].pat].patSpr;

        }

    }

    public void PatternButtons(int val)
    {
        PatternAttribute = val;
    }

   

    public void SetPatternAttribute(int val)
    {
        colorPanel.SetActive(false);
        colorPanelscroll.SetActive(false);
        patternSelectionPanel.SetActive(false);
        patternSelectionPanelscroll.SetActive(false);

        if (val == 0)
        {
            colorPanelscroll.SetActive(true);
            colorPanel.SetActive(true);

            colorb1Oline.SetActive(true);
            colorb2Oline.SetActive(false);
            patternb2Oline.SetActive(false);

            colorPanel.GetComponent<ColorPan>().SetColor1Delegate();
        }
        else if (val == 1)
        {
            colorPanelscroll.SetActive(true);
            colorPanel.SetActive(true);

            colorb1Oline.SetActive(false);
            colorb2Oline.SetActive(true);
            patternb2Oline.SetActive(false);

            colorPanel.GetComponent<ColorPan>().SetColor2Delegate();
        }
        else if (val == 2)
        {
            patternSelectionPanelscroll.SetActive(true);
            patternSelectionPanel.SetActive(true);

            colorb1Oline.SetActive(false);
            colorb2Oline.SetActive(false);
            patternb2Oline.SetActive(true);
        }

       
    }
}
