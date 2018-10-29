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
    private bool isDone;

    public float currentTime;
    public float maxTime = 40;

    public Text textObject;
    public Text interactText;
    public string interactMessage = "Press 'e' to open door.";

	// Use this for initialization
	void Start () {
        blastDoorAnimator = blastDoor.GetComponent<Animator>();
        currentTime = maxTime;
        textObject.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.E) && !isActive && hasPower && inRange) {
            isActive = true;
            foreach (GameObject spawner in enemySpawners) {
                spawner.GetComponent<EnemySpawner>().SetIsActive(true);
            }
            blastDoorAnimator.Play("DoorOpening");
            textObject.enabled = true;
            isDone = true;
        }

        if (isActive) {
            StartCoroutine(MainTimer());
        } else if (!isActive) {
            StopCoroutine(MainTimer());
        }

        if (isActive && currentTime <= 0) {
            foreach(GameObject spawner in enemySpawners) {
                spawner.GetComponent<EnemySpawner>().SetIsActive(false);
            }
            isActive = false;
            textObject.enabled = false;
        }

    }

    private IEnumerator MainTimer() {
        currentTime -= Time.deltaTime;
        float minutes = Mathf.Floor(currentTime / 60);
        float seconds = Mathf.RoundToInt(currentTime % 59);

        if (seconds < 10) {
            textObject.text = "0" + minutes + ":0" + seconds;
        } else {
            textObject.text = "0" + minutes + ":" + seconds;
        }
        yield return new WaitForSeconds(1.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !isActive && hasPower) {
            inRange = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player" && !isActive && hasPower && !isDone) {
            interactText.enabled = true;
            interactText.text = interactMessage;
        } else if (other.gameObject.tag == "Player" && isActive && isDone) {
            interactText.enabled = false;
            interactText.text = "";
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            interactText.enabled = false;
            interactText.text = "";
            inRange = false;
        }
    }

    public void SetHasPower(bool power) {
        hasPower = true;
    }

}
