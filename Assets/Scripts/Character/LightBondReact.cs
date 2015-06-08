using UnityEngine;
using System.Collections;

public class LightBondReact : MonoBehaviour {

    public GameObject reactingObject;
    public bool activeOnBond = true;
    private bool wasBonded = false;
    private bool turnOn = false;
    void Start()
    {
        wasBonded = !Globals.Instance.playersBonded;
        CheckBond();
    }

    void Update()
    {
        CheckBond();

        if(turnOn)
        {
            if (reactingObject.GetComponent<Light>().spotAngle < 50)
                reactingObject.GetComponent<Light>().spotAngle += Time.deltaTime * 20.0f;

            if (reactingObject.GetComponent<Light>().intensity < 3.5f)
                reactingObject.GetComponent<Light>().intensity += Time.deltaTime * 2.0f;

            //reactingObject.light.intensity = 5.0f;
        }
        if (!turnOn)
        {
            //reactingObject.SetActive(!Globals.Instance.playersBonded);
            if (reactingObject.GetComponent<Light>().spotAngle > 20)
                reactingObject.GetComponent<Light>().spotAngle -= Time.deltaTime * 20.0f;
            if (reactingObject.GetComponent<Light>().intensity > 1.0f)
                reactingObject.GetComponent<Light>().intensity -= Time.deltaTime * 2.0f;

            //reactingObject.light.range = 25.0f;
            //reactingObject.light.intensity = 1.0f;
   
        }


    }

    private void CheckBond()
    {
        if (wasBonded != Globals.Instance.playersBonded && reactingObject != null)
        {
            if (!wasBonded)
            {
                turnOn = true;
            }
            else
            {
                turnOn = false;
            }
            wasBonded = Globals.Instance.playersBonded;
        }
    }
}
