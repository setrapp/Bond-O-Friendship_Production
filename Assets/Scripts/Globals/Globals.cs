using UnityEngine;
using System.Collections;
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


    public static InputDevice startingDevice;
    public static InputDevice playerOneDevice;
    public static InputDevice playerTwoDevice;

    public static int numberOfControllers;

    public static bool isPaused;

	public GameObject canvasPaused;

	public bool updatePlayersOnLoad = true;

	public static bool sharing = false;
	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

}
