using UnityEngine;
using UnityEngine.EventSystems; 
// 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    public enum LRUDN{ Left,Right,Up,Down,Neutral }
    public RectTransform lever;
    public RectTransform plate;
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
    private bool isDragging;
    
    [Range(10f, 150f)] 
    public float leverRange;
    
    private void Awake(){
        leverHeadingLRN = LRUDN.Neutral;
        leverHeadingUDN = LRUDN.Neutral;
        plate = GetComponent<RectTransform>();
        speedLevel = 0;
        isDragging = false;
        plateHalfWidth = plate.sizeDelta.x/2f;
        plateHalfHeight = plate.sizeDelta.y/2f;
        plateWidth = plate.sizeDelta.x;
        plateHeight = plate.sizeDelta.y;
    }

    public void OnBeginDrag(PointerEventData eventData){   
        var inputDir = eventData.position;
        lever.anchoredPosition = inputDir;
    }
    
    public void OnDrag(PointerEventData eventData){
        isDragging = true;
        calcLeverHeadingDir();
        setLeverUDN();
        setLeverLRN();
        setPlayerSpeedLevel();
        setLeverPosition(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData){
        isDragging = false;
        lever.anchoredPosition = plate.position;
        setPlayerSpeedLevel(0);
    }

    private void setLeverPosition(PointerEventData eventData){
        //레버와 플레이트의 거리
        Vector2 inputDir = eventData.position - plate.anchoredPosition;
        //범위 밖으로 끌고나가면 클램프
        Vector2 clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        //레버 위치 지정
        lever.anchoredPosition = clampedDir;
    }

    public void setPlayerSpeedLevel(){ 
        Vector3 offset = plate.position - lever.position;
        float sqrLen = offset.sqrMagnitude;
        if(sqrLen == 0){ return; }
        if(sqrLen > 18400){
            speedLevel = 1;
        } else if(sqrLen > 9000){
            speedLevel = 2;
        }
    }

    public void setPlayerSpeedLevel(int n){ 
        if(n >= 0 && n <3) speedLevel = n;
    }

    private void calcLeverHeadingDir(){
        leverVec = new Vector2(
            lever.anchoredPosition.x - plate.position.x,
            lever.anchoredPosition.y - plate.position.y
        );
        
        leverDir = leverVec.normalized;
    }

    private void setLeverLRN(){
        if(leverDir.x < 0) leverHeadingLRN = LRUDN.Left;
        else if(leverDir.x > 0) leverHeadingLRN = LRUDN.Right;
        else leverHeadingLRN = LRUDN.Neutral;
    }

    private void setLeverUDN(){
        if(leverDir.y < 0) leverHeadingUDN = LRUDN.Down;
        else if(leverDir.y > 0) leverHeadingUDN = LRUDN.Up;
        else leverHeadingUDN = LRUDN.Neutral;
    }

    public int getLeverLRN(){
        if(leverHeadingLRN == LRUDN.Left) return 0;
        else if(leverHeadingLRN == LRUDN.Right) return 1;
        else return 4;
    }
    
    public int getLeverUDN(){
        if(leverHeadingUDN == LRUDN.Up) return 2;
        else if(leverHeadingUDN == LRUDN.Down) return 3;
        else return 4;
    }

    public bool getIsDragging(){
        return isDragging;
    }

    public int getPlayerSpeedLevel(){ return speedLevel; }

    public Vector2 getLeverDir(){
        if(leverDir != null){
            return this.leverDir;
        } else {
            return Vector2.zero;
        }
    }
}