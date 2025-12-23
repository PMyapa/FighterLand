using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class jetfix : MonoBehaviour
{
    JetGame jetgame;

    public int jetIndex;
    public int JetOrderIndex;

    public int[] JetOrder; 

    public GameObject[] jets;

    public JetDataSet jetsdata = new JetDataSet();

    public Color[] allColors;
    public PatternVar[] allPatterns;
    public ThemeVar[] allThemes;


    public GameObject jetGO;

    public Dictionary<string, int> ThemeNametoInt;


    public float patternCost;
    public float themeCost;

    [SerializeField] GameObject modPriceTag;
    [SerializeField] TextMeshProUGUI modPriceText;

    // Start is called before the first frame update
    void Start()
    {
        SetThemes();
        jetgame = GameObject.FindObjectOfType<JetGame>();  
        LoadFromJson();
        jetgame.jetStart();
        GetUserJet();

    }

    

    void FixedUpdate()
    {
       
        transform.Rotate(0, 6 * Time.fixedDeltaTime, 0);
    }

    [System.Serializable]
    public class JetData
    {
        public int matt;
        
        public int col1;
        public int col2;
        public int pat;

        public string thm;

        public string jk;
    }

    [System.Serializable]
    public class JetDataSet
    {
        public JetData[] jd ;
    }

   
    [System.Serializable]
    public class PatternVar
    {
        public Texture2D patTex ;
        public Sprite patSpr ;
    }
       
    [System.Serializable]
    public class ThemeVar
    {
        public Sprite themeSpr ;
        public string themeName;
        public Texture2D[] themetexs;
    }

   
    public void LoadFromJson()
    {
        
        string jetdatastring = PlayerPrefs.GetString("jdata", JsonUtility.ToJson(jetsdata));
       
        JetDataSet savedjetset = JsonUtility.FromJson<JetDataSet>(jetdatastring);

        Debug.Log(jetdatastring.ToString());

        jetsdata.jd = new JetData[jets.Length];

        for (int i = 0; i < jets.Length; i++)
        {
            if (jetsdata.jd[i] == null)
            {
                jetsdata.jd[i] = new JetData();
            }
            if (i < savedjetset.jd.Length )
            {
                //only update excitng jets
                jetsdata.jd[i] = savedjetset.jd[i];                
            }
                        
        }

        jetsdata.jd[JetOrder[0]].jk = jets[JetOrder[0]].GetComponent<JetView>().Lock;
    }


    public void SaveToJson()
    {
       
        PlayerPrefs.SetString("jdata", JsonUtility.ToJson(jetsdata));
        PlayerPrefs.Save();
    }



    


    public void nextJet()
    {
        if(JetOrderIndex < JetOrder.Length)
        { 
            JetOrderIndex = JetOrderIndex + 1;
        }
        else
        {
            JetOrderIndex = JetOrderIndex - 1;
        }
        jetIndex = JetOrder[JetOrderIndex];

        UpdateJet(jetIndex);
        UpdateJetLook();
        SetUserJet(jetsdata.jd[jetIndex].jk == jets[jetIndex].GetComponent<JetView>().Lock);
    }


    public void preJet()
    {
        if (JetOrderIndex >= 1)
        {
            JetOrderIndex = JetOrderIndex - 1;
        }
        else
        {
            JetOrderIndex = 0;
        }
        jetIndex = JetOrder[JetOrderIndex];

        UpdateJet(jetIndex);
        UpdateJetLook();
        SetUserJet(jetsdata.jd[jetIndex].jk == jets[jetIndex].GetComponent<JetView>().Lock);
    }

    public void GetUserJet()
    {
        JetOrderIndex = jetgame.userJetIndex;
        jetIndex = JetOrder[JetOrderIndex];

        UpdateJet(jetIndex);
        UpdateJetLook();
    }

    public void SetUserJet(bool haveit)
    {
        if (!haveit)
        {
            return;
        }

        jetgame.userJetIndex = JetOrderIndex;
    }

    public void UpdateJet(int jeti)
    {
        

        jetIndex = jeti;


        if (jetGO != null)
        {
            Destroy(jetGO);
        }
        
        
        
        

        //Vector3 jetPos = this.transform.position;
        Vector3 jetPos = this.transform.GetChild(0).gameObject.transform.position;
        Quaternion jetRot = this.transform.GetChild(0).gameObject.transform.rotation;

        jetGO = (GameObject)Instantiate(jets[jetIndex], jetPos, jetRot, this.transform);

        jetGO.transform.GetChild(1).gameObject.SetActive(false);

        //UpdateJetLook();

    }

    public float ModificationTotalCost()
    {
        float _costTotal = patternCost + themeCost;
        modPriceTag.SetActive(_costTotal != 0);
        modPriceText.text = _costTotal.ToString("n0");
        return _costTotal;
        
    }


    public void UpdateJetLook()
    {

        Material[] mat = new Material[2];

        mat[0] = jetGO.GetComponent<JetView>().myMats[0];
        mat[1] = jetGO.GetComponent<JetView>().myMats[1];

        mat[0].SetColor("Color_1", allColors[jetsdata.jd[jetIndex].col1]);
        mat[0].SetColor("Color_2", allColors[jetsdata.jd[jetIndex].col2]);
        mat[0].SetTexture("pat_0", allPatterns[jetsdata.jd[jetIndex].pat].patTex);


        int themetoint;
        try {
            themetoint = ThemeNametoInt[jetsdata.jd[jetIndex].thm];
        }
        catch {
            jetsdata.jd[jetIndex].thm = allThemes[0].themeName;
            themetoint = ThemeNametoInt[jetsdata.jd[jetIndex].thm];
        }

        
        mat[1].SetTexture("theme_0", allThemes[themetoint].themetexs[jetIndex]);

        var jetGORenderer = jetGO.GetComponent<Renderer>();
        jetGORenderer.material = mat[jetsdata.jd[jetIndex].matt];

        jetgame.jetO.jet = jets[jetIndex];
        jetgame.jetO.jetMat = mat[jetsdata.jd[jetIndex].matt];
    }


    void SetThemes()
    {
        if (ThemeNametoInt == null)
        {
            ThemeNametoInt = new Dictionary<string, int>();
            for (int i = 0; i < allThemes.Length; i++)
            {
                ThemeNametoInt.Add(allThemes[i].themeName, i);
            }
        }

       
    }
}
