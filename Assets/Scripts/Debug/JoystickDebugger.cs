using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class JoystickDebugger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    const int dataLen = 4;

    [SerializeField]
    public TextMeshProUGUI[] touchPosTXTx = new TextMeshProUGUI[dataLen];
    public TextMeshProUGUI[] touchPosTXTy = new TextMeshProUGUI[dataLen];

    public TextMeshProUGUI touchType;
    public TextMeshProUGUI touchState;
    public TextMeshProUGUI joyPosX;
    public TextMeshProUGUI joyPosY;

    public RectTransform joystickPlate;

    void Awake()
    {
        // tm.GetComponent<TMPro.TextMeshProUGUI>().text = "game";
        touchType.text = "Touch Type : None";
        touchState.text = "Touch State : None";
        for (int i = 0; i < dataLen; i++)
        {
            touchPosTXTx[i].text = $"x{i + 1}";
            touchPosTXTy[i].text = $"y{i + 1}";
        }
    }

    void Update()
    {
        Touch[] touches = new Touch[dataLen];
        try
        {
            for (int i = 0; i < dataLen; i++)
            {
                touches[i] = Input.GetTouch(i);
                int touchX = (int)touches[i].position.x;
                int touchY = (int)touches[i].position.y;
                touchPosTXTx[i].text = $"x{i + 1} : {touchX}";
                touchPosTXTy[i].text = $"y{i + 1} : {touchY}";
            }
        }
        catch
        {
            return;
        }
        switch (touches[0].phase)
        {
            case TouchPhase.Began:
                touchState.text = "Touch State : Began";
                break;
            case TouchPhase.Moved:
                touchState.text = "Touch State : Moved";
                break;
            case TouchPhase.Stationary:
                touchState.text = "Touch State : Stationary";
                break;
            case TouchPhase.Ended:
                touchState.text = "Touch State : Ended";
                break;
            case TouchPhase.Canceled:
                touchState.text = "Touch State : Canceled";
                break;
        }
        joyPosX.text = "JoyX :" + joystickPlate.position.x.ToString();
        joyPosY.text = "JoyY :" + joystickPlate.position.y.ToString();
    }

    public void OnPointerDown(PointerEventData e)
    {
        touchType.text = "OnPointerDown";
    }

    public void OnPointerUp(PointerEventData e)
    {
        touchType.text = "OnPointerUp";
    }

    public void OnDrag(PointerEventData e)
    {
        touchType.text = "OnDrag";
    }
}


