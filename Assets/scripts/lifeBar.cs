using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lifeBar : MonoBehaviour
{
    public Slider sliderl;
    public player Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindObjectOfType<player>();

    }

    // Update is called once per frame
    void Update()
    {
        float LifeRem = Player.lifeRemain;
        float TLife = Player.life ;

        sliderl.value = LifeRem;
        sliderl.maxValue = TLife;
    }
}
