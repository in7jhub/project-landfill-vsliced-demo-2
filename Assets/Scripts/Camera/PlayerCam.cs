using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform player;
    Vector3 targetPosition;
    Vector3 camOffset;
    Vector2 joystickInfluence;
    Vector2 joystickInfluenceMax;
    public float joystickInfluenceSpeed = 0.06f;
    public float camSpeed = 0.01f;
    public VirtualJoystick joystick;

    void Start()
    {
        Application.targetFrameRate = 120;
        targetPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        camOffset = new Vector3(1, 25f, -35f);
        joystickInfluence = new Vector2(0f, 0f);
        joystickInfluenceMax = new Vector2(2f, 4f);
    }

    void FixedUpdate()
    {
        joystickInfluence = Vector2.Lerp(
            joystickInfluence,
            new Vector2(
                joystick.getLeverDir().x * joystickInfluenceMax.x,
                joystick.getLeverDir().y * joystickInfluenceMax.y
            ),
            joystickInfluenceSpeed * (joystick.getIsDragging() ? 1 : 10)
        );

        targetPosition = new Vector3(
            player.position.x + joystickInfluence.x,
            player.position.y + camOffset.y,
            player.position.z + camOffset.z + joystickInfluence.y
        );

        transform.position = new Vector3(
            targetPosition.x,
            Vector3.Lerp(transform.position, targetPosition, camSpeed).y,
            targetPosition.z
        );
    }
}
