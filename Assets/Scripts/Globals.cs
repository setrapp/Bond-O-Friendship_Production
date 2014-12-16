using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {


	public enum JoyStick{Joy1, Joy2, Joy3, Joy4};
	
	public enum ControlScheme{sticks, triggers};
	
	public static ControlScheme playerOneControlScheme;
	public static ControlScheme playerTwoControlScheme;

	
	public static JoyStick playerOneJoystickNumber;
	public static JoyStick playerTwoJoystickNumber;

	public static bool sharing = false;
	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

}
