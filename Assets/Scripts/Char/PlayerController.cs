using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum LRUDN { Left, Right, Up, Down, Neutral }
    private bool isPlayerPreparingVehicle;
    private LRUDN playerHeadingLRN;
    private LRUDN playerHeadingUDN;
    public float speed;
    public Rigidbody rb;
    Vector2 playerDir;
    public VirtualJoystick joystick;

    void Start()
    {
        playerHeadingLRN = LRUDN.Neutral;
        playerHeadingUDN = LRUDN.Neutral;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerDir = getLeverDir(joystick);
        setPlayerLRN(joystick);
        setPlayerUDN(joystick);
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(playerDir.x * speed, -100, playerDir.y * speed));
    }

    Vector2 getLeverDir(VirtualJoystick joystick)
    {
        if (joystick.getIsDragging())
        {
            return joystick.getLeverDir();
        }
        else
        {
            return Vector2.zero;
        }
    }

    public void printPlayerHeadingLRUDN()
    {
        Debug.Log(playerHeadingLRN);
        Debug.Log(playerHeadingUDN);
    }

    private void setPlayerLRN(VirtualJoystick _controller)
    {
        if (_controller.getLeverLRN() == 0) playerHeadingLRN = LRUDN.Left;
        else if (_controller.getLeverLRN() == 1) playerHeadingLRN = LRUDN.Right;
        else playerHeadingLRN = LRUDN.Neutral;
    }

    private void setPlayerUDN(VirtualJoystick _controller)
    {
        if (_controller.getLeverUDN() == 2) playerHeadingUDN = LRUDN.Up;
        else if (_controller.getLeverUDN() == 3) playerHeadingUDN = LRUDN.Down;
        else playerHeadingUDN = LRUDN.Neutral;
    }
}

