using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public enum JoyStickDirection { Horizontal, Vertical , Both}

public class joystick : MonoBehaviour , IDragHandler , IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Background;
    public JoyStickDirection joyStickDirection = JoyStickDirection.Both;
    public RectTransform Handle;
    [Range(0, 2f)] public float HandleLimit = 1f;
    Vector2 input = Vector2.zero;
    controls Controls;

    public float Vertical { get { return input.y; } }
    public float Horizontal { get { return input.x; } }

    Vector2 JoyPosition = Vector2.zero;

    void Start()
    {
        Controls = FindObjectOfType<player>().GetComponent<controls>(); 
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        Background.gameObject.SetActive(true);
        OnDrag(eventdata);
        JoyPosition = eventdata.position;
        Background.position = eventdata.position;
        Handle.anchoredPosition = Vector2.zero;
    }
    
    public void OnDrag(PointerEventData eventdata)
    {
        //Vector2 JoyDirection = eventdata.position - RectTransformUtility.WorldToScreenPoint(new Camera(), Background.position);
        Vector2 JoyDirection = eventdata.position - JoyPosition;
        input = (JoyDirection.magnitude > Background.sizeDelta.x / 2f) ? JoyDirection.normalized : JoyDirection / (Background.sizeDelta.x / 2f);

        if (joyStickDirection == JoyStickDirection.Horizontal)
            input = new Vector2(input.x, 0f);
        if (joyStickDirection == JoyStickDirection.Vertical)
            input = new Vector2(0f,input.y );
        Handle.anchoredPosition = (input * Background.sizeDelta.x / 2f) * HandleLimit;

    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        Background.gameObject.SetActive(false);
        input = Vector2.zero;
        Handle.anchoredPosition = Vector2.zero;


    }

    public void FixedUpdate()
    {
        Controls.SideInput(Horizontal);
        Controls.UpInput(Vertical);
    }
}
