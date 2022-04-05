using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour {
    public float speed = 10f;
    Rigidbody rb;
    Vector3 dir = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	// 키 입력과 이동방향 계산
    void Update()
    {
        InputAndDir();
    }

	// 계산된 방향으로 물리적인 이동 구현
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
    }

	// 키 입력과 그에 따른 이동방향을 계산하는 함수
    void InputAndDir()
    {
        dir.x = Input.GetAxis("Horizontal");   // x축 방향 키 입력
        dir.z = Input.GetAxis("Vertical");     // z축 방향 키 입력
        if (dir != Vector3.zero)   // 키입력이 존재하는 경우
        {
            transform.forward = dir;	// 키 입력 시, 입력된 방향으로 캐릭터의 방향을 바꿈
        }
    }
}