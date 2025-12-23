using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorTools : MonoBehaviour
{
    

    [SerializeField] GameObject PatternPan;
    [SerializeField] GameObject ThemePan;

    [SerializeField] GameObject PatternPanOline;
    [SerializeField] GameObject ThemePanOline;


    jetfix jetfx;

    private int _jetmattype;
    public int JetMatType
    {
        get
        {
            return _jetmattype;
        }

        set
        {
            if (value == _jetmattype)
            {
                return;
            }
            _jetmattype = value;
            SetMatTypePannel(value);
        }

    }


    

    // Start is called before the first frame update
    void Start()
    {
        jetfx = FindObjectOfType<jetfix>();
    }


    void LateUpdate()
    {
        if (jetfx != null)
        {
            JetMatType = jetfx.jetsdata.jd[jetfx.jetIndex].matt;
        }

    }

    void OnEnable()
    {
        JetMatType = 0;
        SetMatTypePannel(0);
    }

    public void SetMatType(int val)
    {
        if (jetfx.jetsdata.jd[jetfx.jetIndex].matt == val)
        {
            return;
        }
        jetfx.jetsdata.jd[jetfx.jetIndex].matt = val;
        jetfx.UpdateJetLook();
        JetMatType = val;


        jetfx.themeCost = 10000f;
        jetfx.ModificationTotalCost();
    }

    public void SetMatTypePannel(int val)
    {
        if (val == 0)
        {
            PatternPan.SetActive(true);
            ThemePan.SetActive(false);
            
            PatternPanOline.SetActive(true);
            ThemePanOline.SetActive(false);
        }
        else if (val == 1)
        {
            PatternPan.SetActive(false);
            ThemePan.SetActive(true);
            
            PatternPanOline.SetActive(false);
            ThemePanOline.SetActive(true);
        }
    }

}
