using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tPoint : MonoBehaviour
{

    public float spriteYP;
    player players;


    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectOfType<player>();
    }

    // Update is called once per frame
    void Update()
    {

        spriteYP = players.altitude;

        float modeSYP = spriteYP * spriteYP * spriteYP * -0.000016f;


        Vector3 position = new Vector3(0, modeSYP, 0);
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.localPosition = position;
    }
}
