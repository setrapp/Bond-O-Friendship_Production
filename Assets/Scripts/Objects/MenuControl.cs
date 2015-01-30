using UnityEngine;
using System.Collections;
using InControl;

public class MenuControl : MonoBehaviour {

	public string startScene;
    InputDevice device;
	

	// Use this for initialization
	void Start () {
        Globals.playerOneDevice = InputManager.Devices[0];
        Globals.startingDevice = InputManager.Devices[0];
        Globals.numberOfControllers = InputManager.Devices.Count;
    }


	// Update is called once per frame
    void Update()
    {
        GameObject[] messages = GameObject.FindGameObjectsWithTag("TranslevelMessage");
        bool levelLoadFound = false;
        for (int i = 0; i < messages.Length && !levelLoadFound; i++)
        {
            TranslevelMessage message = messages[i].GetComponent<TranslevelMessage>();
            if (message != null && message.messageName == "LevelLoad")
            {
                startScene = message.message;
                Destroy(message.gameObject);
            }
        }
    }		

    public void SetPlayerDevices()
    {

        if (InputManager.Devices.Count == 1)
            Globals.playerTwoDevice = InputManager.Devices[0];
        else if (InputManager.Devices.Count > 1)
            Globals.playerTwoDevice = InputManager.Devices[1];
        else
            Globals.playerTwoDevice = null;
    }
    public void MainMenuLoadLevel()
    {
        Application.LoadLevel(startScene);
    }

}
