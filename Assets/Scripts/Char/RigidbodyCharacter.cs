using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour {
    public enum LRUDN{ Left,Right,Up,Down,Neutral }
    private LRUDN playerHeadingLRN;
    private LRUDN playerHeadingUDN;
    public float speed;
    public Rigidbody rb;
    Vector2 playerDir;
    public VirtualJoystick controller;

    void Start(){
        playerHeadingLRN = LRUDN.Neutral;
        playerHeadingUDN = LRUDN.Neutral;
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        playerDir = getLeverDir(controller);
    }

    private void FixedUpdate(){
        rb.AddForce(new Vector3(playerDir.x*speed,1/*!!!중력이 유지되도록 바꾸기!!!*/,playerDir.y*speed));
        // Debug.Log(playerHeadingLRN);
        // Debug.Log(playerHeadingUDN);
    }

    Vector2 getLeverDir(VirtualJoystick _controller){
        if(_controller.isDragging){
            return _controller.getLeverDir();
        } else {
            return Vector2.zero;
        }
    }

    private void setPlayerLRN(VirtualJoystick _controller){
        if(_controller.getLeverLRN() == 0) playerHeadingLRN = LRUDN.Left;
        else if(_controller.getLeverLRN() == 1) playerHeadingLRN = LRUDN.Right;
        else playerHeadingLRN = LRUDN.Neutral;
    }

    private void setPlayerUDN(VirtualJoystick _controller){
        if(_controller.getLeverUDN() == 2) playerHeadingUDN = LRUDN.Up;
        else if(_controller.getLeverUDN() == 3) playerHeadingUDN = LRUDN.Down;
        else playerHeadingUDN = LRUDN.Neutral;
    }
}
