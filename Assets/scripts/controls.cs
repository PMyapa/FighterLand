using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ControlMode { keyboard = 1, Touch =2 , Joystick = 3, Accel = 4};

public class controls : MonoBehaviour
{
    //public bool pc = false;

    public ControlMode control;
    public float Steer;
    public float Liver;
    public float TouchSteer;
    public float TouchLiver;
    public GameObject HUI;
    public GameObject VUI;
    public GameObject JSUI;

    

    public void SideInput(float inputh) { TouchSteer = inputh; }
    public void UpInput(float inputv) { TouchLiver = inputv; }
    player Player;
    JetGame jetgame;

    [SerializeField] float AccelSpeed;
    [SerializeField] int controltype;
    [SerializeField] float Accelshow;
    [SerializeField] float recoveryRate;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<player>();
        jetgame = GameObject.FindObjectOfType<JetGame>();

        if (jetgame == null)
        {

            controltype = 0;
        }
        else
        {

            controltype = jetgame.controlt;
        }


#if UNITY_STANDALONE_WIN

  controltype = 3;

#endif

#if UNITY_EDITOR

  controltype = 3;

#endif


        if (controltype == 0)
        {
            control = ControlMode.Touch;
        }
        if (controltype == 1)
        {
            control = ControlMode.Joystick;
        }
        if (controltype == 2)
        {
            control = ControlMode.Accel;
        }
        if (controltype == 3)
        {
            control = ControlMode.keyboard;
        }


        if (control == ControlMode.keyboard)
        {
            
            HUI.SetActive(false);
            VUI.SetActive(false);
            JSUI.SetActive(false);

        }
        else if (control == ControlMode.Touch)

        {
            
            HUI.SetActive(true);
            VUI.SetActive(true);
            JSUI.SetActive(false);
        }
        else if (control == ControlMode.Joystick)

        {
            
            HUI.SetActive(false);
            VUI.SetActive(false);
            JSUI.SetActive(true);
        }
        else if (control == ControlMode.Accel)
        {
            

            HUI.SetActive(false);
            VUI.SetActive(true);
            JSUI.SetActive(false);
        }

    }

    // Update is called once per frame
    public void Update() 
    {
        if (control == ControlMode.keyboard)
        {
            Steer = Input.GetAxis("Horizontal");
            Liver = Input.GetAxis("Vertical");

        }
        else if (control == ControlMode.Touch)

        {
            Steer = Mathf.MoveTowards(Steer, TouchSteer, recoveryRate * Time.deltaTime);
            Liver = Mathf.MoveTowards(Liver, TouchLiver, recoveryRate * Time.deltaTime);
            //Steer = SideInput(); 
        }
        else if (control == ControlMode.Joystick)

        {
            Steer = TouchSteer;
            Liver = TouchLiver;
            
        }
        else if (control == ControlMode.Accel)
        {
            float AccelInput = Input.acceleration.x  * AccelSpeed;
            float AccelInputTPT = AccelInput * AccelInput * AccelInput *3f;
            float AccelClamp = Mathf.Clamp(AccelInputTPT, -1f, 1f);

            float Accelabs = Mathf.Abs(AccelClamp);
            float AccelSign = Mathf.Sign(AccelInput);


            
    
            Accelshow = AccelClamp  ;


            Liver = Mathf.MoveTowards(Liver, TouchLiver, recoveryRate * Time.deltaTime);
            Steer = Accelshow;

        }
    }

    public void FixedUpdate()
    {
        Player.controlType(Steer,Liver);
    }

}
