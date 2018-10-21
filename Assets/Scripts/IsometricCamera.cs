using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour {

    public GameObject player;
    public GameObject target;
    private bool newFocus;
    private float maxFocusTime = 4.0f;
    private float currentFocusTime;

    public float height;
    public float zDisp;

    public float cameraSpeed = 1.0f;
    private Vector3 newCamPos;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(transform.position.x, height, transform.position.z - zDisp);
        target = player;
	}
	
	// Update is called once per frame
	void Update () {

        //If Player Alive...
        if (player) {
            CameraMovement();
        }

        if(newFocus) {
            currentFocusTime += Time.deltaTime;
            if(currentFocusTime >= maxFocusTime) {
                ResetCamera();
            }
        }

	}

    public void ResetCamera() {
        newFocus = false;
        currentFocusTime = 0;
        target = player;
        player.GetComponent<Player>().SetCanMove(true);
    }

    public void SetTimedCameraTarget(GameObject newTarget, float focusTime) {
        target = newTarget;
        maxFocusTime = focusTime;
        newFocus = true;
        player.GetComponent<Player>().SetCanMove(false);
    }

    public void SetCameraTarget(GameObject newTarget) {
        target = newTarget;
        player.GetComponent<Player>().SetCanMove(false);
    }

    //Camera Pans (Lerps) towards position above player avatar
    void CameraMovement() {

        if(target != null) {
            newCamPos = target.transform.position;

            newCamPos.y = target.transform.position.y + height;
            newCamPos.z = target.transform.position.z - zDisp;

            transform.position = Vector3.Lerp(transform.position, newCamPos, cameraSpeed * Time.deltaTime);
        }
    }
}
