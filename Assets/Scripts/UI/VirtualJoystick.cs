using UnityEngine;
using UnityEngine.EventSystems; 
// 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler{
    public enum LRUDN{ Left,Right,Up,Down,Neutral }
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
    
    [Range(10f, 150f)] 
    public float leverRange;
    
    private void Awake(){
        leverRectTransform.localPosition = Vector2.zero;
        // plateRectTransform.position = new Vector2(100,100);
        leverHeadingLRN = LRUDN.Neutral;
        leverHeadingUDN = LRUDN.Neutral;
        speedLevel = 0;
        isDragging = false;
        plateHalfWidth = plateRectTransform.sizeDelta.x/2f;
        plateHalfHeight = plateRectTransform.sizeDelta.y/2f;
        plateWidth = plateRectTransform.sizeDelta.x;
        plateHeight = plateRectTransform.sizeDelta.y;
    }

    // private void Update(){
    //     if(Input.touchCount > 0){
    //         Debug.Log("asd;lfkdas");
    //         Touch touch = Input.GetTouch(0);
    //         Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
    //         plate.anchoredPosition = pos;
    //     }
    // }

    public void OnBeginDrag(PointerEventData e){
        plateRectTransform.position = e.position;
    }

    public void OnDrag(PointerEventData e){
        isDragging = true;
        setLeverUDN();
        setLeverLRN();
        setPlayerSpeedLevel();
        setLeverPosition(e);
        setLeverHeadingDir(leverRectTransform, plateRectTransform);
    }

    private void OnEndDrag(PointerEventData e){
        isDragging = false;
        // setJoystickVisibility(false);
        // lever.anchoredPosition = plate.anchoredPosition;
        // setPlayerSpeedLevel(0);
    }

    public void OnPointerDown(PointerEventData e){
        plateRectTransform.position = e.position;
    }

    public void OnPointerUp(PointerEventData e)
    {
        leverRectTransform.localPosition = Vector2.zero;
        plateRectTransform.localPosition = new Vector2(0, -500);
        setLeverHeadingDir(leverRectTransform, plateRectTransform);
    }

    private void setPlatePosition(PointerEventData e){plateRectTransform.position = e.position;}

    private void setLeverPosition(PointerEventData e){
        leverRectTransform.position = e.position;
        leverRectTransform.localPosition = Vector2.ClampMagnitude(
            e.position - (Vector2)plateRectTransform.position,
            plateRectTransform.rect.width * 0.5f
        );
        // //레버와 플레이트의 거리
        // Vector2 inputDir = e.position - plate.anchoredPosition;
        // //범위 밖으로 끌고나가면 클램프
        // Vector2 clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        // //레버 위치 지정
        // lever.anchoredPosition = clampedDir;
    }

    public void setPlayerSpeedLevel(){ 
        Vector3 offset = plateRectTransform.position - leverRectTransform.position;
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

    private void setLeverHeadingDir(RectTransform _lever, RectTransform _plate){
        leverDir = new Vector2(
            _lever.position.x - _plate.position.x,
            _lever.position.y - _plate.position.y
        ).normalized;
    }

    public void setJoystickVisibility(bool _visibility){
        plateGObj.SetActive(_visibility);
        leverGObj.SetActive(_visibility);
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

    public int getPlayerSpeedLevel(){ return speedLevel; }

    public Vector2 getLeverDir(){
        if(leverDir != null){
            return this.leverDir;
        } else {
            return Vector2.zero;
        }
    }
}
