using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    JetGame jetgame;

    [SerializeField] Button backgroundMusicButton;
    [SerializeField] Sprite[] musicSprite;

    [SerializeField] TMPro.TMP_Dropdown dropdown;

    [SerializeField] GameObject InstructionContent;

    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject GeneralButton;


    [SerializeField] GameObject InstructionPanel;
    [SerializeField] GameObject InstructionButton;

    [SerializeField] Color selectedbuttoncollor;

    void OnEnable()
    {

        jetgame = GameObject.FindObjectOfType<JetGame>();
        int controltype = jetgame.controlt;
        dropdown.value = controltype;
        backgroundMusicButton.GetComponent<Image>().sprite = musicSprite[jetgame.musict];

    }

    public void Controller()
    {
        jetgame.controlt = dropdown.value;

    }

    public void SetBackgroundMusicButton()
    {
        if (jetgame.musict == 0)
        {
            jetgame.musict = 1;
        }else if (jetgame.musict == 1)
        {
            jetgame.musict = 0;
        }

        backgroundMusicButton.GetComponent<Image>().sprite = musicSprite[jetgame.musict];

        jetgame.SetBackGroundMusic();
    }

    public void SetMenuType(string type)
    {
        if (type == "General")
        {
            GeneralPanel.SetActive(true);
            InstructionPanel.SetActive(false);

            GeneralButton.GetComponent<Image>().color = selectedbuttoncollor;
            InstructionButton.GetComponent<Image>().color = Color.white;
        }
        else if (type == "Instructions")
        {
            GeneralPanel.SetActive(false);
            InstructionPanel.SetActive(true);

            GeneralButton.GetComponent<Image>().color = Color.white;
            InstructionButton.GetComponent<Image>().color = selectedbuttoncollor;

            InstructionContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        }
    }

}
