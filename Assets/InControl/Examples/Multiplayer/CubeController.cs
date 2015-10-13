using System;
using UnityEngine;
using InControl;


namespace MultiplayerExample
{
	public class CubeController : MonoBehaviour
	{
		Renderer cachedRenderer;

		public int playerNum;

        public InputDevice inputDevice;

		void Start()
		{
			cachedRenderer = GetComponent<Renderer>();
		}



		void Update()
		{

           

			if (inputDevice == null)
			{
				// If no controller exists for this cube, just make it translucent.
				cachedRenderer.material.color = new Color( 1.0f, 1.0f, 1.0f, 0.2f );
			}
			else
			{
                if (!inputDevice.IsAttached)
                {
                    if(playerNum == 0)
                    Debug.Log(InputManager.Devices.IndexOf(inputDevice));
                   // Debug.Log("Here");
                }
                else
                {
                    
                    UpdateCubeWithInputDevice(inputDevice);
                }
			}

            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckForController();
            }

            
            
		}

        void CheckForController()
        {
            inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
        }


		void UpdateCubeWithInputDevice( InputDevice inputDevice )
		{
			// Set object material color based on which action is pressed.
			if (inputDevice.Action1)
			{
				cachedRenderer.material.color = Color.green;
			}
			else
			if (inputDevice.Action2)
			{
				cachedRenderer.material.color = Color.red;
			}
			else
			if (inputDevice.Action3)
			{
				cachedRenderer.material.color = Color.blue;
			}
			else
			if (inputDevice.Action4)
			{
				cachedRenderer.material.color = Color.yellow;
			}
			else
			{
				cachedRenderer.material.color = Color.white;
			}
			
			// Rotate target object with both sticks and d-pad.
			transform.Rotate( Vector3.down, 500.0f * Time.deltaTime * inputDevice.Direction.X, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.Direction.Y, Space.World );
			transform.Rotate( Vector3.down, 500.0f * Time.deltaTime * inputDevice.RightStickX, Space.World );
			transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.RightStickY, Space.World );
		}
	}
}

