using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour {
    public float speed = 10f;
    public Rigidbody rb;
    Vector3 force;
    public VirtualJoystick controller;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

	// 키 입력과 이동방향 계산
    void Update(){
        InputAndDir();
        rb.AddForce(new Vector3(1,0,0));
    }

	// 계산된 방향으로 물리적인 이동 구현
    private void FixedUpdate(){
        rb.AddForce(new Vector3(1,0,0));
    }

	// 키 입력과 그에 따른 이동방향을 계산하는 함수
    void InputAndDir(){
        // dir.x = Input.GetAxis("Horizontal");   // x축 방향 키 입력
        // dir.z = Input.GetAxis("Vertical");     // z축 방향 키 입력
        // Debug.Log("X: "+dir.x);
        // Debug.Log("Z: "+dir.z);
        if (controller.getIsDragging()){
            //transform.velocity = Quaternion.Euler(0,0,0);
            //현재 보고 있는 방향을 정의
        }
    }
}
