using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour {

    // Dialogue Variables
    [Header("Dialogue Variables")]
    public int NpcID;
    public Dialogue[] dialogue;
    public Dialogue[] dialogueAfterTalk;
    public bool canTalk;
    public bool isTalking;

    // Completed Quest Objects
    [Header("Reward Variables")]
    public bool isRewardGiver;
    public GameObject rewardObject;
    public float rewardHeightPosition = 1.0f;
    public GameObject rewardSpawnLocation;

    private bool hasSpoken;
    private bool finishedQuest;
    private bool isUnlocked;
    private bool isRewarded;

    private int currentNpcID;

    private GameManager gameManager;
    private GameObject cameraObject;

    // Autotrigger Variables
    [Header("Autotrigger Variables")]
    public bool isAutomatic;
    private bool hasTriggered;
    private bool autoComplete;

    [Header("Event Tags")]
    public bool isLightSwitch;
    public GameObject doorTrigger;

    //UI Elements
    [Header("UI Variables")]
    public Text interactText;
    public string interactMessage = "Press 'e' to talk";

    // Expression Variables
    public GameObject questionObject;
    public GameObject updateObject;

    private GameObject spawnedQuestionObject;
    private GameObject spawnedUpdateObject;

    private bool hasUpdatedExpression;
    private bool questExpression;
    private int expressionType;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

        // Spawn expression Icons so we don't have to go through and update all the NPC's
        if(!isAutomatic) {
            Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
            spawnedQuestionObject = Instantiate(questionObject, spawnLocation, transform.rotation);
            Vector3 spawnLocation2 = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
            spawnedUpdateObject = Instantiate(updateObject, spawnLocation, transform.rotation);
            updateExpression(0);
        }

        // Show the correct expression depending on the dialogue type
        if(!isAutomatic) {
            updateExpression(2); // Set Exclamation Mark active if NPC or Sign
        } else if (isAutomatic) {
            updateExpression(0); // Disable Both (Because Auto-trigger)
        }
    }

    public void Update() {

        currentNpcID = FindObjectOfType<DialogueManager>().getCurrentNpcID();
        autoComplete = FindObjectOfType<DialogueManager>().getIsAutoComplete();

        if (canTalk) {
            isTalking = FindObjectOfType<DialogueManager>().getIsTalking();
        }


        if (!isAutomatic && canTalk && Input.GetKeyDown(KeyCode.E)) {
            StartTalking();
        }

        // Used to continue to next sentence, without allowing player to repeat dialogue after completed
        if(isAutomatic && !autoComplete && canTalk && Input.GetKeyDown(KeyCode.E)) {
            StartTalking();
        }

        if (currentNpcID == NpcID && !canTalk) {
            FindObjectOfType<DialogueManager>().EndDialogue();
            isTalking = false;
        }
    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    // Code for triggering the dialogue interaction
    private void StartTalking() {

        if(isAutomatic) {
            interactText.enabled = false;
        }

        //Remove Update expression once spoken to
        if(!isAutomatic) {
            interactText.enabled = false;
            updateExpression(0);
        }

        if (!isTalking) {
            if (hasSpoken) {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogueAfterTalk);
            } else {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                hasSpoken = true;

                // If the NPC is supposed to provide the player with a reward for talking to them, spawn reward item
                if (isRewardGiver && !isRewarded) {
                    //Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
                    Instantiate(rewardObject, rewardSpawnLocation.transform.position, transform.rotation);
                    isRewarded = true;
                }

            }
        } else {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            FindObjectOfType<DialogueManager>().setCurrentNpcID(NpcID);
            FindObjectOfType<DialogueManager>().setIsAutomatic(isAutomatic);
            FindObjectOfType<DialogueManager>().setIsPowerTrigger(isLightSwitch);
            FindObjectOfType<DialogueManager>().setDoorObject(doorTrigger);
            canTalk = true;

            if (isAutomatic && !hasTriggered) {
                StartTalking();
                hasTriggered = true;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(!isAutomatic && other.gameObject.tag == "Player") {
            if(!isTalking) {
                interactText.enabled = true;
                interactText.text = interactMessage;
            } else {
                interactText.enabled = false;
                interactText.text = "";
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!isAutomatic && other.gameObject.tag == "Player") {
            canTalk = false;
            interactText.enabled = false;
            interactText.text = "";
        }
    }

    private void updateExpression(int expressionType) {
        if (expressionType == 1) {              //Question Mark is Active
            spawnedQuestionObject.SetActive(true);
            spawnedUpdateObject.SetActive(false);
        } else if (expressionType == 2) {       //Exclamation is Active
            spawnedQuestionObject.SetActive(false);
            spawnedUpdateObject.SetActive(true);
        } else {                                //None are Active
            spawnedQuestionObject.SetActive(false);
            spawnedUpdateObject.SetActive(false);
        }
    }

    public void newExpression(int questID) {
        if(NpcID == questID && !isAutomatic) {
            updateExpression(2);
        }
    }

}
