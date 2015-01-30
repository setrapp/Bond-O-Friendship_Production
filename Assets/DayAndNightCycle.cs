using UnityEngine;
using System.Collections;

public class DayAndNightCycle : MonoBehaviour {

    public GameObject directionalLight;
    public short dayDuration = 30;
    public float angleAmplitude = 45.0f;
    public float rate = 0.05f;
    private float dayTime = 0.0f;
    private float dayBegin;
    private Transform temp;

	// Use this for initialization
	void Start () {
        //temp = directionalLight.transform;
        dayBegin = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (dayTime - dayBegin > dayDuration)
        {
            dayBegin = Time.time;
            dayTime = 0.0f;
        }
        dayTime = Time.time - dayBegin;
        directionalLight.transform.Rotate(0.0f, rate, 0.0f);
	}
}
