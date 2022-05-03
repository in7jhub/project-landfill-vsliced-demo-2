using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GestureDebugUI : MonoBehaviour
{
    public GameObject controlPanel;
    public Transform player;
    Vector3[] playerPos;

    private Transform vehicleState;
    private Transform jumpState;
    private Transform specialState;
    private Transform interactionState;
    private Transform crawlState;
    private Transform moveState;
    private Transform idleState;
    private Transform dashState;

    public TextMeshProUGUI TMvehicleState;
    public TextMeshProUGUI TMjumpState;
    public TextMeshProUGUI TMspecialState;
    public TextMeshProUGUI TMinteractionState;
    public TextMeshProUGUI TMcrawlState;
    public TextMeshProUGUI TMmoveState;
    public TextMeshProUGUI TMidleState;
    public TextMeshProUGUI TMdashState;

    public Image touchTimeGauge;

    //public string vehicleStr;

    public float holdTimeMax = 2.0f;
    private float curHoldTime = 0f;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        controlPanel = GameObject.Find("ControlPanel");

        playerPos = new Vector3[30];
        for (int i = 0; i < 30; i++)
        {
            playerPos[i] = new Vector3(0, 0, 0);
        }

        vehicleState = transform.Find("VehicleState");
        jumpState = transform.Find("JumpState");
        specialState = transform.Find("SpecialState");
        interactionState = transform.Find("InteractionState");
        crawlState = transform.Find("CrawlState");
        moveState = transform.Find("MoveState");
        idleState = transform.Find("IdleState");
        dashState = transform.Find("DashState");

        TMvehicleState = transform.Find("VehicleState").GetComponent<TextMeshProUGUI>();
        TMjumpState = transform.Find("JumpState").GetComponent<TextMeshProUGUI>();
        TMspecialState = transform.Find("SpecialState").GetComponent<TextMeshProUGUI>();
        TMinteractionState = transform.Find("InteractionState").GetComponent<TextMeshProUGUI>();
        TMcrawlState = transform.Find("CrawlState").GetComponent<TextMeshProUGUI>();
        TMmoveState = transform.Find("MoveState").GetComponent<TextMeshProUGUI>();
        TMidleState = transform.Find("IdleState").GetComponent<TextMeshProUGUI>();
        TMdashState = transform.Find("DashState").GetComponent<TextMeshProUGUI>();
    }
}