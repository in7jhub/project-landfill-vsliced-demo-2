using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureAnalyzer : MonoBehaviour
{
    public TouchLogger touch;
    public GestureDebugger debugger;
    private VirtualJoystick joystick;

    public float cntLongTouchTime = 0;
    public float cntShortTouchTime = 0;
    public float shortTouchTimeUnit;
    private int cntShortTouch = 0;
    public float cntTouchIntervalTime = 0;
    public float shortTouchIntervalTimeUnit;
    public float meaninglessLongTime;
    public float doubleTouchCooldown = 0;
    public float doubleTouchCooltimeUnit;
    public int touchSampleRate = 400;
    public bool isDebuggerOn;
    public bool cntShortTouchFlag = true;
    public bool isDoubleShortTouch = false;
    public float shortTouchCooldown = 0;
    public float shortTouchCooltime;

    public enum GestureState { none, ready, fire }

    public void Start()
    {
        joystick = transform.GetComponent<VirtualJoystick>();
        touch = new TouchLogger(joystick.touchSampleRate);

        shortTouchTimeUnit = Time.deltaTime * 15f;
        shortTouchIntervalTimeUnit = Time.deltaTime * 10f;
        meaninglessLongTime = Time.deltaTime * 100f;
        doubleTouchCooltimeUnit = Time.deltaTime * 100f;
        shortTouchCooltime = Time.deltaTime * 30f;
    }

    private void Update()
    {
        //황금 디버그 코드 지우지 마
        // Debug.Log(
        //     "coolT : " + doubleTouchCooldown.ToString() + '\n' +
        //     "shortT: " + cntShortTouchTime.ToString() + '\n' +
        //     "long  :" + cntLongTouchTime.ToString() + '\n' +
        //     "intv  :" + cntTouchIntervalTime.ToString() + '\n' +
        //     "shtFir: " + detectShortTouch(cntShortTouchTime, cntTouchIntervalTime).ToString() + '\n'
        // );

        //트리플터치 등을 방지하는 쿨타임
        if (doubleTouchCooldown > 0)
        {
            doubleTouchCooldown -= Time.deltaTime;
            if (doubleTouchCooldown < 0)
            {
                doubleTouchCooldown = 0;
            }
        }

        //숏터치 남용을 방지하는 쿨타임
        if (shortTouchCooldown > 0)
        {
            shortTouchCooldown -= Time.deltaTime;
            if (shortTouchCooldown < 0)
            {
                shortTouchCooldown = 0;
            }
        }

        //터치한 순간 플래그 온, 유효 숏터치 시간 후 오프
        if (joystick.shortTouchTimerFlag)
        {
            cntShortTouchTime += Time.deltaTime;
            if (cntShortTouchFlag)
            {
                cntTouchIntervalTime = 0;
                cntShortTouch++;
                cntShortTouchFlag = false;
            }
            if (cntShortTouchTime > shortTouchTimeUnit)
            {
                cntShortTouch = 0;
                cntShortTouchTime = 0;
                joystick.shortTouchTimerFlag = false;
            }
        }
        else
        {
            cntShortTouchTime = 0;
        }

        //터치한 순간 플래그 온, 터치 종료 시 오프 
        if (joystick.longTouchTimerFlag)
        {
            cntLongTouchTime += Time.deltaTime;
            if (cntLongTouchTime > meaninglessLongTime)
            {
                cntLongTouchTime = meaninglessLongTime;
            }
        }
        else
        {
            cntLongTouchTime = 0;
        }

        //터치 떼는 순간 플래그 온, 유효 시간이 지나면 오프 
        if (joystick.touchIntervalTimerFlag && cntTouchIntervalTime < shortTouchIntervalTimeUnit)
        {
            cntShortTouchFlag = true;
            if (cntShortTouch > 1)
            {
                isDoubleShortTouch = true;
            }
            cntTouchIntervalTime += Time.deltaTime;
        }
        else
        {
            joystick.touchIntervalTimerFlag = false;
        }

        if (cntTouchIntervalTime > shortTouchIntervalTimeUnit - Time.deltaTime * 2)
        {
            cntShortTouch = 0;
        }

        if (isDebuggerOn)
        {
            debugger.debugShortTouch(GestureState.fire);
            debugger.setShortTouchTimeGauge(cntShortTouchTime, shortTouchTimeUnit);
            debugger.setIntervalTimeGauge(cntTouchIntervalTime, shortTouchIntervalTimeUnit);
        }

        //위에서 잰 시간을 기반으로 제스처 탐지
        detectCircling();
        cntLongStopTouchTime();
        detectShortTouch(cntShortTouchTime, cntTouchIntervalTime);
        detectMovingTouch();
        detectNoTouch();
        detectFastLongDragFromJoystickOrigin();
        detectFastDoubleTouch(cntShortTouchTime, cntTouchIntervalTime);
    }

    public GestureState detectCircling()
    {
        int cntCCW = 0;
        int cntCW = 0;
        int cntUndefinedValue = 0;
        bool ticketToFire = true;
        for (int i = 0; i < touch.getLogLen() - 1; i++)
        {
            if (touch.log.dir[i] == TouchLogger.Log.DirType.undefined)
            {
                //로그 탐색 중에 undefined가 3번 걸리면 none 반환
                cntUndefinedValue++;
                if (cntUndefinedValue > 3)
                {
                    //ticketToFire = false;
                }
            }

            if (touch.log.dir[i] == TouchLogger.Log.DirType.ccw) { cntCCW++; }

            if (touch.log.dir[i] == TouchLogger.Log.DirType.cw) { cntCW++; }

            //방향이 일관적이지 않으면 로그 지워버리고 none 반환
            if (cntCCW > 3 && cntCW > 3)
            {
                //touch.clear(true);
                cntCCW = 0;
                cntCW = 0;
                ticketToFire = false;
            }
        }

        if ((cntCCW > 150 || cntCW > 150) && ticketToFire)
        {
            if (isDebuggerOn) { debugger.debugCircling(GestureState.fire); }
            return GestureState.fire;
        }

        if ((cntCCW > 50 || cntCW > 50) && ticketToFire)
        {
            if (isDebuggerOn) { debugger.debugCircling(GestureState.ready); }
            return GestureState.ready;
        }

        // 구현해야하는 것 회전시간, 로그 이동거리 합산 마음에 드는지.
        if (isDebuggerOn) { debugger.debugCircling(GestureState.none); }
        return GestureState.none;
    }

    public float cntLongStopTouchTime()
    {
        if (!isStopTouch()) { cntLongTouchTime = 0; }
        if (isDebuggerOn)
        {
            debugger.debugCntLongStopTouchTime(cntLongTouchTime);
            debugger.setTouchTimeGauge(cntLongTouchTime, meaninglessLongTime);
        }
        return cntLongTouchTime;
    }

    public GestureState detectShortTouch(float _touchTime, float _touchIntervalTime)
    {
        if (cntShortTouch == 1
        && _touchIntervalTime > shortTouchIntervalTimeUnit - Time.deltaTime * 3
        && !isDoubleShortTouch)
        {
            shortTouchCooldown = shortTouchCooltime;
            if (isDebuggerOn) { debugger.debugShortTouch(GestureState.fire); }
            return GestureState.fire;
        }
        if (isDebuggerOn && shortTouchCooldown == 0) { debugger.debugShortTouch(GestureState.none); }
        return GestureState.none;
    }

    public GestureState detectFastDoubleTouch(float _touchTime, float _touchIntervalTime)
    {
        Debug.Log(cntShortTouch);
        if (_touchIntervalTime < shortTouchIntervalTimeUnit
            && doubleTouchCooldown == 0
            && isDoubleShortTouch)
        {
            cntShortTouch = 0;
            isDoubleShortTouch = false;
            doubleTouchCooldown = doubleTouchCooltimeUnit;
            if (isDebuggerOn) { debugger.debugFastDoubleTouch(GestureState.fire); }
            return GestureState.fire;
        }

        if (isDebuggerOn && doubleTouchCooldown == 0) { debugger.debugFastDoubleTouch(GestureState.none); }
        return GestureState.none;
    }

    public GestureState detectMovingTouch()
    {
        if (detectNoTouch() == GestureState.fire)
        {
            if (isDebuggerOn) { debugger.debugMovingTouch(GestureState.none); }
            return GestureState.none;
        }

        if (!isStopTouch())
        {
            if (isDebuggerOn) { debugger.debugMovingTouch(GestureState.fire); }
            return GestureState.fire;
        }

        if (isDebuggerOn) { debugger.debugMovingTouch(GestureState.none); }
        return GestureState.none;
    }

    public GestureState detectNoTouch()
    {
        if (touch.log.pos[touch.curIdx] == touch.undefinedPos)
        {
            joystick.stopTouchFlag = true;
            if (isDebuggerOn) { debugger.debugNoTouch(GestureState.fire); }
            return GestureState.fire;
        }
        else
        {
            if (isDebuggerOn) { debugger.debugNoTouch(GestureState.none); }
            return GestureState.none;
        }
    }

    public GestureState detectFastLongDragFromJoystickOrigin()
    {
        return GestureState.none;
    }

    public bool isStopTouch()
    {
        if (!joystick.stopTouchFlag) { return false; }
        float xLen = Mathf.Abs(touch.log.origin.x - touch.log.pos[touch.curIdx].x);
        float yLen = Mathf.Abs(touch.log.origin.y - touch.log.pos[touch.curIdx].y);

        if (xLen + yLen < joystick.movingDetectionRange)
        {
            return true;
        }
        else
        {
            joystick.stopTouchFlag = false;
            return false;
        }
    }

    public int getCntShortTouch()
    {
        return cntShortTouch;
    }
}