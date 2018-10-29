using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    [Header("Door Variables")]
    public Animator doorAnimator;
    public bool isNoPower;
    public bool isLocked;

    // Animation Names
    const string doorStartClosed = "doorStartClosed";
    const string doorStartOpen = "doorStartOpen";
    const string doorClosing = "doorClosing";
    const string doorOpening = "doorOpening";


    // Use this for initialization
    void Start () {
		if(!isNoPower) {
            doorAnimator.Play(doorStartClosed);
        } else {
            doorAnimator.Play(doorStartOpen);
        }
	}
	
	// Update is called once per frame
	void Update () {
  //      if(Input.GetKeyDown(KeyCode.E) && !isNoPower) {
  //          Debug.Log("Door Open");
  //          doorAnimator.Play(doorOpening);
  //      }

  //      if (Input.GetKeyUp(KeyCode.E) && !isNoPower) {
  //          Debug.Log("Door Close");
  //          doorAnimator.Play(doorClosing);
  //      }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !isNoPower && !isLocked) {
            doorAnimator.Play(doorOpening);
        }
    }


    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player" && !isNoPower && !isLocked) {
            doorAnimator.Play(doorClosing);
        }
    }

    public void SetIsNoPower(bool newPower) {
        isNoPower = newPower;
    }
}
