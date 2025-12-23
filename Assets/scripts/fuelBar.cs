using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fuelBar : MonoBehaviour
{
    public Slider slider;


    public player Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindObjectOfType<player>();

    }

    // Update is called once per frame
    void Update()
    {
        float fuelval = Player.fuel;
        float fueltank =  Player.fuelTankSize ; 

        slider.value = fuelval;
        slider.maxValue = fueltank;
    }
}
