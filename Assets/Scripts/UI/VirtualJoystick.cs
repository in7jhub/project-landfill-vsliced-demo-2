using UnityEngine;
using UnityEngine.EventSystems; 
// 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    private Vector2 leverVector;
    private int playerSpeedLevel;
    private int playerHeadingAngle;
    private bool isDragging;

    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
        playerSpeedLevel = 0;
        isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData){   
        var inputDir = eventData.position;
        lever.anchoredPosition = inputDir;
    }
    
    public void OnDrag(PointerEventData eventData){
        isDragging = true;
        setPlayerHeadingAngle();
        setPlayerSpeedLevel();
        var inputDir = eventData.position ;
        lever.anchoredPosition = inputDir;
    }
    
    public void OnEndDrag(PointerEventData eventData){
        isDragging = false;
        lever.anchoredPosition = rectTransform.position;
        setPlayerSpeedLevel(0);
    }

    public void setPlayerSpeedLevel(){ 
        Vector3 offset = rectTransform.position - lever.position;
        float sqrLen = offset.sqrMagnitude;
        if(sqrLen == 0){ return; }
        if(sqrLen > 18400){
            playerSpeedLevel = 1;
        } else if(sqrLen > 9000){
            playerSpeedLevel = 2;
        }
    }

    public void setPlayerSpeedLevel(int n){ 
        if(n >= 0 && n <3) playerSpeedLevel = n;
    }

    public void setPlayerHeadingAngle(){
        float width = rectTransform.position.x - lever.anchoredPosition.x;
        float height = rectTransform.position.y - lever.anchoredPosition.y;
        
        float radian = Mathf.Atan2(height,width);
        float angle = radian * 180 / Mathf.PI;
        //transform.rotation = Quaternion.Euler(0, 0, angle); //0~360 환산
    }

    public bool getIsDragging(){
        return isDragging;
    }

    public int getPlayerSpeedLevel(){ return playerSpeedLevel; }
}