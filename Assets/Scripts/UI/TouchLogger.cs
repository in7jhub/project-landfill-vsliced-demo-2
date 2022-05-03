using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class TouchLogger
{
    //Log - 제스쳐 검출에 필요한 정보들 : 위치, 이전 점과의 거리
    public struct Log
    {
        public enum DirType { cw, ccw, undefined }
        public Vector2 origin;
        public Vector2[] pos;
        public float[] dist;
        public DirType[] dir;
    }

    public int curIdx;
    public int logLen;
    private const int undefinedNum = -999;
    public Vector2 undefinedPos;
    public Log log;
    private bool isRecordStarted;

    public TouchLogger(int _logLen)
    {
        isRecordStarted = false;
        logLen = _logLen;
        curIdx = 0;
        undefinedPos = new Vector2(undefinedNum, undefinedNum);

        log.origin = undefinedPos;
        log.pos = new Vector2[logLen];
        log.dir = new Log.DirType[logLen];
        log.dist = new float[logLen];

        clear(true);
    }

    public TouchLogger() : this(200) { }

    public void record(PointerEventData e)
    {
        if (isArrOverflown(curIdx, logLen))
        {
            shiftIdx();
            curIdx = logLen - 1;
        }
        else
        {
            curIdx++;
        }
        //모든 로그 저장
        log.pos[curIdx] = e.position;
        Vector2 curPos = log.pos[curIdx];
        Vector2 prevPos = log.pos[curIdx - 1 == -1 ? 0 : curIdx - 1];
        log.dir[curIdx] = calcClockDir(curPos, prevPos, log.origin);
        log.dist[curIdx] = (curPos - log.origin).sqrMagnitude;
        //로그 저장 끝
    }

    private Log.DirType calcClockDir(Vector2 v1, Vector2 v2, Vector2 center)
    {
        float crossProduct = (v2.x - v1.x) * (center.y - v1.y) - (center.x - v1.x) * (v2.y - v1.y);

        if (center == undefinedPos)
        {
            return Log.DirType.undefined;
        }

        if (crossProduct > 0)
        {
            return Log.DirType.cw;
        }

        if (crossProduct < 0)
        {
            return Log.DirType.ccw;
        }

        return Log.DirType.undefined;
    }

    public void startRecord(PointerEventData e)
    {
        log.origin = e.position;
        isRecordStarted = true;
    }

    public void stopRecord()
    {
        isRecordStarted = false;
        clear(true);
    }

    public bool isArrOverflown(int idx, int arrLen)
    {
        if (idx >= arrLen - 1) return true;
        else return false;
    }

    public void shiftIdx()
    {
        for (int i = 0; i < logLen - 2; i++)
        {
            log.dist[i] = log.dist[i + 1];
            log.dir[i] = log.dir[i + 1];
            log.pos[i] = log.pos[i + 1];
            log.dist[i] = log.dist[i + 1];
        }
    }

    public void clear(bool b)
    {
        if (!b) { return; }
        for (int i = 0; i < logLen; i++)
        {
            log.dist[i] = undefinedNum;
            log.dir[i] = Log.DirType.undefined;
            log.pos[i] = undefinedPos;
        }
        curIdx = 0;
    }

    public int getLogLen()
    {
        return logLen;
    }

    public bool getIsRecordStarted()
    {
        return isRecordStarted;
    }
}