using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Globals : MonoBehaviour {
	private static Globals instance = null;
	public static Globals Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject instanceObject = GameObject.FindGameObjectWithTag("Globals");
				if (instanceObject != null)
				{
					instance = instanceObject.GetComponent<Globals>();
				}
			}
			return instance;
		}
	}

	public enum JoyStick{Joy1, Joy2, Joy3, Joy4};

	public enum ControlScheme{sticks, triggers};

	public PlayerInput player1;
	public PlayerInput player2;

	public GameObject initialPlayerHolder = null;

	public bool autoAttractor = false;
	public bool fluffsThrowable = true;
	public float fluffLeaveDistance = 1.0f;
	public float fluffLeaveAttractWait = 3.0f;
	public float fluffLeaveEmbed = 1.0f;

    public static InputDevice startingDevice;
    public static InputDevice playerOneDevice;
    public static InputDevice playerTwoDevice;

    public static int numberOfControllers;

    public static bool isPaused;

    public static bool usingController;

	public GameObject canvasPaused;

	[Header("Fluff Depth Mask")]
	public bool visibilityDepthMaskNeeded = false;
	[Header("Fluff Depth Mask")]
    public GameObject depthMaskPrefab;
	[Header("Fluff Depth Mask")]
    public GameObject depthMaskHolderPrefab;

	public GameObject orphanFluffHolderPrefab;

	[SerializeField]
	public List<Fluff> allFluffs;

	public bool updatePlayersOnLoad = true;

	public static bool sharing = false;

	public EtherRing existingEther = null;

	void Awake()
	{
		if (!Application.isEditor)
		{
			Screen.showCursor = false;
		}
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
