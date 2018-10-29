using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructManager : MonoBehaviour {

    [Header("Activation Variables")]
    public GameObject padObject;
    public bool isActive;
    public bool isDestructing;
    public GameObject[] triggers;
    public int currentCount;
    private int maxCount = 3;
    private bool triggerDestruction;

    private float currentMainTime;
    private float maxBetweenTime = 120;
    private float maxMainTime = 180;

    public int currentMachineID;

    private GameObject playerObject;

    public Text textObject;
    public Text triggerText;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        padObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(currentMainTime <= 0 && !isDestructing) {
            isActive = false;
            textObject.enabled = false;
            currentCount = 0;
            triggerText.text = "Triggers left: " + currentCount + " / " + maxCount;
            foreach (GameObject trigger in triggers) {
                trigger.GetComponent<DestructTrigger>().SetIsActive(false);
            }
        }

        if(isActive && !isDestructing) {
            StartCoroutine(MainTimer());
        } else if (!isActive && !isDestructing) {
            StopCoroutine(MainTimer());
        }

        if(currentCount >= maxCount && !isDestructing) {
            currentMainTime = maxMainTime;
            triggerText.text = "Self Destruct Activated: Get to the Proto-Sub!";
            padObject.SetActive(true);
            isDestructing = true;
        }

        if(isDestructing) {
            StartCoroutine(MainTimer());
        }

        if (currentMainTime <= 0 && isDestructing) {
            playerObject.GetComponent<Player>().takeDamage(playerObject.GetComponent<Player>().health);
        }

    }

    private IEnumerator MainTimer() {
        currentMainTime -= Time.deltaTime;
        float minutes = Mathf.Floor(currentMainTime / 60);
        float seconds = Mathf.RoundToInt(currentMainTime % 59);

        if(seconds < 10) {
            textObject.text = "0" + minutes + ":0" + seconds;
        } else {
            textObject.text = "0" + minutes + ":" + seconds;
        }


        yield return new WaitForSeconds(1.0f);
    }

    public void TriggerNew() {
        isActive = true;
        textObject.enabled = true;
        triggerText.enabled = true;
        currentMainTime = maxBetweenTime;
        currentCount++;
        triggerText.text = "Triggers left: " + currentCount + " / " + maxCount;
    }
}
