using UnityEngine;
using System.Collections;

public class SpawnLocater : MonoBehaviour {

    public bool isStart = true;

	void Start()
    {
        if (Globals.Instance != null)
        {
            if (isStart)
            {
                Globals.Instance.startSpawnLocation = gameObject;
            }
            else
            {
                Globals.Instance.continueSpawnLocation = gameObject;
            }
        }
    }
}
