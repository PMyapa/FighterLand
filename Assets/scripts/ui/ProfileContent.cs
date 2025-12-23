using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileContent : MonoBehaviour
{

    [SerializeField] InputField NameTextInput;
    [SerializeField] InputField ContactTextInput;
    [SerializeField] GameObject subbutton;

    JetGame jetgame;
    private void OnEnable()
    {
        jetgame = FindObjectOfType<JetGame>();
        NameTextInput.text = jetgame.UserName;
        ContactTextInput.text = jetgame.Contact;
        subbutton.SetActive(false);

        jetgame.playfabManager.SubmitName(jetgame.UserName);
        jetgame.playfabManager.SubmitContact(jetgame.Contact);
    }

    public void submitProfile()
    {
        jetgame.UserName = NameTextInput.text;
        jetgame.Contact = ContactTextInput.text;

        jetgame.playfabManager.SubmitName(jetgame.UserName);
        jetgame.playfabManager.SubmitContact(jetgame.Contact);
    }
}
