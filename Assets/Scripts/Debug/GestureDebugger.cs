using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureDebugger : MonoBehaviour
{
    GameObject gestureDebugUIobj;
    GameObject gestureDebugUIprefab;
    GameObject player;
    Image touchTimeGauge;
    Image shortTouchTimeGauge;
    Image intervalTimeGauge;

    public void Start()
    {
        player = GameObject.Find("Player");
        gestureDebugUIobj = Instantiate(Resources.Load("DebugUI/GestureDebugUI", typeof(GameObject))) as GameObject;
        gestureDebugUIobj.transform.SetParent(player.transform);
        gestureDebugUIobj.GetComponent<RectTransform>().localPosition = new Vector3(0.8f, -0.2f, -0.8f);
        gestureDebugUIobj.GetComponent<RectTransform>().localScale = new Vector3(5.76f, 5.56f, 0.9f);
        gestureDebugUIobj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        touchTimeGauge = GameObject.Find("TouchTimeGauge").GetComponent<Image>();
        shortTouchTimeGauge = GameObject.Find("ShortTouchTimeGauge").GetComponent<Image>();
        intervalTimeGauge = GameObject.Find("IntervalTimeGauge").GetComponent<Image>();
    }

    public void debugCircling(GestureAnalyzer.GestureState gs)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMvehicleState.text = "Vehicle : " + gs.ToString();
    }

    public void debugCntLongStopTouchTime(float touchTime)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMjumpState.text = "Jump : " + touchTime.ToString();
    }

    public void debugShortTouch(GestureAnalyzer.GestureState gs)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMinteractionState.text = "Interact : " + gs.ToString();
    }

    public void debugFastDoubleTouch(GestureAnalyzer.GestureState gs)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMspecialState.text = "Special : " + gs.ToString();
    }

    public void debugMovingTouch(GestureAnalyzer.GestureState gs)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMmoveState.text = "Move : " + gs.ToString();
    }

    public void debugNoTouch(GestureAnalyzer.GestureState gs)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMidleState.text = "Idle : " + gs.ToString();
    }

    public void debugFastLongDragFromJoystickOrigin(GestureAnalyzer.GestureState gs)
    {
        gestureDebugUIobj.GetComponent<GestureDebugUI>().TMdashState.text = "Dash : " + gs.ToString();
    }

    public void setTouchTimeGauge(float time, float maxTime)
    {
        touchTimeGauge.fillAmount = time / maxTime;
    }

    public void setShortTouchTimeGauge(float time, float maxTime)
    {
        shortTouchTimeGauge.fillAmount = time / maxTime;
    }

    public void setIntervalTimeGauge(float time, float maxTime)
    {
        intervalTimeGauge.fillAmount = time / maxTime;
    }
}


