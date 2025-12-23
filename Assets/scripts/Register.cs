using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{

    [SerializeField] GameObject GeneralPan;
    [SerializeField] GameObject InstructionPan;


    [SerializeField] GameObject InstructionContent;


    JetGame jetgame;

    [SerializeField] TMPro.TMP_Dropdown dropdown;


    [SerializeField] InputField NameTextInput;

    void OnEnable()
    {

        jetgame = GameObject.FindObjectOfType<JetGame>();
        int controltype = jetgame.controlt;
        dropdown.value = controltype;

    }

    public void Controller()
    {
        jetgame.controlt = dropdown.value;

    }

    public void NextRegister()
    {
        submitName();

        GeneralPan.SetActive(false);
        InstructionPan.SetActive(true);

        InstructionContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
    }

    public void submitName()
    {
        jetgame.UserName = NameTextInput.text;

        jetgame.playfabManager.SubmitName(jetgame.UserName);
    }
}
