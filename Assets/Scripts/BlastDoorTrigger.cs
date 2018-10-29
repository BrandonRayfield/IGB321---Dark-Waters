using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlastDoorTrigger : MonoBehaviour {

    [Header("Main Objects")]
    public GameObject blastDoor;
    private Animator blastDoorAnimator;

    public GameObject[] enemySpawners;

    public bool isActive;
    public bool hasPower;
    public bool inRange;

    public float currentTime;
    private float maxTime = 60;

    public Text textObject;
    public Text interactText;
    public string interactMessage = "Press 'e' to open door.";

	// Use this for initialization
	void Start () {
        blastDoorAnimator = blastDoor.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.E) && !isActive && hasPower) {
            isActive = true;
            foreach (GameObject spawner in enemySpawners) {
                spawner.GetComponent<EnemySpawner>().SetIsActive(true);
            }
            blastDoorAnimator.Play("DoorOpening");
        }

        if(isActive && currentTime <= 0) {
            foreach(GameObject spawner in enemySpawners) {
                spawner.GetComponent<EnemySpawner>().SetIsActive(false);
            }
            isActive = false;
        }

        if (isActive) {
            StartCoroutine(MainTimer());
        } else if (!isActive) {
            StopCoroutine(MainTimer());
        }
    }

    private IEnumerator MainTimer() {
        currentTime -= Time.deltaTime;
        textObject.text = Mathf.Floor(currentTime / 60) + ":" + Mathf.RoundToInt(currentTime % 59);
        yield return new WaitForSeconds(1.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !isActive && hasPower) {
            inRange = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player" && !isActive && hasPower) {
            interactText.enabled = true;
            interactText.text = interactMessage;
        } else if (other.gameObject.tag == "Player" && isActive) {
            interactText.enabled = false;
            interactText.text = "";
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            interactText.enabled = false;
            interactText.text = "";
        }
    }

}
