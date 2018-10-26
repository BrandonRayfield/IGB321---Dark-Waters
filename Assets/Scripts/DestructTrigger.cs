using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructTrigger : MonoBehaviour {

    // Dialogue Variables
    [Header("Machine Variables")]
    public bool isActive;
    public bool inRange;

    private GameManager gameManager;
    private DestructManager destManager;
    private GameObject cameraObject;

    //UI Elements
    [Header("UI Variables")]
    public Text interactText;
    public string interactMessage = "Press 'e' to activate";

    // Expression Variables
    public GameObject questionObject;
    public GameObject updateObject;

    public float rewardHeightPosition;

    private GameObject spawnedQuestionObject;
    private GameObject spawnedUpdateObject;

    private bool hasUpdatedExpression;
    private bool questExpression;
    private int expressionType;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        destManager = gameManager.GetComponent<DestructManager>();

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

        // Spawn expression Icons so we don't have to go through and update all the NPC's
        if (!isActive) {
            Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
            spawnedQuestionObject = Instantiate(questionObject, spawnLocation, transform.rotation);
            Vector3 spawnLocation2 = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
            spawnedUpdateObject = Instantiate(updateObject, spawnLocation, transform.rotation);
            updateExpression(0);
        }

    }

    public void Update() {

        // Show the correct expression depending on the dialogue type
        if (!isActive) {
            updateExpression(2); // Set Exclamation Mark active if NPC or Sign
        } else {
            updateExpression(0); // Disable Both (Because Auto-trigger)
        }




        if (inRange && !isActive && Input.GetKeyDown(KeyCode.E)) {
            isActive = true;
            destManager.TriggerNew();
        }


    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            inRange = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player" && !isActive) {
            interactText.enabled = true;
            interactText.text = interactMessage;
        } else if(other.gameObject.tag == "Player" && isActive) {
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

    public void SetIsActive(bool active) {
        isActive = active;
    }

}
