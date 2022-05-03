using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum LRUDN { Left, Right, Up, Down, Neutral }
    public GameObject leverGObj;
    public GameObject plateGObj;
    public RectTransform leverRectTransform;
    public RectTransform plateRectTransform;
    private float plateHalfWidth;
    private float plateHalfHeight;
    private float plateWidth;
    private float plateHeight;
    private Vector2 leverVec;
    private Vector2 leverDir;
    private int speedLevel;
    private int leverHeadingAngle;
    public LRUDN leverHeadingLRN;
    public LRUDN leverHeadingUDN;
    public bool isDragging;
    public bool shortTouchTimerFlag;
    public bool longTouchTimerFlag;
    public bool touchIntervalTimerFlag;
    public bool circleFlag;
    public bool stopTouchFlag;
    public GameObject controlPanel;

    [Range(150, 200)]
    public int dashDetectionRange;

    [Range(5, 30)]
    public int movingDetectionRange;

    public GestureAnalyzer gesture;

    [Range(10, 150)]
    public float leverRange;

    [SerializeField]
    public bool gestureDebuggerOn;
    public int touchSampleRate;

    private void Awake()
    {
        //pui.vehicleStr = "game";
        leverRectTransform.localPosition = Vector2.zero;
        leverHeadingLRN = LRUDN.Neutral;
        leverHeadingUDN = LRUDN.Neutral;
        speedLevel = 0;
        isDragging = false;
        plateHalfWidth = plateRectTransform.sizeDelta.x / 2f;
        plateHalfHeight = plateRectTransform.sizeDelta.y / 2f;
        plateWidth = plateRectTransform.sizeDelta.x;
        plateHeight = plateRectTransform.sizeDelta.y;

        controlPanel = controlPanel.AddComponent<GestureAnalyzer>().gameObject;

        if (gestureDebuggerOn)
        {
            gesture.debugger = controlPanel.AddComponent<GestureDebugger>();
            gesture.touchSampleRate = touchSampleRate;
            gesture.isDebuggerOn = true;
        }
        //circleFlag = false;
        //coroutineFindingCirclingGesture = null;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        plateRectTransform.position = e.position;
    }

    public void OnDrag(PointerEventData e)
    {
        gesture.touch.record(e);
        isDragging = true;
        setLeverUDN();
        setLeverLRN();
        setLeverPosition(e);
        setLeverHeadingDir(leverRectTransform, plateRectTransform);
    }

    private void OnEndDrag(PointerEventData e)
    {
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData e)
    {
        stopTouchFlag = true;
        gesture.touch.record(e);
        touchIntervalTimerFlag = false;
        longTouchTimerFlag = true;
        circleFlag = true;
        shortTouchTimerFlag = true;
        // if (coroutineFindingCirclingGesture != null) StopCoroutine(findCirclingGesture(e));
        // else coroutineFindingCirclingGesture = StartCoroutine(findCirclingGesture(e));

        plateRectTransform.position = e.position;
        gesture.touch.log.origin = e.position;
    }

    public void OnPointerUp(PointerEventData e)
    {
        stopTouchFlag = false;
        //circleFlag = false;
        gesture.touch.stopRecord();
        //황금 디버그 코드 Debug.Log("dbTch : " + gesture.detectFastDoubleTouch(gesture.cntShortTouchTime, gesture.cntTouchIntervalTime).ToString() + '\n');
        touchIntervalTimerFlag = true;
        shortTouchTimerFlag = false;
        circleFlag = false;
        longTouchTimerFlag = false;
        isDragging = false;
        //stopGestureCoroutine(ref coroutineFindingCirclingGesture);

        // if (gesture.detectLongStopTouch() > Time.deltaTime * 30)
        // {
        //     gesture.qwe.jumpState = GestureAnalyzer.GestureState.fire;

        // }
        leverRectTransform.localPosition = Vector2.zero;
        plateRectTransform.localPosition = new Vector2(0, -500);
        setLeverHeadingDir(leverRectTransform, plateRectTransform);
    }

    // private void stopGestureCoroutine(ref Coroutine c)
    // {
    //     StopCoroutine(c);
    //     c = null;
    // }

    private void setPlatePosition(PointerEventData e)
    {
        plateRectTransform.position = e.position;
    }

    private void setLeverPosition(PointerEventData e)
    {
        Vector2 pos = e.position - (Vector2)gesture.touch.log.origin;
        pos = Vector2.ClampMagnitude(pos, leverRange);
        leverRectTransform.localPosition = pos;
    }

    private void setLeverHeadingDir(RectTransform _lever, RectTransform _plate)
    {
        leverDir = new Vector2(
            _lever.position.x - _plate.position.x,
            _lever.position.y - _plate.position.y
        ).normalized;
    }

    public void setJoystickVisibility(bool _visibility)
    {
        plateGObj.SetActive(_visibility);
        leverGObj.SetActive(_visibility);
    }

    private void setLeverLRN()
    {
        if (leverDir.x < 0) leverHeadingLRN = LRUDN.Left;
        else if (leverDir.x > 0) leverHeadingLRN = LRUDN.Right;
        else leverHeadingLRN = LRUDN.Neutral;
    }

    private void setLeverUDN()
    {
        if (leverDir.y < 0) leverHeadingUDN = LRUDN.Down;
        else if (leverDir.y > 0) leverHeadingUDN = LRUDN.Up;
        else leverHeadingUDN = LRUDN.Neutral;
    }

    public int getLeverLRN()
    {
        if (leverHeadingLRN == LRUDN.Left) return 0;
        else if (leverHeadingLRN == LRUDN.Right) return 1;
        else return 4;
    }

    public int getLeverUDN()
    {
        if (leverHeadingUDN == LRUDN.Up) return 2;
        else if (leverHeadingUDN == LRUDN.Down) return 3;
        else return 4;
    }

    public int getPlayerSpeedLevel() { return speedLevel; }

    public Vector2 getLeverDir()
    {
        if (leverDir != null)
        {
            return this.leverDir;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public bool getIsDragging()
    {
        return isDragging;
    }

    // IEnumerator findCirclingGesture(PointerEventData e)
    // {
    //     gesture.detectCircling();
    //     yield return new WaitForSeconds(0.2f);
    // }
}







