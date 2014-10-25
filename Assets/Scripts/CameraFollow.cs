using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform player1;
    public Transform player2;
    public float smoothness = 20;
    private Vector3 targetPosition;

	void Update () {
        targetPosition = new Vector3((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, -10);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1/smoothness);
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1 / smoothness);
	}
}
